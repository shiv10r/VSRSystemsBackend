using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable("service_categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Slug).HasMaxLength(220).IsRequired();
        builder.Property(c => c.Tagline).HasMaxLength(500);
        builder.Property(c => c.ImageUrl).HasMaxLength(1000);
        builder.Property(c => c.SortOrder).HasDefaultValue(0);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => c.SortOrder);

        builder.HasMany(c => c.Services)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("services");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.CategoryId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Slug).HasMaxLength(220).IsRequired();
        builder.Property(s => s.ShortDescription).HasMaxLength(500);
        builder.Property(s => s.LongDescription).HasMaxLength(3000);
        builder.Property(s => s.ImageUrl).HasMaxLength(1000);
        builder.Property(s => s.IsEmergency).HasDefaultValue(false);
        builder.Property(s => s.NeedsInspection).HasDefaultValue(false);
        builder.Property(s => s.InspectionFee).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.CategoryId);
        builder.HasIndex(s => s.IsActive);
        builder.HasIndex(s => s.IsEmergency);

        builder.HasMany(s => s.Problems)
            .WithOne(p => p.Service)
            .HasForeignKey(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Packages)
            .WithOne(p => p.Service)
            .HasForeignKey(p => p.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.AddOns)
            .WithOne(a => a.Service)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Warranties)
            .WithOne(w => w.Service)
            .HasForeignKey(w => w.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ServiceProblemConfiguration : IEntityTypeConfiguration<ServiceProblem>
{
    public void Configure(EntityTypeBuilder<ServiceProblem> builder)
    {
        builder.ToTable("service_problems");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.SortOrder).HasDefaultValue(0);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.ServiceId);
    }
}

public class ServicePackageConfiguration : IEntityTypeConfiguration<ServicePackage>
{
    public void Configure(EntityTypeBuilder<ServicePackage> builder)
    {
        builder.ToTable("service_packages");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.ShortDescription).HasMaxLength(500);
        builder.Property(p => p.DetailedDescription).HasMaxLength(2000);
        builder.Property(p => p.BasePrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.DurationMins).HasDefaultValue(60);
        builder.Property(p => p.WhatIncluded).HasMaxLength(2000);
        builder.Property(p => p.WhatExcluded).HasMaxLength(2000);
        builder.Property(p => p.Warranty).HasMaxLength(200);
        builder.Property(p => p.InspectionRequired).HasDefaultValue(false);
        builder.Property(p => p.PartsIncluded).HasDefaultValue(false);
        builder.Property(p => p.MinimumCharge).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.CancellationRule).HasMaxLength(200);
        builder.Property(p => p.IsPopular).HasDefaultValue(false);
        builder.Property(p => p.IsEmergencyEligible).HasDefaultValue(false);
        builder.Property(p => p.IsActive).HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => new { p.ServiceId, p.Name }).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.ServiceId);
        builder.HasIndex(p => p.IsActive);

        builder.HasMany(p => p.PackageAddOns)
            .WithOne(pa => pa.Package)
            .HasForeignKey(pa => pa.PackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ServiceAddOnConfiguration : IEntityTypeConfiguration<ServiceAddOn>
{
    public void Configure(EntityTypeBuilder<ServiceAddOn> builder)
    {
        builder.ToTable("service_add_ons");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Name).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(a => a.DurationMins).HasDefaultValue(0);
        builder.Property(a => a.IsActive).HasDefaultValue(true);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.ServiceId);
        builder.HasIndex(a => a.IsActive);
    }
}

public class ServicePackageAddOnConfiguration : IEntityTypeConfiguration<ServicePackageAddOn>
{
    public void Configure(EntityTypeBuilder<ServicePackageAddOn> builder)
    {
        builder.ToTable("service_package_add_ons");
        builder.HasKey(pa => pa.Id);
        builder.Property(pa => pa.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(pa => pa.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(pa => pa.AddOnId).HasMaxLength(50).IsRequired();
        builder.Property(pa => pa.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(pa => pa.UpdatedAt);
        builder.Property(pa => pa.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(pa => new { pa.PackageId, pa.AddOnId }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class ServiceWarrantyConfiguration : IEntityTypeConfiguration<ServiceWarranty>
{
    public void Configure(EntityTypeBuilder<ServiceWarranty> builder)
    {
        builder.ToTable("service_warranties");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(w => w.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(w => w.WarrantyDays).HasDefaultValue(0);
        builder.Property(w => w.Terms).HasMaxLength(1000);
        builder.Property(w => w.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(w => w.UpdatedAt);
        builder.Property(w => w.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(w => w.ServiceId);
    }
}

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.LaunchedAt);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Name).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.IsActive);

        builder.HasMany(c => c.Zones)
            .WithOne(z => z.City)
            .HasForeignKey(z => z.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("zones");
        builder.HasKey(z => z.Id);
        builder.Property(z => z.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(z => z.CityId).HasMaxLength(50).IsRequired();
        builder.Property(z => z.Name).HasMaxLength(100).IsRequired();
        builder.Property(z => z.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(z => z.UpdatedAt);
        builder.Property(z => z.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(z => new { z.CityId, z.Name }).IsUnique().HasFilter("\"IsDeleted\" = false");

        builder.HasMany(z => z.Localities)
            .WithOne(l => l.Zone)
            .HasForeignKey(l => l.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class LocalityConfiguration : IEntityTypeConfiguration<Locality>
{
    public void Configure(EntityTypeBuilder<Locality> builder)
    {
        builder.ToTable("localities");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.ZoneId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
        builder.Property(l => l.Pincode).HasMaxLength(20);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => new { l.ZoneId, l.Name }).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(l => l.Pincode);
    }
}

public class PincodeConfiguration : IEntityTypeConfiguration<Pincode>
{
    public void Configure(EntityTypeBuilder<Pincode> builder)
    {
        builder.ToTable("pincodes");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Code).HasMaxLength(20).IsRequired();
        builder.Property(p => p.CityId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.IsServiceable).HasDefaultValue(true);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.CityId);
    }
}

public class ServiceAreaConfiguration : IEntityTypeConfiguration<ServiceArea>
{
    public void Configure(EntityTypeBuilder<ServiceArea> builder)
    {
        builder.ToTable("service_areas");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.CityId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.ZoneId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.IsActive).HasDefaultValue(true);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => new { a.CityId, a.ZoneId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(a => a.IsActive);
    }
}

public class ServiceAreaServiceConfiguration : IEntityTypeConfiguration<ServiceAreaService>
{
    public void Configure(EntityTypeBuilder<ServiceAreaService> builder)
    {
        builder.ToTable("service_area_services");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.ServiceAreaId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => new { s.ServiceAreaId, s.ServiceId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.ServiceId);
    }
}

public class PriceRuleConfiguration : IEntityTypeConfiguration<PriceRule>
{
    public void Configure(EntityTypeBuilder<PriceRule> builder)
    {
        builder.ToTable("price_rules");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.PackageId).HasMaxLength(50);
        builder.Property(r => r.CityId).HasMaxLength(50);
        builder.Property(r => r.RuleType).HasMaxLength(20).HasDefaultValue("discount");
        builder.Property(r => r.Value).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.ValidFrom);
        builder.Property(r => r.ValidTo);
        builder.Property(r => r.IsActive).HasDefaultValue(true);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.ServiceId);
        builder.HasIndex(r => r.RuleType);
        builder.HasIndex(r => r.IsActive);
    }
}

public class PriceQuoteConfiguration : IEntityTypeConfiguration<PriceQuote>
{
    public void Configure(EntityTypeBuilder<PriceQuote> builder)
    {
        builder.ToTable("price_quotes");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(q => q.QuoteNumber).HasMaxLength(50).IsRequired();
        builder.Property(q => q.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(q => q.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(q => q.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(q => q.AddressId).HasMaxLength(50);
        builder.Property(q => q.BasePrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.AddOnsTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.MaterialsTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.FeesTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.TravelCharge).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.UrgentCharge).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.PlatformFee).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.DiscountTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.TaxTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.GrandTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(q => q.CouponId).HasMaxLength(50);
        builder.Property(q => q.CouponCode).HasMaxLength(200);
        builder.Property(q => q.ExpiresAt).IsRequired();
        builder.Property(q => q.Version).HasDefaultValue(1);
        builder.Property(q => q.Status).HasMaxLength(30).HasDefaultValue("active");
        builder.Property(q => q.Notes).HasMaxLength(2000);
        builder.Property(q => q.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(q => q.UpdatedAt);
        builder.Property(q => q.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(q => q.QuoteNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(q => q.CustomerId);
        builder.HasIndex(q => q.ServiceId);
        builder.HasIndex(q => q.Status);
        builder.HasIndex(q => q.ExpiresAt);

        builder.HasMany(q => q.Revisions)
            .WithOne(r => r.PriceQuote)
            .HasForeignKey(r => r.PriceQuoteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuoteRevisionConfiguration : IEntityTypeConfiguration<QuoteRevision>
{
    public void Configure(EntityTypeBuilder<QuoteRevision> builder)
    {
        builder.ToTable("quote_revisions");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.PriceQuoteId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.RevisionNumber).HasDefaultValue(1);
        builder.Property(r => r.Reason).HasMaxLength(1000);
        builder.Property(r => r.PreviousTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.NewTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.CreatedBy).HasMaxLength(50);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.PriceQuoteId);
    }
}
