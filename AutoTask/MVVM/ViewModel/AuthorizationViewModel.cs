using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using AutoTask.Domain.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;
using AutoTask.UI.Core.Interface;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls authorization/registration Window
    /// </summary>
    public partial class AuthorizationViewModel : ObservableObject
    {
        [ObservableProperty]
        private IAccount currentAccount;
        [ObservableProperty]
        private User newUser = new User();
        [ObservableProperty]
        private bool isCollapsed;
        private IAccountOperation accountService;

        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand AuthorizeUserCommand { get; set; }
        public RelayCommand ChangeVisibilityCommand { get; set; }

        public AuthorizationViewModel(IAccount account, IAccountOperation accountOperation)
        {
            CurrentAccount = account;
            accountService = accountOperation;
            isCollapsed = true;

            RegisterUserCommand = new RelayCommand(() =>
            {
                accountService.Register(newUser);
                accountService.LogIn(newUser.Email, newUser.Password);
            });

            AuthorizeUserCommand = new RelayCommand(() =>
            {
                accountService.LogIn(newUser.Email, newUser.Password);
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
