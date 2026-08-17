using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class PurchaseOrderDto
{
    public string Id { get; set; } = string.Empty;
    public string PoNumber { get; set; } = string.Empty;
    public string SupplierId { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime ExpectedDelivery { get; set; }
    public string WarehouseId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<PurchaseOrderLineDto> Lines { get; set; } = new();
    public decimal Total { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PurchaseOrderLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total => Qty * UnitPrice;
    public DateTime CreatedAt { get; set; }
}

public class CreatePurchaseOrderDto
{
    [Required]
    [MaxLength(50)]
    public string SupplierId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public DateTime ExpectedDelivery { get; set; } = DateTime.UtcNow.AddDays(7);

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<CreatePurchaseOrderLineDto> Lines { get; set; } = new();
}

public class CreatePurchaseOrderLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    public decimal UnitPrice { get; set; } = 0;
}

public class UpdatePurchaseOrderDto
{
    [MaxLength(50)]
    public string SupplierId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public DateTime ExpectedDelivery { get; set; }

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public List<UpdatePurchaseOrderLineDto> Lines { get; set; } = new();
}

public class UpdatePurchaseOrderLineDto
{
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    public decimal UnitPrice { get; set; } = 0;
}

public class ReceivePurchaseOrderDto
{
    public bool IsComplete { get; set; } = false;
    public List<ReceivePoLineDto> Lines { get; set; } = new();
}

public class ReceivePoLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int ReceivedQty { get; set; }
    public int DamagedQty { get; set; }
    public int RejectedQty { get; set; }
    public List<PutawayBinDto> Putaway { get; set; } = new();
}

public class PutawayBinDto
{
    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public int Qty { get; set; }
}