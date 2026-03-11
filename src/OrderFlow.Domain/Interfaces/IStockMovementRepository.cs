using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Interfaces;

public interface IStockMovementRepository : IRepository<StockMovement>
{
    Task<(IEnumerable<StockMovement> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, Guid? productId, MovementType? type, DateTime? startDate, DateTime? endDate);
}