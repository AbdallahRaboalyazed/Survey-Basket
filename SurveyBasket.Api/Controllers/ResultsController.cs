using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;

namespace SurveyBasket.Controllers
{
    [Route("api/{pollId}")]
    [ApiController]
    [HasPermission(Permissions.Results)]
    public class ResultsController(IResultService  resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;
        [HttpGet("results")]
        public async Task<IActionResult> PollVotes(int pollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);
             
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("votes-per-day")]
        public async Task<IActionResult> PollVotesPerDay(int pollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetPollVotesPerDayAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("votes-per-question")]
        public async Task<IActionResult> VotesPerQuestion(int pollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetVotesPerQuestionAsync(pollId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        
    }
}
