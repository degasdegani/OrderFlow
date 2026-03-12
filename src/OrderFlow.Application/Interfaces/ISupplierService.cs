using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Supplier;

namespace OrderFlow.Application.Interfaces;

public interface ISupplierService
{
    Task<PagedResultDto<SupplierResponseDto>> GetAllAsync(int page, int pageSize, string? search);
    Task<ApiResponseDto<SupplierResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<SupplierResponseDto>> CreateAsync(CreateSupplierDto dto);
    Task<ApiResponseDto<SupplierResponseDto>> UpdateAsync(Guid id, UpdateSupplierDto dto);
    Task DeleteAsync(Guid id);
}