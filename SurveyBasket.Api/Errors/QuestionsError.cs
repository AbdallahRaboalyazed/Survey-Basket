namespace SurveyBasket.Errors;

public static class QuestionsError
{
    public static readonly Error QuestionAlreadyExists = new("Question Already Exists", "Question With Same Content Already Exists in  the same poll", StatusCodes.Status404NotFound);

    public static readonly Error DuplicateQuestion = new("Duplicate Question", "Question With Same Content Already Exists in the same poll", StatusCodes.Status409Conflict);

    public static readonly Error QuestionNotFound = new("Question Not Found", "Question Not Found in the poll", StatusCodes.Status404NotFound);

    public static Error UserAlreadyVoted = new("User Already Voted", "User Already Voted for this question", StatusCodes.Status409Conflict);
}