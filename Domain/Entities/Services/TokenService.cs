using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrackMyAssets_API.Domain.Entities.Interfaces;


namespace TrackMyAssets_API.Domain.Entities.Services;

public class TokenService : ITokenService
{

    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Guid GetUserId(HttpContext http)
    {
        var claim = http.User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!.Value);
    }

    public string GenerateTokenJwt(Guid id, string email, string role)
    {
        var key = _configuration["Jwt"];

        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                                new Claim(ClaimTypes.Email, email),
                                new Claim(ClaimTypes.Role, role)
                            };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}
