using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Authentication;
using SurveyBasket.Services;
using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
  //   [Authorize]   متحطهاش هنا
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

            return token.IsSuccess ? Ok(token.Value) : token.ToProblem();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] AuthRefreshRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return token.IsSuccess ? Ok(token.Value) : token.ToProblem();
        }

        [HttpPut("revoke-refresh-token")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] AuthRefreshRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _authService.ConfirmEmailAsync(request);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
        {
            var result = await _authService.ResendConfirmationEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }



        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var result = await _authService.SendResetPasswordCodeAsync(request.Email);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }


        [HttpGet("test")]
        public IActionResult Test()
        { 
            var permissions = Permissions.GetAllPermissions();
            return Ok(permissions);
        }


    }

}
