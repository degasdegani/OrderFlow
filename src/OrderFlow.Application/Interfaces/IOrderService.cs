using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Order;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Interfaces;

public interface IOrderService
{
    Task<PagedResultDto<OrderResponseDto>> GetAllAsync(int page, int pageSize, OrderStatus? status, Guid? customerId, DateTime? startDate, DateTime? endDate);
    Task<ApiResponseDto<OrderResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<OrderResponseDto>> CreateAsync(CreateOrderDto dto, Guid userId);
    Task<ApiResponseDto<OrderResponseDto>> UpdateStatusAsync(Guid id, UpdateOrderStatusDto dto);
    Task CancelAsync(Guid id, Guid userId);
}