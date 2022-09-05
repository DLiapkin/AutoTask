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
        private HttpClient client = new HttpClient();

        public Account()
        {
            JwtToken = String.Empty;
            client.BaseAddress = new Uri("https://localhost:7120/");
            User = new User();
            IsLoggedIn = false;
        }

        /// <summary>
        /// Logs out current user
        /// </summary>
        public void LogOut()
        {
            if (!isLoggedIn)
            {
                return;
            }
            HttpResponseMessage response = client.GetAsync("api/Logout").Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            JwtToken = String.Empty;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            User = new User();
            IsLoggedIn = false;
        }

        /// <summary>
        /// Logs in current user
        /// </summary>
        /// <param name="email">Email of current user</param>
        /// <param name="password">Password of current user</param>
        public async void LogIn(string email, string password)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/Login", new { email, password }).Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            JwtToken = await response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            response = client.GetAsync("api/User").Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            User user = await response.Content.ReadFromJsonAsync<User>();
            if (user != null)
            {
                User = user;
                IsLoggedIn = true;
            }
        }

        /// <summary>
        /// Updates information about current user
        /// </summary>
        /// <param name="editedUser">User that contains new information</param>
        public void UpdateInfo(User editedUser)
        {
            HttpResponseMessage response = client.PutAsJsonAsync("api/User", editedUser).Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        /// <param name="id">Id of the user in database</param>
        public void DeleteAccount()
        {
            HttpResponseMessage response = client.DeleteAsync("api/User").Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            JwtToken = String.Empty;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
        }
    }
}
