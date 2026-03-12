using FluentValidation;
using OrderFlow.Application.DTOs.Auth;

namespace OrderFlow.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Nome completo é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres.")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres.")
            .Matches("[A-Z]").WithMessage("Senha deve ter pelo menos uma letra maiúscula.")
            .Matches("[a-z]").WithMessage("Senha deve ter pelo menos uma letra minúscula.")
            .Matches("[0-9]").WithMessage("Senha deve ter pelo menos um número.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Perfil de usuário inválido.");
    }
}