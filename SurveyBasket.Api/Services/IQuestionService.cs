using SurveyBasket.Contracts.Common;

namespace SurveyBasket.Services;

public interface IQuestionService
{

    public Task<Result<IEnumerable<QuestionResponse>>> GetAvailable(int pollid , string userid , CancellationToken cancellationToken = default);
    public Task<Result<QuestionResponse>> GetAsync(int pollid , int questionid, CancellationToken cancellationToken = default);
    public Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollid, RequestFilters filters ,  CancellationToken cancellationToken = default);
    public Task<Result<QuestionResponse>> AddQuestionAsync( int pollid , QuestionRequest questionRequest , CancellationToken cancellationToken = default );
    public Task<Result> ToggleStatusAsync(int pollid, int questionid, CancellationToken cancellationToken = default);

    public Task<Result> UpdateQuestionAndThierAnswersAsync(int pollid, int questionid, QuestionRequest questionRequest, CancellationToken cancellationToken = default);

}
