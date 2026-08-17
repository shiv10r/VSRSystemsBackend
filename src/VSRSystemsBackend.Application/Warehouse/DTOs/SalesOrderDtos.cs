using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class SalesOrderDto
{
    public string Id { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string WarehouseId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<SalesOrderLineDto> Lines { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SalesOrderLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public int TaxPct { get; set; }
    public int DiscountPct { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateSalesOrderDto
{
    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [MaxLength(500)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<CreateSalesOrderLineDto> Lines { get; set; } = new();
}

public class CreateSalesOrderLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    public decimal Price { get; set; } = 0;

    public int TaxPct { get; set; } = 18;

    public int DiscountPct { get; set; } = 0;
}

public class UpdateSalesOrderDto
{
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [MaxLength(500)]
    public string DeliveryAddress { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public List<UpdateSalesOrderLineDto> Lines { get; set; } = new();
}

public class UpdateSalesOrderLineDto
{
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    public decimal Price { get; set; } = 0;

    public int TaxPct { get; set; } = 18;

    public int DiscountPct { get; set; } = 0;
}