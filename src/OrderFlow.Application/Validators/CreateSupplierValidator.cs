using FluentValidation;
using OrderFlow.Application.DTOs.Supplier;

namespace OrderFlow.Application.Validators;

public class CreateSupplierValidator : AbstractValidator<CreateSupplierDto>
{
    public CreateSupplierValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Razão social é obrigatória.")
            .MinimumLength(3).WithMessage("Razão social deve ter no mínimo 3 caracteres.")
            .MaximumLength(200).WithMessage("Razão social deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Must(doc => doc.Replace(".", "").Replace("-", "").Replace("/", "").Length == 14)
            .WithMessage("CNPJ deve ter 14 dígitos.");

        RuleFor(x => x.ContactName)
            .MaximumLength(150).WithMessage("Nome do contato deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres.");
    }
}