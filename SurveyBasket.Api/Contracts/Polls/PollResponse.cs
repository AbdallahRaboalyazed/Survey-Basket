namespace SurveyBasket.Contracts.Polls;

public record PollResponse ( 
    int Id ,
    string Title ,
    string Summary ,
    bool IsPuplished ,
    DateOnly StartsAt,
    DateOnly EndsAt
    );

