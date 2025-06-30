namespace SurveyBasket.Errors;

public static class PollsError
{
    public static readonly Error PollNotFound = new("Not Found Poll", " No Poll Was Found By Given Id" , StatusCodes.Status404NotFound);

    public static readonly Error PollTitleAlreadyExists = new("Poll Already Exists", "Poll With Same Title Already Exists", StatusCodes.Status409Conflict);
}