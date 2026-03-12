using FluentValidation;
using OrderFlow.Application.DTOs.Order;

namespace OrderFlow.Application.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Cliente é obrigatório.");

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Desconto deve ser maior ou igual a zero.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("O pedido deve ter pelo menos um item.")
            .Must(items => items.Count <= 50).WithMessage("O pedido não pode ter mais de 50 itens.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Produto é obrigatório.");

            item.RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.")
                .LessThanOrEqualTo(9999).WithMessage("Quantidade não pode ser maior que 9999.");

            item.RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Desconto do item deve ser maior ou igual a zero.");
        });
    }
}