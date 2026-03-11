using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities;

public class Payment : BaseEntity
{
    public Guid OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string? Notes { get; set; }

    public Order Order { get; set; } = null!;
}