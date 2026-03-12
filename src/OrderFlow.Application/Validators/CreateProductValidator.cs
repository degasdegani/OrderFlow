using FluentValidation;
using OrderFlow.Application.DTOs.Product;

namespace OrderFlow.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres.");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU é obrigatório.")
            .MaximumLength(50).WithMessage("SKU deve ter no máximo 50 caracteres.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Categoria é obrigatória.");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Fornecedor é obrigatório.");

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Preço de custo deve ser maior ou igual a zero.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("Preço de venda deve ser maior que zero.")
            .GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("Preço de venda deve ser maior ou igual ao preço de custo.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantidade em estoque deve ser maior ou igual a zero.");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("Estoque mínimo deve ser maior ou igual a zero.");

        RuleFor(x => x.Unit)
            .NotEmpty().WithMessage("Unidade é obrigatória.")
            .MaximumLength(10).WithMessage("Unidade deve ter no máximo 10 caracteres.");
    }
}