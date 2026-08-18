using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupons");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Code).HasMaxLength(100).IsRequired();
        builder.Property(c => c.DiscountType).HasMaxLength(15).HasDefaultValue("flat");
        builder.Property(c => c.Value).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.MaxDiscount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.MinOrderValue).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.UsageLimit).HasDefaultValue(0);
        builder.Property(c => c.PerCustomerLimit).HasDefaultValue(1);
        builder.Property(c => c.TargetType).HasMaxLength(50);
        builder.Property(c => c.TargetValue).HasMaxLength(50);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => c.TargetType);
    }
}

public class CouponRedemptionConfiguration : IEntityTypeConfiguration<CouponRedemption>
{
    public void Configure(EntityTypeBuilder<CouponRedemption> builder)
    {
        builder.ToTable("coupon_redemptions");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.CouponId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.DiscountApplied).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => new { r.CouponId, r.CustomerId, r.BookingId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.BookingId);
    }
}

public class ReferralConfiguration : IEntityTypeConfiguration<Referral>
{
    public void Configure(EntityTypeBuilder<Referral> builder)
    {
        builder.ToTable("referrals");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.ReferrerCustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.RefereeCustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.RewardAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.ReferrerCustomerId);
        builder.HasIndex(r => r.RefereeCustomerId).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.Status);
    }
}

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.ToTable("membership_plans");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.DurationDays).HasDefaultValue(365);
        builder.Property(p => p.BenefitsJson).HasMaxLength(3000).HasDefaultValue("[]");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Name).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.IsActive);
    }
}

public class CustomerMembershipConfiguration : IEntityTypeConfiguration<CustomerMembership>
{
    public void Configure(EntityTypeBuilder<CustomerMembership> builder)
    {
        builder.ToTable("customer_memberships");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.PlanId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.StartedAt).IsRequired();
        builder.Property(m => m.ExpiresAt).IsRequired();
        builder.Property(m => m.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => new { m.CustomerId, m.Status });
        builder.HasIndex(m => m.ExpiresAt);
    }
}

public class HomeServiceReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("home_service_reviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Rating).HasDefaultValue(5);
        builder.Property(r => r.Comment).HasMaxLength(3000);
        builder.Property(r => r.TagsJson).HasMaxLength(1000).HasDefaultValue("[]");
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.BookingId).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.ProfessionalId);
        builder.HasIndex(r => r.Rating);
    }
}

public class ReviewMediaConfiguration : IEntityTypeConfiguration<ReviewMedia>
{
    public void Configure(EntityTypeBuilder<ReviewMedia> builder)
    {
        builder.ToTable("review_media");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.ReviewId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.MediaUrl).HasMaxLength(1000);
        builder.Property(m => m.MediaType).HasMaxLength(10).HasDefaultValue("image");
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => m.ReviewId);
    }
}
