using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Interfaces;
using OrderFlow.Infrastructure.Data;

namespace OrderFlow.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search, string? city)
    {
        var query = _dbSet.Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.ToLower().Contains(search.ToLower()) ||
                x.Document.ToLower().Contains(search.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(x => x.City.ToLower() == city.ToLower());
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Name)
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