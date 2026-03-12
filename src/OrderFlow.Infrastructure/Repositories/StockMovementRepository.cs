using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class StockMovementRepository : BaseRepository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<StockMovement> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, Guid? productId, MovementType? type, DateTime? startDate, DateTime? endDate)
    {
        var query = _dbSet
            .Include(x => x.Product)
            .Include(x => x.User)
            .Where(x => x.IsActive);

        if (productId.HasValue)
            query = query.Where(x => x.ProductId == productId.Value);

        if (type.HasValue)
            query = query.Where(x => x.MovementType == type.Value);

        if (startDate.HasValue)
            query = query.Where(x => x.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(x => x.CreatedAt <= endDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}