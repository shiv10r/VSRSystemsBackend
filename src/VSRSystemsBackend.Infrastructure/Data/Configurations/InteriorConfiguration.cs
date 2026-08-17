using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Interior;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class InteriorProjectConfiguration : IEntityTypeConfiguration<InteriorProject>
{
    public void Configure(EntityTypeBuilder<InteriorProject> builder)
    {
        builder.ToTable("interior_projects");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.PropertyType).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Location).HasMaxLength(500).IsRequired();
        builder.Property(p => p.TotalArea).HasDefaultValue(0);
        builder.Property(p => p.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);
    }
}

public class InteriorRoomConfiguration : IEntityTypeConfiguration<InteriorRoom>
{
    public void Configure(EntityTypeBuilder<InteriorRoom> builder)
    {
        builder.ToTable("interior_rooms");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.Property(r => r.RoomType).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Length).HasDefaultValue(0);
        builder.Property(r => r.Width).HasDefaultValue(0);
        builder.Property(r => r.Height).HasDefaultValue(0);
        builder.Property(r => r.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(r => r.Notes).HasMaxLength(1000);
        builder.Property(r => r.Image).HasMaxLength(5000);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.ProjectId);
    }
}

public class InteriorDesignConfiguration : IEntityTypeConfiguration<InteriorDesign>
{
    public void Configure(EntityTypeBuilder<InteriorDesign> builder)
    {
        builder.ToTable("interior_designs");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.ProjectId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.RoomId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
        builder.Property(d => d.Style).HasMaxLength(20).IsRequired();
        builder.Property(d => d.Color).HasMaxLength(20).IsRequired();
        builder.Property(d => d.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("generating");
        builder.Property(d => d.Favorite).HasDefaultValue(false);
        builder.Property(d => d.Saved).HasDefaultValue(false);
        builder.Property(d => d.CurrentVersion).HasDefaultValue(0);
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.ProjectId);
        builder.HasIndex(d => d.RoomId);
        builder.HasIndex(d => d.Status);
    }
}

public class DesignVersionConfiguration : IEntityTypeConfiguration<DesignVersion>
{
    public void Configure(EntityTypeBuilder<DesignVersion> builder)
    {
        builder.ToTable("design_versions");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(v => v.DesignId).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Version).HasDefaultValue(0);
        builder.Property(v => v.Style).HasMaxLength(20).IsRequired();
        builder.Property(v => v.Color).HasMaxLength(20).IsRequired();
        builder.Property(v => v.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(v => v.Prompt).HasMaxLength(2000);
        builder.Property(v => v.CreatedAt).HasDefaultValueSql("NOW()");

        builder.Property(v => v.ProductIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}

public class InteriorProductConfiguration : IEntityTypeConfiguration<InteriorProduct>
{
    public void Configure(EntityTypeBuilder<InteriorProduct> builder)
    {
        builder.ToTable("interior_products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Category).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Material).HasMaxLength(100);
        builder.Property(p => p.Color).HasMaxLength(50);
        builder.Property(p => p.Width).HasMaxLength(50);
        builder.Property(p => p.Depth).HasMaxLength(50);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Category);
    }
}