using OrderFlow.Domain.Enums;

namespace OrderFlow.Domain.Entities;

public class StockMovement : BaseEntity
{
    public Guid ProductId { get; set; }
    public MovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid UserId { get; set; }

    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;
}