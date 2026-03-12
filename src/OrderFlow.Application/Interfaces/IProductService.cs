using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Product;

namespace OrderFlow.Application.Interfaces;

public interface IProductService
{
    Task<PagedResultDto<ProductResponseDto>> GetAllAsync(int page, int pageSize, string? search, Guid? categoryId, bool? lowStock);
    Task<ApiResponseDto<ProductResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<List<ProductResponseDto>>> GetLowStockAsync();
    Task<ApiResponseDto<ProductResponseDto>> CreateAsync(CreateProductDto dto);
    Task<ApiResponseDto<ProductResponseDto>> UpdateAsync(Guid id, UpdateProductDto dto);
    Task DeleteAsync(Guid id);
}