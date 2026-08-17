using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Travel;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class TravelPackageConfiguration : IEntityTypeConfiguration<TravelPackage>
{
    public void Configure(EntityTypeBuilder<TravelPackage> builder)
    {
        builder.ToTable("travel_packages");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Code).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(5000);
        builder.Property(p => p.DestinationId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Category).HasMaxLength(50).IsRequired();
        builder.Property(p => p.DurationDays).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.DiscountedPrice).HasColumnType("decimal(18,2)");
        builder.Property(p => p.Inclusions).HasMaxLength(3000);
        builder.Property(p => p.Exclusions).HasMaxLength(3000);
        builder.Property(p => p.Itinerary).HasMaxLength(10000);
        builder.Property(p => p.ImageUrls).HasMaxLength(5000);
        builder.Property(p => p.MaxGroupSize).HasDefaultValue(0);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.DestinationId);
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.Status);
    }
}

public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        builder.ToTable("destinations");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.Code).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Country).HasMaxLength(100).IsRequired();
        builder.Property(d => d.State).HasMaxLength(100);
        builder.Property(d => d.Description).HasMaxLength(3000);
        builder.Property(d => d.ImageUrls).HasMaxLength(5000);
        builder.Property(d => d.Latitude).HasColumnType("double precision");
        builder.Property(d => d.Longitude).HasColumnType("double precision");
        builder.Property(d => d.BestTimeToVisit).HasMaxLength(200);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(d => d.Country);
        builder.HasIndex(d => d.Status);
    }
}

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.BookingNumber).HasMaxLength(50).IsRequired();
        builder.Property(b => b.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(b => b.CustomerEmail).HasMaxLength(200).IsRequired();
        builder.Property(b => b.CustomerPhone).HasMaxLength(20).IsRequired();
        builder.Property(b => b.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.TravelDate).IsRequired();
        builder.Property(b => b.NumberOfTravelers).HasDefaultValue(1);
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.PaidAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.BalanceAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(b => b.PaymentStatus).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(b => b.PaymentMethod).HasMaxLength(50);
        builder.Property(b => b.SpecialRequests).HasMaxLength(2000);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.BookingNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.PackageId);
        builder.HasIndex(b => b.TravelDate);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.PaymentStatus);
    }
}

public class GroupTripConfiguration : IEntityTypeConfiguration<GroupTrip>
{
    public void Configure(EntityTypeBuilder<GroupTrip> builder)
    {
        builder.ToTable("group_trips");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(g => g.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(g => g.StartDate).IsRequired();
        builder.Property(g => g.EndDate).IsRequired();
        builder.Property(g => g.MaxCapacity).HasDefaultValue(0);
        builder.Property(g => g.CurrentBookings).HasDefaultValue(0);
        builder.Property(g => g.Status).HasMaxLength(20).HasDefaultValue("upcoming");
        builder.Property(g => g.TripLeader).HasMaxLength(200);
        builder.Property(g => g.Notes).HasMaxLength(2000);
        builder.Property(g => g.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(g => g.UpdatedAt);
        builder.Property(g => g.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(g => g.PackageId);
        builder.HasIndex(g => g.StartDate);
        builder.HasIndex(g => g.Status);
    }
}

public class TravelWishlistConfiguration : IEntityTypeConfiguration<TravelWishlist>
{
    public void Configure(EntityTypeBuilder<TravelWishlist> builder)
    {
        builder.ToTable("travel_wishlists");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(w => w.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(w => w.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(w => w.UpdatedAt);
        builder.Property(w => w.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(w => w.CustomerId).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class TravelWishlistItemConfiguration : IEntityTypeConfiguration<TravelWishlistItem>
{
    public void Configure(EntityTypeBuilder<TravelWishlistItem> builder)
    {
        builder.ToTable("travel_wishlist_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.WishlistId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.AddedAt).HasDefaultValueSql("NOW()");
    }
}