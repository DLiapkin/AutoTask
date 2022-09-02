using System;
using System.Linq;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using AutoTask.Shared;
using AutoTask.UI.MVVM.View;
using AutoTask.UI.MVVM.Model;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls processes and tasks displaying on Process View
    /// </summary>
    public partial class ProcessViewModel : ObservableObject
    {
        private HttpClient client = new HttpClient();
        [ObservableProperty]
        private Account currentAccount;
        [ObservableProperty]
        private Task newTask = new Task();
        [ObservableProperty]
        private Task currentTask = new Task();
        [ObservableProperty]
        private Process newProcess = new Process();
        [ObservableProperty]
        private Process currentProcess = new Process();
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(UpdateCurrentProcess))]
        private string selected = string.Empty;
        [ObservableProperty]
        public ObservableCollection<string> processesNames = new ObservableCollection<string>();
        [ObservableProperty]
        private ObservableCollection<Task> newTasks = new ObservableCollection<Task>();
        [ObservableProperty]
        private ObservableCollection<Task> inProgressTasks = new ObservableCollection<Task>();
        [ObservableProperty]
        private ObservableCollection<Task> closedTasks = new ObservableCollection<Task>();

        public RelayCommand CreateTaskWindowCommand { get; set; }
        public RelayCommand EditTaskWindowCommand { get; set; }
        public RelayCommand CreateTaskCommand { get; set; }
        public RelayCommand UpdateTaskCommand { get; set; }
        public RelayCommand DeleteTaskCommand { get; set; }

        public RelayCommand CreateProcessWindowCommand { get; set; }
        public RelayCommand EditProcessWindowCommand { get; set; }
        public RelayCommand CreateProcessCommand { get; set; }
        public RelayCommand UpdateProcessCommand { get; set; }
        public RelayCommand DeleteProcessCommand { get; set; }

        public ProcessViewModel()
        {
            client.BaseAddress = new Uri("https://localhost:7107/");
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

            UnitOfWork unitOfWork = new UnitOfWork();
            UpdateProcesses();
            CurrentAccount = new Account();

            CreateTaskCommand = new RelayCommand(() =>
            {
                if (newTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    CurrentAccount.UpdateUser();
                    if (CurrentAccount.IsLoggedIn)
                    {
                        taskOperation.CreateTask(newTask.Name, newTask.Status, newTask.Progress, newTask.Priority, CurrentProcess.Id, CurrentAccount.User.Id);
                    }
                    else
                    {
                        taskOperation.CreateTask(newTask.Name, newTask.Status, newTask.Progress, newTask.Priority, CurrentProcess.Id, null);
                    }
                    UpdateCurrentProcess();
                    UpdateTasks();
                }
            });

            UpdateTaskCommand = new RelayCommand(() =>
            {
                if (currentTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    taskOperation.UpdateTask(CurrentTask.Id, CurrentTask.Name, CurrentTask.Status, CurrentTask.Progress, CurrentTask.Priority, CurrentProcess.Id);
                    UpdateCurrentProcess();
                    UpdateTasks();
                }
            });

            DeleteTaskCommand = new RelayCommand(() =>
            {
                if (currentTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    taskOperation.DeleteTask(CurrentTask.Id);
                    NewTasks.Remove(CurrentTask);
                    UpdateCurrentProcess();
                    UpdateTasks();
                }
            });

            CreateProcessCommand = new RelayCommand(() =>
            {
                if (newProcess == null)
                {
                    return;
                }
                HttpResponseMessage response = client.PostAsJsonAsync("api/Process", newProcess).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
                UpdateProcesses();
            });

            UpdateProcessCommand = new RelayCommand(() =>
            {
                if (currentProcess == null)
                {
                    return;
                }
                HttpResponseMessage response = client.PutAsJsonAsync($"api/Process/{currentProcess.Id}", currentProcess).Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
                UpdateProcesses();
            });

            DeleteProcessCommand = new RelayCommand(() =>
            {
                if (currentProcess == null)
                {
                    return;
                }
                HttpResponseMessage response = client.DeleteAsync($"api/Process/{currentProcess.Id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
                CurrentProcess = new Process();
                UpdateProcesses();
                newTasks.Clear();
                inProgressTasks.Clear();
                closedTasks.Clear();
            });

            CreateTaskWindowCommand = new RelayCommand(() =>
            {
                CreateTaskWindow createTaskWindow = new CreateTaskWindow(this);
                createTaskWindow.Show();
            });

            EditTaskWindowCommand = new RelayCommand(() =>
            {
                EditTaskWindow editTaskWindow = new EditTaskWindow(this);
                editTaskWindow.Show();
            });

            CreateProcessWindowCommand = new RelayCommand(() =>
            {
                CreateProcessWindow createProcessWindow = new CreateProcessWindow(this);
                createProcessWindow.Show();
            });

            EditProcessWindowCommand = new RelayCommand(() =>
            {
                EditProcessWindow editProcessWindow = new EditProcessWindow(this);
                editProcessWindow.Show();
            });
        }

        /// <summary>
        /// Updates current process by selected from ComboBox
        /// </summary>
        private async void UpdateCurrentProcess()
        {
            HttpResponseMessage response = client.GetAsync("api/Process").Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable processes = await response.Content.ReadFromJsonAsync<IEnumerable<Process>>();
                foreach (Process process in processes)
                {
                    if (process.Name.Equals(Selected))
                    {
                        CurrentProcess = process;
                        UpdateTasks();
                    }
                }
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Updates processes for ComboBox
        /// </summary>
        private async void UpdateProcesses()
        {
            HttpResponseMessage response = client.GetAsync("api/Process").Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable processes = await response.Content.ReadFromJsonAsync<IEnumerable<Process>>();
                processesNames.Clear();
                foreach (Process process in processes)
                {
                    processesNames.Add(process.Name);
                }
            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }

        /// <summary>
        /// Updates tasks for DataGrids from database by current process
        /// </summary>
        private void UpdateTasks()
        {
            if (currentProcess != null)
            {
                IEnumerable temp = CurrentProcess.Tasks.Where(o => o.Status.Equals("New"));
                NewTasks.Clear();
                foreach (Task task in temp)
                {
                    NewTasks.Add(task);
                }
                temp = CurrentProcess.Tasks.Where(o => o.Status.Equals("In Progress"));
                InProgressTasks.Clear();
                foreach (Task task in temp)
                {
                    InProgressTasks.Add(task);
                }
                temp = CurrentProcess.Tasks.Where(o => o.Status.Equals("Closed"));
                ClosedTasks.Clear();
                foreach (Task task in temp)
                {
                    ClosedTasks.Add(task);
                }
            }
        }

        /// <summary>
        /// Overriding base OnPropertyChanged to call method UpdateCurrentProcess() on Selected property changing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == nameof(Selected))
            {
                UpdateCurrentProcess();
            }
        }
    }
}