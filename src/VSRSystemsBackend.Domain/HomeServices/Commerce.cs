using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class Coupon : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(15)]
    public string DiscountType { get; set; } = "flat"; // flat/percent

    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaxDiscount { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinOrderValue { get; set; } = 0;

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public int UsageLimit { get; set; } = 0;
    public int PerCustomerLimit { get; set; } = 1;

    [MaxLength(50)]
    public string? TargetType { get; set; } // first_booking/category/service/city/membership/referral/generic

    [MaxLength(50)]
    public string? TargetValue { get; set; }

    public bool IsActive { get; set; } = true;
}

public class CouponRedemption : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CouponId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountApplied { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(CouponId))]
    public virtual Coupon Coupon { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;
}

public class Referral : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ReferrerCustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RefereeCustomerId { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal RewardAmount { get; set; } = 0;

    [MaxLength(20)]
    public string Status { get; set; } = "pending"; // pending/rewarded

    // Navigation
    [ForeignKey(nameof(ReferrerCustomerId))]
    public virtual Customer ReferrerCustomer { get; set; } = null!;

    [ForeignKey(nameof(RefereeCustomerId))]
    public virtual Customer RefereeCustomer { get; set; } = null!;
}

public class MembershipPlan : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    public int DurationDays { get; set; } = 365;

    [MaxLength(3000)]
    public string BenefitsJson { get; set; } = "[]";

    public bool IsActive { get; set; } = true;
}

public class CustomerMembership : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PlanId { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active"; // active/expired/cancelled

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(PlanId))]
    public virtual MembershipPlan Plan { get; set; } = null!;
}

public class Review : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public int Rating { get; set; } = 5; // 1-5

    [MaxLength(3000)]
    public string Comment { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string TagsJson { get; set; } = "[]";

    // Rating dimensions (§76)
    public int? Quality { get; set; }
    public int? Professionalism { get; set; }
    public int? Punctuality { get; set; }
    public int? Cleanliness { get; set; }
    public int? Communication { get; set; }
    public int? Value { get; set; }

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;

    public virtual ICollection<ReviewMedia> Media { get; set; } = new List<ReviewMedia>();
}

public class ReviewMedia : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ReviewId { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string MediaUrl { get; set; } = string.Empty;

    [MaxLength(10)]
    public string MediaType { get; set; } = "image"; // image/video

    // Navigation
    [ForeignKey(nameof(ReviewId))]
    public virtual Review Review { get; set; } = null!;
}
