using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    public class ProcessOperation
    {
        public void CreateProcess(string name, string status, string beginDate, string endDate, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }
            if (String.IsNullOrEmpty(beginDate))
            {
                throw new ArgumentNullException("beginDate");
            }
            else
            {
                if (!DateTime.TryParse(beginDate, out _))
                {
                    throw new ArgumentException("beginDate");
                }
            }
            //if (String.IsNullOrEmpty(endDate))
            //{
            //    throw new ArgumentNullException("endDate");
            //}
            //else
            //{
            //    if (!DateTime.TryParse(endDate, out _))
            //    {
            //        throw new ArgumentException("endDate");
            //    }
            //}
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            Process process = new Process()
            {
                Name = name,
                Status = status,
                Begin = DateTime.Parse(beginDate),
                End = null,
                Description = description
            };
            if (DateTime.TryParse(endDate, out _))
            {
                process.End = DateTime.Parse(endDate);
            }
            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.Processes.Create(process);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

        public void UpdateProcess(int id, string name, string status, string beginDate, string endDate, string description)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(status))
            {
                throw new ArgumentNullException("status");
            }
            if (String.IsNullOrEmpty(beginDate))
            {
                throw new ArgumentNullException("name");
            }
            else
            {
                if (!DateTime.TryParse(beginDate, out _))
                {
                    throw new ArgumentException("beginDate");
                }
            }
            //if (String.IsNullOrEmpty(endDate))
            //{
            //    throw new ArgumentNullException("endDate");
            //}
            //else
            //{
            //    if (!DateTime.TryParse(endDate, out _))
            //    {
            //        throw new ArgumentException("endDate");
            //    }
            //}
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = unitOfWork.Processes.Get(id);
            process.Name = name;
            process.Status = status;
            process.Begin = DateTime.Parse(beginDate);
            process.Description = description;
            if (DateTime.TryParse(endDate, out _))
            {
                process.End = DateTime.Parse(endDate);
            }
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
