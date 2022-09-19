using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoTask.Shared.Interface;
using AutoTask.Domain.Model;

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserOperation userOperation;

        public UserController(IUserOperation operation)
        {
            userOperation = operation;
        }

        /// <summary>
        /// Gets logged in user
        /// </summary>
        /// <response code="200">Successufully returns logged in user</response>
        /// <response code="404">User is not found</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            User? user = await GetCurrentUser();
            if (user != null)
            {
                return Ok(user);
            }
            return BadRequest();
        }

        /// <summary>
        /// Gets current user claims
        /// </summary>
        /// <returns>Current user</returns>
        private async Task<User?> GetCurrentUser()
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

        /// <summary>
        /// Saves new user to database
        /// </summary>
        /// <response code="200">Successufully saves new task</response>
        /// <response code="400">Invalid input</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] User value)
        {
            try
            {
                await userOperation.CreateUser(value.Name, value.Surname, value.Email, value.Password);
                return Ok();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                Console.WriteLine(exeption.StackTrace);
                return BadRequest();
            }
        }

        /// <summary>
        /// Updates user info in database
        /// </summary>
        /// <response code="200">Successufully updated</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User value)
        {
            try
            {
                await userOperation.UpdateUser(id, value.Name, value.Surname, value.Email, value.Password, false);
                return Ok();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                Console.WriteLine(exeption.StackTrace);
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes user from database by id
        /// </summary>
        /// <response code="200">Successufully deleted</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await userOperation.DeleteUser(id);
                return Ok();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                Console.WriteLine(exeption.StackTrace);
                return BadRequest();
            } 
        }
    }
}
