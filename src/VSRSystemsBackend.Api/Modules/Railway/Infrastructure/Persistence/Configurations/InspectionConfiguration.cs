using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Inspection;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;

public sealed class InspectionTemplateConfiguration : IEntityTypeConfiguration<InspectionTemplate>
{
    public void Configure(EntityTypeBuilder<InspectionTemplate> builder)
    {
        builder.ToTable("InspectionTemplates", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Name).HasMaxLength(200).IsRequired();
        builder.Property(item => item.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.OwnsMany(item => item.Items, owned =>
        {
            owned.ToTable("InspectionTemplateItems", "railway");
            owned.WithOwner().HasForeignKey("InspectionTemplateId");
            owned.HasKey(item => item.Id);
            owned.Property(item => item.ItemId).HasMaxLength(80).IsRequired();
            owned.Property(item => item.Label).HasMaxLength(300).IsRequired();
            owned.HasIndex("InspectionTemplateId", nameof(InspectionTemplateItem.ItemId)).IsUnique();
        });
    }
}

public sealed class InspectionRunConfiguration : IEntityTypeConfiguration<InspectionRun>
{
    public void Configure(EntityTypeBuilder<InspectionRun> builder)
    {
        builder.ToTable("InspectionRuns", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.ReviewReason).HasMaxLength(1000);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.Ignore(item => item.DomainEvents);
        builder.OwnsMany(item => item.Requirements, owned =>
        {
            owned.ToTable("InspectionRunRequirements", "railway");
            owned.WithOwner().HasForeignKey("InspectionRunId");
            owned.HasKey(item => item.Id);
            owned.Property(item => item.ItemId).HasMaxLength(80).IsRequired();
        });
        builder.OwnsMany(item => item.Answers, owned =>
        {
            owned.ToTable("InspectionAnswers", "railway");
            owned.WithOwner().HasForeignKey("InspectionRunId");
            owned.HasKey(item => item.Id);
            owned.Property(item => item.ItemId).HasMaxLength(80).IsRequired();
            owned.Property(item => item.Response).HasMaxLength(1000).IsRequired();
            owned.Property(item => item.EvidenceIdList).HasMaxLength(4000);
        });
    }
}

public sealed class InspectionPlanConfiguration : IEntityTypeConfiguration<InspectionPlan>
{
    public void Configure(EntityTypeBuilder<InspectionPlan> builder)
    {
        builder.ToTable("InspectionPlans", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Schedule).HasMaxLength(120).IsRequired();
        builder.Property(item => item.TimeZone).HasMaxLength(80).IsRequired();
        builder.Property(item => item.Version).IsConcurrencyToken();
    }
}

public sealed class InspectionAssignmentConfiguration : IEntityTypeConfiguration<InspectionAssignment>
{
    public void Configure(EntityTypeBuilder<InspectionAssignment> builder)
    {
        builder.ToTable("InspectionAssignments", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.OccurrenceKey).HasMaxLength(180).IsRequired();
        builder.HasIndex(item => new { item.OrganizationId, item.PlanId, item.TargetId, item.OccurrenceKey }).IsUnique();
    }
}

public sealed class DefectConfiguration : IEntityTypeConfiguration<Defect>
{
    public void Configure(EntityTypeBuilder<Defect> builder)
    {
        builder.ToTable("Defects", "railway");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Description).HasMaxLength(2000).IsRequired();
        builder.Property(item => item.Severity).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(item => item.Version).IsConcurrencyToken();
        builder.HasIndex(item => new { item.OrganizationId, item.DivisionId, item.Status, item.Severity });
    }
}
