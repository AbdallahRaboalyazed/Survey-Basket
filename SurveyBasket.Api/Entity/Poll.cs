namespace SurveyBasket.Entity;

public class Poll : AuditableEntity
{
    public int Id { get; set; }
    public String Title { get; set; } = string.Empty;
    public String Summary { get; set; } = string.Empty;
    public bool IsPuplished { get; set; }
    public DateOnly StartsAt { get; set; } 
    public DateOnly EndsAt { get; set; }
    public ICollection<Question> Questions { get; set; } = [];
    public ICollection<Vote> Votes { get; set; } = [];

}
