using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Stock;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Interfaces;

public interface IStockService
{
    Task<PagedResultDto<StockMovementResponseDto>> GetMovementsAsync(int page, int pageSize, Guid? productId, MovementType? type, DateTime? startDate, DateTime? endDate);
    Task<ApiResponseDto<StockMovementResponseDto>> CreateMovementAsync(CreateStockMovementDto dto, Guid userId);
}