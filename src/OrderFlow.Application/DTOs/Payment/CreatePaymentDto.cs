using OrderFlow.Domain.Enums;

namespace OrderFlow.Application.DTOs.Payment;

public class CreatePaymentDto
{
    public Guid OrderId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}