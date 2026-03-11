using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(Guid id);
    Task<(IEnumerable<Order> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, OrderStatus? status, Guid? customerId, DateTime? startDate, DateTime? endDate);
    Task<int> GetOrderCountByPeriodAsync(DateTime start, DateTime end);
    Task<decimal> GetTotalSalesByPeriodAsync(DateTime start, DateTime end);
}