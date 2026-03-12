using AutoMapper;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Dashboard;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public DashboardService(ICustomerRepository customerRepository, IProductRepository productRepository,
        IOrderRepository orderRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<DashboardSummaryDto>> GetSummaryAsync()
    {
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1);

        var customers = await _customerRepository.GetAllAsync();
        var products = await _productRepository.GetAllAsync();
        var lowStockProducts = await _productRepository.GetLowStockAsync();

        var ordersToday = await _orderRepository.GetOrderCountByPeriodAsync(today, today.AddDays(1));
        var revenueToday = await _orderRepository.GetTotalSalesByPeriodAsync(today, today.AddDays(1));
        var ordersMonth = await _orderRepository.GetOrderCountByPeriodAsync(startOfMonth, today.AddDays(1));
        var revenueMonth = await _orderRepository.GetTotalSalesByPeriodAsync(startOfMonth, today.AddDays(1));

        var (pendingOrders, _) = await _orderRepository.GetPagedAsync(1, 1000, OrderStatus.Pending, null, null, null);

        var summary = new DashboardSummaryDto
        {
            TotalCustomers = customers.Count(),
            TotalProducts = products.Count(),
            LowStockProductsCount = lowStockProducts.Count(),
            OrdersToday = ordersToday,
            RevenueToday = revenueToday,
            OrdersThisMonth = ordersMonth,
            RevenueThisMonth = revenueMonth,
            PendingOrdersCount = pendingOrders.Count()
        };

        return ApiResponseDto<DashboardSummaryDto>.Success(summary);
    }

    public async Task<ApiResponseDto<List<SalesByPeriodDto>>> GetSalesByPeriodAsync(
        DateTime startDate, DateTime endDate)
    {
        var result = new List<SalesByPeriodDto>();
        var current = startDate.Date;

        while (current <= endDate.Date)
        {
            var nextDay = current.AddDays(1);
            var count = await _orderRepository.GetOrderCountByPeriodAsync(current, nextDay);
            var total = await _orderRepository.GetTotalSalesByPeriodAsync(current, nextDay);

            result.Add(new SalesByPeriodDto
            {
                Date = current,
                OrderCount = count,
                TotalAmount = total
            });

            current = nextDay;
        }

        return ApiResponseDto<List<SalesByPeriodDto>>.Success(result);
    }
}