using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoTask.Domain.Model;

namespace AutoTask.Domain.Repository
{
    /// <summary>
    /// Class for basic CRUD operations on processes
    /// </summary>
    public class ProcessRepository : IRepository<Process>
    {
        DomainContext database;

        public ProcessRepository(DomainContext context)
        {
            this.database = context;
        }

        public async System.Threading.Tasks.Task<IEnumerable<Process>> GetAll()
        {
            return await database.Processes.ToListAsync();
        }

        public async Task<Process> Get(int id)
        {
            return await database.Processes.FindAsync(id);
        }

        public async System.Threading.Tasks.Task Create(Process process)
        {
            await database.Processes.AddAsync(process);
        }

        public void Update(Process process)
        {
            database.Entry(process).State = EntityState.Modified;
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            Process process = await database.Processes.FindAsync(id);
            if (process != null)
            {
                database.Processes.Remove(process);
            }
        }
    }
}
