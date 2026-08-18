using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class PaymentService : IPaymentService
{
    private const string TestWebhookSecret = "rzp_test_secret";
    private const string TestKeyId = "rzp_test_key";

    private readonly IPaymentRepository _paymentRepository;
    private readonly IRefundRepository _refundRepository;
    private readonly ICreditTransactionRepository _creditTransactionRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPriceQuoteRepository _priceQuoteRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        IRefundRepository refundRepository,
        ICreditTransactionRepository creditTransactionRepository,
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IPriceQuoteRepository priceQuoteRepository)
    {
        _paymentRepository = paymentRepository;
        _refundRepository = refundRepository;
        _creditTransactionRepository = creditTransactionRepository;
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _priceQuoteRepository = priceQuoteRepository;
    }

    public async Task<Result<PaymentInitiationResponseDto>> CreateOrderAsync(CreatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(dto.BookingId, cancellationToken);
        if (booking == null)
            return Result<PaymentInitiationResponseDto>.Failure("Booking not found");

        if (booking.PaymentStatus == "paid")
            return Result<PaymentInitiationResponseDto>.Failure("Booking is already paid");

        var quote = await _priceQuoteRepository.GetActiveForBookingAsync(booking.Id, cancellationToken);
        var amount = quote?.GrandTotal ?? 0m;

        var payment = new Payment
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            BookingId = booking.Id,
            PaymentNumber = GeneratePaymentNumber(),
            Amount = amount,
            Method = dto.Method,
            Status = "pending",
            GatewayProvider = "razorpay",
            GatewayOrderId = "order_" + Guid.NewGuid().ToString("N")[..14],
            WebhookVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment, cancellationToken);

        var customer = await _customerRepository.GetByIdAsync(booking.CustomerId, cancellationToken);

        return Result<PaymentInitiationResponseDto>.Success(new PaymentInitiationResponseDto
        {
            PaymentId = payment.Id,
            BookingId = booking.Id,
            Amount = payment.Amount,
            Currency = "INR",
            GatewayProvider = "razorpay",
            GatewayOrderId = payment.GatewayOrderId ?? string.Empty,
            GatewayKeyId = TestKeyId,
            CustomerName = customer?.DisplayName,
            CustomerEmail = customer?.Email,
            CustomerPhone = customer?.Phone
        });
    }

    public async Task<Result<PaymentDto>> VerifyAndConfirmAsync(RazorpayPaymentCaptureDto capture, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByGatewayOrderIdAsync(capture.RazorpayOrderId, cancellationToken);
        if (payment == null)
            return Result<PaymentDto>.Failure("Payment order not found");

        var expectedSignature = ComputeHmacSha256($"{capture.RazorpayOrderId}|{capture.RazorpayPaymentId}", TestWebhookSecret);
        if (!string.Equals(expectedSignature, capture.RazorpaySignature, StringComparison.OrdinalIgnoreCase))
            return Result<PaymentDto>.Failure("Payment signature verification failed");

        payment.Status = "paid";
        payment.PaidAt = DateTime.UtcNow;
        payment.WebhookVerified = true;
        payment.GatewayRef = capture.RazorpayPaymentId;
        payment.GatewayPaymentId = capture.RazorpayPaymentId;
        payment.GatewaySignature = capture.RazorpaySignature;
        payment.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        var booking = await _bookingRepository.GetByIdAsync(payment.BookingId, cancellationToken);
        if (booking != null && booking.PaymentStatus != "paid")
        {
            booking.PaymentStatus = "paid";
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    public async Task<Result<PaymentDto>> GetByBookingAsync(string bookingId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByBookingAsync(bookingId, cancellationToken);
        if (payment == null)
            return Result<PaymentDto>.Failure("Payment not found");

        return Result<PaymentDto>.Success(MapToDto(payment));
    }

    public async Task<Result<IReadOnlyList<PaymentDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetByCustomerAsync(customerId, cancellationToken);
        var dtos = payments.Select(MapToDto).ToList();
        return Result<IReadOnlyList<PaymentDto>>.Success(dtos);
    }

    public async Task<Result<RefundDto>> RefundAsync(string bookingId, string reason, decimal? amount, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByBookingAsync(bookingId, cancellationToken);
        if (payment == null)
            return Result<RefundDto>.Failure("Payment not found");

        if (payment.Status != "paid")
            return Result<RefundDto>.Failure("Payment not refundable");

        var refund = new Refund
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            PaymentId = payment.Id,
            BookingId = bookingId,
            Amount = amount ?? payment.Amount,
            Reason = reason,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _refundRepository.AddAsync(refund, cancellationToken);

        payment.Status = "refunded";
        payment.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(payment, cancellationToken);

        var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
        if (booking != null && booking.PaymentStatus != "refunded")
        {
            booking.PaymentStatus = "refunded";
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }

        return Result<RefundDto>.Success(new RefundDto
        {
            Id = refund.Id,
            PaymentId = refund.PaymentId,
            BookingId = refund.BookingId,
            Amount = refund.Amount,
            Reason = refund.Reason,
            Status = refund.Status
        });
    }

    public async Task<Result> HandleWebhookAsync(string payload, string signatureHeader, CancellationToken cancellationToken = default)
    {
        var expectedSignature = ComputeHmacSha256(payload, TestWebhookSecret);
        if (!string.Equals(expectedSignature, signatureHeader, StringComparison.OrdinalIgnoreCase))
            return Result.Failure("Invalid webhook signature");

        using var document = JsonDocument.Parse(payload);
        var root = document.RootElement;

        var eventType = GetString(root, "event") ?? string.Empty;

        var payloadElement = root.ValueKind == JsonValueKind.Object && root.TryGetProperty("payload", out var p)
            ? p
            : default;

        var orderId = string.Empty;
        var paymentId = string.Empty;

        if (payloadElement.ValueKind == JsonValueKind.Object)
        {
            if (payloadElement.TryGetProperty("order", out var order) &&
                order.ValueKind == JsonValueKind.Object &&
                order.TryGetProperty("entity", out var orderEntity))
            {
                orderId = GetString(orderEntity, "id") ?? string.Empty;
            }

            if (payloadElement.TryGetProperty("payment", out var paymentElement) &&
                paymentElement.ValueKind == JsonValueKind.Object &&
                paymentElement.TryGetProperty("entity", out var paymentEntity))
            {
                paymentId = GetString(paymentEntity, "id") ?? string.Empty;
                if (string.IsNullOrEmpty(orderId))
                    orderId = GetString(paymentEntity, "order_id") ?? string.Empty;
            }

            if (string.IsNullOrEmpty(orderId) && string.IsNullOrEmpty(paymentId) &&
                payloadElement.TryGetProperty("refund", out var refund) &&
                refund.ValueKind == JsonValueKind.Object &&
                refund.TryGetProperty("entity", out var refundEntity))
            {
                paymentId = GetString(refundEntity, "payment_id") ?? string.Empty;
            }
        }

        var payment = !string.IsNullOrEmpty(orderId)
            ? await _paymentRepository.GetByGatewayOrderIdAsync(orderId, cancellationToken)
            : null;

        if (payment == null && !string.IsNullOrEmpty(paymentId))
        {
            var candidates = await _paymentRepository.FindAsync(
                p => p.GatewayPaymentId == paymentId || p.GatewayRef == paymentId, cancellationToken);
            payment = candidates.FirstOrDefault();
        }

        if (payment == null)
            return Result.Success();

        switch (eventType)
        {
            case "payment.captured":
            case "payment.paid":
            {
                if (payment.Status != "paid")
                {
                    payment.Status = "paid";
                    payment.PaidAt = DateTime.UtcNow;
                    payment.WebhookVerified = true;
                    if (!string.IsNullOrEmpty(paymentId))
                    {
                        payment.GatewayRef = paymentId;
                        payment.GatewayPaymentId = paymentId;
                    }
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _paymentRepository.UpdateAsync(payment, cancellationToken);

                    var booking = await _bookingRepository.GetByIdAsync(payment.BookingId, cancellationToken);
                    if (booking != null && booking.PaymentStatus != "paid")
                    {
                        booking.PaymentStatus = "paid";
                        booking.UpdatedAt = DateTime.UtcNow;
                        await _bookingRepository.UpdateAsync(booking, cancellationToken);
                    }
                }
                break;
            }
            case "refund.processed":
            {
                if (payment.Status != "refunded")
                {
                    payment.Status = "refunded";
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _paymentRepository.UpdateAsync(payment, cancellationToken);

                    var booking = await _bookingRepository.GetByIdAsync(payment.BookingId, cancellationToken);
                    if (booking != null && booking.PaymentStatus != "refunded")
                    {
                        booking.PaymentStatus = "refunded";
                        booking.UpdatedAt = DateTime.UtcNow;
                        await _bookingRepository.UpdateAsync(booking, cancellationToken);
                    }
                }
                break;
            }
        }

        return Result.Success();
    }

    public async Task<Result<WalletDto>> GetWalletAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var balance = await _creditTransactionRepository.GetWalletBalanceAsync(customerId, cancellationToken);
        var transactions = await _creditTransactionRepository.GetByCustomerAsync(customerId, cancellationToken);

        var dtos = transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new CreditTransactionDto
            {
                Id = t.Id,
                CustomerId = t.CustomerId,
                Amount = t.Amount,
                Type = t.Type,
                Reason = t.Reason,
                ReferenceBookingId = t.ReferenceBookingId,
                BalanceAfter = t.BalanceAfter
            })
            .ToList();

        return Result<WalletDto>.Success(new WalletDto
        {
            CustomerId = customerId,
            Balance = balance,
            Transactions = dtos
        });
    }

    public async Task<Result<CreditTransactionDto>> AddCreditsAsync(string customerId, decimal amount, string reason, CancellationToken cancellationToken = default)
    {
        var priorBalance = await _creditTransactionRepository.GetWalletBalanceAsync(customerId, cancellationToken);

        var transaction = new CreditTransaction
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            CustomerId = customerId,
            Amount = amount,
            Type = "credit",
            Reason = reason,
            BalanceAfter = priorBalance + amount,
            CreatedAt = DateTime.UtcNow
        };

        await _creditTransactionRepository.AddAsync(transaction, cancellationToken);

        return Result<CreditTransactionDto>.Success(new CreditTransactionDto
        {
            Id = transaction.Id,
            CustomerId = transaction.CustomerId,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Reason = transaction.Reason,
            BalanceAfter = transaction.BalanceAfter
        });
    }

    private static PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            BookingId = payment.BookingId,
            PaymentNumber = payment.PaymentNumber,
            Amount = payment.Amount,
            Method = payment.Method,
            Status = payment.Status,
            GatewayRef = payment.GatewayRef,
            PaidAt = payment.PaidAt,
            GatewayProvider = payment.GatewayProvider,
            GatewayOrderId = payment.GatewayOrderId,
            WebhookVerified = payment.WebhookVerified
        };
    }

    private static string GeneratePaymentNumber()
    {
        return $"PAY-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
    }

    private static string ComputeHmacSha256(string data, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(propertyName, out var property) &&
            property.ValueKind == JsonValueKind.String)
        {
            return property.GetString();
        }
        return null;
    }
}