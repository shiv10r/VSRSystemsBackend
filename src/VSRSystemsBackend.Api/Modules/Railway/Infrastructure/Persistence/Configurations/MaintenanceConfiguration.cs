using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.ToTable("WorkOrders", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.SourceType).HasMaxLength(80).IsRequired();
        builder.Property(item => item.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.Priority).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.BlockReason).HasMaxLength(1000);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.DivisionId, item.Status, item.Priority });
        builder.HasIndex(item => new { item.OrganizationId, item.SourceType, item.SourceId }).IsUnique();
        builder.OwnsMany(item => item.Tasks, owned =>
        {
            owned.ToTable("WorkOrderTasks", "railway");
            owned.WithOwner().HasForeignKey("WorkOrderId");
            owned.HasKey(item => item.Id);
            owned.Property(item => item.Description).HasMaxLength(1000).IsRequired();
        });
        builder.OwnsMany(item => item.History, owned =>
        {
            owned.ToTable("WorkOrderHistory", "railway");
            owned.WithOwner().HasForeignKey("WorkOrderId");
            owned.HasKey(item => item.Id);
            owned.Property(item => item.Status).HasConversion<string>().HasMaxLength(32);
            owned.Property(item => item.Reason).HasMaxLength(1000).IsRequired();
        });
    }
}

public sealed class MaintenancePlanConfiguration : IEntityTypeConfiguration<MaintenancePlan>
{
    public void Configure(EntityTypeBuilder<MaintenancePlan> builder)
    {
        builder.ToTable("MaintenancePlans", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Name).HasMaxLength(200).IsRequired();
        builder.Property(item => item.RecurrenceRule).HasMaxLength(120).IsRequired();
        builder.Property(item => item.Version).IsConcurrencyToken();
    }
}
