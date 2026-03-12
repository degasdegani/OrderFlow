using FluentValidation;
using OrderFlow.Application.DTOs.Stock;

namespace OrderFlow.Application.Validators;

public class CreateStockMovementValidator : AbstractValidator<CreateStockMovementDto>
{
    public CreateStockMovementValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Produto é obrigatório.");

        RuleFor(x => x.MovementType)
            .IsInEnum().WithMessage("Tipo de movimentação inválido.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Motivo é obrigatório.")
            .MinimumLength(3).WithMessage("Motivo deve ter no mínimo 3 caracteres.")
            .MaximumLength(500).WithMessage("Motivo deve ter no máximo 500 caracteres.");
    }
}