using AutoTask.UI.Core.Interface;
using AutoTask.UI.MVVM.Model.Interface;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using AutoTask.Domain.Model;
using System.Windows;
using System.Configuration;

namespace AutoTask.UI.Core
{
    public class AccountOperation : IAccountOperation
    {
        private IAccount account;
        private HttpClient client = new HttpClient();

        public AccountOperation(IAccount account, HttpClient client)
        {
            this.account = account;
            this.client = client;
            this.client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("AutoTaskApi"));
        }

        /// <summary>
        /// Logs out current user
        /// </summary>
        public void LogOut()
        {
            if (!account.IsLoggedIn)
            {
                return;
            }
            HttpResponseMessage response = client.GetAsync("api/Login").Result;
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
            account.JwtToken = String.Empty;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.JwtToken);
            account.User = new User();
            account.IsLoggedIn = false;
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
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
            account.JwtToken = await response.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.JwtToken);
            response = client.GetAsync("api/User").Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
            User user = await response.Content.ReadFromJsonAsync<User>();
            if (user != null)
            {
                account.User = user;
                account.IsLoggedIn = true;
            }
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="newUser">New user to register</param>
        public void Register(User newUser)
        {
            HttpResponseMessage response = client.PostAsJsonAsync("api/User", newUser).Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
        }

        /// <summary>
        /// Updates information about current user
        /// </summary>
        /// <param name="editedUser">User that contains new information</param>
        public void UpdateInfo(User editedUser)
        {
            HttpResponseMessage response = client.PutAsJsonAsync($"api/User/{editedUser.Id}", editedUser).Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        /// <param name="id">Id of the user in database</param>
        public void DeleteAccount()
        {
            HttpResponseMessage response = client.DeleteAsync($"api/User/{account.User.Id}").Result;
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
            account.JwtToken = String.Empty;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", account.JwtToken);
        }
    }
}
