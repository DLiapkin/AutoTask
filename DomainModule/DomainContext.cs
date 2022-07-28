using System.Data.Entity;
using DomainModule.Model;

namespace DomainModule
{
    /// <summary>
    /// Class for access to database
    /// </summary>
    public class DomainContext : DbContext
    {
        public DomainContext() : base("DefaultConnection")
        {

        }

        public DbSet<Process> Processes { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
