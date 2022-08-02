using System.Linq;
using AutoTask.Shared;
using AutoTask.UI.MVVM.Model;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls authorization/registration Window
    /// </summary>
    public partial class AuthorizationViewModel : ObservableObject
    {
        [ObservableProperty]
        private Account currentAccount;
        [ObservableProperty]
        private User newUser = new User();
        [ObservableProperty]
        private bool isCollapsed;

        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand AuthorizeUserCommand { get; set; }
        public RelayCommand ChangeVisibilityCommand { get; set; }

        public AuthorizationViewModel(Account account)
        {
            CurrentAccount = account;
            isCollapsed = true;

            RegisterUserCommand = new RelayCommand(() =>
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

            AuthorizeUserCommand = new RelayCommand(() =>
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

            ChangeVisibilityCommand = new RelayCommand(() =>
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
