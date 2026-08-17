using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Hotel;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class GuestConfiguration : IEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("guests");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(g => g.Name).HasMaxLength(200).IsRequired();
        builder.Property(g => g.Phone).HasMaxLength(20).IsRequired();
        builder.Property(g => g.Email).HasMaxLength(200);
        builder.Property(g => g.Nationality).HasMaxLength(100);
        builder.Property(g => g.Vip).HasDefaultValue(false);
        builder.Property(g => g.Stays).HasDefaultValue(0);
        builder.Property(g => g.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(g => g.UpdatedAt);
        builder.Property(g => g.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(g => g.Phone).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(g => g.Email).HasFilter("\"IsDeleted\" = false");
    }
}

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.Number).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Floor).IsRequired();
        builder.Property(r => r.Type).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("vacant-clean");
        builder.Property(r => r.Rate).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.Number).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.Type);
    }
}

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.Confirmation).HasMaxLength(50).IsRequired();
        builder.Property(r => r.GuestId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.GuestName).HasMaxLength(200).IsRequired();
        builder.Property(r => r.RoomNumber).HasMaxLength(20).IsRequired();
        builder.Property(r => r.CheckIn).IsRequired();
        builder.Property(r => r.CheckOut).IsRequired();
        builder.Property(r => r.Adults).HasDefaultValue(1);
        builder.Property(r => r.Children).HasDefaultValue(0);
        builder.Property(r => r.Source).HasMaxLength(20).HasDefaultValue("Direct");
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("confirmed");
        builder.Property(r => r.Balance).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.Confirmation).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.GuestId);
        builder.HasIndex(r => r.RoomNumber);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.CheckIn);
        builder.HasIndex(r => r.CheckOut);
    }
}

public class HousekeepingTaskConfiguration : IEntityTypeConfiguration<HousekeepingTask>
{
    public void Configure(EntityTypeBuilder<HousekeepingTask> builder)
    {
        builder.ToTable("housekeeping_tasks");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(h => h.RoomNumber).HasMaxLength(20).IsRequired();
        builder.Property(h => h.Assignee).HasMaxLength(100).IsRequired();
        builder.Property(h => h.Task).HasMaxLength(30).IsRequired();
        builder.Property(h => h.Priority).HasMaxLength(10).HasDefaultValue("normal");
        builder.Property(h => h.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(h => h.Scheduled).IsRequired();
        builder.Property(h => h.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(h => h.UpdatedAt);
        builder.Property(h => h.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(h => h.RoomNumber);
        builder.HasIndex(h => h.Assignee);
        builder.HasIndex(h => h.Status);
        builder.HasIndex(h => h.Scheduled);
    }
}