using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Order;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository,
        IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<OrderResponseDto>> GetAllAsync(
        int page, int pageSize, OrderStatus? status, Guid? customerId, DateTime? startDate, DateTime? endDate)
    {
        var (items, totalCount) = await _orderRepository.GetPagedAsync(page, pageSize, status, customerId, startDate, endDate);

        return new PagedResultDto<OrderResponseDto>
        {
            Items = _mapper.Map<List<OrderResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<OrderResponseDto>> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id)
            ?? throw new NotFoundException("Pedido", id);

        return ApiResponseDto<OrderResponseDto>.Success(_mapper.Map<OrderResponseDto>(order));
    }

    public async Task<ApiResponseDto<OrderResponseDto>> CreateAsync(CreateOrderDto dto, Guid userId)
    {
        if (!dto.Items.Any())
            throw new BusinessRuleException("O pedido deve ter pelo menos um item.");

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Discount = dto.Discount,
            Notes = dto.Notes
        };

        decimal subTotal = 0;

        foreach (var itemDto in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId)
                ?? throw new NotFoundException("Produto", itemDto.ProductId);

            if (product.StockQuantity < itemDto.Quantity)
                throw new BusinessRuleException(
                    $"Estoque insuficiente para o produto '{product.Name}'. " +
                    $"Disponível: {product.StockQuantity}, Solicitado: {itemDto.Quantity}.");

            var totalPrice = (product.SalePrice * itemDto.Quantity) - itemDto.Discount;

            var item = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.SalePrice,
                Discount = itemDto.Discount,
                TotalPrice = totalPrice
            };

            order.Items.Add(item);
            subTotal += totalPrice;

            product.StockQuantity -= itemDto.Quantity;
            _productRepository.Update(product);
        }

        order.SubTotal = subTotal;
        order.TotalAmount = subTotal - dto.Discount;

        await _orderRepository.AddAsync(order);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Pedido criado com sucesso. ID: {Id}", order.Id);

        var created = await _orderRepository.GetByIdWithItemsAsync(order.Id);
        return ApiResponseDto<OrderResponseDto>.Success(
            _mapper.Map<OrderResponseDto>(created), "Pedido criado com sucesso.");
    }

    public async Task<ApiResponseDto<OrderResponseDto>> UpdateStatusAsync(Guid id, UpdateOrderStatusDto dto)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id)
            ?? throw new NotFoundException("Pedido", id);

        if (order.Status == OrderStatus.Cancelled)
            throw new BusinessRuleException("Não é possível alterar o status de um pedido cancelado.");

        if (order.Status == OrderStatus.Delivered)
            throw new BusinessRuleException("Não é possível alterar o status de um pedido entregue.");

        order.Status = dto.Status;
        _orderRepository.Update(order);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Status do pedido atualizado. ID: {Id}, Status: {Status}", id, dto.Status);

        return ApiResponseDto<OrderResponseDto>.Success(
            _mapper.Map<OrderResponseDto>(order), "Status atualizado com sucesso.");
    }

    public async Task CancelAsync(Guid id, Guid userId)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(id)
            ?? throw new NotFoundException("Pedido", id);

        if (order.Status == OrderStatus.Cancelled)
            throw new BusinessRuleException("Pedido já está cancelado.");

        if (order.Status == OrderStatus.Delivered)
            throw new BusinessRuleException("Não é possível cancelar um pedido já entregue.");

        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
                _productRepository.Update(product);
            }
        }

        order.Status = OrderStatus.Cancelled;
        _orderRepository.Update(order);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Pedido cancelado. ID: {Id}", id);
    }
}