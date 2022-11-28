using AutoTask.Domain.Model;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoTask.UI.MVVM.Model.Interface;
using AutoTask.UI.Core.Interface;

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
        private IAccountOperation accountService;

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public AccountViewModel(IAccount account, IAccountOperation accountOperation)
        {
            CurrentAccount = account;
            CurrentUser = currentAccount.User;
            accountService = accountOperation;

            EditCommand = new RelayCommand(() =>
            {
                accountService.UpdateInfo(CurrentUser);
            });

            DeleteCommand = new RelayCommand(() =>
            {
                accountService.DeleteAccount();
                CurrentUser = CurrentAccount.User;
            });
        }
    }
}
