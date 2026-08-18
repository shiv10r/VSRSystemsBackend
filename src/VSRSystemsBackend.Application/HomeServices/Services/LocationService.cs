using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IServiceCatalogRepository _catalogRepository;

    public LocationService(ILocationRepository locationRepository, IServiceCatalogRepository catalogRepository)
    {
        _locationRepository = locationRepository;
        _catalogRepository = catalogRepository;
    }

    public async Task<Result<IReadOnlyList<CityDto>>> GetCitiesAsync(CancellationToken cancellationToken = default)
    {
        var cities = await _locationRepository.GetActiveCitiesAsync(cancellationToken);
        var dtos = cities
            .OrderBy(c => c.Name)
            .Select(c => new CityDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                LaunchedAt = c.LaunchedAt,
                Zones = (c.Zones ?? new List<Zone>())
                    .OrderBy(z => z.Name)
                    .Select(z => new ZoneDto
                    {
                        Id = z.Id,
                        CityId = z.CityId,
                        Name = z.Name,
                        Localities = (z.Localities ?? new List<Locality>())
                            .OrderBy(l => l.Name)
                            .Select(l => new LocalityDto
                            {
                                Id = l.Id,
                                ZoneId = l.ZoneId,
                                Name = l.Name,
                                Pincode = l.Pincode
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();

        return Result<IReadOnlyList<CityDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ZoneDto>>> GetZonesAsync(string cityId, CancellationToken cancellationToken = default)
    {
        var zones = await _locationRepository.GetZonesByCityAsync(cityId, cancellationToken);
        var dtos = zones
            .OrderBy(z => z.Name)
            .Select(z => new ZoneDto
            {
                Id = z.Id,
                CityId = z.CityId,
                Name = z.Name
            })
            .ToList();

        return Result<IReadOnlyList<ZoneDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<LocalityDto>>> GetLocalitiesAsync(string zoneId, CancellationToken cancellationToken = default)
    {
        var localities = await _locationRepository.GetLocalitiesByZoneAsync(zoneId, cancellationToken);
        var dtos = localities
            .OrderBy(l => l.Name)
            .Select(l => new LocalityDto
            {
                Id = l.Id,
                ZoneId = l.ZoneId,
                Name = l.Name,
                Pincode = l.Pincode
            })
            .ToList();

        return Result<IReadOnlyList<LocalityDto>>.Success(dtos);
    }

    public async Task<Result<ServiceabilityResultDto>> CheckServiceabilityAsync(ServiceabilityRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Pincode))
            return Result<ServiceabilityResultDto>.Failure("Pincode is required");

        var pincode = await _locationRepository.GetPincodeAsync(dto.Pincode.Trim(), cancellationToken);
        if (pincode == null || !pincode.IsServiceable)
        {
            return Result<ServiceabilityResultDto>.Success(new ServiceabilityResultDto
            {
                IsServiceable = false,
                Pincode = dto.Pincode,
                ServiceAvailable = false,
                Message = "We do not serve this pincode yet"
            });
        }

        var zones = await _locationRepository.GetZonesByCityAsync(pincode.CityId, cancellationToken);
        Zone? zone = null;
        Locality? locality = null;

        foreach (var z in zones)
        {
            var localities = await _locationRepository.GetLocalitiesByZoneAsync(z.Id, cancellationToken);
            var match = localities.FirstOrDefault(l => l.Pincode == pincode.Code);
            if (match != null)
            {
                zone = z;
                locality = match;
                break;
            }
        }

        var city = pincode.City;
        var result = new ServiceabilityResultDto
        {
            IsServiceable = true,
            CityId = pincode.CityId,
            CityName = city?.Name ?? string.Empty,
            ZoneId = zone?.Id,
            ZoneName = zone?.Name,
            LocalityId = locality?.Id,
            LocalityName = locality?.Name,
            Pincode = dto.Pincode,
            ServiceAvailable = true,
            Message = "Serviceable"
        };

        if (!string.IsNullOrWhiteSpace(dto.ServiceId))
        {
            var serviceArea = zone != null
                ? await _locationRepository.GetServiceAreaAsync(pincode.CityId, zone.Id, cancellationToken)
                : null;
            if (serviceArea == null || !await _locationRepository.IsServiceInAreaAsync(serviceArea.Id, dto.ServiceId, cancellationToken))
            {
                result.ServiceAvailable = false;
                result.Message = "This service is not available in your area yet";
            }
        }

        return Result<ServiceabilityResultDto>.Success(result);
    }
}