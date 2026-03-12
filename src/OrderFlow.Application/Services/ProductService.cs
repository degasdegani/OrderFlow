using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Product;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, IUnitOfWork unitOfWork,
        IMapper mapper, ILogger<ProductService> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<ProductResponseDto>> GetAllAsync(
        int page, int pageSize, string? search, Guid? categoryId, bool? lowStock)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(page, pageSize, search, categoryId, lowStock);

        return new PagedResultDto<ProductResponseDto>
        {
            Items = _mapper.Map<List<ProductResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<ProductResponseDto>> GetByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Produto", id);

        return ApiResponseDto<ProductResponseDto>.Success(_mapper.Map<ProductResponseDto>(product));
    }

    public async Task<ApiResponseDto<List<ProductResponseDto>>> GetLowStockAsync()
    {
        var products = await _repository.GetLowStockAsync();
        return ApiResponseDto<List<ProductResponseDto>>.Success(
            _mapper.Map<List<ProductResponseDto>>(products));
    }

    public async Task<ApiResponseDto<ProductResponseDto>> CreateAsync(CreateProductDto dto)
    {
        if (await _repository.SkuExistsAsync(dto.SKU))
            throw new BusinessRuleException($"Já existe um produto com o SKU '{dto.SKU}'.");

        var product = _mapper.Map<Product>(dto);
        await _repository.AddAsync(product);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Produto criado com sucesso. ID: {Id}", product.Id);

        var created = await _repository.GetByIdAsync(product.Id);
        return ApiResponseDto<ProductResponseDto>.Success(
            _mapper.Map<ProductResponseDto>(created), "Produto criado com sucesso.");
    }

    public async Task<ApiResponseDto<ProductResponseDto>> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Produto", id);

        if (await _repository.SkuExistsAsync(dto.SKU, id))
            throw new BusinessRuleException($"Já existe um produto com o SKU '{dto.SKU}'.");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.SKU = dto.SKU;
        product.BarCode = dto.BarCode;
        product.CategoryId = dto.CategoryId;
        product.SupplierId = dto.SupplierId;
        product.CostPrice = dto.CostPrice;
        product.SalePrice = dto.SalePrice;
        product.StockQuantity = dto.StockQuantity;
        product.MinimumStock = dto.MinimumStock;
        product.Unit = dto.Unit;

        _repository.Update(product);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Produto atualizado com sucesso. ID: {Id}", product.Id);

        var updated = await _repository.GetByIdAsync(product.Id);
        return ApiResponseDto<ProductResponseDto>.Success(
            _mapper.Map<ProductResponseDto>(updated), "Produto atualizado com sucesso.");
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Produto", id);

        _repository.Delete(product);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Produto removido com sucesso. ID: {Id}", id);
    }
}