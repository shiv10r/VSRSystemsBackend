using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PaymentNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Method).HasMaxLength(20).HasDefaultValue("upi");
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("initiated");
        builder.Property(p => p.GatewayRef).HasMaxLength(200);
        builder.Property(p => p.GatewayProvider).HasMaxLength(20);
        builder.Property(p => p.GatewayOrderId).HasMaxLength(200);
        builder.Property(p => p.GatewayPaymentId).HasMaxLength(200);
        builder.Property(p => p.GatewaySignature).HasMaxLength(500);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PaymentNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.GatewayOrderId);
        builder.HasIndex(p => p.GatewayPaymentId);
    }
}

public class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("refunds");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.PaymentId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.Reason).HasMaxLength(1000);
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("requested");
        builder.Property(r => r.ProcessedBy).HasMaxLength(50);
        builder.Property(r => r.GatewayRefundId).HasMaxLength(200);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.PaymentId);
        builder.HasIndex(r => r.BookingId);
        builder.HasIndex(r => r.Status);
    }
}

public class CreditTransactionConfiguration : IEntityTypeConfiguration<CreditTransaction>
{
    public void Configure(EntityTypeBuilder<CreditTransaction> builder)
    {
        builder.ToTable("credit_transactions");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.Type).HasMaxLength(10).HasDefaultValue("credit");
        builder.Property(c => c.Reason).HasMaxLength(500);
        builder.Property(c => c.ReferenceBookingId).HasMaxLength(50);
        builder.Property(c => c.BalanceAfter).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.ReferenceBookingId);
    }
}

public class CommissionRuleConfiguration : IEntityTypeConfiguration<CommissionRule>
{
    public void Configure(EntityTypeBuilder<CommissionRule> builder)
    {
        builder.ToTable("commission_rules");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.CategoryId).HasMaxLength(50);
        builder.Property(r => r.ServiceId).HasMaxLength(50);
        builder.Property(r => r.CityId).HasMaxLength(50);
        builder.Property(r => r.ProfessionalTier).HasMaxLength(20);
        builder.Property(r => r.RatePercent).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.FlatFee).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.CategoryId);
        builder.HasIndex(r => r.ServiceId);
        builder.HasIndex(r => r.IsActive);
    }
}

public class ProfessionalEarningConfiguration : IEntityTypeConfiguration<ProfessionalEarning>
{
    public void Configure(EntityTypeBuilder<ProfessionalEarning> builder)
    {
        builder.ToTable("professional_earnings");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(e => e.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(e => e.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(e => e.GrossAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.MaterialsExcludedAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.CommissionAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.AdjustmentAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.TaxWithheldAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.NetAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(e => new { e.ProfessionalId, e.BookingId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(e => e.BookingId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.SettledAt);
    }
}

public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
{
    public void Configure(EntityTypeBuilder<Payout> builder)
    {
        builder.ToTable("payouts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.PeriodStart).IsRequired();
        builder.Property(p => p.PeriodEnd).IsRequired();
        builder.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(p => p.FailureReason).HasMaxLength(1000);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.ProfessionalId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.PeriodStart);
    }
}

public class ProfessionalAdjustmentConfiguration : IEntityTypeConfiguration<ProfessionalAdjustment>
{
    public void Configure(EntityTypeBuilder<ProfessionalAdjustment> builder)
    {
        builder.ToTable("professional_adjustments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.BookingId).HasMaxLength(50);
        builder.Property(a => a.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(a => a.Reason).HasMaxLength(1000);
        builder.Property(a => a.CreatedBy).HasMaxLength(50);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.ProfessionalId);
        builder.HasIndex(a => a.BookingId);
    }
}

public class ProfessionalIncentiveConfiguration : IEntityTypeConfiguration<ProfessionalIncentive>
{
    public void Configure(EntityTypeBuilder<ProfessionalIncentive> builder)
    {
        builder.ToTable("professional_incentives");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.IncentiveType).HasMaxLength(50).IsRequired();
        builder.Property(i => i.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.PeriodStart).IsRequired();
        builder.Property(i => i.PeriodEnd).IsRequired();
        builder.Property(i => i.Status).HasMaxLength(20).HasDefaultValue("accrued");
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(i => i.UpdatedAt);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(i => i.ProfessionalId);
        builder.HasIndex(i => i.Status);
    }
}

public class PaymentGatewayWebhookEventConfiguration : IEntityTypeConfiguration<PaymentGatewayWebhookEvent>
{
    public void Configure(EntityTypeBuilder<PaymentGatewayWebhookEvent> builder)
    {
        builder.ToTable("payment_gateway_webhook_events");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(e => e.Provider).HasMaxLength(20).HasDefaultValue("razorpay");
        builder.Property(e => e.EventType).HasMaxLength(100).IsRequired();
        builder.Property(e => e.PayloadJson).HasMaxLength(5000).HasDefaultValue("{}");
        builder.Property(e => e.Processed).HasDefaultValue(false);
        builder.Property(e => e.BookingId).HasMaxLength(50);
        builder.Property(e => e.ProcessingError).HasMaxLength(1000);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(e => new { e.Provider, e.EventType });
        builder.HasIndex(e => e.Processed);
        builder.HasIndex(e => e.BookingId);
        builder.HasIndex(e => e.SignatureValid);
    }
}

public class PaymentGatewaySettingConfiguration : IEntityTypeConfiguration<PaymentGatewaySetting>
{
    public void Configure(EntityTypeBuilder<PaymentGatewaySetting> builder)
    {
        builder.ToTable("payment_gateway_settings");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Provider).HasMaxLength(20).HasDefaultValue("razorpay");
        builder.Property(s => s.Mode).HasMaxLength(10).HasDefaultValue("test");
        builder.Property(s => s.KeyId).HasMaxLength(500);
        builder.Property(s => s.KeySecretRef).HasMaxLength(500);
        builder.Property(s => s.WebhookSecretRef).HasMaxLength(500);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => new { s.Provider, s.Mode }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.IsActive);
    }
}
