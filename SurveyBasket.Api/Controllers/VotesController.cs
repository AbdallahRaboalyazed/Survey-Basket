using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Votes;
using SurveyBasket.Errors;
using SurveyBasket.Extensions;

namespace SurveyBasket.Controllers
{
    [Route("api/{pollId}")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Member)]
    public class VotesController(IQuestionService questionService , IVoteService voteService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteService _voteService = voteService;

        [HttpGet]
        public async Task<IActionResult> Start( [FromRoute] int pollId , CancellationToken cancellationToken )
        {
            var userid = User.GetUserId();

            var result = await _questionService.GetAvailable(pollId, userid, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

      
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            var userid = User.GetUserId();
            var result = await _voteService.AddAsync(pollId, userid, voteRequest, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();
        }


        

    }
}
