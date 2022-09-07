using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoTask.WebAPI.Model;
using AutoTask.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using AutoTask.Shared.Interface;

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private IUserOperation userOperation;

        public LoginController(IConfiguration configuration, IUserOperation operation)
        {
            _configuration = configuration;
            userOperation = operation;
        }

        /// <summary>
        /// Authorizes user and gives JWT token
        /// </summary>
        /// <response code="200">Successufully authorized</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Not found</response>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            User user = Authenticate(userLogin);
            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found!");
        }

        /// <summary>
        /// Logs out current user
        /// </summary>
        /// <response code="200">Successufully logged out</response>
        /// <response code="401">Not authorized</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// Generates JWT token to authorizing user
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>JWT token</returns>
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Authorizes user by provided user login info
        /// </summary>
        /// <param name="userLogin">User login info</param>
        /// <returns>Current user</returns>
        private User? Authenticate(UserLogin userLogin)
        {
            User currentUser = userOperation.GetAll()
                .FirstOrDefault(u => u.Email.Equals(userLogin.Email) && u.Password.Equals(userLogin.Password));

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}
