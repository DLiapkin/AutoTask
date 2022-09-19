using AutoTask.Domain.Model;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface IUserOperation
    {
        System.Threading.Tasks.Task CreateUser(string name, string surname, string email, string password);
        System.Threading.Tasks.Task DeleteUser(int id);
        System.Threading.Tasks.Task<IEnumerable<User>> GetAll();
        System.Threading.Tasks.Task UpdateUser(int id, string name, string surname, string email, string password, bool isLogged);
    }
}