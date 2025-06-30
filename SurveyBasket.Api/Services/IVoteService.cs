namespace SurveyBasket.Services;

public interface IVoteService
{
    public Task<Result> AddAsync(int pollid, string userid, VoteRequest voteRequest  ,  CancellationToken cancellationToken);
}
