namespace OrderFlow.Application.DTOs.Dashboard;

public class SalesByPeriodDto
{
    public DateTime Date { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalAmount { get; set; }
}