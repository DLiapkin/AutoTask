using System;
using System.Collections.Generic;
using System.Linq;
using AutoTask.Domain.Repository;
using AutoTask.Domain.Model;
using AutoTask.Shared.Interface;

namespace AutoTask.Shared.Service
{
    /// <summary>
    /// Class for operations on users
    /// </summary>
    public class UserOperation : IUserOperation
    {
        /// <summary>
        /// Gets all users from database
        /// </summary>
        /// <returns>Collection of users</returns>
        public IEnumerable<User> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetAll();
            if (users != null)
            {
                return users;
            }
            return Enumerable.Empty<User>();
        }

        /// <summary>
        /// Creates new user by provided parameters
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <param name="surname">Surname of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
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

            UnitOfWork unitOfWork = new UnitOfWork();
            if (!unitOfWork.Users.GetAll().Where(u => u.Email.Equals(email)).Any())
            {
                User user = new User()
                {
                    Name = name,
                    Surname = surname,
                    Email = email,
                    Password = password,
                    IsLogged = true
                };
                unitOfWork.Users.Create(user);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Updates user by provided parameters
        /// </summary>
        /// <param name="id">Id of the user in database </param>
        /// <param name="name">Name of the user</param>
        /// <param name="surname">Surname of the user</param>
        /// <param name="email">Email of the user</param>
        /// <param name="password">Password of the user</param>
        /// <param name="isLogged">Status of the user</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateUser(int id, string name, string surname, string email, string password, bool isLogged)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }
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
            if (user != null)
            {
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.Password = password;
                user.IsLogged = isLogged;
                unitOfWork.Users.Update(user);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Deletes user by provided id in database
        /// </summary>
        /// <param name="id">Id of the user in database</param>
        /// <exception cref="ArgumentException"></exception>
        public void DeleteUser(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("id");
            }

            UnitOfWork unitOfWork = new UnitOfWork();
            User user = unitOfWork.Users.Get(id);
            if (user != null)
            {
                List<Task> tasksToDelete = user.Tasks.ToList();
                foreach (Task task in tasksToDelete)
                {
                    unitOfWork.Tasks.Delete(task.Id);
                }
                unitOfWork.Users.Delete(user.Id);
                unitOfWork.Save();
            }
            unitOfWork.Dispose();
        }
    }
}
