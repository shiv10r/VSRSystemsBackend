using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.HomeServices.DTOs;

public class CustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? DefaultAddressId { get; set; }
    public decimal WalletBalance { get; set; }
    public string? MembershipPlanId { get; set; }
    public string? ReferralCode { get; set; }
    public string? ReferredByCustomerId { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<CustomerAddressDto> Addresses { get; set; } = new();
}

public class CustomerAddressDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Line1 { get; set; } = string.Empty;
    public string Line2 { get; set; } = string.Empty;
    public string? CityId { get; set; }
    public string? ZoneId { get; set; }
    public string? LocalityId { get; set; }
    public string Pincode { get; set; } = string.Empty;
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public bool IsDefault { get; set; }
    public string? ContactPerson { get; set; }
    public string? ContactPhone { get; set; }
    public string? AccessInstructions { get; set; }
    public string? CityName { get; set; }
    public string? ZoneName { get; set; }
    public string? LocalityName { get; set; }
}

public class CreateCustomerAddressDto
{
    [Required]
    [MaxLength(50)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Line1 { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Line2 { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CityId { get; set; }

    [MaxLength(50)]
    public string? ZoneId { get; set; }

    [MaxLength(50)]
    public string? LocalityId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Pincode { get; set; } = string.Empty;

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public bool IsDefault { get; set; } = false;

    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    [MaxLength(500)]
    public string? AccessInstructions { get; set; }
}

public class UpdateCustomerAddressDto
{
    [MaxLength(50)]
    public string Label { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Line1 { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Line2 { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CityId { get; set; }

    [MaxLength(50)]
    public string? ZoneId { get; set; }

    [MaxLength(50)]
    public string? LocalityId { get; set; }

    [MaxLength(20)]
    public string Pincode { get; set; } = string.Empty;

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public bool IsDefault { get; set; }

    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    [MaxLength(500)]
    public string? AccessInstructions { get; set; }
}

public class SetDefaultAddressDto
{
    [Required]
    [MaxLength(50)]
    public string AddressId { get; set; } = string.Empty;
}

public class EnsureCustomerDto
{
    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? FullName { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }
}