using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class StockCountConfiguration : IEntityTypeConfiguration<StockCount>
{
    public void Configure(EntityTypeBuilder<StockCount> builder)
    {
        builder.ToTable("stock_counts");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.CountNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Location).HasMaxLength(50).IsRequired();
        builder.Property(s => s.WarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Date).HasDefaultValueSql("NOW()");
        builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("open");
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.CountNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.WarehouseId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.Date);

        builder.HasMany(s => s.Items)
            .WithOne(l => l.StockCount)
            .HasForeignKey(l => l.StockCountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class StockCountLineConfiguration : IEntityTypeConfiguration<StockCountLine>
{
    public void Configure(EntityTypeBuilder<StockCountLine> builder)
    {
        builder.ToTable("stock_count_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.StockCountId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.SystemQty).HasDefaultValue(0);
        builder.Property(l => l.PhysicalQty).HasDefaultValue(0);
        builder.Property(l => l.Difference).HasDefaultValue(0);
        builder.Property(l => l.Reason).HasMaxLength(500);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMember>
{
    public void Configure(EntityTypeBuilder<StaffMember> builder)
    {
        builder.ToTable("staff_members");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Role).HasMaxLength(100);
        builder.Property(s => s.Phone).HasMaxLength(20);
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.Property(s => s.LastAttendance).HasMaxLength(20);
        builder.Property(s => s.LastAttendanceDate);
        builder.Property(s => s.DailyRate).HasColumnType("decimal(18,2)");
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Name);
        builder.HasIndex(s => s.IsActive);
    }
}

public class ProjectRecordConfiguration : IEntityTypeConfiguration<ProjectRecord>
{
    public void Configure(EntityTypeBuilder<ProjectRecord> builder)
    {
        builder.ToTable("project_records");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Client).HasMaxLength(200);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("planned");
        builder.Property(p => p.StartDate).HasDefaultValueSql("NOW()");
        builder.Property(p => p.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.Latitude).HasColumnType("double precision");
        builder.Property(p => p.Longitude).HasColumnType("double precision");
        builder.Property(p => p.WarehouseId).HasMaxLength(50);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.WarehouseId);
        builder.HasIndex(p => p.StartDate);

        builder.HasMany(p => p.Attendances)
            .WithOne(a => a.Project)
            .HasForeignKey(a => a.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Logs)
            .WithOne(l => l.Project)
            .HasForeignKey(l => l.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Parties)
            .WithOne(p => p.Project)
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProjectAttendanceConfiguration : IEntityTypeConfiguration<ProjectAttendance>
{
    public void Configure(EntityTypeBuilder<ProjectAttendance> builder)
    {
        builder.ToTable("project_attendances");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.StaffId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Date).HasDefaultValueSql("NOW()");
        builder.Property(a => a.Status).HasMaxLength(20).HasDefaultValue("present");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class ProjectLogConfiguration : IEntityTypeConfiguration<ProjectLog>
{
    public void Configure(EntityTypeBuilder<ProjectLog> builder)
    {
        builder.ToTable("project_logs");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Description).HasMaxLength(1000);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class ProjectTransactionConfiguration : IEntityTypeConfiguration<ProjectTransaction>
{
    public void Configure(EntityTypeBuilder<ProjectTransaction> builder)
    {
        builder.ToTable("project_transactions");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Type).HasMaxLength(100);
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(t => t.Date).HasDefaultValueSql("NOW()");
    }
}

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("project_tasks");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Title).HasMaxLength(500).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(1000);
        builder.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(t => t.DueDate);
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class ProjectPartyConfiguration : IEntityTypeConfiguration<ProjectParty>
{
    public void Configure(EntityTypeBuilder<ProjectParty> builder)
    {
        builder.ToTable("project_parties");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Role).HasMaxLength(100);
        builder.Property(p => p.Phone).HasMaxLength(20);
    }
}

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("stock_movements");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Sku).HasMaxLength(100);
        builder.Property(s => s.Type).HasMaxLength(30).IsRequired();
        builder.Property(s => s.Qty).HasDefaultValue(0);
        builder.Property(s => s.From).HasMaxLength(200);
        builder.Property(s => s.To).HasMaxLength(200);
        builder.Property(s => s.Reason).HasMaxLength(500);
        builder.Property(s => s.RefNumber).HasMaxLength(50);
        builder.Property(s => s.Date).HasDefaultValueSql("NOW()");
        builder.Property(s => s.Notes).HasMaxLength(1000);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.ItemId);
        builder.HasIndex(s => s.Type);
        builder.HasIndex(s => s.Date);
        builder.HasIndex(s => s.RefNumber);
    }
}

public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
{
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        builder.ToTable("stock_adjustments");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Sku).HasMaxLength(100);
        builder.Property(s => s.Location).HasMaxLength(50);
        builder.Property(s => s.OldQty).HasDefaultValue(0);
        builder.Property(s => s.NewQty).HasDefaultValue(0);
        builder.Property(s => s.Difference).HasDefaultValue(0);
        builder.Property(s => s.Reason).HasMaxLength(200);
        builder.Property(s => s.Remarks).HasMaxLength(1000);
        builder.Property(s => s.Date).HasDefaultValueSql("NOW()");
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.ItemId);
        builder.HasIndex(s => s.Date);
    }
}