using FluentValidation;
using OrderFlow.Application.DTOs.Customer;

namespace OrderFlow.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Document)
            .NotEmpty().WithMessage("Documento é obrigatório.")
            .Must(doc => doc.Replace(".", "").Replace("-", "").Replace("/", "").Length == 11 ||
                         doc.Replace(".", "").Replace("-", "").Replace("/", "").Length == 14)
            .WithMessage("Documento deve ser um CPF (11 dígitos) ou CNPJ (14 dígitos) válido.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres.");

        RuleFor(x => x.City)
            .MaximumLength(200).WithMessage("Cidade deve ter no máximo 200 caracteres.");

        RuleFor(x => x.State)
            .MaximumLength(2).WithMessage("Estado deve ter no máximo 2 caracteres.");

        RuleFor(x => x.ZipCode)
            .MaximumLength(10).WithMessage("CEP deve ter no máximo 10 caracteres.");
    }
}   