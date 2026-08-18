using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class PriceQuoteService : IPriceQuoteService
{
    private const decimal PlatformFeeRate = 0.10m;
    private const decimal PlatformFeeCap = 1000m;
    private const decimal TaxRate = 0.18m;

    private readonly IPriceQuoteRepository _quoteRepository;
    private readonly IServiceCatalogRepository _catalogRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ICustomerRepository _customerRepository;

    public PriceQuoteService(
        IPriceQuoteRepository quoteRepository,
        IServiceCatalogRepository catalogRepository,
        ICouponRepository couponRepository,
        ICustomerRepository customerRepository)
    {
        _quoteRepository = quoteRepository;
        _catalogRepository = catalogRepository;
        _couponRepository = couponRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result<PriceQuoteDto>> CreateQuoteAsync(CreatePriceQuoteDto dto, CancellationToken cancellationToken = default)
    {
        var package = (await _catalogRepository.GetPackagesByServiceAsync(dto.ServiceId, cancellationToken))
            .FirstOrDefault(p => p.Id == dto.PackageId);
        if (package == null)
            return Result<PriceQuoteDto>.Failure("Package not found");

        var customer = await ResolveCustomerByAddressAsync(dto.AddressId, cancellationToken);
        if (customer == null)
            return Result<PriceQuoteDto>.Failure("Customer not found for the given address");

        var addOns = (await _catalogRepository.GetAddOnsByServiceAsync(dto.ServiceId, cancellationToken))
            .Where(a => dto.AddOnIds.Contains(a.Id))
            .ToList();

        var basePrice = package.BasePrice;
        var addOnsTotal = addOns.Sum(a => a.Price);
        var subtotal = basePrice + addOnsTotal;

        var discountTotal = 0m;
        string? couponId = null;
        string? couponCode = null;
        if (!string.IsNullOrWhiteSpace(dto.CouponCode))
        {
            var coupon = await _couponRepository.GetByCodeAsync(dto.CouponCode.Trim(), cancellationToken);
            if (coupon == null)
                return Result<PriceQuoteDto>.Failure("Coupon not found");

            if (!coupon.IsActive)
                return Result<PriceQuoteDto>.Failure("Coupon is not active");

            var now = DateTime.UtcNow;
            if (coupon.ValidFrom.HasValue && now < coupon.ValidFrom.Value)
                return Result<PriceQuoteDto>.Failure("Coupon is not yet valid");

            if (coupon.ValidTo.HasValue && now > coupon.ValidTo.Value)
                return Result<PriceQuoteDto>.Failure("Coupon has expired");

            if (coupon.UsageLimit > 0 &&
                await _couponRepository.GetRedemptionCountAsync(coupon.Id, cancellationToken) >= coupon.UsageLimit)
                return Result<PriceQuoteDto>.Failure("Coupon usage limit reached");

            if (coupon.PerCustomerLimit > 0 &&
                await _couponRepository.GetCustomerRedemptionCountAsync(coupon.Id, customer.Id, cancellationToken) >= coupon.PerCustomerLimit)
                return Result<PriceQuoteDto>.Failure("Coupon already used by this customer");

            if (coupon.MinOrderValue > 0 && subtotal < coupon.MinOrderValue)
                return Result<PriceQuoteDto>.Failure("Minimum order value not met for this coupon");

            discountTotal = coupon.DiscountType == "percent"
                ? subtotal * coupon.Value / 100m
                : coupon.Value;

            if (coupon.MaxDiscount > 0)
                discountTotal = Math.Min(discountTotal, coupon.MaxDiscount);

            discountTotal = Math.Min(discountTotal, subtotal);
            couponId = coupon.Id;
            couponCode = coupon.Code;
        }

        var discountedSubtotal = subtotal - discountTotal;
        var platformFee = Math.Min(subtotal * PlatformFeeRate, PlatformFeeCap);
        var taxTotal = discountedSubtotal * TaxRate;
        var grandTotal = discountedSubtotal + platformFee + taxTotal;

        var quote = new PriceQuote
        {
            Id = NewId(),
            QuoteNumber = GenerateQuoteNumber(),
            CustomerId = customer.Id,
            ServiceId = dto.ServiceId,
            PackageId = dto.PackageId,
            AddressId = dto.AddressId,
            BasePrice = Math.Round(basePrice, 2),
            AddOnsTotal = Math.Round(addOnsTotal, 2),
            PlatformFee = Math.Round(platformFee, 2),
            DiscountTotal = Math.Round(discountTotal, 2),
            TaxTotal = Math.Round(taxTotal, 2),
            GrandTotal = Math.Round(grandTotal, 2),
            CouponId = couponId,
            CouponCode = couponCode,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Version = 1,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await _quoteRepository.AddAsync(quote, cancellationToken);

        var dtoResult = ToPriceQuoteDto(quote);
        dtoResult.LineItems = BuildLineItems(quote, package, addOns);
        return Result<PriceQuoteDto>.Success(dtoResult);
    }

    public async Task<Result<PriceQuoteDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var quote = await _quoteRepository.GetByIdAsync(id, cancellationToken);
        if (quote == null)
            return Result<PriceQuoteDto>.Failure("Price quote not found");

        return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));
    }

    public async Task<Result<PriceQuoteDto>> GetByQuoteNumberAsync(string quoteNumber, CancellationToken cancellationToken = default)
    {
        var quote = await _quoteRepository.GetByQuoteNumberAsync(quoteNumber, cancellationToken);
        if (quote == null)
            return Result<PriceQuoteDto>.Failure("Price quote not found");

        return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));
    }

    public async Task<Result<IReadOnlyList<PriceQuoteDto>>> GetByCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var quotes = await _quoteRepository.GetByCustomerAsync(customerId, cancellationToken);
        return Result<IReadOnlyList<PriceQuoteDto>>.Success(quotes.Select(ToPriceQuoteDto).ToList());
    }

    public async Task<Result<PriceQuoteDto>> ApproveAsync(string id, CancellationToken cancellationToken = default)
    {
        var quote = await _quoteRepository.GetByIdAsync(id, cancellationToken);
        if (quote == null)
            return Result<PriceQuoteDto>.Failure("Price quote not found");

        if (quote.Status == "approved")
            return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));

        if (quote.Status != "pending")
            return Result<PriceQuoteDto>.Failure($"Price quote cannot be approved in status {quote.Status}");

        quote.Status = "approved";
        quote.UpdatedAt = DateTime.UtcNow;
        await _quoteRepository.UpdateAsync(quote, cancellationToken);

        return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));
    }

    public async Task<Result<PriceQuoteDto>> DeclineAsync(string id, string reason, CancellationToken cancellationToken = default)
    {
        var quote = await _quoteRepository.GetByIdAsync(id, cancellationToken);
        if (quote == null)
            return Result<PriceQuoteDto>.Failure("Price quote not found");

        if (quote.Status == "declined")
            return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));

        if (quote.Status != "pending")
            return Result<PriceQuoteDto>.Failure($"Price quote cannot be declined in status {quote.Status}");

        quote.Status = "declined";
        quote.Notes = reason;
        quote.UpdatedAt = DateTime.UtcNow;
        await _quoteRepository.UpdateAsync(quote, cancellationToken);

        return Result<PriceQuoteDto>.Success(ToPriceQuoteDto(quote));
    }

    public async Task<Result<IReadOnlyList<QuoteRevisionDto>>> GetRevisionsAsync(string priceQuoteId, CancellationToken cancellationToken = default)
    {
        var revisions = await _quoteRepository.GetRevisionsAsync(priceQuoteId, cancellationToken);
        var dtos = revisions
            .OrderByDescending(r => r.RevisionNumber)
            .Select(r => new QuoteRevisionDto
            {
                Id = r.Id,
                PriceQuoteId = r.PriceQuoteId,
                RevisionNumber = r.RevisionNumber,
                Reason = r.Reason,
                PreviousTotal = r.PreviousTotal,
                NewTotal = r.NewTotal,
                CreatedBy = r.CreatedBy
            })
            .ToList();

        return Result<IReadOnlyList<QuoteRevisionDto>>.Success(dtos);
    }

    private async Task<Customer?> ResolveCustomerByAddressAsync(string addressId, CancellationToken cancellationToken)
    {
        var customers = await _customerRepository.FindAsync(c => c.Addresses.Any(a => a.Id == addressId), cancellationToken);
        return customers.FirstOrDefault();
    }

    private static PriceQuoteDto ToPriceQuoteDto(PriceQuote q) => new()
    {
        Id = q.Id,
        QuoteNumber = q.QuoteNumber,
        CustomerId = q.CustomerId,
        ServiceId = q.ServiceId,
        ServiceName = q.Service?.Name ?? string.Empty,
        PackageId = q.PackageId,
        PackageName = q.Package?.Name ?? string.Empty,
        AddressId = q.AddressId,
        BasePrice = q.BasePrice,
        AddOnsTotal = q.AddOnsTotal,
        MaterialsTotal = q.MaterialsTotal,
        FeesTotal = q.FeesTotal,
        TravelCharge = q.TravelCharge,
        UrgentCharge = q.UrgentCharge,
        PlatformFee = q.PlatformFee,
        DiscountTotal = q.DiscountTotal,
        TaxTotal = q.TaxTotal,
        GrandTotal = q.GrandTotal,
        CouponCode = q.CouponCode,
        ExpiresAt = q.ExpiresAt,
        Version = q.Version,
        Status = q.Status,
        LineItems = ToLineItems(q)
    };

    private static List<QuoteLineItemDto> ToLineItems(PriceQuote q)
    {
        var items = new List<QuoteLineItemDto>();
        if (q.BasePrice > 0)
            items.Add(new QuoteLineItemDto { Description = "Base price", Amount = q.BasePrice, Type = "base" });
        if (q.AddOnsTotal > 0)
            items.Add(new QuoteLineItemDto { Description = "Add-ons", Amount = q.AddOnsTotal, Type = "addon" });
        if (q.PlatformFee > 0)
            items.Add(new QuoteLineItemDto { Description = "Platform fee", Amount = q.PlatformFee, Type = "fee" });
        if (q.DiscountTotal > 0)
            items.Add(new QuoteLineItemDto { Description = "Discount", Amount = -q.DiscountTotal, Type = "discount" });
        if (q.TaxTotal > 0)
            items.Add(new QuoteLineItemDto { Description = "Tax", Amount = q.TaxTotal, Type = "tax" });
        return items;
    }

    private static List<QuoteLineItemDto> BuildLineItems(PriceQuote q, ServicePackage package, IReadOnlyList<ServiceAddOn> addOns)
    {
        var items = new List<QuoteLineItemDto>
        {
            new() { Description = package.Name, Amount = q.BasePrice, Type = "base" }
        };

        foreach (var addOn in addOns)
            items.Add(new QuoteLineItemDto { Description = addOn.Name, Amount = addOn.Price, Type = "addon" });

        if (q.PlatformFee > 0)
            items.Add(new QuoteLineItemDto { Description = "Platform fee", Amount = q.PlatformFee, Type = "fee" });
        if (q.DiscountTotal > 0)
            items.Add(new QuoteLineItemDto { Description = "Discount", Amount = -q.DiscountTotal, Type = "discount" });
        if (q.TaxTotal > 0)
            items.Add(new QuoteLineItemDto { Description = "GST (18%)", Amount = q.TaxTotal, Type = "tax" });

        return items;
    }

    private static string NewId() => Guid.NewGuid().ToString("N")[..20];

    private static string GenerateQuoteNumber()
        => $"HSQ-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(0, 10000):D4}";
}