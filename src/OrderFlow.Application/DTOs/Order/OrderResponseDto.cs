using OrderFlow.Application.DTOs.Payment;

namespace OrderFlow.Application.DTOs.Order;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
    public List<PaymentResponseDto>? Payments { get; set; }
    public DateTime CreatedAt { get; set; }
}