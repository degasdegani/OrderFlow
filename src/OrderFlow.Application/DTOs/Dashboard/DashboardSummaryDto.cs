namespace OrderFlow.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalCustomers { get; set; }
    public int TotalProducts { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int OrdersToday { get; set; }
    public decimal RevenueToday { get; set; }
    public int OrdersThisMonth { get; set; }
    public decimal RevenueThisMonth { get; set; }
    public int LowStockProductsCount { get; set; }
    public int PendingOrdersCount { get; set; }
    public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
}