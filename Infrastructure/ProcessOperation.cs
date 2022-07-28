using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    /// <summary>
    /// Class for operations on processes
    /// </summary>
    public class ProcessOperation
    {
        /// <summary>
        /// Creates process by provided parameters
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <param name="beginDate">Date of begining of the process</param>
        /// <param name="endDate">Date of ending of the process</param>
        /// <param name="description">Description of the process</param>
        /// <exception cref="ArgumentNullException"></exception>
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

            UnitOfWork unitOfWork = new UnitOfWork();
            if (!unitOfWork.Processes.GetAll().Where(u => u.Name.Equals(name)).Any())
            {
                Process process = new Process()
                {
                    Name = name,
                    Begin = beginDate,
                    End = endDate,
                    Description = description
                };
                unitOfWork.Processes.Create(process);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Updates process by provided parameters
        /// </summary>
        /// <param name="id">Id of the process in database</param>
        /// <param name="name">Name of the process</param>
        /// <param name="beginDate">Date of begining of the process</param>
        /// <param name="endDate">Date of ending of the process</param>
        /// <param name="description">Description of the process</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
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
            if (endDate < beginDate)
            {
                throw new ArgumentException("endDate");
            }
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = unitOfWork.Processes.Get(id);
            if (process != null)
            {
                process.Name = name;
                process.Begin = beginDate;
                process.End = endDate;
                process.Description = description;
                unitOfWork.Processes.Update(process);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Deletes process by provided id in database
        /// </summary>
        /// <param name="id">Id of the process in database</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteProcess(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = unitOfWork.Processes.Get(id);
            if (process != null)
            {
                List<Task> tasksToDelete = process.Tasks.ToList();
                foreach (Task task in tasksToDelete)
                {
                    unitOfWork.Tasks.Delete(task.Id);
                }
                unitOfWork.Processes.Delete(process.Id);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }
    }
}
