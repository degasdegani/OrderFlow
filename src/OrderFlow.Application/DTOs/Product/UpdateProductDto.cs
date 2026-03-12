namespace OrderFlow.Application.DTOs.Product;

public class UpdateProductDto
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
}