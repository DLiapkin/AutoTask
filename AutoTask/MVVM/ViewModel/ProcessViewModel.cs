using System;
using System.Linq;
using AutoTask.Core;
using DomainModule.Model;
using DomainModule.Repository;
using System.Collections;
using System.Collections.ObjectModel;
using Infrastructure;
using AutoTask.MVVM.View;

namespace AutoTask.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls processes and tasks displaying on Process View
    /// </summary>
    public class ProcessViewModel : ObservableObject
    {
        public ObservableCollection<string> processesNames = new ObservableCollection<string>();
        private ObservableCollection<Task> newTasks = new ObservableCollection<Task>();
        private ObservableCollection<Task> inProgressTasks = new ObservableCollection<Task>();
        private ObservableCollection<Task> closedTasks = new ObservableCollection<Task>();
        private string selected = string.Empty;
        private Process currentProcess = new Process();
        private Process newProcess = new Process();
        private Task currentTask = new Task();
        private Task newTask = new Task();
        private User currentUser = new User();

        public User CurrentUser 
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged();
            }
        }

        public string Selected
        {
            get 
            {
                return selected; 
            }
            set
            {
                selected = value;
                OnPropertyChanged();
                UpdateCurrentProcess();
            }
        }

        public Process CurrentProcess 
        { 
            get
            {
                return currentProcess;
            }
            set
            {
                currentProcess = value;
                OnPropertyChanged();
            }
        }

        public Task CurrentTask
        {
            get
            {
                return currentTask;
            }
            set
            {
                currentTask = value;
                OnPropertyChanged();
            }
        }

        public Task NewTask
        {
            get
            {
                return newTask;
            }
            set
            {
                newTask = value;
                OnPropertyChanged();
            }
        }

        public Process NewProcess
        {
            get
            {
                return newProcess;
            }
            set
            {
                newProcess = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ProcessesNames 
        {
            get => processesNames;
            set
            {
                processesNames = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Task> NewTasks
        {
            get => newTasks;
            set
            {
                newTasks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Task> InProgressTasks
        {
            get
            {
                return inProgressTasks;
            }
            set
            {
                inProgressTasks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Task> ClosedTasks
        {
            get
            {
                return closedTasks;
            }
            set
            {
                closedTasks = value;
                OnPropertyChanged();
            }
        }

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
            UpdateCurrentUser();

            CreateTaskCommand = new RelayCommand(o =>
            {
                if (newTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    UpdateCurrentUser();
                    if (CurrentUser.IsLogged)
                    {
                        taskOperation.CreateTask(newTask.Name, newTask.Status, newTask.Progress, newTask.Priority, CurrentProcess.Id, CurrentUser.Id);
                    }
                    else
                    {
                        taskOperation.CreateTask(newTask.Name, newTask.Status, newTask.Progress, newTask.Priority, CurrentProcess.Id, null);
                    }
                    UpdateCurrentProcess();
                    UpdateTasks();
                }
            });

            UpdateTaskCommand = new RelayCommand(o =>
            {
                if (currentTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    taskOperation.UpdateTask(CurrentTask.Id, CurrentTask.Name, CurrentTask.Status, CurrentTask.Progress, CurrentTask.Priority, CurrentProcess.Id);
                    UpdateCurrentProcess();
                    UpdateTasks();
                }
            });

            DeleteTaskCommand = new RelayCommand(o =>
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

            CreateProcessCommand = new RelayCommand(o =>
            {
                if (newProcess != null)
                {
                    ProcessOperation processOperation = new ProcessOperation();
                    processOperation.CreateProcess(newProcess.Name, newProcess.Begin, newProcess.End, newProcess.Description);
                    UpdateProcesses();
                }
            });

            UpdateProcessCommand = new RelayCommand(o =>
            {
                if (currentProcess != null)
                {
                    ProcessOperation processOperation = new ProcessOperation();
                    processOperation.UpdateProcess(CurrentProcess.Id, CurrentProcess.Name, CurrentProcess.Begin, CurrentProcess.End, CurrentProcess.Description);
                    UpdateProcesses();
                }
            });

            DeleteProcessCommand = new RelayCommand(o =>
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

            CreateTaskWindowCommand = new RelayCommand(o =>
            {
                CreateTaskWindow createTaskWindow = new CreateTaskWindow(this);
                createTaskWindow.Show();
            });

            EditTaskWindowCommand = new RelayCommand(o =>
            {
                EditTaskWindow editTaskWindow = new EditTaskWindow(this);
                editTaskWindow.Show();
            });

            CreateProcessWindowCommand = new RelayCommand(o =>
            {
                CreateProcessWindow createProcessWindow = new CreateProcessWindow(this);
                createProcessWindow.Show();
            });

            EditProcessWindowCommand = new RelayCommand(o =>
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

        private void UpdateCurrentUser()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            CurrentUser = unitOfWork.Users.GetAll().FirstOrDefault(o => o.IsLogged);
            if (CurrentUser == null)
            {
                CurrentUser = new User();
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
    }
}