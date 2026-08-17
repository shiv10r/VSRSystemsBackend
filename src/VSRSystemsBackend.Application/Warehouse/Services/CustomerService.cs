using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Application.Warehouse.Interfaces;
using VSRSystemsBackend.Core.Common;
using VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Application.Warehouse.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto>> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByGstinAsync(dto.Gstin, cancellationToken))
            return Result<CustomerDto>.Failure("Customer with this GSTIN already exists");

        var customer = _mapper.Map<Customer>(dto);
        customer.Id = Guid.NewGuid().ToString("N")[..20];
        customer.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(customer, cancellationToken);
        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
    }

    public async Task<Result<CustomerDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return Result<CustomerDto>.Failure("Customer not found");

        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
    }

    public async Task<Result<CustomerDto>> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByGstinAsync(gstin, cancellationToken);
        if (customer == null)
            return Result<CustomerDto>.Failure("Customer not found");

        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
    }

    public async Task<Result<PagedResult<CustomerDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => c.Name.Contains(request.SearchTerm) || c.Company.Contains(request.SearchTerm) || c.Gstin.Contains(request.SearchTerm));
        }

        var totalCount = await _repository.CountAsync(cancellationToken: cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortDescending 
                ? query.OrderByDescending(e => EF.Property<object>(e, request.SortBy))
                : query.OrderBy(e => EF.Property<object>(e, request.SortBy));
        }
        else
        {
            query = query.OrderBy(c => c.Name);
        }

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result<PagedResult<CustomerDto>>.Success(
            PagedResult<CustomerDto>.Create(
                _mapper.Map<List<CustomerDto>>(items),
                totalCount,
                request.PageNumber,
                request.PageSize
            )
        );
    }

    public async Task<Result<CustomerDto>> UpdateAsync(string id, UpdateCustomerDto dto, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return Result<CustomerDto>.Failure("Customer not found");

        if (dto.Gstin != customer.Gstin && await _repository.ExistsByGstinAsync(dto.Gstin, cancellationToken))
            return Result<CustomerDto>.Failure("Customer with this GSTIN already exists");

        _mapper.Map(dto, customer);
        customer.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(customer, cancellationToken);
        return Result<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer));
    }

    public async Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        if (customer == null)
            return Result.Failure("Customer not found");

        await _repository.DeleteAsync(customer, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<CustomerDto>>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _repository.GetActiveCustomersAsync(cancellationToken);
        return Result<IReadOnlyList<CustomerDto>>.Success(_mapper.Map<List<CustomerDto>>(customers));
    }
}