using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Customer;

namespace OrderFlow.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResultDto<CustomerResponseDto>> GetAllAsync(int page, int pageSize, string? search, string? city);
    Task<ApiResponseDto<CustomerResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<CustomerResponseDto>> CreateAsync(CreateCustomerDto dto);
    Task<ApiResponseDto<CustomerResponseDto>> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task DeleteAsync(Guid id);
}