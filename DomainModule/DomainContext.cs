using AutoTask.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace AutoTask.Domain
{
    /// <summary>
    /// Class for access to database
    /// </summary>
    public class DomainContext : DbContext
    {
        public DbSet<Process> Processes { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public DomainContext()
        {
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
