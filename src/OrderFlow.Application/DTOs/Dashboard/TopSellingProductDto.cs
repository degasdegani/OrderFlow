namespace OrderFlow.Application.DTOs.Dashboard;

public class TopSellingProductDto
{
    public string ProductName { get; set; } = string.Empty;
    public int TotalSold { get; set; }
}