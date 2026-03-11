using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities;

public class Order : BaseEntity
{
    public int OrderNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }

    public Customer Customer { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}