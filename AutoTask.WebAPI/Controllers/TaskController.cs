﻿using AutoTask.Shared.Interface;
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

        /// <summary>
        /// Returns all tasks
        /// </summary>
        /// <response code="200">Successufully returns tasks</response>
        /// <response code="404">Tasks are not found</response>
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

        /// <summary>
        /// Returns task by id
        /// </summary>
        /// <response code="200">Successufully returns task</response>
        /// <response code="400">Invalid input</response>
        /// <response code="404">Task is not found</response>
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

        /// <summary>
        /// Saves new task to database
        /// </summary>
        /// <response code="200">Successufully saves new task</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpPost]
        [Authorize]
        public void Post([FromBody] Task value)
        {
            taskOperation.CreateTask(value.Name, value.Status, value.Progress, value.Priority, value.ProcessId, value.UserId);
        }

        /// <summary>
        /// Updates task info in database
        /// </summary>
        /// <response code="200">Successufully updated</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpPut("{id}")]
        [Authorize]
        public void Put(int id, [FromBody] Task value)
        {
            taskOperation.UpdateTask(id, value.Name, value.Status, value.Progress, value.Priority, value.ProcessId);
        }

        /// <summary>
        /// Deletes task from database by id
        /// </summary>
        /// <response code="200">Successufully deleted</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Not authorized</response>
        [HttpDelete("{id}")]
        [Authorize]
        public void Delete(int id)
        {
            taskOperation.DeleteTask(id);
        }
    }
}
