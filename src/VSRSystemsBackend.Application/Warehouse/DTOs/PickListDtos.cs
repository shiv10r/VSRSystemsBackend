using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class PickListDto
{
    public string Id { get; set; } = string.Empty;
    public string PickNumber { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<PickLineDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PickLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int RequiredQty { get; set; }
    public int PickedQty { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePickListDto
{
    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PickNumber { get; set; } = string.Empty;

    public List<CreatePickLineDto> Items { get; set; } = new();
}

public class CreatePickLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Location { get; set; } = string.Empty;

    public int RequiredQty { get; set; }
}

public class UpdatePickListDto
{
    public string Status { get; set; } = string.Empty;

    public List<UpdatePickLineDto> Items { get; set; } = new();
}

public class UpdatePickLineDto
{
    public int PickedQty { get; set; }
}