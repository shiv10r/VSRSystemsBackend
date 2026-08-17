using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class SalesOrderConfiguration : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("sales_orders");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(s => s.OrderDate).HasDefaultValueSql("NOW()");
        builder.Property(s => s.WarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("created");
        builder.Property(s => s.SubTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.TaxTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.DiscountTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.GrandTotal).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(s => s.DeliveryAddress).HasMaxLength(500);
        builder.Property(s => s.Notes).HasMaxLength(1000);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.OrderNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.CustomerId);
        builder.HasIndex(s => s.WarehouseId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.OrderDate);

        builder.HasMany(s => s.Lines)
            .WithOne(l => l.SalesOrder)
            .HasForeignKey(l => l.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SalesOrderLineConfiguration : IEntityTypeConfiguration<SalesOrderLine>
{
    public void Configure(EntityTypeBuilder<SalesOrderLine> builder)
    {
        builder.ToTable("sales_order_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.SalesOrderId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Sku).HasMaxLength(100);
        builder.Property(l => l.Qty).HasDefaultValue(0);
        builder.Property(l => l.Price).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(l => l.TaxPct).HasDefaultValue(18);
        builder.Property(l => l.DiscountPct).HasDefaultValue(0);
        builder.Property(l => l.Total).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
{
    public void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        builder.ToTable("stock_transfers");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(s => s.TransferNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.FromWarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.ToWarehouseId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.Date).HasDefaultValueSql("NOW()");
        builder.Property(s => s.Status).HasMaxLength(20).HasDefaultValue("created");
        builder.Property(s => s.Notes).HasMaxLength(1000);
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt);
        builder.Property(s => s.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(s => s.TransferNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(s => s.FromWarehouseId);
        builder.HasIndex(s => s.ToWarehouseId);
        builder.HasIndex(s => s.Status);
        builder.HasIndex(s => s.Date);

        builder.HasMany(s => s.Items)
            .WithOne(l => l.StockTransfer)
            .HasForeignKey(l => l.StockTransferId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class StockTransferLineConfiguration : IEntityTypeConfiguration<StockTransferLine>
{
    public void Configure(EntityTypeBuilder<StockTransferLine> builder)
    {
        builder.ToTable("stock_transfer_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.StockTransferId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Sku).HasMaxLength(100);
        builder.Property(l => l.Qty).HasDefaultValue(0);
        builder.Property(l => l.FromBin).HasMaxLength(50);
        builder.Property(l => l.ToBin).HasMaxLength(50);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class PickListConfiguration : IEntityTypeConfiguration<PickList>
{
    public void Configure(EntityTypeBuilder<PickList> builder)
    {
        builder.ToTable("pick_lists");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.PickNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.OrderId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PickNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.OrderId);
        builder.HasIndex(p => p.Status);

        builder.HasMany(p => p.Items)
            .WithOne(l => l.PickList)
            .HasForeignKey(l => l.PickListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PickPickLineConfiguration : IEntityTypeConfiguration<PickLine>
{
    public void Configure(EntityTypeBuilder<PickLine> builder)
    {
        builder.ToTable("pick_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.PickListId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Sku).HasMaxLength(100);
        builder.Property(l => l.Location).HasMaxLength(50);
        builder.Property(l => l.RequiredQty).HasDefaultValue(0);
        builder.Property(l => l.PickedQty).HasDefaultValue(0);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.ToTable("packages");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.OrderId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(p => p.TotalWeight).HasMaxLength(50);
        builder.Property(p => p.Dimensions).HasMaxLength(100);
        builder.Property(p => p.PackageCount).HasDefaultValue(0);
        builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(p => p.Remarks).HasMaxLength(1000);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.PackageId).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.OrderId);
        builder.HasIndex(p => p.Status);

        builder.HasMany(p => p.Items)
            .WithOne(i => i.Package)
            .HasForeignKey(i => i.PackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PackageItemConfiguration : IEntityTypeConfiguration<PackageItem>
{
    public void Configure(EntityTypeBuilder<PackageItem> builder)
    {
        builder.ToTable("package_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.PackageId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(i => i.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(i => i.Qty).HasDefaultValue(0);
        builder.Property(i => i.CreatedAt).HasDefaultValueSql("NOW()");
    }
}

public class DispatchConfiguration : IEntityTypeConfiguration<Dispatch>
{
    public void Configure(EntityTypeBuilder<Dispatch> builder)
    {
        builder.ToTable("dispatches");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.DispatchNumber).HasMaxLength(50).IsRequired();
        builder.Property(d => d.OrderId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(d => d.CustomerName).HasMaxLength(200).IsRequired();
        builder.Property(d => d.PackageId).HasMaxLength(50);
        builder.Property(d => d.Transporter).HasMaxLength(200);
        builder.Property(d => d.Courier).HasMaxLength(200);
        builder.Property(d => d.TrackingNumber).HasMaxLength(100);
        builder.Property(d => d.DispatchDate).HasDefaultValueSql("NOW()");
        builder.Property(d => d.VehicleNumber).HasMaxLength(50);
        builder.Property(d => d.Driver).HasMaxLength(100);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("ready");
        builder.Property(d => d.Remarks).HasMaxLength(1000);
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.DispatchNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(d => d.OrderId);
        builder.HasIndex(d => d.Status);
    }
}

public class ReturnRecordConfiguration : IEntityTypeConfiguration<ReturnRecord>
{
    public void Configure(EntityTypeBuilder<ReturnRecord> builder)
    {
        builder.ToTable("return_records");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.ReturnNumber).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Type).HasMaxLength(20).HasDefaultValue("customer");
        builder.Property(r => r.PartyName).HasMaxLength(200).IsRequired();
        builder.Property(r => r.OriginalRef).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Date).HasDefaultValueSql("NOW()");
        builder.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("requested");
        builder.Property(r => r.Remarks).HasMaxLength(1000);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.ReturnNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(r => r.Type);
        builder.HasIndex(r => r.Status);

        builder.HasMany(r => r.Items)
            .WithOne(l => l.ReturnRecord)
            .HasForeignKey(l => l.ReturnRecordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReturnLineConfiguration : IEntityTypeConfiguration<ReturnLine>
{
    public void Configure(EntityTypeBuilder<ReturnLine> builder)
    {
        builder.ToTable("return_lines");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.ReturnRecordId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.ItemName).HasMaxLength(200).IsRequired();
        builder.Property(l => l.Qty).HasDefaultValue(0);
        builder.Property(l => l.Reason).HasMaxLength(500);
        builder.Property(l => l.Condition).HasMaxLength(20).HasDefaultValue("good");
        builder.Property(l => l.Action).HasMaxLength(30).HasDefaultValue("restock");
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
    }
}