using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    /// <summary>
    /// Class for operations on tasks
    /// </summary>
    public class TaskOperation
    {
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
        public void CreateTask(string name, string status, int progress, int priority, int parentId, int? userId)
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
            if (!unitOfWork.Tasks.GetAll().Where(t => t.Name.Equals(name)).Any()
                && unitOfWork.Processes.Get(parentId) != null)
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
                unitOfWork.Tasks.Create(task);
                unitOfWork.Save();
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
        public void UpdateTask(int id, string name, string status, int progress, int priority, int parentId)
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
            Task task = unitOfWork.Tasks.Get(id);
            if (task != null && unitOfWork.Processes.Get(parentId) != null)
            {
                task.Name = name;
                task.Status = status;
                task.Progress = progress;
                task.Priority = priority;
                task.ProcessId = parentId;
                unitOfWork.Tasks.Update(task);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Deletes task by provided id in database
        /// </summary>
        /// <param name="id">Id of the task in database</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteTask(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.Tasks.Delete(id);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }
    }
}
