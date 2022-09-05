using AutoTask.Domain.Model;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoTask.UI.MVVM.Model.Interface;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls Account View
    /// </summary>
    public partial class AccountViewModel : ObservableObject
    {
        [ObservableProperty]
        private IAccount currentAccount;
        [ObservableProperty]
        private User currentUser;

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public AccountViewModel(IAccount account)
        {
            CurrentAccount = account;
            CurrentUser = currentAccount.User;

            EditCommand = new RelayCommand(() =>
            {
                CurrentAccount.UpdateInfo(CurrentUser);
            });

            DeleteCommand = new RelayCommand(() =>
            {
                CurrentAccount.DeleteAccount();
                CurrentUser = CurrentAccount.User;
            });
        }
    }
}
