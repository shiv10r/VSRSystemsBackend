using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class DispatchDto
{
    public string Id { get; set; } = string.Empty;
    public string DispatchNumber { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string PackageId { get; set; } = string.Empty;
    public string Transporter { get; set; } = string.Empty;
    public string Courier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Driver { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDispatchDto
{
    [Required]
    [MaxLength(50)]
    public string OrderId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string DispatchNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Transporter { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Courier { get; set; } = string.Empty;

    [MaxLength(100)]
    public string TrackingNumber { get; set; } = string.Empty;

    public DateTime DispatchDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Driver { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;
}

public class UpdateDispatchDto
{
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Transporter { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Courier { get; set; } = string.Empty;

    [MaxLength(100)]
    public string TrackingNumber { get; set; } = string.Empty;

    public DateTime DispatchDate { get; set; }

    [MaxLength(50)]
    public string VehicleNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Driver { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;
}