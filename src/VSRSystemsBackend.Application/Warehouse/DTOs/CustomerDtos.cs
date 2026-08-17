using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class CustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Gstin { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCustomerDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Company { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gstin { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(500)]
    public string BillingAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class UpdateCustomerDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Company { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Gstin { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(500)]
    public string BillingAddress { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShippingAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}