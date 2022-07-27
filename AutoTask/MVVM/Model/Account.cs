using System.Linq;
using DomainModule.Model;
using DomainModule.Repository;
using AutoTask.Core;

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
            UnitOfWork unitOfWork = new UnitOfWork();
            User = unitOfWork.Users.GetAll().FirstOrDefault(o => o.IsLogged);
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
            }
        }
    }
}
