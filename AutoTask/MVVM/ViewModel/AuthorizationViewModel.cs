﻿using System.Linq;
using AutoTask.UI.Core;
using AutoTask.UI.MVVM.Model;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using AutoTask.Shared;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls authorization/registration Window
    /// </summary>
    public class AuthorizationViewModel : ObservableObject
    {
        private Account currentAccount;
        private User newUser = new User();
        private bool isCollapsed;

        public Account CurrentAccount 
        {
            get
            {
                return currentAccount;
            }
            set
            {
                currentAccount = value;
                OnPropertyChanged();
            }
        }

        public User NewUser 
        {
            get
            {
                return newUser;
            }
            set
            {
                newUser = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used for collapsing authorization view elements
        /// </summary>
        public bool IsCollapsed 
        { 
            get
            {
                return isCollapsed;
            }
            set
            {
                isCollapsed = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand AuthorizeUserCommand { get; set; }
        public RelayCommand ChangeVisibilityCommand { get; set; }

        public AuthorizationViewModel(Account account)
        {
            CurrentAccount = account;
            isCollapsed = true;

            RegisterUserCommand = new RelayCommand(o =>
            {
                UserOperation userOperation = new UserOperation();
                UnitOfWork unitOfWork = new UnitOfWork();
                User user = unitOfWork.Users.GetAll().FirstOrDefault(u => u.Email.Equals(newUser.Email));
                if (user == null)
                {
                    userOperation.CreateUser(newUser.Name, newUser.Surname, newUser.Email, newUser.Password);
                    CurrentAccount.User = newUser;
                    CurrentAccount.IsLoggedIn = true;
                    CurrentAccount.IsLoggedOut = false;
                }
            });

            AuthorizeUserCommand = new RelayCommand(o =>
            {
                UnitOfWork unitOfWork = new UnitOfWork();
                User user = unitOfWork.Users.GetAll().FirstOrDefault(u => u.Email.Equals(newUser.Email) && u.Password.Equals(newUser.Password));
                if (user != null)
                {
                    UserOperation userOperation = new UserOperation();
                    userOperation.UpdateUser(user.Id, user.Name, user.Surname, user.Email, user.Password, true);
                    CurrentAccount.User = user;
                    CurrentAccount.IsLoggedIn = true;
                    CurrentAccount.IsLoggedOut = false;
                }
            });

            ChangeVisibilityCommand = new RelayCommand(o =>
            {
                if (IsCollapsed)
                {
                    IsCollapsed = false;
                }
                else
                {
                    IsCollapsed = true;
                }
            });
        }
    }
}
