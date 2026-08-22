using VSRSystemsBackend.Application.HomeServices.DTOs;
using VSRSystemsBackend.Application.HomeServices.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

public class CustomerAddressesService : ICustomerAddressesService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IUserRepository _userRepository;

    public CustomerAddressesService(
        ICustomerRepository customerRepository,
        ILocationRepository locationRepository,
        IUserRepository userRepository)
    {
        _customerRepository = customerRepository;
        _locationRepository = locationRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<CustomerDto>> EnsureCustomerAsync(EnsureCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Email = email,
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? email : dto.FullName!.Trim(),
                Phone = dto.Phone ?? string.Empty,
                Status = "active",
                CreatedAt = DateTime.UtcNow,
            };
            await _userRepository.AddAsync(user, cancellationToken);

            var role = await _userRepository.GetRoleByNameAsync("customer", cancellationToken);
            if (role != null)
                await _userRepository.AssignRoleAsync(user.Id, role.Id, cancellationToken);
        }

        var customer = await _customerRepository.GetByUserIdAsync(user.Id, cancellationToken);
        if (customer == null)
        {
            customer = new Customer
            {
                Id = Guid.NewGuid().ToString("N"),
                UserId = user.Id,
                DisplayName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                CreatedAt = DateTime.UtcNow,
            };
            await _customerRepository.AddAsync(customer, cancellationToken);
        }

        var addresses = await _customerRepository.GetAddressesAsync(customer.Id, cancellationToken);

        return Result<CustomerDto>.Success(new CustomerDto
        {
            Id = customer.Id,
            UserId = customer.UserId,
            DisplayName = customer.DisplayName,
            DefaultAddressId = customer.DefaultAddressId,
            WalletBalance = customer.WalletBalance,
            MembershipPlanId = customer.MembershipPlanId,
            ReferralCode = customer.ReferralCode,
            ReferredByCustomerId = customer.ReferredByCustomerId,
            Phone = customer.Phone,
            Email = customer.Email,
            Addresses = addresses.Select(a => MapToDto(a)).ToList(),
        });
    }

    public async Task<Result<IReadOnlyList<CustomerAddressDto>>> GetAddressesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var addresses = await _customerRepository.GetAddressesAsync(customerId, cancellationToken);
        var dtos = addresses
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.Label)
            .Select(MapToDto)
            .ToList();

        return Result<IReadOnlyList<CustomerAddressDto>>.Success(dtos);
    }

    public async Task<Result<CustomerAddressDto>> GetAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default)
    {
        var address = await _customerRepository.GetAddressAsync(customerId, addressId, cancellationToken);
        if (address == null)
            return Result<CustomerAddressDto>.Failure("Address not found");

        return Result<CustomerAddressDto>.Success(MapToDto(address));
    }

    public async Task<Result<CustomerAddressDto>> CreateAddressAsync(string customerId, CreateCustomerAddressDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer == null)
            return Result<CustomerAddressDto>.Failure("Customer not found");

        // Validate pincode exists and is serviceable
        var pincode = await _locationRepository.GetPincodeAsync(dto.Pincode.Trim(), cancellationToken);
        if (pincode == null || !pincode.IsServiceable)
            return Result<CustomerAddressDto>.Failure("Invalid or non-serviceable pincode");

        // If this is the first address, make it default
        var existingAddresses = await _customerRepository.GetAddressesAsync(customerId, cancellationToken);
        bool isFirstAddress = existingAddresses.Count == 0;

        var address = new CustomerAddress
        {
            Id = Guid.NewGuid().ToString("N"),
            CustomerId = customerId,
            Label = dto.Label,
            Line1 = dto.Line1,
            Line2 = dto.Line2,
            CityId = dto.CityId ?? pincode.CityId,
            ZoneId = dto.ZoneId,
            LocalityId = dto.LocalityId,
            Pincode = dto.Pincode,
            Lat = dto.Lat,
            Lng = dto.Lng,
            IsDefault = dto.IsDefault || isFirstAddress,
            ContactPerson = dto.ContactPerson,
            ContactPhone = dto.ContactPhone,
            AccessInstructions = dto.AccessInstructions,
        };

        // If setting as default, unset other defaults
        if (address.IsDefault)
        {
            foreach (var existing in existingAddresses)
            {
                existing.IsDefault = false;
                await _customerRepository.UpdateAddressAsync(existing, cancellationToken);
            }
        }

        await _customerRepository.AddAddressAsync(address, cancellationToken);

        return Result<CustomerAddressDto>.Success(MapToDto(address));
    }

    public async Task<Result<CustomerAddressDto>> UpdateAddressAsync(string customerId, string addressId, UpdateCustomerAddressDto dto, CancellationToken cancellationToken = default)
    {
        var address = await _customerRepository.GetAddressAsync(customerId, addressId, cancellationToken);
        if (address == null)
            return Result<CustomerAddressDto>.Failure("Address not found");

        // Validate pincode if changed
        if (!string.IsNullOrWhiteSpace(dto.Pincode) && dto.Pincode != address.Pincode)
        {
            var pincode = await _locationRepository.GetPincodeAsync(dto.Pincode.Trim(), cancellationToken);
            if (pincode == null || !pincode.IsServiceable)
                return Result<CustomerAddressDto>.Failure("Invalid or non-serviceable pincode");
            
            address.CityId = pincode.CityId;
        }

        address.Label = dto.Label;
        address.Line1 = dto.Line1;
        address.Line2 = dto.Line2;
        address.CityId = dto.CityId ?? address.CityId;
        address.ZoneId = dto.ZoneId;
        address.LocalityId = dto.LocalityId;
        address.Pincode = string.IsNullOrWhiteSpace(dto.Pincode) ? address.Pincode : dto.Pincode;
        address.Lat = dto.Lat;
        address.Lng = dto.Lng;
        address.ContactPerson = dto.ContactPerson;
        address.ContactPhone = dto.ContactPhone;
        address.AccessInstructions = dto.AccessInstructions;

        await _customerRepository.UpdateAddressAsync(address, cancellationToken);

        return Result<CustomerAddressDto>.Success(MapToDto(address));
    }

    public async Task<Result> DeleteAddressAsync(string customerId, string addressId, CancellationToken cancellationToken = default)
    {
        var address = await _customerRepository.GetAddressAsync(customerId, addressId, cancellationToken);
        if (address == null)
            return Result.Failure("Address not found");

        var wasDefault = address.IsDefault;
        await _customerRepository.RemoveAddressAsync(address, cancellationToken);

        // If deleted was default, make another address default
        if (wasDefault)
        {
            var remaining = await _customerRepository.GetAddressesAsync(customerId, cancellationToken);
            if (remaining.Count > 0)
            {
                remaining[0].IsDefault = true;
                await _customerRepository.UpdateAddressAsync(remaining[0], cancellationToken);
            }
        }

        return Result.Success();
    }

    public async Task<Result> SetDefaultAddressAsync(string customerId, SetDefaultAddressDto dto, CancellationToken cancellationToken = default)
    {
        var address = await _customerRepository.GetAddressAsync(customerId, dto.AddressId, cancellationToken);
        if (address == null)
            return Result.Failure("Address not found");

        var allAddresses = await _customerRepository.GetAddressesAsync(customerId, cancellationToken);
        foreach (var a in allAddresses)
        {
            a.IsDefault = a.Id == dto.AddressId;
            await _customerRepository.UpdateAddressAsync(a, cancellationToken);
        }

        return Result.Success();
    }

    private static CustomerAddressDto MapToDto(CustomerAddress address)
    {
        return new CustomerAddressDto
        {
            Id = address.Id,
            CustomerId = address.CustomerId,
            Label = address.Label,
            Line1 = address.Line1,
            Line2 = address.Line2,
            CityId = address.CityId,
            ZoneId = address.ZoneId,
            LocalityId = address.LocalityId,
            Pincode = address.Pincode,
            Lat = address.Lat,
            Lng = address.Lng,
            IsDefault = address.IsDefault,
            ContactPerson = address.ContactPerson,
            ContactPhone = address.ContactPhone,
            AccessInstructions = address.AccessInstructions,
        };
    }
}
