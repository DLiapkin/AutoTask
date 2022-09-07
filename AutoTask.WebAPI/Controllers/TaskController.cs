using AutoTask.Shared.Interface;
using Microsoft.AspNetCore.Mvc;
using Task = AutoTask.Domain.Model.Task;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        ITaskOperation taskOperation;

        public TaskController(ITaskOperation operation)
        {
            taskOperation = operation;
        }

        // GET: api/<TaskController>
        [HttpGet]
        public IActionResult Get()
        {
            List<Task> tasks = taskOperation.GetAll().ToList();
            if (tasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(tasks);
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Task task = taskOperation.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // POST api/<TaskController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] Task value)
        {
            taskOperation.CreateTask(value.Name, value.Status, value.Progress, value.Priority, value.ProcessId, value.UserId);
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] Task value)
        {
            taskOperation.UpdateTask(id, value.Name, value.Status, value.Progress, value.Priority, value.ProcessId);
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            taskOperation.DeleteTask(id);
        }
    }
}
