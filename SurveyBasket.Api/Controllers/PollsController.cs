using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Entity;
using SurveyBasket.Errors;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")] // /api/polls
    [ApiController]
    [Authorize]
    public class Polls(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _PollService = pollService;

        [HttpGet("Current")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> GetCurrentAll(CancellationToken cancellationToken)
        {
            return Ok(await _PollService.GetCurrentPollAsync(cancellationToken));
        }

        [HttpGet]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _PollService.GetAllAsync(cancellationToken));
        }


        [HttpGet("{id}")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var poll = await _PollService.GetAsync(id, cancellationToken); // دي مش اللي اللي انا فيها دي اللي جوا ال service
            return  poll.IsSuccess ?
                Ok(poll.Value) : poll.ToProblem();

        }


        [HttpPost]
        [HasPermission(Permissions.AddPolls)]
        public async Task<IActionResult> Add([FromQuery] PollRequest pollrequest , CancellationToken cancellationToken = default)
        {

          var pollresponse =   await _PollService.AddAsync(pollrequest, cancellationToken);

            return pollresponse.IsSuccess ? CreatedAtAction(nameof(Get), new { id = pollresponse.Value.Id }, pollresponse.Value)
                : pollresponse.ToProblem();
        }

        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest pollrequest, CancellationToken cancellationToken = default)
        {
            var result = await _PollService.UpdateAsync(id, pollrequest, cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            // التحقق من نوع الخطأ وإرجاع الحالة المناسبة
            if (result.Error == PollsError.PollTitleAlreadyExists)
            {
                return Conflict(result.ToProblem());
            }

            if (result.Error == PollsError.PollNotFound)
            {
                return NotFound(result.ToProblem());
            }

            // في حالة حدوث خطأ غير متوقع
            return BadRequest(result.ToProblem());
        }


        [HttpDelete("{id}")]
        [HasPermission(Permissions.DeletePolls)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var isdeleted = await _PollService.DeleteAsync(id, cancellationToken); // دي مش اللي اللي انا فيها دي اللي جوا ال service
            return isdeleted.IsSuccess ? NoContent() : isdeleted.ToProblem();

        }
        // add toggleIspublished
        [HttpPut("{id}/toggle")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> ToggleIsPublishedAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            var isToggled = await _PollService.ToggleIsPpublishedAsync(id, cancellationToken);
            return isToggled.IsSuccess ? NoContent() : isToggled.ToProblem();
        }

    }
}
