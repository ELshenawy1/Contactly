using Azure;
using Contactly.Core.Common;
using Contactly.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Contactly.API.Controllers
{
    [Route("api/AuthApi")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        protected APIResponse _response;
        private string secretKey;

        public AuthAPIController(IConfiguration configuration)
        {
            _response = new();
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!((model.UserName == "user1" && model.Password == "user1")
                || (model.UserName == "user2" && model.Password == "user2")))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username of password is incorrect!");
                return BadRequest(_response);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescripttor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, (model.UserName == "user1") ? "1" : "2"),
                    new Claim(ClaimTypes.Name, model.UserName),
                }),
                Expires = DateTime.Now.AddMinutes(59),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescripttor);
            var tokenStr = tokenHandler.WriteToken(token);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = tokenStr;
            return Ok(_response);

        }
    }
}
