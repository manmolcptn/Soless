using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SolessBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SolessBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenParameters;

        public AuthController(IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginModel model)
        {
            if (model.Email == "pato@gmail.com" && model.Password == "patocontraseña22")
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = new Dictionary<string, Object>
                    {
                        { "id", Guid.NewGuid().ToString() },
                        { ClaimTypes.Role, "admin" }
                    },
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials(
                        _tokenParameters.IssuerSigningKey,
                        SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string stringToken = tokenHandler.WriteToken(token);

                return Ok(stringToken);
            }
            return Unauthorized("Non existing User");
        }
    }
}
