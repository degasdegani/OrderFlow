using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Category)
            .Include(x => x.Supplier)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, Guid? categoryId, bool? lowStock)
    {
        var query = _dbSet
            .Include(x => x.Category)
            .Include(x => x.Supplier)
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.ToLower().Contains(search.ToLower()) ||
                x.SKU.ToLower().Contains(search.ToLower()) ||
                x.Description.ToLower().Contains(search.ToLower()));
        }

        if (categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);

        if (lowStock == true)
            query = query.Where(x => x.StockQuantity <= x.MinimumStock);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync()
    {
        return await _dbSet
            .Include(x => x.Category)
            .Where(x => x.IsActive && x.StockQuantity <= x.MinimumStock)
            .OrderBy(x => x.StockQuantity)
            .ToListAsync();
    }

    public async Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null)
    {
        var query = _dbSet.Where(x => x.SKU == sku && x.IsActive);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync();
    }
}