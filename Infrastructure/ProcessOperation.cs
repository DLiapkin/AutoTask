using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    public class ProcessOperation
    {
        public void CreateProcess(string name, DateTime beginDate, DateTime? endDate, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (beginDate == null)
            {
                throw new ArgumentNullException("beginDate");
            }
            if (endDate == null)
            {
                throw new ArgumentNullException("endDate");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            Process process = new Process()
            {
                Name = name,
                Begin = beginDate,
                End = endDate,
                Description = description
            };
            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.Processes.Create(process);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

        public void UpdateProcess(int id, string name, DateTime beginDate, DateTime? endDate, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (beginDate == null)
            {
                throw new ArgumentNullException("name");
            }
            if (endDate == null)
            {
                throw new ArgumentNullException("endDate");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = unitOfWork.Processes.Get(id);
            process.Name = name;
            process.Begin = beginDate;
            process.End = endDate;
            process.Description = description;
            unitOfWork.Processes.Update(process);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

        public void DeleteProcess(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = unitOfWork.Processes.Get(id);
            List<Task> tasksToDelete = process.Tasks.ToList();
            foreach (Task task in tasksToDelete)
            {
                unitOfWork.Tasks.Delete(task.Id);
            }
            unitOfWork.Processes.Delete(process.Id);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }
    }
}
