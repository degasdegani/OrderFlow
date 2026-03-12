namespace OrderFlow.Application.DTOs.Product;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? BarCode { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinimumStock { get; set; }
    public string Unit { get; set; } = string.Empty;
    public bool IsLowStock { get; set; }
    public decimal ProfitMargin { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}