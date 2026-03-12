using OrderFlow.Application.DTOs.Category;
using OrderFlow.Application.DTOs.Common;

namespace OrderFlow.Application.Interfaces;

public interface ICategoryService
{
    Task<PagedResultDto<CategoryResponseDto>> GetAllAsync(int page, int pageSize, string? search);
    Task<ApiResponseDto<CategoryResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<CategoryResponseDto>> CreateAsync(CreateCategoryDto dto);
    Task<ApiResponseDto<CategoryResponseDto>> UpdateAsync(Guid id, UpdateCategoryDto dto);
    Task DeleteAsync(Guid id);
}