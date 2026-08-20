using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class ServiceCatalogService : IServiceCatalogService
{
    private readonly IServiceCatalogRepository _catalogRepository;
    private readonly ILocationRepository _locationRepository;

    public ServiceCatalogService(IServiceCatalogRepository catalogRepository, ILocationRepository locationRepository)
    {
        _catalogRepository = catalogRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<IReadOnlyList<ServiceCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _catalogRepository.GetActiveCategoriesAsync(cancellationToken);
        var dtos = categories
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Select(ToCategoryDto)
            .ToList();

        return Result<IReadOnlyList<ServiceCategoryDto>>.Success(dtos);
    }

    public async Task<Result<ServiceCategoryDto>> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var category = await _catalogRepository.GetCategoryBySlugAsync(slug, cancellationToken);
        if (category == null)
            return Result<ServiceCategoryDto>.Failure("Category not found");

        return Result<ServiceCategoryDto>.Success(ToCategoryDto(category));
    }

    public async Task<Result<ServiceCategoryDto>> CreateCategoryAsync(CreateServiceCategoryDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<ServiceCategoryDto>.Failure("Name is required");

        var slug = string.IsNullOrWhiteSpace(dto.Slug)
            ? dto.Name.ToLower().Trim().Replace(" ", "-")
            : dto.Slug.Trim().ToLower();

        var existing = await _catalogRepository.GetCategoryBySlugAsync(slug, cancellationToken);
        if (existing != null)
            return Result<ServiceCategoryDto>.Failure($"A category with slug '{slug}' already exists");

        var category = new ServiceCategory
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = dto.Name.Trim(),
            Slug = slug,
            Tagline = dto.Description ?? string.Empty,
            ImageUrl = dto.Icon ?? string.Empty,
            SortOrder = dto.SortOrder,
            IsActive = dto.IsActive
        };

        var created = await _catalogRepository.AddCategoryAsync(category, cancellationToken);
        return Result<ServiceCategoryDto>.Success(ToCategoryDto(created));
    }

    public async Task<Result<IReadOnlyList<ServiceDto>>> GetServicesAsync(string? categoryId, string? cityId, CancellationToken cancellationToken = default)
    {
        var services = await _catalogRepository.GetActiveServicesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(categoryId))
            services = services.Where(s => s.CategoryId == categoryId).ToList();

        if (!string.IsNullOrWhiteSpace(cityId))
        {
            var zones = await _locationRepository.GetZonesByCityAsync(cityId, cancellationToken);
            if (zones.Count > 0)
            {
                var areaIds = new List<string>();
                foreach (var zone in zones)
                {
                    var area = await _locationRepository.GetServiceAreaAsync(cityId, zone.Id, cancellationToken);
                    if (area != null)
                        areaIds.Add(area.Id);
                }
                var serviceIds = new HashSet<string>();
                foreach (var areaId in areaIds)
                {
                    var ids = await _locationRepository.GetServiceIdsInAreaAsync(areaId, cancellationToken);
                    foreach (var id in ids)
                        serviceIds.Add(id);
                }
                services = services.Where(s => serviceIds.Contains(s.Id)).ToList();
            }
        }

        var dtos = services
            .OrderBy(s => s.Name)
            .Select(ToServiceDto)
            .ToList();

        return Result<IReadOnlyList<ServiceDto>>.Success(dtos);
    }

    public async Task<Result<ServiceDto>> GetServiceBySlugAsync(string slug, string? cityId, CancellationToken cancellationToken = default)
    {
        var service = await _catalogRepository.GetServiceBySlugAsync(slug, cancellationToken);
        if (service == null)
            return Result<ServiceDto>.Failure("Service not found");

        if (!string.IsNullOrWhiteSpace(cityId) && !await IsServiceAvailableInCityAsync(service.Id, cityId, cancellationToken))
            return Result<ServiceDto>.Failure("Service not available in this city");

        var dto = ToServiceDto(service);
        dto.Packages = (await _catalogRepository.GetActivePackagesByServiceAsync(service.Id, cancellationToken))
            .OrderBy(p => p.BasePrice)
            .Select(ToPackageDto)
            .ToList();
        dto.AddOns = (await _catalogRepository.GetAddOnsByServiceAsync(service.Id, cancellationToken))
            .Where(a => a.IsActive)
            .Select(ToAddOnDto)
            .ToList();
        dto.Problems = (await _catalogRepository.GetProblemsByServiceAsync(service.Id, cancellationToken))
            .OrderBy(p => p.SortOrder)
            .Select(ToProblemDto)
            .ToList();

        return Result<ServiceDto>.Success(dto);
    }

    public async Task<Result<IReadOnlyList<ServicePackageDto>>> GetPackagesAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        var packages = await _catalogRepository.GetActivePackagesByServiceAsync(serviceId, cancellationToken);
        var dtos = packages
            .OrderBy(p => p.BasePrice)
            .Select(ToPackageDto)
            .ToList();

        return Result<IReadOnlyList<ServicePackageDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ServiceAddOnDto>>> GetAddOnsAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        var addOns = await _catalogRepository.GetAddOnsByServiceAsync(serviceId, cancellationToken);
        var dtos = addOns
            .Where(a => a.IsActive)
            .OrderBy(a => a.Price)
            .Select(ToAddOnDto)
            .ToList();

        return Result<IReadOnlyList<ServiceAddOnDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ServiceProblemDto>>> GetProblemsAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        var problems = await _catalogRepository.GetProblemsByServiceAsync(serviceId, cancellationToken);
        var dtos = problems
            .OrderBy(p => p.SortOrder)
            .Select(ToProblemDto)
            .ToList();

        return Result<IReadOnlyList<ServiceProblemDto>>.Success(dtos);
    }

    public async Task<Result<SearchCatalogResultDto>> SearchAsync(SearchCatalogQueryDto query, CancellationToken cancellationToken = default)
    {
        var services = await _catalogRepository.GetActiveServicesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(query.Q))
        {
            var q = query.Q.Trim();
            services = services
                .Where(s => s.Name.Contains(q, StringComparison.OrdinalIgnoreCase)
                            || s.ShortDescription.Contains(q, StringComparison.OrdinalIgnoreCase)
                            || s.LongDescription.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(query.CategoryId))
            services = services.Where(s => s.CategoryId == query.CategoryId).ToList();

        if (query.EmergencyOnly == true)
            services = services.Where(s => s.IsEmergency).ToList();

        if (!string.IsNullOrWhiteSpace(query.Pincode))
        {
            var areaIds = await GetAreaIdsForPincodeAsync(query.Pincode, cancellationToken);
            if (areaIds.Count > 0)
            {
                var serviceIds = new HashSet<string>();
                foreach (var areaId in areaIds)
                {
                    var ids = await _locationRepository.GetServiceIdsInAreaAsync(areaId, cancellationToken);
                    foreach (var id in ids)
                        serviceIds.Add(id);
                }
                services = services.Where(s => serviceIds.Contains(s.Id)).ToList();
            }
        }

        var total = services.Count;
        var items = services
            .OrderBy(s => s.Name)
            .Skip((Math.Max(query.Page, 1) - 1) * Math.Max(query.PageSize, 1))
            .Take(Math.Max(query.PageSize, 1))
            .Select(ToServiceDto)
            .ToList();

        return Result<SearchCatalogResultDto>.Success(new SearchCatalogResultDto
        {
            Items = items,
            Total = total,
            Page = Math.Max(query.Page, 1),
            PageSize = Math.Max(query.PageSize, 1)
        });
    }

    public async Task<Result<IReadOnlyList<ServiceCategoryDto>>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
        => await GetCategoriesAsync(cancellationToken);

    private async Task<bool> IsServiceAvailableInCityAsync(string serviceId, string cityId, CancellationToken cancellationToken)
    {
        var zones = await _locationRepository.GetZonesByCityAsync(cityId, cancellationToken);
        foreach (var zone in zones)
        {
            var area = await _locationRepository.GetServiceAreaAsync(cityId, zone.Id, cancellationToken);
            if (area == null)
                continue;
            if (await _locationRepository.IsServiceInAreaAsync(area.Id, serviceId, cancellationToken))
                return true;
        }
        return false;
    }

    private async Task<List<string>> GetAreaIdsForPincodeAsync(string pincode, CancellationToken cancellationToken)
    {
        var pin = await _locationRepository.GetPincodeAsync(pincode, cancellationToken);
        if (pin == null)
            return new List<string>();

        var areaIds = new List<string>();
        var zones = await _locationRepository.GetZonesByCityAsync(pin.CityId, cancellationToken);
        foreach (var zone in zones)
        {
            var area = await _locationRepository.GetServiceAreaAsync(pin.CityId, zone.Id, cancellationToken);
            if (area != null)
                areaIds.Add(area.Id);
        }
        return areaIds;
    }

    private static ServiceCategoryDto ToCategoryDto(ServiceCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Slug = c.Slug,
        Tagline = c.Tagline,
        ImageUrl = c.ImageUrl,
        SortOrder = c.SortOrder,
        IsActive = c.IsActive,
        ServiceCount = c.Services?.Count(s => s.IsActive) ?? 0
    };

    private static ServiceDto ToServiceDto(Service s) => new()
    {
        Id = s.Id,
        CategoryId = s.CategoryId,
        CategoryName = s.Category?.Name ?? string.Empty,
        Name = s.Name,
        Slug = s.Slug,
        ShortDescription = s.ShortDescription,
        LongDescription = s.LongDescription,
        ImageUrl = s.ImageUrl,
        IsEmergency = s.IsEmergency,
        NeedsInspection = s.NeedsInspection,
        InspectionFee = s.InspectionFee,
        IsActive = s.IsActive,
        StartingPrice = s.Packages?.Where(p => p.IsActive).Select(p => p.BasePrice).DefaultIfEmpty(0).Min() ?? 0
    };

    private static ServicePackageDto ToPackageDto(ServicePackage p) => new()
    {
        Id = p.Id,
        ServiceId = p.ServiceId,
        Name = p.Name,
        ShortDescription = p.ShortDescription,
        BasePrice = p.BasePrice,
        DurationMins = p.DurationMins,
        WhatIncluded = p.WhatIncluded,
        Warranty = p.Warranty,
        IsPopular = p.IsPopular,
        IsActive = p.IsActive,
        DiscountedPrice = p.DetailedDescription.Contains("discount", StringComparison.OrdinalIgnoreCase) ? p.BasePrice : null
    };

    private static ServiceAddOnDto ToAddOnDto(ServiceAddOn a) => new()
    {
        Id = a.Id,
        ServiceId = a.ServiceId,
        Name = a.Name,
        Price = a.Price,
        DurationMins = a.DurationMins,
        IsActive = a.IsActive
    };

    private static ServiceProblemDto ToProblemDto(ServiceProblem p) => new()
    {
        Id = p.Id,
        ServiceId = p.ServiceId,
        Name = p.Name,
        Description = p.Description,
        SortOrder = p.SortOrder
    };
}