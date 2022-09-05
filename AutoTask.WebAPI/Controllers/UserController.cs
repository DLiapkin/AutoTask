using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoTask.Shared;
using AutoTask.Domain.Model;

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            User? user = GetCurrentUser();

            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }

        private User? GetCurrentUser()
        {
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                IEnumerable<Claim> userClaims = identity.Claims;

                return new User
                {
                    Id = int.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value),
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                };
            }

            return null;
        }

        // POST api/<UserController>
        [HttpPost]
        [AllowAnonymous]
        public void Post([FromBody] User value)
        {
            UserOperation operation = new UserOperation();
            operation.CreateUser(value.Name, value.Surname, value.Email, value.Password);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] User value)
        {
            UserOperation operation = new UserOperation();
            operation.UpdateUser(id, value.Name, value.Surname, value.Email, value.Password, false);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            UserOperation operation = new UserOperation();
            operation.DeleteUser(id);
        }
    }
}
