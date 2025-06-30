

namespace SurveyBasket.Contracts.Polls;

public class AuthRefreshRequestValidator : AbstractValidator<AuthRefreshRequest>
{
    public AuthRefreshRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
