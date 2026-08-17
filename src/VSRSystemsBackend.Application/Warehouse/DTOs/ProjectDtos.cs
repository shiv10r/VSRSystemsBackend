using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class ProjectRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Client { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public decimal Budget { get; set; }
    public string? Address { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? WarehouseId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateProjectRecordDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Client { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "planned";

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public decimal Budget { get; set; } = 0;

    [MaxLength(500)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(50)]
    public string? WarehouseId { get; set; }
}

public class UpdateProjectRecordDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Client { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public decimal Budget { get; set; } = 0;

    [MaxLength(500)]
    public string? Address { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [MaxLength(50)]
    public string? WarehouseId { get; set; }
}