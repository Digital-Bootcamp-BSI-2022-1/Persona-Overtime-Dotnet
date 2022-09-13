using dotnet_2.Infrastructure.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dotnet_2.Infrastructure.Shared
{
    public class Jwt
    {
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var expires = DateTime.Now.AddDays(3);
            var key = Encoding.UTF8.GetBytes("Secrettttttttttt");

            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                 new Claim("id", user.id.ToString()),
                 new Claim("name", user.name!.ToString())
                 }),
                Expires = expires,
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Token GetTokenClaim(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            var handler = new JwtSecurityTokenHandler();
            var tokenSplit = token.Split(' ');
            var jwt = handler.ReadJwtToken(tokenSplit[1]);
            var result = new Token();
            foreach (Claim claim in jwt.Claims)
            {
                if (claim.Type == "id")
                {
                    result.id = claim.Value;
                }
            }

            return result;
        }
    }

}

