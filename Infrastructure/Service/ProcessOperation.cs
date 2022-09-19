using System;
using System.Collections.Generic;
using System.Linq;
using AutoTask.Domain.Repository;
using AutoTask.Domain.Model;
using AutoTask.Shared.Interface;

namespace AutoTask.Shared.Service
{
    /// <summary>
    /// Class for operations on processes
    /// </summary>
    public class ProcessOperation : IProcessOperation
    {
        /// <summary>
        /// Gets all processes from database
        /// </summary>
        /// <returns>Collection of processes</returns>
        public async System.Threading.Tasks.Task<IEnumerable<Process>> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<Process> processes = await unitOfWork.Processes.GetAll();
            if (processes != null)
            {
                return processes;
            }
            return Enumerable.Empty<Process>();
        }

        /// <summary>
        /// Gets process by provided id
        /// </summary>
        /// <param name="id">Id of the process in database</param>
        /// <returns>Found Process</returns>
        public async System.Threading.Tasks.Task<Process> GetById(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = await unitOfWork.Processes.Get(id);
            return process;
        }

        /// <summary>
        /// Creates process by provided parameters
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <param name="beginDate">Date of begining of the process</param>
        /// <param name="endDate">Date of ending of the process</param>
        /// <param name="description">Description of the process</param>
        /// <exception cref="ArgumentNullException"></exception>
        public async System.Threading.Tasks.Task CreateProcess(string name, DateTime beginDate, DateTime? endDate, string description)
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
            IEnumerable<Process> processes = await unitOfWork.Processes.GetAll();
            if (!processes.Where(u => u.Name.Equals(name)).Any())
            {
                Process process = new Process()
                {
                    Name = name,
                    Begin = beginDate,
                    End = endDate,
                    Description = description
                };
                await unitOfWork.Processes.Create(process);
                await unitOfWork.Save();
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
        public async System.Threading.Tasks.Task UpdateProcess(int id, string name, DateTime beginDate, DateTime? endDate, string description)
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
            Process process = await unitOfWork.Processes.Get(id);
            if (process != null)
            {
                process.Name = name;
                process.Begin = beginDate;
                process.End = endDate;
                process.Description = description;
                unitOfWork.Processes.Update(process);
                await unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Deletes process by provided id in database
        /// </summary>
        /// <param name="id">Id of the process in database</param>
        /// <exception cref="ArgumentException"></exception>
        public async System.Threading.Tasks.Task DeleteProcess(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            Process process = await unitOfWork.Processes.Get(id);
            if (process != null)
            {
                List<Task> tasksToDelete = process.Tasks.ToList();
                foreach (Task task in tasksToDelete)
                {
                    await unitOfWork.Tasks.Delete(task.Id);
                }
                await unitOfWork.Processes.Delete(process.Id);
                await unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }
    }
}
