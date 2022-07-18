using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DomainModule
{
    class DomainContext : DbContext
    {
        public DomainContext() : base("DefaultConnection")
        {

        }

        public DbSet<Process> Processes { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
