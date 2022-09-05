using AutoTask.Shared;
using Microsoft.AspNetCore.Mvc;
using AutoTask.Domain.Repository;
using Task = AutoTask.Domain.Model.Task;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private List<Task> _tasks;

        public TaskController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            _tasks = unitOfWork.Tasks.GetAll().ToList();
        }

        // GET: api/<TaskController>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return _tasks;
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public Task Get(int id)
        {
            return _tasks.First(p => p.Id == id);
        }

        // POST api/<TaskController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] Task value)
        {
            TaskOperation taskOperation = new TaskOperation();
            taskOperation.CreateTask(value.Name, value.Status, value.Progress, value.Priority, value.ProcessId, value.UserId);
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] Task value)
        {
            TaskOperation taskOperation = new TaskOperation();
            taskOperation.UpdateTask(id, value.Name, value.Status, value.Progress, value.Priority, value.ProcessId);
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            TaskOperation taskOperation = new TaskOperation();
            taskOperation.DeleteTask(id);
        }
    }
}
