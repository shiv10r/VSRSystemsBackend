using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class ProfessionalService : IProfessionalService
{
    private readonly IProfessionalRepository _professionalRepository;

    public ProfessionalService(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<Result<ProfessionalDetailDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetWithDetailsAsync(id, cancellationToken);
        if (professional == null)
            return Result<ProfessionalDetailDto>.Failure("Professional not found");

        return Result<ProfessionalDetailDto>.Success(ToDetailDto(professional));
    }

    public async Task<Result<PagedResult<ProfessionalDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var professionals = await _professionalRepository.GetAllAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var q = request.SearchTerm.Trim();
            professionals = professionals
                .Where(p => p.DisplayName.Contains(q, StringComparison.OrdinalIgnoreCase)
                            || (p.Phone != null && p.Phone.Contains(q, StringComparison.OrdinalIgnoreCase))
                            || (p.Email != null && p.Email.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            var desc = request.SortDescending;
            professionals = (request.SortBy.ToLowerInvariant()) switch
            {
                "qualityscore" => desc
                    ? professionals.OrderByDescending(p => p.QualityScore).ToList()
                    : professionals.OrderBy(p => p.QualityScore).ToList(),
                "joinedat" => desc
                    ? professionals.OrderByDescending(p => p.JoinedAt).ToList()
                    : professionals.OrderBy(p => p.JoinedAt).ToList(),
                _ => desc
                    ? professionals.OrderByDescending(p => p.DisplayName).ToList()
                    : professionals.OrderBy(p => p.DisplayName).ToList(),
            };
        }
        else
        {
            professionals = professionals.OrderBy(p => p.DisplayName).ToList();
        }

        var total = professionals.Count;
        var page = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Max(request.PageSize, 1);
        var items = professionals
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Result<PagedResult<ProfessionalDto>>.Success(PagedResult<ProfessionalDto>.Create(items, total, page, pageSize));
    }

    public async Task<Result<PagedResult<ProfessionalDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var professionals = await _professionalRepository.GetByStatusAsync(status, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var q = request.SearchTerm.Trim();
            professionals = professionals
                .Where(p => p.DisplayName.Contains(q, StringComparison.OrdinalIgnoreCase)
                            || (p.Phone != null && p.Phone.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        var total = professionals.Count;
        var page = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Max(request.PageSize, 1);
        var items = professionals
            .OrderBy(p => p.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Result<PagedResult<ProfessionalDto>>.Success(PagedResult<ProfessionalDto>.Create(items, total, page, pageSize));
    }

    public async Task<Result<PagedResult<ProfessionalDto>>> GetByServiceAsync(string serviceId, string? cityId, string? zoneId, PagedRequest request, CancellationToken cancellationToken = default)
    {
        var professionals = await _professionalRepository.GetAllAsync(cancellationToken);

        professionals = professionals
            .Where(p => p.Skills.Any(s => s.ServiceId == serviceId && !s.IsDeleted))
            .ToList();

        if (!string.IsNullOrWhiteSpace(cityId))
            professionals = professionals
                .Where(p => p.ServiceAreas.Any(a => a.CityId == cityId && !a.IsDeleted))
                .ToList();

        if (!string.IsNullOrWhiteSpace(zoneId))
            professionals = professionals
                .Where(p => p.ServiceAreas.Any(a => a.ZoneId == zoneId && !a.IsDeleted))
                .ToList();

        var total = professionals.Count;
        var page = Math.Max(request.PageNumber, 1);
        var pageSize = Math.Max(request.PageSize, 1);
        var items = professionals
            .OrderBy(p => p.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Result<PagedResult<ProfessionalDto>>.Success(PagedResult<ProfessionalDto>.Create(items, total, page, pageSize));
    }

    public async Task<Result<IReadOnlyList<ProfessionalAvailabilityDto>>> GetAvailabilitiesAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var availabilities = await _professionalRepository.GetAvailabilitiesAsync(professionalId, cancellationToken);
        var dtos = availabilities
            .OrderBy(a => a.DayOfWeek)
            .ThenBy(a => a.StartTime)
            .Select(a => new ProfessionalAvailabilityDto
            {
                Id = a.Id,
                ProfessionalId = a.ProfessionalId,
                DayOfWeek = a.DayOfWeek,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                IsRecurring = a.IsRecurring
            })
            .ToList();

        return Result<IReadOnlyList<ProfessionalAvailabilityDto>>.Success(dtos);
    }

    public async Task<Result<ProfessionalDto>> UpdateProfileAsync(string id, RegisterProfessionalDto dto, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(id, cancellationToken);
        if (professional == null)
            return Result<ProfessionalDto>.Failure("Professional not found");

        professional.DisplayName = dto.DisplayName.Trim();
        professional.Gender = dto.Gender ?? string.Empty;
        professional.Dob = dto.Dob;
        professional.Phone = dto.Phone.Trim();
        professional.Email = dto.Email;

        await _professionalRepository.UpdateAsync(professional, cancellationToken);
        return Result<ProfessionalDto>.Success(ToDto(professional));
    }

    public async Task<Result<ProfessionalDto>> VerifyAsync(string id, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(id, cancellationToken);
        if (professional == null)
            return Result<ProfessionalDto>.Failure("Professional not found");

        professional.OnboardingStatus = "verified";
        await _professionalRepository.UpdateAsync(professional, cancellationToken);
        return Result<ProfessionalDto>.Success(ToDto(professional));
    }

    public async Task<Result<ProfessionalDto>> SuspendAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(id, cancellationToken);
        if (professional == null)
            return Result<ProfessionalDto>.Failure("Professional not found");

        professional.OnboardingStatus = "suspended";
        await _professionalRepository.UpdateAsync(professional, cancellationToken);
        return Result<ProfessionalDto>.Success(ToDto(professional));
    }

    public async Task<Result> AddAvailabilityAsync(string professionalId, UpdateProfessionalAvailabilityDto dto, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(professionalId, cancellationToken);
        if (professional == null)
            return Result.Failure("Professional not found");

        var availability = new ProfessionalAvailability
        {
            Id = Guid.NewGuid().ToString("N"),
            ProfessionalId = professionalId,
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            IsRecurring = dto.IsRecurring
        };
        await _professionalRepository.AddAvailabilityAsync(availability, cancellationToken);
        return Result.Success();
    }

    public async Task<Result> RemoveAvailabilityAsync(string professionalId, string availabilityId, CancellationToken cancellationToken = default)
    {
        var availabilities = await _professionalRepository.GetAvailabilitiesAsync(professionalId, cancellationToken);
        var availability = availabilities.FirstOrDefault(a => a.Id == availabilityId);
        if (availability == null)
            return Result.Failure("Availability not found");

        await _professionalRepository.DeleteAvailabilityAsync(availability, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<ProfessionalDto>> ReviewDocumentAsync(string professionalId, string documentId, ReviewProfessionalDocumentDto dto, CancellationToken cancellationToken = default)
    {
        var professional = await _professionalRepository.GetByIdAsync(professionalId, cancellationToken);
        if (professional == null)
            return Result<ProfessionalDto>.Failure("Professional not found");

        var documents = await _professionalRepository.GetDocumentsAsync(professionalId, cancellationToken);
        var document = documents.FirstOrDefault(d => d.Id == documentId);
        if (document == null)
            return Result<ProfessionalDto>.Failure("Document not found");

        document.Status = dto.Status;
        document.RejectionReason = dto.Status == "rejected" ? dto.RejectionReason : null;
        document.ReviewedAt = DateTime.UtcNow;

        await _professionalRepository.UpdateDocumentAsync(document, cancellationToken);
        return Result<ProfessionalDto>.Success(ToDto(professional));
    }

    public async Task<Result<IReadOnlyList<ProfessionalPerformanceDto>>> GetPerformanceAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var performances = await _professionalRepository.GetPerformanceAsync(professionalId, cancellationToken);

        var dtos = performances.Select(p => new ProfessionalPerformanceDto
        {
            Id = p.Id,
            ProfessionalId = p.ProfessionalId,
            PeriodStart = p.PeriodStart,
            PeriodEnd = p.PeriodEnd,
            JobsCompleted = p.JobsCompleted,
            JobsCancelled = p.JobsCancelled,
            AvgRating = p.AvgRating,
            OnTimeRate = p.OnTimeRate,
            AcceptanceRate = p.AcceptanceRate
        }).ToList();

        return Result<IReadOnlyList<ProfessionalPerformanceDto>>.Success(dtos);
    }

    private static ProfessionalDto ToDto(Professional p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        DisplayName = p.DisplayName,
        Gender = p.Gender,
        Dob = p.Dob,
        OnboardingStatus = p.OnboardingStatus,
        QualityScore = p.QualityScore,
        Tier = p.Tier,
        JoinedAt = p.JoinedAt,
        Phone = p.Phone,
        Email = p.Email,
        Skills = p.Skills.Where(s => !s.IsDeleted).Select(s => s.ServiceId).ToList(),
        ServiceAreaNames = p.ServiceAreas.Where(a => !a.IsDeleted).Select(a => $"{a.CityId}/{a.ZoneId}").ToList()
    };

    private static ProfessionalDetailDto ToDetailDto(Professional p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        DisplayName = p.DisplayName,
        Gender = p.Gender,
        Dob = p.Dob,
        OnboardingStatus = p.OnboardingStatus,
        QualityScore = p.QualityScore,
        Tier = p.Tier,
        JoinedAt = p.JoinedAt,
        Phone = p.Phone,
        Email = p.Email,
        Skills = p.Skills.Where(s => !s.IsDeleted).Select(s => s.ServiceId).ToList(),
        ServiceAreaNames = p.ServiceAreas.Where(a => !a.IsDeleted).Select(a => $"{a.CityId}/{a.ZoneId}").ToList(),
        Documents = p.Documents.Where(d => !d.IsDeleted).Select(d => new ProfessionalDocumentDto
        {
            Id = d.Id,
            ProfessionalId = d.ProfessionalId,
            DocType = d.DocType,
            FileUrl = d.FileUrl,
            Status = d.Status,
            ReviewedAt = d.ReviewedAt,
            RejectionReason = d.RejectionReason
        }).ToList(),
        Availabilities = p.Availabilities.Where(a => !a.IsDeleted).Select(a => new ProfessionalAvailabilityDto
        {
            Id = a.Id,
            ProfessionalId = a.ProfessionalId,
            DayOfWeek = a.DayOfWeek,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            IsRecurring = a.IsRecurring
        }).ToList(),
        Performances = p.Performances.Where(perf => !perf.IsDeleted).Select(perf => new ProfessionalPerformanceDto
        {
            Id = perf.Id,
            ProfessionalId = perf.ProfessionalId,
            PeriodStart = perf.PeriodStart,
            PeriodEnd = perf.PeriodEnd,
            JobsCompleted = perf.JobsCompleted,
            JobsCancelled = perf.JobsCancelled,
            AvgRating = perf.AvgRating,
            OnTimeRate = perf.OnTimeRate,
            AcceptanceRate = perf.AcceptanceRate
        }).ToList()
    };
}