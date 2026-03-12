using OrderFlow.Application.DTOs.Common;
using OrderFlow.Application.DTOs.Payment;
using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.Interfaces;

public interface IPaymentService
{
    Task<PagedResultDto<PaymentResponseDto>> GetAllAsync(int page, int pageSize, Guid? orderId, PaymentStatus? status);
    Task<ApiResponseDto<PaymentResponseDto>> GetByIdAsync(Guid id);
    Task<ApiResponseDto<PaymentResponseDto>> CreateAsync(CreatePaymentDto dto);
}