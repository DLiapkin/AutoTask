using System.Collections.Generic;

namespace AutoTask.Domain.Model
{
    public  class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLogged { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
