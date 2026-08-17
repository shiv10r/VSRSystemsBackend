using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class StockCountDto
{
    public string Id { get; set; } = string.Empty;
    public string CountNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<StockCountLineDto> Items { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class StockCountLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int SystemQty { get; set; }
    public int PhysicalQty { get; set; }
    public int Difference { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateStockCountDto
{
    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<CreateStockCountLineDto> Items { get; set; } = new();
}

public class CreateStockCountLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int SystemQty { get; set; }
    public int PhysicalQty { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class UpdateStockCountDto
{
    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<UpdateStockCountLineDto> Items { get; set; } = new();
}

public class UpdateStockCountLineDto
{
    public int PhysicalQty { get; set; }
    public string Reason { get; set; } = string.Empty;
}