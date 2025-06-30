namespace SurveyBasket.Entity;

public sealed class Vote
{
    public int Id { get; set; }
    public int PollId { get; set; }

    public String? UserId { get; set; }
    public DateTime SubmitedOn { get; set; } = DateTime.UtcNow;
    public Poll Poll { get; set; } = default!;

    public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];

    public ApplicationUser? User { get; set; }
}
