using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.DTOs.Stock;

public class CreateStockMovementDto
{
    public Guid ProductId { get; set; }
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}