﻿using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    public class TaskOperation
    {
        public void CreateTask(string name, string status, int progress, int priority, int parentId)
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
            if (unitOfWork.Processes.Get(parentId) != null)
            {
                Task task = new Task()
                {
                    Name = name,
                    Status = status,
                    Progress = progress,
                    Priority = priority,
                    ProcessId = parentId
                };
                unitOfWork.Tasks.Create(task);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

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
            if (!String.IsNullOrWhiteSpace(name))
            {
                task.Name = name;
            }
            if (!String.IsNullOrWhiteSpace(status))
            {
                task.Status = status;
            }
            task.Progress = progress;
            task.Priority = priority;
            if (task.ProcessId != parentId && unitOfWork.Processes.Get(parentId) != null)
            {
                task.ProcessId = parentId;
            }
            unitOfWork.Tasks.Update(task);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

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