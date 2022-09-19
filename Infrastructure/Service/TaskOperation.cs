using System;
using System.Linq;
using AutoTask.Domain.Repository;
using AutoTask.Domain.Model;
using AutoTask.Shared.Interface;
using System.Collections.Generic;

namespace AutoTask.Shared.Service
{
    /// <summary>
    /// Class for operations on tasks
    /// </summary>
    public class TaskOperation : ITaskOperation
    {
        /// <summary>
        /// Gets all tasks from database
        /// </summary>
        /// <returns>Collection of tasks</returns>
        public async System.Threading.Tasks.Task<IEnumerable<Task>> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<Task> tasks = await unitOfWork.Tasks.GetAll();
            if (tasks != null)
            {
                return tasks;
            }
            return Enumerable.Empty<Task>();
        }

        /// <summary>
        /// Gets task by provided id
        /// </summary>
        /// <param name="id">Id of the task in database</param>
        /// <returns>Found task</returns>
        public async System.Threading.Tasks.Task<Task> GetById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Task task = await unitOfWork.Tasks.Get(id);
            return task;
        }

        /// <summary>
        /// Creates new task by provided parameters
        /// </summary>
        /// <param name="name">Name of the task</param>
        /// <param name="status">Status of the task</param>
        /// <param name="progress">Progress of the task</param>
        /// <param name="priority">Priority of the task</param>
        /// <param name="parentId">Id of the parent process</param>
        /// <param name="userId">Id of the user who created the task</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async System.Threading.Tasks.Task CreateTask(string name, string status, int progress, int priority, int parentId, int? userId)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }
            if (progress < 0 || progress > 100)
            {
                throw new ArgumentException("progress");
            }
            if (priority < 0)
            {
                throw new ArgumentException("priority");
            }
            if (parentId < 0)
            {
                throw new ArgumentException("parentId");
            }
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }
            if (userId < 0)
            {
                throw new ArgumentException("userId");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<Task> tasks = await unitOfWork.Tasks.GetAll();
            if (!tasks.Where(t => t.Name.Equals(name)).Any()
                && await unitOfWork.Processes.Get(parentId) != null)
            {
                Task task = new Task()
                {
                    Name = name,
                    Status = status,
                    Progress = progress,
                    Priority = priority,
                    ProcessId = parentId,
                    UserId = userId
                };
                await unitOfWork.Tasks.Create(task);
                await unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Updates task by provided parameters 
        /// </summary>
        /// <param name="id">Id of the task in database</param>
        /// <param name="name">Name of the task</param>
        /// <param name="status">Status of the task</param>
        /// <param name="progress">Progress of the task</param>
        /// <param name="priority">Priority of the task</param>
        /// <param name="parentId">Id of the parent process</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async System.Threading.Tasks.Task UpdateTask(int id, string name, string status, int progress, int priority, int parentId)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }
            if (priority < 0)
            {
                throw new ArgumentException("priority");
            }
            if (parentId < 0)
            {
                throw new ArgumentException("parentId");
            }
            if (progress < 0 || progress > 100)
            {
                throw new ArgumentException("progress");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Task task = await unitOfWork.Tasks.Get(id);
            if (task != null && await unitOfWork.Processes.Get(parentId) != null)
            {
                task.Name = name;
                task.Status = status;
                task.Progress = progress;
                task.Priority = priority;
                task.ProcessId = parentId;
                unitOfWork.Tasks.Update(task);
                await unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Deletes task by provided id in database
        /// </summary>
        /// <param name="id">Id of the task in database</param>
        /// <exception cref="ArgumentException"></exception>
        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            await unitOfWork.Tasks.Delete(id);
            await unitOfWork.Save();
            unitOfWork.Dispose();
        }
    }
}
