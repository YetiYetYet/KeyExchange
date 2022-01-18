using API.DTO;
using API.DTO.Authentification;
using FluentValidation;

namespace API.Utils.Validator;

public class RegisterUserRequestValidator : CustomValidator<RegisterDto>
{
    public RegisterUserRequestValidator(RegisterRequirementSettings? registerRequirementSettings = null)
    {
        RuleFor(p => p.Password).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(6).MaximumLength(20);
        RuleFor(p => p.Username).Cascade(CascadeMode.Stop).NotEmpty().MinimumLength(6).MaximumLength(20);
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress();
        RuleFor(p => p.ConfirmEmail).Cascade(CascadeMode.Stop).NotEmpty().EmailAddress().Must((model, field) => field == model.Email);;
        RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop).NotEmpty().Must((model, field) => field == model.Password);
    }
}