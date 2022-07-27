using System.Collections.Generic;
using System.Data.Entity;
using DomainModule.Model;

namespace DomainModule.Repository
{
    public class ProcessRepository : IRepository<Process>
    {
        DomainContext database;

        public ProcessRepository(DomainContext context)
        {
            this.database = context;
        }

        public IEnumerable<Process> GetAll()
        {
            return database.Processes;
        }

        public Process Get(int id)
        {
            return database.Processes.Find(id);
        }

        public void Create(Process process)
        {
            database.Processes.Add(process);
        }

        public void Update(Process process)
        {
            database.Entry(process).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Process process = database.Processes.Find(id);
            if (process != null)
                database.Processes.Remove(process);
        }
    }
}
