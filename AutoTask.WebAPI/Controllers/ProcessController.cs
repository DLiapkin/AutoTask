using AutoTask.Shared.Interface;
using AutoTask.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        IProcessOperation processOperation;

        public ProcessController(IProcessOperation operation)
        {
            processOperation = operation;
        }

        /// <summary>
        /// Returns all processes
        /// </summary>
        /// <response code="200">Successufully returns processes</response>
        /// <response code="404">Processes are not found</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<Process> processes = await processOperation.GetAll();
            if (!processes.Any())
            {
                return NotFound();
            }
            return Ok(processes);
        }

        /// <summary>
        /// Returns process by id
        /// </summary>
        /// <response code="200">Successufully returns process</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Process is not found</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Process process = await processOperation.GetById(id);
            if (process == null)
            {
                return NotFound();
            }
            return Ok(process);
        }

        /// <summary>
        /// Saves new process to database
        /// </summary>
        /// <response code="200">Successufully saves new process</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Process value)
        {
            try
            {
                await processOperation.CreateProcess(value.Name, value.Begin, value.End, value.Description);
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
        /// Updates process info in database
        /// </summary>
        /// <response code="200">Successufully updated</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Process value)
        {
            try 
            {
                await processOperation.UpdateProcess(id, value.Name, value.Begin, value.End, value.Description);
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
        /// Deletes process from database by id
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
                await processOperation.DeleteProcess(id);
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
