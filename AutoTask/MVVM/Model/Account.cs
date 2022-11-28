using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using AutoTask.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;

namespace AutoTask.UI.MVVM.Model
{
    /// <summary>
    /// Class for current user manipulation
    /// </summary>
    public partial class Account : ObservableObject, IAccount
    {
        [ObservableProperty]
        private User user;
        [ObservableProperty]
        private bool isLoggedIn;
        public string JwtToken { get; set; }

        public Account()
        {
            JwtToken = String.Empty;
            User = new User();
            IsLoggedIn = false;
        }
    }
}
