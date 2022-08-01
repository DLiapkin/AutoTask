using System.Collections.Generic;
using System.Data.Entity;
using AutoTask.Domain.Model;

namespace AutoTask.Domain.Repository
{
    /// <summary>
    /// Class for basic CRUD operations on users
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        DomainContext database;

        public UserRepository(DomainContext context)
        {
            this.database = context;
        }

        public void Create(User item)
        {
            database.Users.Add(item);
        }

        public User Get(int id)
        {
            return database.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return database.Users;
        }

        public void Update(User item)
        {
            database.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = database.Users.Find(id);
            if (user != null)
                database.Users.Remove(user);
        }
    }
}
