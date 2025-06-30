namespace SurveyBasket.Services;

public class ResultService(AppDBcontext context) : IResultService
{
    private readonly AppDBcontext _context = context;
    public async Task<Result<PollVoteResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollvote = await _context.Polls
            .Where(x => x.Id == pollId)
            .Select(x => new PollVoteResponse(
                x.Title,
                x.Votes.Select(v => new VoteResponse(
                    $"{v.User.FirstName} - {v.User.LastName}",
                    v.SubmitedOn,
                    v.VoteAnswers.Select( a=> new QuestionAnswerResponse(
                        a.Question.Content,
                        a.Answer.Content
                        ))
                        ))  
                        )).SingleOrDefaultAsync(cancellationToken);

        if (pollvote == null)
            return Result.Failure<PollVoteResponse>(PollsError.PollNotFound);

        return Result.Success(pollvote);
    }

    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetPollVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollexists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollexists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollsError.PollNotFound);

        var voteperday = await _context.Votes
            .Where(x => x.PollId == pollId)
            .GroupBy(x => x.SubmitedOn.Date)
            .Select(x => new VotesPerDayResponse(DateOnly.FromDateTime(x.Key), x.Count()))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerDayResponse>>(voteperday);
    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {

        var pollexists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollexists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollsError.PollNotFound);


        var votesPerQuestion = await _context.Questions
     .Where(q => q.PollId == pollId) // جلب الأسئلة داخل الاستبيان المحدد
     .Select(q => new VotesPerQuestionResponse(
         q.Content, // نص السؤال
         q.Answers.Select(a => new VotesPerAnswerResponse(
             a.Content, // نص الإجابة
             a.Vote.Count() // حساب عدد مرات التصويت لكل إجابة
         ))
     ))
     .ToListAsync(cancellationToken);

 return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);

    }  


}
