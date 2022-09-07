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

        // GET: api/<ProcessController>
        [HttpGet]
        public IActionResult Get()
        {
            List<Process> processes = processOperation.GetAll().ToList();
            if (processes.Count == 0)
            {
                return NotFound();
            }
            return Ok(processes);
        }

        // GET api/<ProcessController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Process process = processOperation.GetById(id);
            if (process == null)
            {
                return NotFound();
            }
            return Ok(process);
        }

        // POST api/<ProcessController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] Process value)
        {
            processOperation.CreateProcess(value.Name, value.Begin, value.End, value.Description);
        }

        // PUT api/<ProcessController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] Process value)
        {
            processOperation.UpdateProcess(id, value.Name, value.Begin, value.End, value.Description);
        }

        // DELETE api/<ProcessController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            processOperation.DeleteProcess(id);
        }
    }
}
