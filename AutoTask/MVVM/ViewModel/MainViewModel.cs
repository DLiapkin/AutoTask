using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoTask.Core;
using AutoTask.MVVM.Model;
using AutoTask.MVVM.View;
using DomainModule.Repository;

namespace AutoTask.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls Main Window
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        public ProcessViewModel ProcessVM { get; set; }
        public AccountViewModel AccountVM { get; set; }
        public MyTasksViewModel MyTasksVM { get; set; }

        public RelayCommand ProcessViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand MyTasksViewCommand { get; set; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand LogOutCommand { get; set; }

        private ObservableObject currentView;
        private Account currentAccount;

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

        public ObservableObject CurrentView
        {
            get 
            { 
                return currentView; 
            }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CurrentAccount = new Account();

            ProcessVM = new ProcessViewModel();
            CurrentView = ProcessVM;

            ProcessViewCommand = new RelayCommand(o =>
            {
                CurrentAccount = new Account();
                CurrentView = ProcessVM;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                AccountVM = new AccountViewModel();
                CurrentView = AccountVM;
            });

            MyTasksViewCommand = new RelayCommand(o =>
            {
                MyTasksVM = new MyTasksViewModel();
                CurrentView = MyTasksVM;
            });

            LogOutCommand = new RelayCommand(o =>
            {
                CurrentAccount.IsLoggedOut = true;
                CurrentAccount.IsLoggedIn = false;
                CurrentAccount.LogOut();
                ProcessVM = new ProcessViewModel();
                CurrentView = ProcessVM;
            });

            LogInCommand = new RelayCommand(o =>
            {
                AuthorizationViewModel authorizationViewModel = new AuthorizationViewModel(CurrentAccount);
                AuthorizationWindow authorizationWindow = new AuthorizationWindow(authorizationViewModel);
                authorizationWindow.Show();
            });
        }
    }
}
