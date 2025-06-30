namespace SurveyBasket.Services;

public interface IResultService
{
    public Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId , CancellationToken cancellationToken = default);

    public Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default);

    public Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
}
