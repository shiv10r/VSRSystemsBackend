using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class StockTransferDto
{
    public string Id { get; set; } = string.Empty;
    public string TransferNumber { get; set; } = string.Empty;
    public string FromWarehouseId { get; set; } = string.Empty;
    public string ToWarehouseId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<StockTransferLineDto> Items { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class StockTransferLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string? FromBin { get; set; }
    public string? ToBin { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateStockTransferDto
{
    [Required]
    [MaxLength(50)]
    public string FromWarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ToWarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<CreateStockTransferLineDto> Items { get; set; } = new();
}

public class CreateStockTransferLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    [MaxLength(50)]
    public string? FromBin { get; set; }

    [MaxLength(50)]
    public string? ToBin { get; set; }
}

public class UpdateStockTransferDto
{
    [MaxLength(50)]
    public string FromWarehouseId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ToWarehouseId { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public List<UpdateStockTransferLineDto> Items { get; set; } = new();
}

public class UpdateStockTransferLineDto
{
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    [MaxLength(50)]
    public string? FromBin { get; set; }

    [MaxLength(50)]
    public string? ToBin { get; set; }
}

public class ReceiveStockTransferDto
{
    public List<ReceiveTransferLineDto> Lines { get; set; } = new();
}

public class ReceiveTransferLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int ReceivedQty { get; set; }
    public int DamagedQty { get; set; }
}