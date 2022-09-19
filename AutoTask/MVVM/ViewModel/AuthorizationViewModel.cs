using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using AutoTask.Domain.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;

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
        private HttpClient client;

        public RelayCommand RegisterUserCommand { get; set; }
        public RelayCommand AuthorizeUserCommand { get; set; }
        public RelayCommand ChangeVisibilityCommand { get; set; }

        public AuthorizationViewModel(IAccount account, HttpClient httpClient)
        {
            CurrentAccount = account;
            isCollapsed = true;
            client = httpClient;

            RegisterUserCommand = new RelayCommand(() =>
            {
                HttpResponseMessage response = client.PostAsJsonAsync("api/User", newUser).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
                CurrentAccount.LogIn(newUser.Email, newUser.Password);
            });

            AuthorizeUserCommand = new RelayCommand(() =>
            {
                CurrentAccount.LogIn(newUser.Email, newUser.Password);
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
