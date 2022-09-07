using AutoTask.Domain.Model;
using System.Collections.Generic;

namespace AutoTask.Shared.Interface
{
    public interface IUserOperation
    {
        void CreateUser(string name, string surname, string email, string password);
        void DeleteUser(int id);
        IEnumerable<User> GetAll();
        void UpdateUser(int id, string name, string surname, string email, string password, bool isLogged);
    }
}