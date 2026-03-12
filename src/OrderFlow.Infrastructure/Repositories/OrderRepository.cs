using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Items).ThenInclude(x => x.Product)
            .Include(x => x.Customer)
            .Include(x => x.User)
            .Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<(IEnumerable<Order> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, OrderStatus? status, Guid? customerId, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet
            .Include(x => x.Customer)
            .Where(x => x.IsActive);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        if (customerId.HasValue)
            query = query.Where(x => x.CustomerId == customerId.Value);

        if (startDate.HasValue)
            query = query.Where(x => x.OrderDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.OrderDate <= endDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> GetOrderCountByPeriodAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Where(x => x.IsActive &&
                        x.Status != OrderStatus.Cancelled &&
                        x.OrderDate >= start &&
                        x.OrderDate <= end)
            .CountAsync();
    }

    public async Task<decimal> GetTotalSalesByPeriodAsync(DateTime start, DateTime end)
    {
        return await _dbSet
            .Where(x => x.IsActive &&
                        x.Status != OrderStatus.Cancelled &&
                        x.OrderDate >= start &&
                        x.OrderDate <= end)
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0;
    }
}