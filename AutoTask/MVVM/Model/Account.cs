using System.Linq;
using DomainModule.Model;
using DomainModule.Repository;
using AutoTask.Core;
using Infrastructure;

namespace AutoTask.MVVM.Model
{
    public class Account : ObservableObject
    {
        private User user;
        private bool isLoggedOut;
        private bool isLoggedIn;

        public User User
        {
            get 
            { 
                return user; 
            }
            set 
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoggedOut
        {
            get
            {
                return isLoggedOut;
            }
            set
            {
                isLoggedOut = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }
            set
            {
                isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        public Account()
        {
            UpdateUser();
        }

        public void LogOut()
        {
            if (User.IsLogged)
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                User = unitOfWork.Users.Get(User.Id);
                User.IsLogged = false;
                unitOfWork.Users.Update(User);
                unitOfWork.Save();
                unitOfWork.Dispose();
            }
        }

        public void UpdateInfo(User editedUser)
        {
            UserOperation userOperation = new UserOperation();
            userOperation.UpdateUser(editedUser.Id, editedUser.Name, editedUser.Surname, editedUser.Email, editedUser.Password, true);
        }

        public void DeleteAccount(int id)
        {
            UserOperation userOperation = new UserOperation();
            userOperation.DeleteUser(id);
        }

        public void UpdateUser()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            User = unitOfWork.Users.GetAll().FirstOrDefault(o => o.IsLogged == true);
            if (User == null)
            {
                User = new User()
                {
                    Name = "Guest"
                };
                isLoggedOut = true;
                isLoggedIn = false;
            }
            else
            {
                IsLoggedOut = false;
                isLoggedIn = true;
            }
            unitOfWork.Dispose();
        }
    }
}
