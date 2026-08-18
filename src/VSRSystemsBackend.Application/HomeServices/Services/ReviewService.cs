using System.Text.Json;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IServiceCatalogRepository _serviceCatalogRepository;

    public ReviewService(
        IReviewRepository reviewRepository,
        IBookingRepository bookingRepository,
        IProfessionalRepository professionalRepository,
        ICustomerRepository customerRepository,
        IServiceCatalogRepository serviceCatalogRepository)
    {
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
        _professionalRepository = professionalRepository;
        _customerRepository = customerRepository;
        _serviceCatalogRepository = serviceCatalogRepository;
    }

    public async Task<Result<ReviewDto>> SubmitAsync(CreateReviewDto dto, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(dto.BookingId, cancellationToken);
        if (booking == null)
            return Result<ReviewDto>.Failure("Booking not found");
        if (booking.Status != "completed")
            return Result<ReviewDto>.Failure("Only completed bookings can be reviewed");
        if (await _reviewRepository.ExistsForBookingAsync(dto.BookingId, cancellationToken))
            return Result<ReviewDto>.Failure("Booking already reviewed");

        var review = new Review
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            BookingId = booking.Id,
            CustomerId = booking.CustomerId,
            ProfessionalId = booking.AssignedProfessionalId ?? string.Empty,
            Rating = dto.Rating,
            Comment = dto.Comment,
            TagsJson = SerializeTags(dto.Tags),
            Quality = dto.Quality,
            Professionalism = dto.Professionalism,
            Punctuality = dto.Punctuality,
            Cleanliness = dto.Cleanliness,
            Communication = dto.Communication,
            Value = dto.Value,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var url in dto.MediaUrls)
        {
            review.Media.Add(new ReviewMedia
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                ReviewId = review.Id,
                MediaUrl = url,
                MediaType = "image",
                CreatedAt = DateTime.UtcNow
            });
        }

        await _reviewRepository.AddAsync(review, cancellationToken);
        await UpdateProfessionalQualityScoreAsync(review, cancellationToken);

        return Result<ReviewDto>.Success(await ToReviewDtoAsync(review, booking, cancellationToken));
    }

    public async Task<Result<IReadOnlyList<ReviewDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetByProfessionalAsync(professionalId, cancellationToken);
        var dtos = new List<ReviewDto>();
        foreach (var review in reviews)
            dtos.Add(await ToReviewDtoAsync(review, null, cancellationToken));

        return Result<IReadOnlyList<ReviewDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ReviewDto>>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default)
    {
        var reviews = await _reviewRepository.GetByServiceAsync(serviceId, cancellationToken);
        var dtos = new List<ReviewDto>();
        foreach (var review in reviews)
            dtos.Add(await ToReviewDtoAsync(review, null, cancellationToken));

        return Result<IReadOnlyList<ReviewDto>>.Success(dtos);
    }

    public async Task<Result<ReviewDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByBookingAsync(bookingId, cancellationToken);
        if (review == null)
            return Result<ReviewDto>.Failure("Review not found");

        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        return Result<ReviewDto>.Success(await ToReviewDtoAsync(review, booking, cancellationToken));
    }

    public async Task<Result<ReviewDto>> ReplyAsync(string reviewId, string reply, CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId, cancellationToken);
        if (review == null)
            return Result<ReviewDto>.Failure("Review not found");

        review.Comment = string.IsNullOrWhiteSpace(review.Comment)
            ? reply
            : $"{review.Comment} [Pro reply] {reply}";
        review.UpdatedAt = DateTime.UtcNow;

        await _reviewRepository.UpdateAsync(review, cancellationToken);
        return Result<ReviewDto>.Success(await ToReviewDtoAsync(review, null, cancellationToken));
    }

    private async Task UpdateProfessionalQualityScoreAsync(Review review, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(review.ProfessionalId))
            return;

        var professional = await _professionalRepository.GetByIdAsync(review.ProfessionalId, cancellationToken);
        if (professional == null)
            return;

        var performance = await _professionalRepository.GetCurrentPerformanceAsync(review.ProfessionalId, cancellationToken);
        if (performance == null)
            return;

        var dimensions = new int?[] { review.Quality, review.Professionalism, review.Punctuality, review.Cleanliness, review.Communication, review.Value };
        var values = dimensions.Where(d => d.HasValue).Select(d => d!.Value).ToList();
        if (values.Count == 0)
            return;

        performance.AvgRating = values.Average();
        professional.QualityScore = (decimal)performance.AvgRating;
        professional.UpdatedAt = DateTime.UtcNow;

        await _professionalRepository.UpdateAsync(professional, cancellationToken);
    }

    private async Task<ReviewDto> ToReviewDtoAsync(Review review, Booking? booking, CancellationToken cancellationToken)
    {
        var customerName = review.Customer?.DisplayName ?? string.Empty;
        if (string.IsNullOrEmpty(customerName))
        {
            var customer = await _customerRepository.GetByIdAsync(review.CustomerId, cancellationToken);
            customerName = customer?.DisplayName ?? string.Empty;
        }

        var professionalName = review.Professional?.DisplayName ?? string.Empty;
        if (string.IsNullOrEmpty(professionalName))
        {
            var professional = await _professionalRepository.GetByIdAsync(review.ProfessionalId, cancellationToken);
            professionalName = professional?.DisplayName ?? string.Empty;
        }

        var bookingNumber = booking?.BookingNumber ?? review.Booking?.BookingNumber ?? string.Empty;

        var serviceName = booking?.Service?.Name ?? review.Booking?.Service?.Name ?? string.Empty;
        if (string.IsNullOrEmpty(serviceName))
        {
            var serviceId = booking?.ServiceId ?? review.Booking?.ServiceId;
            if (!string.IsNullOrEmpty(serviceId))
            {
                var service = await _serviceCatalogRepository.GetByIdAsync(serviceId, cancellationToken);
                serviceName = service?.Name ?? string.Empty;
            }
        }

        return new ReviewDto
        {
            Id = review.Id,
            BookingId = review.BookingId,
            BookingNumber = bookingNumber,
            CustomerId = review.CustomerId,
            CustomerName = customerName,
            ProfessionalId = review.ProfessionalId,
            ProfessionalName = professionalName,
            ServiceName = serviceName,
            Rating = review.Rating,
            Comment = review.Comment,
            Tags = DeserializeTags(review.TagsJson),
            Quality = review.Quality,
            Professionalism = review.Professionalism,
            Punctuality = review.Punctuality,
            Cleanliness = review.Cleanliness,
            Communication = review.Communication,
            Value = review.Value,
            Media = (review.Media ?? new List<ReviewMedia>())
                .Select(m => new ReviewMediaDto
                {
                    Id = m.Id,
                    ReviewId = m.ReviewId,
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType
                })
                .ToList()
        };
    }

    private static string SerializeTags(List<string> tags)
    {
        return JsonSerializer.Serialize(tags ?? new List<string>());
    }

    private static List<string> DeserializeTags(string tagsJson)
    {
        if (string.IsNullOrWhiteSpace(tagsJson))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(tagsJson) ?? new List<string>();
        }
        catch (JsonException)
        {
            return new List<string>();
        }
    }
}