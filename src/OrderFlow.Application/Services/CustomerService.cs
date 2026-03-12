using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Customer;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository repository, IUnitOfWork unitOfWork,
        IMapper mapper, ILogger<CustomerService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<CustomerResponseDto>> GetAllAsync(
        int page, int pageSize, string? search, string? city)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, search, city);

        return new PagedResultDto<CustomerResponseDto>
        {
            Items = _mapper.Map<List<CustomerResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<CustomerResponseDto>> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Cliente", id);

        return ApiResponseDto<CustomerResponseDto>.Success(_mapper.Map<CustomerResponseDto>(customer));
    }

    public async Task<ApiResponseDto<CustomerResponseDto>> CreateAsync(CreateCustomerDto dto)
    {
        if (await _repository.DocumentExistsAsync(dto.Document))
            throw new BusinessRuleException($"Já existe um cliente com o documento '{dto.Document}'.");

        var customer = _mapper.Map<Customer>(dto);
        await _repository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Cliente criado com sucesso. ID: {Id}", customer.Id);

        return ApiResponseDto<CustomerResponseDto>.Success(
            _mapper.Map<CustomerResponseDto>(customer), "Cliente criado com sucesso.");
    }

    public async Task<ApiResponseDto<CustomerResponseDto>> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Cliente", id);

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
        customer.Address = dto.Address;
        customer.City = dto.City;
        customer.State = dto.State;
        customer.ZipCode = dto.ZipCode;

        _repository.Update(customer);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Cliente atualizado com sucesso. ID: {Id}", customer.Id);

        return ApiResponseDto<CustomerResponseDto>.Success(
            _mapper.Map<CustomerResponseDto>(customer), "Cliente atualizado com sucesso.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Cliente", id);

        _repository.Delete(customer);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Cliente removido com sucesso. ID: {Id}", id);
    }
}