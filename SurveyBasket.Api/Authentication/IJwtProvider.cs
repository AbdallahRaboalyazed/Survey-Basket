using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authentication;

public interface IJwtProvider
{
    (string token, int expiry) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
