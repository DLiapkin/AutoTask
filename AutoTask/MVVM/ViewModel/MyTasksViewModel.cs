using System.Linq;
using System.Collections.ObjectModel;
using AutoTask.UI.MVVM.Model;
using AutoTask.Domain.Model;
using AutoTask.Domain.Repository;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls My tasks View
    /// </summary>
    public partial class MyTasksViewModel : ObservableObject
    {
        [ObservableProperty]
        private Account currentAccount;
        [ObservableProperty]
        private ObservableCollection<Task> myTasks = new ObservableCollection<Task>();

        public MyTasksViewModel()
        {
            CurrentAccount = new Account();
            UnitOfWork unit = new UnitOfWork();
            User user = unit.Users.Get(currentAccount.User.Id);
            if (user.Tasks != null)
            {
                MyTasks = new ObservableCollection<Task>(user.Tasks.ToList());
            }
            unit.Dispose();
        }
    }
}
