using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class PriceRule : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? PackageId { get; set; }

    [MaxLength(50)]
    public string? CityId { get; set; }

    [Required]
    [MaxLength(20)]
    public string RuleType { get; set; } = "discount"; // surge/discount/seasonal

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; } = 0;

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}

public class PriceQuote : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string QuoteNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? AddressId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal AddOnsTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaterialsTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal FeesTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TravelCharge { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal UrgentCharge { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PlatformFee { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal GrandTotal { get; set; } = 0;

    [MaxLength(50)]
    public string? CouponId { get; set; }

    [MaxLength(200)]
    public string? CouponCode { get; set; }

    public DateTime ExpiresAt { get; set; }
    public int Version { get; set; } = 1;

    [MaxLength(30)]
    public string Status { get; set; } = "active"; // active/expired/accepted/rejected

    [MaxLength(2000)]
    public string? Notes { get; set; }

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;

    [ForeignKey(nameof(PackageId))]
    public virtual ServicePackage Package { get; set; } = null!;

    public virtual ICollection<QuoteRevision> Revisions { get; set; } = new List<QuoteRevision>();
}

public class QuoteRevision : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PriceQuoteId { get; set; } = string.Empty;

    public int RevisionNumber { get; set; } = 1;

    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PreviousTotal { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal NewTotal { get; set; } = 0;

    [MaxLength(50)]
    public string? CreatedBy { get; set; }

    // Navigation
    [ForeignKey(nameof(PriceQuoteId))]
    public virtual PriceQuote PriceQuote { get; set; } = null!;
}
