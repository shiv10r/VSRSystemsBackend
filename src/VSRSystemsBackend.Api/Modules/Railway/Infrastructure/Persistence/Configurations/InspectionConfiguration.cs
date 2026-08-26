using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Domain.Inspection;

public class InspectionConfiguration : IEntityTypeConfiguration<InspectionRun>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InspectionRun> builder)
    {
        builder.ToTable("InspectionRuns");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TemplateVersion).IsRequired().HasMaxLength(50);
        builder.Property(x => x.AssignedInspector).HasMaxLength(100);
        builder.Property(x => x.CreatedBy).HasMaxLength(100);
        builder.Property(x => x.StationId).IsRequired();

        builder.HasMany(x => x.Defects).WithOne().HasForeignKey(x => x.InspectionRunId);
        builder.HasMany(x => x.Amendments).WithOne();
    }
}