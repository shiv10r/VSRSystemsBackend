using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("warehouses");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(w => w.Name).HasMaxLength(200).IsRequired();
        builder.Property(w => w.Code).HasMaxLength(50).IsRequired();
        builder.Property(w => w.Address).HasMaxLength(500);
        builder.Property(w => w.ContactPerson).HasMaxLength(100);
        builder.Property(w => w.Phone).HasMaxLength(20);
        builder.Property(w => w.IsActive).HasDefaultValue(true);
        builder.Property(w => w.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(w => w.UpdatedAt);
        builder.Property(w => w.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(w => w.Code).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(w => w.Name);
        builder.HasIndex(w => w.IsActive);

        builder.HasMany(w => w.Locations)
            .WithOne(l => l.Warehouse)
            .HasForeignKey(l => l.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.InventoryItems)
            .WithOne(i => i.Warehouse)
            .HasForeignKey(i => i.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.TransfersFrom)
            .WithOne(s => s.FromWarehouse)
            .HasForeignKey(s => s.FromWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.TransfersTo)
            .WithOne(s => s.ToWarehouse)
            .HasForeignKey(s => s.ToWarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class LocationBinConfiguration : IEntityTypeConfiguration<LocationBin>
{
    public void Configure(EntityTypeBuilder<LocationBin> builder)
    {
        builder.ToTable("location_bins");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.WarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Code).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Zone).HasMaxLength(20);
        builder.Property(l => l.Rack).HasMaxLength(20);
        builder.Property(l => l.Bin).HasMaxLength(20);
        builder.Property(l => l.Capacity).HasDefaultValue(0);
        builder.Property(l => l.IsActive).HasDefaultValue(true);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => new { l.WarehouseId, l.Code }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(l => l.WarehouseId);
        builder.HasIndex(l => l.IsActive);
    }
}

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(i => i.Sku).HasMaxLength(100).IsRequired();
        builder.Property(i => i.Name).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Category).HasMaxLength(100);
        builder.Property(i => i.Brand).HasMaxLength(100);
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.Unit).HasMaxLength(20).IsRequired();
        builder.Property(i => i.Qty).HasDefaultValue(0);
        builder.Property(i => i.Reserved).HasDefaultValue(0);
        builder.Property(i => i.Damaged).HasDefaultValue(0);
        builder.Property(i => i.Quarantine).HasDefaultValue(0);
        builder.Property(i => i.InTransit).HasDefaultValue(0);
        builder.Property(i => i.ReorderLevel).HasDefaultValue(0);
        builder.Property(i => i.MinStock).HasDefaultValue(0);
        builder.Property(i => i.MaxStock).HasDefaultValue(0);
        builder.Property(i => i.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.SellingPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(i => i.Hsn).HasMaxLength(20);
        builder.Property(i => i.GstPct).HasDefaultValue(18);
        builder.Property(i => i.Barcode).HasMaxLength(100);
        builder.Property(i => i.Weight).HasMaxLength(50);
        builder.Property(i => i.Dimensions).HasMaxLength(100);
        builder.Property(i => i.Location).HasMaxLength(50);
        builder.Property(i => i.WarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.IsActive).HasDefaultValue(true);
        builder.Property(i => i.TrackBatch).HasDefaultValue(false);
        builder.Property(i => i.TrackSerial).HasDefaultValue(false);
        builder.Property(i => i.TrackExpiry).HasDefaultValue(false);
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(i => i.UpdatedAt);
        builder.Property(i => i.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(i => new { i.WarehouseId, i.Sku }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(i => i.WarehouseId);
        builder.HasIndex(i => i.Location);
        builder.HasIndex(i => i.IsActive);
        builder.HasIndex(i => i.Category);
    }
}

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("suppliers");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Company).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Contact).HasMaxLength(100);
        builder.Property(s => s.Phone).HasMaxLength(20);
        builder.Property(s => s.Email).HasMaxLength(200);
        builder.Property(s => s.Gstin).HasMaxLength(20);
        builder.Property(s => s.Address).HasMaxLength(500);
        builder.Property(s => s.PaymentTerms).HasMaxLength(100);
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.Gstin).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.Name);
        builder.HasIndex(s => s.IsActive);
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Company).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Gstin).HasMaxLength(20);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.BillingAddress).HasMaxLength(500);
        builder.Property(c => c.ShippingAddress).HasMaxLength(500);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.Gstin).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.IsActive);
    }
}

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.ToTable("purchase_orders");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.PoNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.SupplierId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.SupplierName).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Date).HasDefaultValueSql("NOW()");
        builder.Property(p => p.ExpectedDelivery).IsRequired();
        builder.Property(p => p.WarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("draft");
        builder.Property(p => p.Total).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(p => p.Notes).HasMaxLength(1000);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PoNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.SupplierId);
        builder.HasIndex(p => p.WarehouseId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.Date);

        builder.HasMany(p => p.Lines)
            .WithOne(l => l.PurchaseOrder)
            .HasForeignKey(l => l.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.ToTable("purchase_order_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.PurchaseOrderId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Qty).HasDefaultValue(0);
        builder.Property(l => l.UnitPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class GrnRecordConfiguration : IEntityTypeConfiguration<GrnRecord>
{
    public void Configure(EntityTypeBuilder<GrnRecord> builder)
    {
        builder.ToTable("grn_records");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(g => g.GrnNumber).HasMaxLength(50).IsRequired();
        builder.Property(g => g.PoId).HasMaxLength(50).IsRequired();
        builder.Property(g => g.PoNumber).HasMaxLength(50).IsRequired();
        builder.Property(g => g.Date).HasDefaultValueSql("NOW()");
        builder.Property(g => g.Notes).HasMaxLength(1000);
        builder.Property(g => g.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(g => g.UpdatedAt);
        builder.Property(g => g.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(g => g.GrnNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(g => g.PoId);
        builder.HasIndex(g => g.Date);

        builder.HasMany(g => g.Lines)
            .WithOne(l => l.GrnRecord)
            .HasForeignKey(l => l.GrnRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class GrnLineConfiguration : IEntityTypeConfiguration<GrnLine>
{
    public void Configure(EntityTypeBuilder<GrnLine> builder)
    {
        builder.ToTable("grn_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.GrnRecordId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.OrderedQty).HasDefaultValue(0);
        builder.Property(l => l.ReceivedQty).HasDefaultValue(0);
        builder.Property(l => l.DamagedQty).HasDefaultValue(0);
        builder.Property(l => l.RejectedQty).HasDefaultValue(0);
        builder.Property(l => l.AcceptedQty).HasDefaultValue(0);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");

        builder.HasMany(l => l.Putaway)
            .WithOne(p => p.GrnLine)
            .HasForeignKey(p => p.GrnLineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PutawayBinConfiguration : IEntityTypeConfiguration<PutawayBin>
{
    public void Configure(EntityTypeBuilder<PutawayBin> builder)
    {
        builder.ToTable("putaway_bins");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.GrnLineId).IsRequired();
        builder.Property(p => p.Location).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Qty).HasDefaultValue(0);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
    }
}