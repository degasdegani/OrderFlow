using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, Guid? orderId, PaymentStatus? status)
    {
        var query = _dbSet
            .Include(x => x.Order).ThenInclude(x => x.Customer)
            .Where(x => x.IsActive);

        if (orderId.HasValue)
            query = query.Where(x => x.OrderId == orderId.Value);

        if (status.HasValue)
            query = query.Where(x => x.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.PaymentDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<decimal> GetTotalPaidByOrderAsync(Guid orderId)
    {
        return await _dbSet
            .Where(x => x.IsActive &&
                        x.OrderId == orderId &&
                        x.Status == PaymentStatus.Paid)
            .SumAsync(x => (decimal?)x.Amount) ?? 0;
    }
}