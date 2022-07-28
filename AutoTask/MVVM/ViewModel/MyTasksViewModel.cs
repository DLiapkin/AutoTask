using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoTask.Core;
using AutoTask.MVVM.Model;
using DomainModule.Model;
using DomainModule.Repository;

namespace AutoTask.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls My tasks View
    /// </summary>
    public class MyTasksViewModel : ObservableObject
    {
        private Account currentAccount;
        private ObservableCollection<Task> myTasks = new ObservableCollection<Task>();

        public Account CurrentAccount
        {
            get
            {
                return currentAccount; 
            }
            set
            {
                currentAccount = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Task> MyTasks
        {
            get
            {
                return myTasks; 
            }
            set 
            {
                myTasks = value;
                OnPropertyChanged();
            }
        }

        public MyTasksViewModel()
        {
            CurrentAccount = new Account();
            UnitOfWork unit = new UnitOfWork();
            User user = unit.Users.Get(CurrentAccount.User.Id);
            if (user.Tasks != null)
            {
                MyTasks = new ObservableCollection<Task>(user.Tasks.ToList());
            }
            unit.Dispose();
        }
    }
}
