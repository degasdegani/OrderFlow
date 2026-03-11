using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, Guid? categoryId, bool? lowStock);
    Task<IEnumerable<Product>> GetLowStockAsync();
    Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null);
}