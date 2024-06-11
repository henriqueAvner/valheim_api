using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_valheim.models;
using Microsoft.IdentityModel.Tokens;

namespace api_valheim.services;

public class TokenGenerator

{
    private string Secret = "4d82a63bbdc67c1e4784edd6587f3730c";

    private int ExpiresIn = 4;

    public string Generate(User user)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = AddClaims(user),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret)), SecurityAlgorithms.HmacSha256),
            Expires = DateTime.Now.AddDays(ExpiresIn)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private ClaimsIdentity AddClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email!));
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Name!));
        claims.AddClaim(new Claim(ClaimTypes.Role, user.Access!));
        return claims;
    }
}