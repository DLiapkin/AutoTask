using System;
using System.Linq;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoTask.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls My tasks View
    /// </summary>
    public partial class MyTasksViewModel : ObservableObject
    {
        [ObservableProperty]
        private IAccount currentAccount;
        [ObservableProperty]
        private ObservableCollection<Task> myTasks = new ObservableCollection<Task>();
        HttpClient client = new HttpClient();

        public MyTasksViewModel(IAccount account)
        {
            CurrentAccount = account;
            CurrentAccount = account;
            client.BaseAddress = new Uri("https://localhost:7107/");
            LoadTasks();
        }

        private async void LoadTasks()
        {
            HttpResponseMessage response = client.GetAsync("api/Task").Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<Task> tasks = await response.Content.ReadFromJsonAsync<IEnumerable<Task>>();
                myTasks = new ObservableCollection<Task>(tasks.Where(t => t.UserId == currentAccount.User.Id).ToList());
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }
    }
}
