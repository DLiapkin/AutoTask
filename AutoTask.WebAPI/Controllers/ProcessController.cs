using AutoTask.Shared;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AutoTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private List<Process> _processes;

        public ProcessController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            _processes = unitOfWork.Processes.GetAll().ToList();
        }

        // GET: api/<ProcessController>
        [HttpGet]
        public IEnumerable<Process> Get()
        {
            return _processes;
        }

        // GET api/<ProcessController>/5
        [HttpGet("{id}")]
        public Process Get(int id)
        {
            return _processes.First(p => p.Id == id);
        }

        // POST api/<ProcessController>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] Process value)
        {
            ProcessOperation processOperation = new ProcessOperation();
            processOperation.CreateProcess(value.Name, value.Begin, value.End, value.Description);
        }

        // PUT api/<ProcessController>/5
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] Process value)
        {
            ProcessOperation processOperation = new ProcessOperation();
            processOperation.UpdateProcess(id, value.Name, value.Begin, value.End, value.Description);
        }

        // DELETE api/<ProcessController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            ProcessOperation processOperation = new ProcessOperation();
            processOperation.DeleteProcess(id);
        }
    }
}
