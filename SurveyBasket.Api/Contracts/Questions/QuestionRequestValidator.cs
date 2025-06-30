using SurveyBasket.Contracts.Questions;
public class VoteRequestValidator : AbstractValidator<QuestionRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x=>x.Content).NotEmpty().Length(1, 500);



        RuleFor(x => x.Answers).NotNull();


        RuleFor(x => x.Answers).NotEmpty().Must(x => x.Count > 1)
        .WithMessage("Question must have at least 2 answers")
        .When(x => x.Answers != null);


        RuleFor(x => x.Answers).NotEmpty().Must(x=>x.Distinct().Count() == x.Count)
            .WithMessage("you can not have duplicate answers for the same question")
            .When(x => x.Answers != null);


    }
}
