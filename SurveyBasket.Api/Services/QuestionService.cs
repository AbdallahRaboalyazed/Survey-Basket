using SurveyBasket.Contracts.Common;
using System.Linq.Dynamic.Core;
namespace SurveyBasket.Services;

public class QuestionService(AppDBcontext context, HybridCache hybridCache) : IQuestionService
{
    private readonly AppDBcontext _context = context;
    private readonly HybridCache _hybridCache = hybridCache;

    private const string cacheprefix = "AvailableQuestions";

    public async Task<Result<QuestionResponse>> AddQuestionAsync(int pollid, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
    {
        // i want to check if the pollid exists in the database
        var pollidisexisted = await _context.Polls.AnyAsync(x => x.Id == pollid, cancellationToken);

        if (!pollidisexisted)
            return Result.Failure<QuestionResponse>(PollsError.PollNotFound);

        // i want to check if the question already exists in this poll
        var questionalreadyexisted = await _context.Questions.AnyAsync(x => x.Content == questionRequest.Content && x.PollId == pollid, cancellationToken);

        if (questionalreadyexisted)
            return Result.Failure<QuestionResponse>(QuestionsError.QuestionAlreadyExists);

        // add the answers which is in the questionRequest to the database
        var question = questionRequest.Adapt<Question>();
        question.PollId = pollid;

        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        await _hybridCache.RemoveAsync($"{cacheprefix}-{pollid}", cancellationToken);

        var response = question.Adapt<QuestionResponse>();

        return Result.Success(response);
    }

    public async Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);

        if (!pollIsExists)
            return Result.Failure<PaginatedList<QuestionResponse>>(PollsError.PollNotFound);

        var query = _context.Questions
            .Where(x => x.PollId == pollId);

        if (!string.IsNullOrEmpty(filters.SearchValue))
        {
            query = query.Where(x => x.Content.Contains(filters.SearchValue));
        }

        if (!string.IsNullOrEmpty(filters.SortColumn))
        {
            query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
        }

        var source = query
                        .Include(x => x.Answers)
                        .ProjectToType<QuestionResponse>()
                        .AsNoTracking();

        var questions = await PaginatedList<QuestionResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize, cancellationToken);

        return Result.Success(questions);
    }

    public async Task<Result<QuestionResponse>> GetAsync(int pollid, int questionid, CancellationToken cancellationToken = default)
    {
        // get the question with id and at the same time with the same pollid
        var question = await _context.Questions
            .Where(x => x.Id == questionid && x.PollId == pollid)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (question == null)
            return Result.Failure<QuestionResponse>(QuestionsError.QuestionNotFound);

        return Result.Success(question);
    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailable(int pollid, string userid, CancellationToken cancellationToken = default)
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
            return Result.Failure<IEnumerable<QuestionResponse>>(PollsError.PollNotFound);

        // get the questions and the answers for this pollid and i want the answer be active too
        var cachekey = $"{cacheprefix}-{pollid}";

        var questions = await _hybridCache.GetOrCreateAsync(cachekey, async cachEntry =>
              await _context.Questions
            .Where(x => x.PollId == pollid && x.IsActive)
            .Include(x => x.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken)
        );

        return Result.Success<IEnumerable<QuestionResponse>>(questions);
    }

    public async Task<Result> ToggleStatusAsync(int pollid, int questionid, CancellationToken cancellationToken = default)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == questionid && x.PollId == pollid, cancellationToken);

        if (question == null)
            return Result.Failure(QuestionsError.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        await _hybridCache.RemoveAsync($"{cacheprefix}-{pollid}" , cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateQuestionAndThierAnswersAsync(int pollid, int questionid, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
    {
        var questioncontentisexist = await _context.Questions.
              AnyAsync(x => x.PollId == pollid &&
              x.Id != questionid && x.Content == questionRequest.Content, cancellationToken);

        if (questioncontentisexist)
            return Result.Failure(QuestionsError.DuplicateQuestion);
        var question = await _context.Questions.Where(x => x.Id == questionid && x.PollId == pollid)
                    .Include(x => x.Answers)
                    .SingleOrDefaultAsync(cancellationToken);
        if (question == null)
            return Result.Failure(QuestionsError.QuestionNotFound);


        question.Content = questionRequest.Content;

        // current content answers
        var currentAnswers = question.Answers.Select(x => x.Content).ToList();

        // get new content answers
        var newAnswers = questionRequest.Answers.Except(currentAnswers).ToList();

        // add new answers to the database
        foreach (var answer in newAnswers)
        {
            question.Answers.Add(new Answer { Content = answer });
        }

        // deactive the unfound answers in the request answers and found in the question
        foreach (var answer in question.Answers)
        {
            answer.IsActive = questionRequest.Answers.Contains(answer.Content);
        }

        await _context.SaveChangesAsync(cancellationToken);

        await _hybridCache.RemoveAsync($"{cacheprefix}-{pollid}", cancellationToken);

        return Result.Success();
    }
}
