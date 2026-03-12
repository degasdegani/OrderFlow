using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Supplier;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(ISupplierRepository repository, IUnitOfWork unitOfWork,
        IMapper mapper, ILogger<SupplierService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<SupplierResponseDto>> GetAllAsync(
        int page, int pageSize, string? search)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, search);

        return new PagedResultDto<SupplierResponseDto>
        {
            Items = _mapper.Map<List<SupplierResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<SupplierResponseDto>> GetByIdAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Fornecedor", id);

        return ApiResponseDto<SupplierResponseDto>.Success(_mapper.Map<SupplierResponseDto>(supplier));
    }

    public async Task<ApiResponseDto<SupplierResponseDto>> CreateAsync(CreateSupplierDto dto)
    {
        if (await _repository.DocumentExistsAsync(dto.Document))
            throw new BusinessRuleException($"Já existe um fornecedor com o documento '{dto.Document}'.");

        var supplier = _mapper.Map<Supplier>(dto);
        await _repository.AddAsync(supplier);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Fornecedor criado com sucesso. ID: {Id}", supplier.Id);

        return ApiResponseDto<SupplierResponseDto>.Success(
            _mapper.Map<SupplierResponseDto>(supplier), "Fornecedor criado com sucesso.");
    }

    public async Task<ApiResponseDto<SupplierResponseDto>> UpdateAsync(Guid id, UpdateSupplierDto dto)
    {
        var supplier = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Fornecedor", id);

        supplier.CompanyName = dto.CompanyName;
        supplier.ContactName = dto.ContactName;
        supplier.Email = dto.Email;
        supplier.Phone = dto.Phone;
        supplier.Address = dto.Address;
        supplier.City = dto.City;
        supplier.State = dto.State;
        supplier.ZipCode = dto.ZipCode;

        _repository.Update(supplier);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Fornecedor atualizado com sucesso. ID: {Id}", supplier.Id);

        return ApiResponseDto<SupplierResponseDto>.Success(
            _mapper.Map<SupplierResponseDto>(supplier), "Fornecedor atualizado com sucesso.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Fornecedor", id);

        _repository.Delete(supplier);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Fornecedor removido com sucesso. ID: {Id}", id);
    }
}