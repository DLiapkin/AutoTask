using System.Collections.Generic;
using System.Data.Entity;
using DomainModule.Model;

namespace DomainModule.Repository
{
    public class TaskRepository : IRepository<Task>
    {
        DomainContext database;

        public TaskRepository(DomainContext context)
        {
            this.database = context;
        }

        public IEnumerable<Task> GetAll()
        {
            return database.Tasks;
        }

        public Task Get(int id)
        {
            return database.Tasks.Find(id);
        }

        public void Create(Task task)
        {
            database.Tasks.Add(task);
        }

        public void Update(Task task)
        {
            database.Entry(task).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Task task = database.Tasks.Find(id);
            if (task != null)
                database.Tasks.Remove(task);
        }
    }
}
