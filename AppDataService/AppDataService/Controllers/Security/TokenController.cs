using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace Kawaii.NetworkDocumentation.AppDataService.Controllers.Security
{
    [RoutePrefix("api/token")]
    public class TokenController : ApiController
    {
        private const string Secret = "SuperGreg123+FcerSec";

        [AllowAnonymous]
        [Route("Generate")]
        [HttpGet]
        public string Get([FromUri] string username, [FromUri] string password)
        {
            if (CheckUser(username, password))
            {
                return this.GenerateToken(username);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private bool CheckUser(string username, string password)
        {
            if (username == "super" && password == "greg")
            {
                return true;
            }

            return false;
        }

        private string GenerateToken(string username, int expireMinutes = 20)
        {
            var symmetricKey = System.Text.Encoding.Unicode.GetBytes(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, username)
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
