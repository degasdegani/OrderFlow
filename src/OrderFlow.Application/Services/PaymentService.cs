using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Payment;
using OrderFlow.Application.Interfaces;
using OrderFlow.Domain.Entities;
using OrderFlow.Domain.Enums;
using OrderFlow.Domain.Exceptions;
using OrderFlow.Domain.Interfaces;

namespace OrderFlow.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository,
        IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResultDto<PaymentResponseDto>> GetAllAsync(
        int page, int pageSize, Guid? orderId, PaymentStatus? status)
    {
        var (items, totalCount) = await _paymentRepository.GetPagedAsync(page, pageSize, orderId, status);

        return new PagedResultDto<PaymentResponseDto>
        {
            Items = _mapper.Map<List<PaymentResponseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ApiResponseDto<PaymentResponseDto>> GetByIdAsync(Guid id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Pagamento", id);

        return ApiResponseDto<PaymentResponseDto>.Success(_mapper.Map<PaymentResponseDto>(payment));
    }

    public async Task<ApiResponseDto<PaymentResponseDto>> CreateAsync(CreatePaymentDto dto)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(dto.OrderId)
            ?? throw new NotFoundException("Pedido", dto.OrderId);

        if (order.Status == OrderStatus.Cancelled)
            throw new BusinessRuleException("Não é possível registrar pagamento para um pedido cancelado.");

        var totalPaid = await _paymentRepository.GetTotalPaidByOrderAsync(dto.OrderId);
        var remaining = order.TotalAmount - totalPaid;

        if (dto.Amount > remaining)
            throw new BusinessRuleException(
                $"Valor do pagamento (R$ {dto.Amount:F2}) excede o saldo restante (R$ {remaining:F2}).");

        var payment = new Payment
        {
            OrderId = dto.OrderId,
            PaymentMethod = dto.PaymentMethod,
            Amount = dto.Amount,
            PaymentDate = DateTime.UtcNow,
            Status = PaymentStatus.Paid,
            Notes = dto.Notes
        };

        await _paymentRepository.AddAsync(payment);

        var newTotalPaid = totalPaid + dto.Amount;
        if (newTotalPaid >= order.TotalAmount)
        {
            order.Status = OrderStatus.Confirmed;
            _orderRepository.Update(order);
        }

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("Pagamento registrado com sucesso. ID: {Id}", payment.Id);

        return ApiResponseDto<PaymentResponseDto>.Success(
            _mapper.Map<PaymentResponseDto>(payment), "Pagamento registrado com sucesso.");
    }
}