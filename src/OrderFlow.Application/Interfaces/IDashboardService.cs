using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Dashboard;

namespace OrderFlow.Application.Interfaces;

public interface IDashboardService
{
    Task<ApiResponseDto<DashboardSummaryDto>> GetSummaryAsync();
    Task<ApiResponseDto<List<SalesByPeriodDto>>> GetSalesByPeriodAsync(DateTime startDate, DateTime endDate);
}