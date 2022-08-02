using System.Linq;
using AutoTask.Shared;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTask.UI.MVVM.Model
{
    /// <summary>
    /// Class for current user manipulation
    /// </summary>
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

        /// <summary>
        /// Used for view elements visibility
        /// </summary>
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

        /// <summary>
        /// Used for view elements visibility
        /// </summary>
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

        /// <summary>
        /// Updates information about current user
        /// </summary>
        /// <param name="editedUser">User that contains new information</param>
        public void UpdateInfo(User editedUser)
        {
            UserOperation userOperation = new UserOperation();
            userOperation.UpdateUser(editedUser.Id, editedUser.Name, editedUser.Surname, editedUser.Email, editedUser.Password, true);
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        /// <param name="id">Id of the user in database</param>
        public void DeleteAccount(int id)
        {
            UserOperation userOperation = new UserOperation();
            userOperation.DeleteUser(id);
        }

        /// <summary>
        /// Updates current user and bool flags for view elements visibility
        /// </summary>
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
