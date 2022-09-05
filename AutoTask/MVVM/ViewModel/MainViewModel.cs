using AutoTask.UI.MVVM.View;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoTask.UI.MVVM.Model.Interface;

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

        public MainViewModel(IAccount account)
        {
            CurrentAccount = account;

            ProcessVM = new ProcessViewModel();
            CurrentView = ProcessVM;

            ProcessViewCommand = new RelayCommand(() =>
            {
                CurrentAccount = account;
                CurrentView = ProcessVM;
            });

            AccountViewCommand = new RelayCommand(() =>
            {
                AccountVM = new AccountViewModel();
                CurrentView = AccountVM;
            });

            MyTasksViewCommand = new RelayCommand(() =>
            {
                MyTasksVM = new MyTasksViewModel();
                CurrentView = MyTasksVM;
            });

            LogOutCommand = new RelayCommand(() =>
            {
                CurrentAccount.IsLoggedIn = false;
                CurrentAccount.LogOut();
                ProcessVM = new ProcessViewModel();
                CurrentView = ProcessVM;
            });

            LogInCommand = new RelayCommand(() =>
            {
                AuthorizationViewModel authorizationViewModel = new AuthorizationViewModel(CurrentAccount);
                AuthorizationWindow authorizationWindow = new AuthorizationWindow(authorizationViewModel);
                authorizationWindow.Show();
            });
        }
    }
}
