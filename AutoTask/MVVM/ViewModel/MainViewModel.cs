using AutoTask.UI.MVVM.View;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;
using System.Net.Http;
using AutoTask.UI.Core.Interface;

namespace AutoTask.UI.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls Main Window
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        public ProcessViewModel ProcessVM { get; set; }
        public AccountViewModel AccountVM { get; set; }
        public MyTasksViewModel MyTasksVM { get; set; }

        public RelayCommand ProcessViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand MyTasksViewCommand { get; set; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }

        [ObservableProperty]
        private object currentView;
        [ObservableProperty]
        private IAccount currentAccount;
        private HttpClient client;
        private IAccountOperation accountService;

        public MainViewModel(IAccount account, HttpClient httpClient, IAccountOperation accountOperation)
        {
            CurrentAccount = account;
            client = httpClient;
            accountService = accountOperation;

            ProcessVM = new ProcessViewModel(currentAccount, client);
            CurrentView = ProcessVM;

            ProcessViewCommand = new RelayCommand(() =>
            {
                ProcessVM = new ProcessViewModel(currentAccount, client);
                CurrentView = ProcessVM;
            });

            AccountViewCommand = new RelayCommand(() =>
            {
                AccountVM = new AccountViewModel(CurrentAccount, accountService);
                CurrentView = AccountVM;
            });

            MyTasksViewCommand = new RelayCommand(() =>
            {
                MyTasksVM = new MyTasksViewModel(CurrentAccount, client);
                CurrentView = MyTasksVM;
            });

            LogOutCommand = new RelayCommand(() =>
            {
                CurrentAccount.IsLoggedIn = false;
                accountService.LogOut();
                ProcessVM = new ProcessViewModel(currentAccount, client);
                CurrentView = ProcessVM;
            });

            LogInCommand = new RelayCommand(() =>
            {
                AuthorizationViewModel authorizationViewModel = new AuthorizationViewModel(CurrentAccount, accountService);
                AuthorizationWindow authorizationWindow = new AuthorizationWindow(authorizationViewModel);
                authorizationWindow.Show();
            });
        }
    }
}
