using OrderFlow.Domain.Entities;

namespace OrderFlow.Domain.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<(IEnumerable<Supplier> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? search);
    Task<bool> DocumentExistsAsync(string document, Guid? excludeId = null);
}