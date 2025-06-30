namespace SurveyBasket.Services;

public class VoteService(AppDBcontext context) : IVoteService
{
    private readonly AppDBcontext _context = context;
    public async Task<Result> AddAsync(int pollid, string userid, VoteRequest voteRequest, CancellationToken cancellationToken)
    {

        // check if the user already voted for this poll
        //  var useralreadyvoted = _context.Votes.Any(x => x.PollId == pollid && x.UserId == userid);
        //  if (useralreadyvoted)
        //     return Result.Failure<QuestionResponse>(QuestionsError.UserAlreadyVoted);


        // check if the pollid exists in the database
        var currentPoll = await _context.Polls
         .Where(x => x.IsPuplished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                 .ProjectToType<PollResponse>().ToListAsync(cancellationToken);

        if (currentPoll == null)
            return Result.Failure(PollsError.PollNotFound);

        // i want to check the questions that you will send to me this already exists in the poll which you want to vote for
        // ex  : if i have questions'ids 1,2,3,4,5 and you want to vote for those questions i should ensure that the ids that you sent to me 1,2,3,4,5 too

        var availablequestions = await _context.Questions
            .Where(x => x.PollId == pollid)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var questionsidsofrequestAnswers = voteRequest.Answers.Select(x => x.QuestionId).ToList();

        //  if (!questionsids.All(x => questions.Contains(x)))
        //    return Result.Failure(QuestionsError.QuestionNotFound);
        // or
        var yup  = questionsidsofrequestAnswers.SequenceEqual(availablequestions);

        if (!yup)
            return Result.Failure(VoteErrors.InvalidQuestions);

        // i checked everything now i can add the vote but i should convert the VoteRequest to Vote manually

        var vote = new Vote
        {
            PollId = pollid,
            UserId = userid,
            VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _context.Votes.AddAsync(vote, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();


    }
}
