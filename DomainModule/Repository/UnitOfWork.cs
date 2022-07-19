using System;

namespace DomainModule.Repository
{
    public class UnitOfWork : IDisposable
    {
        private DomainContext database = new DomainContext();
        private ProcessRepository processRepository;
        private TaskRepository taskRepository;

        public ProcessRepository Processes
        {
            get
            {
                if (processRepository == null)
                    processRepository = new ProcessRepository(database);
                return processRepository;
            }
        }

        public TaskRepository Tasks
        {
            get
            {
                if (taskRepository == null)
                    taskRepository = new TaskRepository(database);
                return taskRepository;
            }
        }

        public void Save()
        {
            database.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    database.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
