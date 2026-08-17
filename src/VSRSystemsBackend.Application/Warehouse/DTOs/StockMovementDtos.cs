using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class StockMovementDto
{
    public string Id { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string RefNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class StockAdjustmentDto
{
    public string Id { get; set; } = string.Empty;
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int OldQty { get; set; }
    public int NewQty { get; set; }
    public int Difference { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateStockAdjustmentDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public int OldQty { get; set; }
    public int NewQty { get; set; }
    public int Difference { get; set; }

    [MaxLength(200)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;
}