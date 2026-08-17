using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class PackageDto
{
    public string Id { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public List<PackageItemDto> Items { get; set; } = new();
    public string TotalWeight { get; set; } = string.Empty;
    public string Dimensions { get; set; } = string.Empty;
    public int PackageCount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PackageItemDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePackageDto
{
    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string TotalWeight { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Dimensions { get; set; } = string.Empty;

    public int PackageCount { get; set; } = 0;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public List<CreatePackageItemDto> Items { get; set; } = new();
}

public class CreatePackageItemDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;
}

public class UpdatePackageDto
{
    [MaxLength(50)]
    public string TotalWeight { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Dimensions { get; set; } = string.Empty;

    public int PackageCount { get; set; } = 0;

    public string Status { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public List<UpdatePackageItemDto> Items { get; set; } = new();
}

public class UpdatePackageItemDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;
}