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
    public class ProcessViewModel : ObservableObject
    {
        public ObservableCollection<string> processesNames = new ObservableCollection<string>();
        private ObservableCollection<Task> newTasks = new ObservableCollection<Task>();
        private Process currentProcess = new Process();
        private Task currentTask = new Task();
        private string selected = string.Empty;
        private Task newTask = new Task();

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
                ChangeCurrentProcess(); //жуткий костылище! Поменять!
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

        public RelayCommand CreateTaskWindowCommand { get; set; }
        public RelayCommand CreateTaskCommand { get; set; }
        public RelayCommand DeleteTaskCommand { get; set; }

        public ProcessViewModel()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable processes = unitOfWork.Processes.GetAll();
            foreach (Process process in processes)
            {
                processesNames.Add(process.Name);
            }

            CreateTaskCommand = new RelayCommand(o =>
            {
                if (newTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    taskOperation.CreateTask(newTask.Name, newTask.Status, newTask.Progress, newTask.Priority, CurrentProcess.Id);
                    IEnumerable temp = unitOfWork.Processes.Get(CurrentProcess.Id).Tasks.Where(t => t.Name.Equals(NewTask.Name));
                    foreach (Task task in temp)
                    {
                        NewTask = task;
                    }
                    NewTasks.Add(NewTask);
                }
            });

            DeleteTaskCommand = new RelayCommand(o =>
            {
                if (currentTask != null)
                {
                    TaskOperation taskOperation = new TaskOperation();
                    taskOperation.DeleteTask(CurrentTask.Id);
                    NewTasks.Remove(CurrentTask);
                }
            });

            CreateTaskWindowCommand = new RelayCommand(o =>
            {
                CreateTaskWindow createTaskWindow = new CreateTaskWindow(this);
                createTaskWindow.Show();
            });
        }

        private void ChangeCurrentProcess()
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
                GetNewTasks();
            }
        }

        private void GetNewTasks()
        {
            IEnumerable temp = CurrentProcess.Tasks.Where(o => o.Status.Equals("New"));
            NewTasks.Clear();
            foreach (Task task in temp)
            {
                NewTasks.Add(task);
            }
        }
    }
}