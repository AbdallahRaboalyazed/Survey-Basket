using Hangfire;
using SurveyBasket.Entity;

namespace SurveyBasket.Services;

public class PollService(AppDBcontext context , INotificationService notificationService) : IPollService
{

    private readonly AppDBcontext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var polls = await _context.Polls.AsNoTracking().ProjectToType<PollResponse>().ToListAsync(cancellationToken);
        return polls;
    }
    public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var poll = await _context.Polls.FindAsync(id, cancellationToken);
        if (poll is null)
        {
            return Result.Failure<PollResponse>(PollsError.PollNotFound);
        }
        return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
    }
    public async Task<Result<PollResponse>> AddAsync(PollRequest pollrequest, CancellationToken cancellationToken = default )
    {
        var isExcist = await _context.Polls.AnyAsync(x => x.Title == pollrequest.Title, cancellationToken: cancellationToken);

        if (isExcist)
        {
            return Result.Failure<PollResponse>(PollsError.PollTitleAlreadyExists);
        }

        var poll = pollrequest.Adapt<Poll>();
        await _context.AddAsync(poll , cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        var pollresponse = poll.Adapt<PollResponse>();
        return Result.Success(pollresponse);
    }

   public async Task<Result>  UpdateAsync(int id, PollRequest pollrequest, CancellationToken cancellationToken = default)
    {

        var isExcist = await _context.Polls.AnyAsync(x => x.Title == pollrequest.Title && x.Id != id, cancellationToken: cancellationToken);

        if (isExcist)
        {
            return Result.Failure<PollResponse>(PollsError.PollTitleAlreadyExists);
        }

        var currentpoll = await _context.Polls.FindAsync(id, cancellationToken);

        if (currentpoll is null)
        {
            return Result.Failure(PollsError.PollNotFound);
        }

        currentpoll.Title = pollrequest.Title;
        currentpoll.Summary = pollrequest.Summary;
        currentpoll.StartsAt = pollrequest.StartsAt;
        currentpoll.EndsAt = pollrequest.EndsAt;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll is null)
        {
            return Result.Failure(PollsError.PollNotFound);
        }
        _context.Polls.Remove(currentPoll);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ToggleIsPpublishedAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await _context.Polls.FindAsync(id, cancellationToken);
        if (currentPoll is null)
        {
            return Result.Failure(PollsError.PollNotFound);
        }
        currentPoll.IsPuplished = !currentPoll.IsPuplished;
        await _context.SaveChangesAsync(cancellationToken);

        if (currentPoll.IsPuplished && currentPoll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotification(currentPoll.Id));


        return  Result.Success();


    }
    public async Task<IEnumerable<PollResponse>> GetCurrentPollAsync(CancellationToken cancellationToken = default)
    {
        // get all availble polls which are published and not expired
        var currentPoll = await _context.Polls
         .Where(x => x.IsPuplished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                                   && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                                       .ProjectToType<PollResponse>()
                                       .ToListAsync(cancellationToken);


        return currentPoll;
    }
}
