namespace OrderFlow.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? BarCode { get; set; }
    public Guid CategoryId { get; set; }
    public Guid SupplierId { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStock { get; set; }
    public string Unit { get; set; } = "UN";

    public Category Category { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public bool IsLowStock => StockQuantity <= MinimumStock;
    public decimal ProfitMargin => CostPrice == 0 ? 0 : Math.Round((SalePrice - CostPrice) / CostPrice * 100, 2);
}