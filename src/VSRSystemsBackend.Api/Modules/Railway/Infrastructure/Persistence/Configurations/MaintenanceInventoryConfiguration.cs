using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Configurations;

public sealed class MaintenancePartConfiguration : IEntityTypeConfiguration<MaintenancePart>
{
    public void Configure(EntityTypeBuilder<MaintenancePart> b) { b.ToTable("MaintenanceParts", "railway"); b.HasKey(x => x.Id); b.Property(x => x.Sku).HasMaxLength(80); b.Property(x => x.Name).HasMaxLength(200); b.Property(x => x.Unit).HasMaxLength(40); b.Property(x => x.Version).IsConcurrencyToken(); b.Ignore(x => x.Available); b.HasIndex(x => new { x.OrganizationId, x.DivisionId, x.Sku }).IsUnique(); }
}
public sealed class PartReservationConfiguration : IEntityTypeConfiguration<PartReservation>
{ public void Configure(EntityTypeBuilder<PartReservation> b) { b.ToTable("PartReservations", "railway"); b.HasKey(x => x.Id); b.Property(x => x.Version).IsConcurrencyToken(); b.HasIndex(x => new { x.OrganizationId, x.WorkOrderId, x.PartId }); } }
public sealed class ProcurementRequestConfiguration : IEntityTypeConfiguration<ProcurementRequest>
{ public void Configure(EntityTypeBuilder<ProcurementRequest> b) { b.ToTable("ProcurementRequests", "railway"); b.HasKey(x => x.Id); b.Property(x => x.Status).HasMaxLength(32); b.Property(x => x.Version).IsConcurrencyToken(); } }
public sealed class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{ public void Configure(EntityTypeBuilder<PurchaseOrder> b) { b.ToTable("PurchaseOrders", "railway"); b.HasKey(x => x.Id); b.Property(x => x.VendorName).HasMaxLength(200); b.Property(x => x.UnitPrice).HasPrecision(18, 2); b.Property(x => x.Status).HasMaxLength(32); b.Property(x => x.Version).IsConcurrencyToken(); } }
public sealed class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
{ public void Configure(EntityTypeBuilder<GoodsReceipt> b) { b.ToTable("GoodsReceipts", "railway"); b.HasKey(x => x.Id); b.HasIndex(x => new { x.OrganizationId, x.PurchaseOrderId }); } }
