using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async System.Threading.Tasks.Task Create(User item)
        {
            await database.Users.AddAsync(item);
        }

        public async Task<User> Get(int id)
        {
            return await database.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await database.Users.ToListAsync();
        }

        public void Update(User item)
        {
            database.Entry(item).State = EntityState.Modified;
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            User user = await database.Users.FindAsync(id);
            if (user != null)
            {
                database.Users.Remove(user);
            }
        }
    }
}
