using System;
using System.Collections.Generic;
using System.Linq;
using DomainModule.Repository;
using DomainModule.Model;

namespace Infrastructure
{
    public class UserOperation
    {
        public void CreateUser(string name, string surname, string email, string password)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(surname))
            {
                throw new ArgumentNullException("surname");
            }
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            User user = new User()
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                IsLogged = true
            };
            UnitOfWork unitOfWork = new UnitOfWork();
            unitOfWork.Users.Create(user);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

        public void UpdateUser(int id, string name, string surname, string email, string password, bool isLogged)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (String.IsNullOrEmpty(surname))
            {
                throw new ArgumentNullException("surname");
            }
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            User user = unitOfWork.Users.Get(id);
            user.Name = name;
            user.Surname = surname;
            user.Email = email;
            user.Password = password;
            user.IsLogged = isLogged;
            unitOfWork.Users.Update(user);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }

        public void DeleteUser(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            User user = unitOfWork.Users.Get(id);
            List<Task> tasksToDelete = user.Tasks.ToList();
            foreach (Task task in tasksToDelete)
            {
                unitOfWork.Tasks.Delete(task.Id);
            }
            unitOfWork.Users.Delete(user.Id);
            unitOfWork.Save();
            unitOfWork.Dispose();
        }
    }
}
