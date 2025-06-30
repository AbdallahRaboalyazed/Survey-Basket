using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Common;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Errors;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;


        [HttpGet("{id}")]
        [HasPermission(Permissions.GetQuestions)]
        public async Task<IActionResult> Get([FromRoute] int id , [FromRoute] int pollId, CancellationToken cancellationToken = default!)
        {
            var result = await _questionService.GetAsync(pollId , id, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("")]
        [HasPermission(Permissions.GetQuestions)]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId, filters, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        [HasPermission(Permissions.AddQuestions)]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken = default!)
        {
            var result = await _questionService.AddQuestionAsync(pollId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPut("{id}/Togglestatus")]
        [HasPermission(Permissions.UpdateQuestions)]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, [FromRoute] int pollId, CancellationToken cancellationToken = default!)
        {
            var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);
            return result.IsSuccess? Ok() : result.ToProblem();               
        }


        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateQuestions)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken = default!)
        {
            var result = await _questionService.UpdateQuestionAndThierAnswersAsync(pollId, id, request, cancellationToken);
            return result.IsSuccess? NoContent() : result.ToProblem();
        }
              
              


    }
}