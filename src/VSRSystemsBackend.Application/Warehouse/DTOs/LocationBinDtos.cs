using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class LocationBinDto
{
    public string Id { get; set; } = string.Empty;
    public string WarehouseId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string Rack { get; set; } = string.Empty;
    public string Bin { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateLocationBinDto
{
    [Required]
    [MaxLength(50)]
    public string WarehouseId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Zone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Rack { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Bin { get; set; } = string.Empty;

    public int Capacity { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

public class UpdateLocationBinDto
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Zone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Rack { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Bin { get; set; } = string.Empty;

    public int Capacity { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}