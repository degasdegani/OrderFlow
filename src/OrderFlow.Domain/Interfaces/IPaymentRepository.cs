using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Interfaces;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<(IEnumerable<Payment> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, Guid? orderId, PaymentStatus? status);
    Task<decimal> GetTotalPaidByOrderAsync(Guid orderId);
}