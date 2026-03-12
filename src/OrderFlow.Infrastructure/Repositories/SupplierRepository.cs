using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Supplier> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search)
    {
        var query = _dbSet.Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.CompanyName.ToLower().Contains(search.ToLower()) ||
                x.Document.ToLower().Contains(search.ToLower()));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.CompanyName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> DocumentExistsAsync(string document, Guid? excludeId = null)
    {
        var query = _dbSet.Where(x => x.Document == document && x.IsActive);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}