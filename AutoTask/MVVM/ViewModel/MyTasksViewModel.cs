using System.Linq;
using System.Collections.ObjectModel;
using AutoTask.UI.Core;
using AutoTask.UI.MVVM.Model;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;

namespace AutoTask.UI.MVVM.ViewModel
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
