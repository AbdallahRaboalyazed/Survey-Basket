using System.Text;
using System.Text.Json;

namespace SurveyBasket.Authentication;

public class JwtProvider(IOptions<JwtOptions> jwtSettings) : IJwtProvider
{
    private readonly JwtOptions _jwtSettings  = jwtSettings.Value;
    public (string token, int expiry) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions)
    {
        
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new(nameof(roles), JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray),
            new(nameof(permissions), JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray)
        ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var singingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //
        var expiresIn = _jwtSettings.ExpiryMinutes;

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresIn),
            signingCredentials: singingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiry: expiresIn * 60);
    }

    public string? ValidateToken(string token)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey (Encoding.ASCII.GetBytes(_jwtSettings.Key));
        try
        {
            tokenhandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

           var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch 
        {
            return null;
        }
    }
}
