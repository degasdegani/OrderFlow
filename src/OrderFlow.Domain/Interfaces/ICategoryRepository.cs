using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<(IEnumerable<Category> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search);
}