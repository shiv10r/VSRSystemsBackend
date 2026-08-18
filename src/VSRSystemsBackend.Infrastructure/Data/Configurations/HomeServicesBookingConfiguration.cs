using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class HomeServiceBookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("home_service_bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.BookingNumber).HasMaxLength(50).IsRequired();
        builder.Property(b => b.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.AddressId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.BookingType).HasMaxLength(20).HasDefaultValue("scheduled");
        builder.Property(b => b.ScheduledStart).IsRequired();
        builder.Property(b => b.ExpectedEnd).IsRequired();
        builder.Property(b => b.Status).HasMaxLength(50).HasDefaultValue("draft");
        builder.Property(b => b.AssignedProfessionalId).HasMaxLength(50);
        builder.Property(b => b.PriceQuoteId).HasMaxLength(50);
        builder.Property(b => b.CurrentQuoteId).HasMaxLength(50);
        builder.Property(b => b.PaymentStatus).HasMaxLength(30).HasDefaultValue("pending");
        builder.Property(b => b.CustomerNotes).HasMaxLength(2000);
        builder.Property(b => b.OpsNotes).HasMaxLength(2000);
        builder.Property(b => b.CancelReason).HasMaxLength(200);
        builder.Property(b => b.OriginalBookingId).HasMaxLength(50);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.BookingNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.AssignedProfessionalId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.PaymentStatus);
        builder.HasIndex(b => b.ScheduledStart);
        builder.HasIndex(b => b.ServiceId);

        builder.HasMany(b => b.Items)
            .WithOne(i => i.Booking)
            .HasForeignKey(i => i.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.AddOns)
            .WithOne(a => a.Booking)
            .HasForeignKey(a => a.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Materials)
            .WithOne(m => m.Booking)
            .HasForeignKey(m => m.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Assignments)
            .WithOne(a => a.Booking)
            .HasForeignKey(a => a.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.StatusHistory)
            .WithOne(h => h.Booking)
            .HasForeignKey(h => h.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Notes)
            .WithOne(n => n.Booking)
            .HasForeignKey(n => n.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BookingItemConfiguration : IEntityTypeConfiguration<BookingItem>
{
    public void Configure(EntityTypeBuilder<BookingItem> builder)
    {
        builder.ToTable("booking_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.Description).HasMaxLength(500);
        builder.Property(i => i.Quantity).HasDefaultValue(1);
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.LineTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(i => i.UpdatedAt);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(i => i.BookingId);
    }
}

public class BookingAddOnConfiguration : IEntityTypeConfiguration<BookingAddOn>
{
    public void Configure(EntityTypeBuilder<BookingAddOn> builder)
    {
        builder.ToTable("booking_add_ons");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.AddOnId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Name).HasMaxLength(200);
        builder.Property(a => a.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => new { a.BookingId, a.AddOnId });
    }
}

public class BookingMaterialConfiguration : IEntityTypeConfiguration<BookingMaterial>
{
    public void Configure(EntityTypeBuilder<BookingMaterial> builder)
    {
        builder.ToTable("booking_materials");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Name).HasMaxLength(200);
        builder.Property(m => m.Quantity).HasDefaultValue(1);
        builder.Property(m => m.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(m => m.LineTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(m => m.PhotoUrl).HasMaxLength(1000);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => m.BookingId);
    }
}

public class BookingAssignmentConfiguration : IEntityTypeConfiguration<BookingAssignment>
{
    public void Configure(EntityTypeBuilder<BookingAssignment> builder)
    {
        builder.ToTable("booking_assignments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Response).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(a => a.DeclineReason).HasMaxLength(500);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => new { a.BookingId, a.ProfessionalId }).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(a => a.ProfessionalId);
        builder.HasIndex(a => a.Response);
    }
}

public class BookingStatusHistoryConfiguration : IEntityTypeConfiguration<BookingStatusHistory>
{
    public void Configure(EntityTypeBuilder<BookingStatusHistory> builder)
    {
        builder.ToTable("booking_status_history");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(h => h.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(h => h.PreviousStatus).HasMaxLength(50);
        builder.Property(h => h.NewStatus).HasMaxLength(50).IsRequired();
        builder.Property(h => h.ChangedBy).HasMaxLength(50);
        builder.Property(h => h.Reason).HasMaxLength(1000);
        builder.Property(h => h.MetadataJson).HasMaxLength(2000).HasDefaultValue("{}");
        builder.Property(h => h.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(h => h.UpdatedAt);
        builder.Property(h => h.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(h => h.BookingId);
        builder.HasIndex(h => h.NewStatus);
        builder.HasIndex(h => h.ChangedAt);
    }
}

public class BookingNoteConfiguration : IEntityTypeConfiguration<BookingNote>
{
    public void Configure(EntityTypeBuilder<BookingNote> builder)
    {
        builder.ToTable("booking_notes");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(n => n.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(n => n.AuthorId).HasMaxLength(50);
        builder.Property(n => n.Note).HasMaxLength(2000);
        builder.Property(n => n.Visibility).HasMaxLength(20).HasDefaultValue("internal");
        builder.Property(n => n.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(n => n.UpdatedAt);
        builder.Property(n => n.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(n => n.BookingId);
    }
}

public class RecurringBookingConfiguration : IEntityTypeConfiguration<RecurringBooking>
{
    public void Configure(EntityTypeBuilder<RecurringBooking> builder)
    {
        builder.ToTable("recurring_bookings");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.AddressId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Frequency).HasMaxLength(20).HasDefaultValue("monthly");
        builder.Property(r => r.PreferredProfessionalId).HasMaxLength(50);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.CustomerId);
        builder.HasIndex(r => r.NextRunAt);
        builder.HasIndex(r => r.IsActive);
    }
}

public class AmcContractConfiguration : IEntityTypeConfiguration<AmcContract>
{
    public void Configure(EntityTypeBuilder<AmcContract> builder)
    {
        builder.ToTable("amc_contracts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.ServiceId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.AddressId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.VisitsPerYear).HasDefaultValue(2);
        builder.Property(c => c.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(c => c.CoveredServices).HasMaxLength(1000);
        builder.Property(c => c.ExcludedParts).HasMaxLength(1000);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.EndDate);
    }
}
