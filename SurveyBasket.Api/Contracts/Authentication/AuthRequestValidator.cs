using SurveyBasket.Contracts.Authentication;

namespace SurveyBasket.Contracts.Polls;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
