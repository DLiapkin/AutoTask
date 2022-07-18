using System;

namespace DomainModule.Repository
{
    public class UnitOfWork : IDisposable
    {
        private DomainContext db = new DomainContext();
        private ProcessRepository processRepository;
        private TaskRepository taskRepository;

        public ProcessRepository Processes
        {
            get
            {
                if (processRepository == null)
                    processRepository = new ProcessRepository(db);
                return processRepository;
            }
        }

        public TaskRepository Tasks
        {
            get
            {
                if (taskRepository == null)
                    taskRepository = new TaskRepository(db);
                return taskRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
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
