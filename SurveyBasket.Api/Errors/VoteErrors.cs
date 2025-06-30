namespace SurveyBasket.Errors;

internal class VoteErrors
{
    public static readonly Error DuplicatedVotes = new ( "Duplicated Votes", "User Already Voted for this question", StatusCodes.Status409Conflict);

    public static readonly Error InvalidQuestions = new("Invalid Questions", "Invalid Questions you should answer all questions", StatusCodes.Status400BadRequest);
}