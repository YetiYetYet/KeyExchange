using API.DTO;
using FluentValidation;

namespace API.Utils.Validator;

public class RegisterUserRequestValidator : CustomValidator<RegisterDto>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(p => p.Password).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(5);
        RuleFor(p => p.Username).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(5);
        RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop).NotEmpty().Must((model, field) => field == model.Password);
    }
}