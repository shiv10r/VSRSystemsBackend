using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class InventoryItemDto
{
    public string Id { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public int Qty { get; set; }
    public int Reserved { get; set; }
    public int Damaged { get; set; }
    public int Quarantine { get; set; }
    public int InTransit { get; set; }
    public int ReorderLevel { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string Hsn { get; set; } = string.Empty;
    public int GstPct { get; set; }
    public string? Barcode { get; set; }
    public string? Weight { get; set; }
    public string? Dimensions { get; set; }
    public string Location { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool TrackBatch { get; set; }
    public bool TrackSerial { get; set; }
    public bool TrackExpiry { get; set; }
    public int AvailableQty { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateInventoryItemDto
{
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;
    public int Reserved { get; set; } = 0;
    public int Damaged { get; set; } = 0;
    public int Quarantine { get; set; } = 0;
    public int InTransit { get; set; } = 0;
    public int ReorderLevel { get; set; } = 0;
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;
    public decimal SellingPrice { get; set; } = 0;
    [MaxLength(20)]
    public string Hsn { get; set; } = string.Empty;
    public int GstPct { get; set; } = 18;
    [MaxLength(100)]
    public string? Barcode { get; set; }
    [MaxLength(50)]
    public string? Weight { get; set; }
    [MaxLength(100)]
    public string? Dimensions { get; set; }
    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool TrackBatch { get; set; } = false;
    public bool TrackSerial { get; set; } = false;
    public bool TrackExpiry { get; set; } = false;
}

public class UpdateInventoryItemDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Unit { get; set; } = string.Empty;

    public int Qty { get; set; } = 0;
    public int Reserved { get; set; } = 0;
    public int Damaged { get; set; } = 0;
    public int Quarantine { get; set; } = 0;
    public int InTransit { get; set; } = 0;
    public int ReorderLevel { get; set; } = 0;
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;
    public decimal SellingPrice { get; set; } = 0;
    [MaxLength(20)]
    public string Hsn { get; set; } = string.Empty;
    public int GstPct { get; set; } = 18;
    [MaxLength(100)]
    public string? Barcode { get; set; }
    [MaxLength(50)]
    public string? Weight { get; set; }
    [MaxLength(100)]
    public string? Dimensions { get; set; }
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool TrackBatch { get; set; } = false;
    public bool TrackSerial { get; set; } = false;
    public bool TrackExpiry { get; set; } = false;
}

public class StockAdjustmentRequest
{
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class StockReservationRequest
{
    public int Quantity { get; set; }
}