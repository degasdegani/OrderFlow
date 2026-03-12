using FluentValidation;
using OrderFlow.Application.DTOs.Payment;

namespace OrderFlow.Application.Validators;

public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Pedido é obrigatório.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Forma de pagamento inválida.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Valor do pagamento deve ser maior que zero.");
    }
}