using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.HomeServices.Interfaces;

public interface IServiceCatalogService
{
    Task<Result<IReadOnlyList<ServiceCategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Result<ServiceCategoryDto>> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Result<ServiceCategoryDto>> CreateCategoryAsync(CreateServiceCategoryDto dto, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ServiceDto>>> GetServicesAsync(string? categoryId, string? cityId, CancellationToken cancellationToken = default);
    Task<Result<ServiceDto>> GetServiceBySlugAsync(string slug, string? cityId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ServicePackageDto>>> GetPackagesAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ServiceAddOnDto>>> GetAddOnsAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ServiceProblemDto>>> GetProblemsAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<Result<SearchCatalogResultDto>> SearchAsync(SearchCatalogQueryDto query, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ServiceCategoryDto>>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}

public interface ILocationService
{
    Task<Result<IReadOnlyList<CityDto>>> GetCitiesAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ZoneDto>>> GetZonesAsync(string cityId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<LocalityDto>>> GetLocalitiesAsync(string zoneId, CancellationToken cancellationToken = default);
    Task<Result<ServiceabilityResultDto>> CheckServiceabilityAsync(ServiceabilityRequestDto dto, CancellationToken cancellationToken = default);
}

public interface IProfessionalService
{
    Task<Result<ProfessionalDetailDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProfessionalDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProfessionalDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProfessionalDto>>> GetByServiceAsync(string serviceId, string? cityId, string? zoneId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfessionalAvailabilityDto>>> GetAvailabilitiesAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<ProfessionalDto>> UpdateProfileAsync(string id, RegisterProfessionalDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProfessionalDto>> VerifyAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ProfessionalDto>> SuspendAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result> AddAvailabilityAsync(string professionalId, UpdateProfessionalAvailabilityDto dto, CancellationToken cancellationToken = default);
    Task<Result> RemoveAvailabilityAsync(string professionalId, string availabilityId, CancellationToken cancellationToken = default);
    Task<Result<ProfessionalDto>> ReviewDocumentAsync(string professionalId, string documentId, ReviewProfessionalDocumentDto dto, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfessionalPerformanceDto>>> GetPerformanceAsync(string professionalId, CancellationToken cancellationToken = default);
}

public interface IPriceQuoteService
{
    Task<Result<PriceQuoteDto>> CreateQuoteAsync(CreatePriceQuoteDto dto, CancellationToken cancellationToken = default);
    Task<Result<PriceQuoteDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PriceQuoteDto>> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PriceQuoteDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<PriceQuoteDto>> ApproveAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PriceQuoteDto>> DeclineAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<QuoteRevisionDto>>> GetRevisionsAsync(string priceQuoteId, CancellationToken cancellationToken = default);
}

public interface IBookingService
{
    Task<Result<BookingDto>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);
    Task<Result<BookingDetailDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> GetByBookingNumberAsync(string bookingNumber, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingDto>>> GetUpcomingForProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingDto>>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> CancelAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> RescheduleAsync(string id, RescheduleBookingDto dto, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> AssignAsync(string id, string professionalId, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> AcceptAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> DeclineAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> StartAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> MarkNoShowAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> RequestAdditionalWorkAsync(string id, ApproveAdditionalWorkDto dto, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> ApproveAdditionalWorkAsync(string id, bool approved, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingStatusHistoryDto>>> GetStatusHistoryAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<BookingDto>> RebookAsync(string id, CancellationToken cancellationToken = default);
}

public interface IAssignmentService
{
    Task<Result<BookingAssignmentDto>> AssignAsync(string bookingId, string professionalId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<BookingAssignmentDto>>> GetAssignmentsAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfessionalDto>>> GetEligibleProfessionalsAsync(string bookingId, CancellationToken cancellationToken = default);
}

public interface IPaymentService
{
    Task<Result<PaymentInitiationResponseDto>> CreateOrderAsync(CreatePaymentDto dto, CancellationToken cancellationToken = default);
    Task<Result<PaymentDto>> VerifyAndConfirmAsync(RazorpayPaymentCaptureDto capture, CancellationToken cancellationToken = default);
    Task<Result<PaymentDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PaymentDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<RefundDto>> RefundAsync(string bookingId, string reason, decimal? amount, CancellationToken cancellationToken = default);
    Task<Result> HandleWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default);
    Task<Result<WalletDto>> GetWalletAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<CreditTransactionDto>> AddCreditsAsync(string customerId, decimal amount, string reason, CancellationToken cancellationToken = default);
}

public interface IEarningsService
{
    Task<Result<ProfessionalEarningDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfessionalEarningDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<EarningsSummaryDto>> GetSummaryAsync(string professionalId, CancellationToken cancellationToken = default);
}

public interface IPayoutService
{
    Task<Result<IReadOnlyList<PayoutDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PayoutDto>>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Result<PayoutDto>> MarkProcessingAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PayoutDto>> MarkPaidAsync(string id, string? reference, CancellationToken cancellationToken = default);
    Task<Result<PayoutDto>> MarkFailedAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<PayoutSummaryDto>> GetPayoutStatusAsync(string professionalId, CancellationToken cancellationToken = default);
}

public interface IAnalyticsService
{
    Task<Result<AnalyticsSummaryDto>> GetSummaryAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TrendPointDto>>> GetBookingsTrendAsync(int days, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TrendPointDto>>> GetRevenueTrendAsync(int days, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TopItemDto>>> GetTopCategoriesAsync(int limit, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TopItemDto>>> GetTopServicesAsync(int limit, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<TopItemDto>>> GetTopCitiesAsync(int limit, CancellationToken cancellationToken = default);
    Task<Result<AssignmentSuccessDto>> GetAssignmentSuccessAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CancellationReasonDto>>> GetCancellationReasonsAsync(CancellationToken cancellationToken = default);
    Task<Result<RepeatRateDto>> GetCustomerRepeatRateAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProviderPerformanceItemDto>>> GetProviderPerformanceAsync(int limit, CancellationToken cancellationToken = default);
    Task<Result<RefundDisputeDto>> GetRefundDisputeRateAsync(CancellationToken cancellationToken = default);
}

public interface IReviewService
{
    Task<Result<ReviewDto>> SubmitAsync(CreateReviewDto dto, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ReviewDto>>> GetByProfessionalAsync(string professionalId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ReviewDto>>> GetByServiceAsync(string serviceId, CancellationToken cancellationToken = default);
    Task<Result<ReviewDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default);
    Task<Result<ReviewDto>> ReplyAsync(string reviewId, string reply, CancellationToken cancellationToken = default);
}

public interface ISupportService
{
    Task<Result<SupportTicketDto>> CreateTicketAsync(CreateSupportTicketDto dto, CancellationToken cancellationToken = default);
    Task<Result<SupportTicketDto>> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<SupportTicketDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<SupportTicketDto>> UpdateStatusAsync(string ticketNumber, string status, string? note, CancellationToken cancellationToken = default);
    Task<Result<DisputeDto>> OpenDisputeAsync(CreateDisputeDto dto, CancellationToken cancellationToken = default);
    Task<Result<DisputeDto>> ResolveDisputeAsync(string disputeId, string resolution, string resolutionNote, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<NotificationDto>>> GetNotificationsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result<int>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> MarkNotificationReadAsync(string notificationId, CancellationToken cancellationToken = default);
}

public interface IHomeServicesService : IServiceCatalogService, ILocationService, IProfessionalService,
    IPriceQuoteService, IBookingService, IAssignmentService, IPaymentService, IEarningsService,
    IPayoutService, IAnalyticsService, IReviewService, ISupportService
{
}

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
    Task<Result<UserDto>> GetMeAsync(string userId, CancellationToken cancellationToken = default);
}