using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search, string? city);
    Task<bool> DocumentExistsAsync(string document, Guid? excludeId = null);
}