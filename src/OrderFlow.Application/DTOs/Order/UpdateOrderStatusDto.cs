using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.DTOs.Order;

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }
}