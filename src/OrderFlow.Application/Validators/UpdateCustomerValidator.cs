using FluentValidation;
using OrderFlow.Application.DTOs.Customer;

namespace OrderFlow.Application.Validators;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

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