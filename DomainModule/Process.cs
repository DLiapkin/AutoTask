using System;
using System.Collections.Generic;

namespace DomainModule
{
    class Process
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
