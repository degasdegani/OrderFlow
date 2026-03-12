namespace OrderFlow.Application.DTOs.Order;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public decimal Discount { get; set; }
    public string? Notes { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new();
}