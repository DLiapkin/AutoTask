using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
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
                if (newProcess != null)
                {
                    ProcessOperation processOperation = new ProcessOperation();
                    processOperation.CreateProcess(newProcess.Name, newProcess.Begin, newProcess.End, newProcess.Description);
                    UpdateProcesses();
                }
            });

            UpdateProcessCommand = new RelayCommand(() =>
            {
                if (currentProcess != null)
                {
                    ProcessOperation processOperation = new ProcessOperation();
                    processOperation.UpdateProcess(CurrentProcess.Id, CurrentProcess.Name, CurrentProcess.Begin, CurrentProcess.End, CurrentProcess.Description);
                    UpdateProcesses();
                }
            });

            DeleteProcessCommand = new RelayCommand(() =>
            {
                if (currentProcess != null)
                {
                    ProcessOperation processOperation = new ProcessOperation();
                    processOperation.DeleteProcess(CurrentProcess.Id);
                    CurrentProcess = new Process();
                    UpdateProcesses();
                    newTasks.Clear();
                    inProgressTasks.Clear();
                    closedTasks.Clear();
                }
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
        private void UpdateCurrentProcess()
        {
            int id = -1;
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable processes = unitOfWork.Processes.GetAll();
            foreach (Process process in processes)
            {
                if (process.Name.Equals(Selected))
                {
                    id = process.Id;
                }
            }
            if (id >= 0)
            {
                CurrentProcess = unitOfWork.Processes.Get(id);
                UpdateTasks();
            }
        }

        /// <summary>
        /// Updates processes for ComboBox
        /// </summary>
        private void UpdateProcesses()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable processes = unitOfWork.Processes.GetAll();
            processesNames.Clear();
            foreach (Process process in processes)
            {
                processesNames.Add(process.Name);
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