namespace SurveyBasket.Contracts.Polls;

public class QuestionRequestValidator : AbstractValidator<PollRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("{PropertyName} is required ya basha");
        RuleFor(x => x.Summary).NotEmpty().WithMessage("{PropertyName} is required ya basha");
        RuleFor(x => x.StartsAt).NotEmpty().WithMessage("{PropertyName} is required ya basha");
        RuleFor(x => x.EndsAt).NotEmpty().WithMessage("{PropertyName} is required ya basha");
        RuleFor(x => x.StartsAt).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("{PropertyName} should be greater than or equal to today");
       // RuleFor(x => x.EndsAt).GreaterThan(x => x.StartsAt).WithMessage("{PropertyName} should be greater than StartsAt");     عايز اعملها بميثود
       RuleFor(x => x).Must(HasValidDate).WithName(nameof(PollRequest.EndsAt))
            .WithMessage("EndsAt should be greater than StartsAt");
    }
    private bool HasValidDate(PollRequest pollRequest)
    {
        return pollRequest.EndsAt >= pollRequest.StartsAt;
    }
}
