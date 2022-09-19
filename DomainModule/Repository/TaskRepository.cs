using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoTask.Domain.Model;

namespace AutoTask.Domain.Repository
{
    /// <summary>
    /// Class for basic CRUD operations on tasks
    /// </summary>
    public class TaskRepository : IRepository<Model.Task>
    {
        DomainContext database;

        public TaskRepository(DomainContext context)
        {
            this.database = context;
        }

        public async Task<IEnumerable<Model.Task>> GetAll()
        {
            return await database.Tasks.ToListAsync();
        }

        public async Task<Model.Task> Get(int id)
        {
            return await database.Tasks.FindAsync(id);
        }

        public async System.Threading.Tasks.Task Create(Model.Task task)
        {
            await database.Tasks.AddAsync(task);
        }

        public void Update(Model.Task task)
        {
            database.Entry(task).State = EntityState.Modified;
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            Model.Task task = await database.Tasks.FindAsync(id);
            if (task != null)
            {
                database.Tasks.Remove(task);
            }
        }
    }
}
