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
        public ProcessViewModel ProcessViewModel { get; set; }
        public AccountViewModel AccountViewModel { get; set; }

        public RelayCommand ProcessViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
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
            UnitOfWork unitOfWork = new UnitOfWork();
            CurrentAccount = new Account();

            ProcessViewModel = new ProcessViewModel();
            AccountViewModel = new AccountViewModel(CurrentAccount.User);
            CurrentView = ProcessViewModel;

            ProcessViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProcessViewModel;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountViewModel;
            });

            LogOutCommand = new RelayCommand(o =>
            {
                CurrentAccount.IsLoggedOut = true;
                CurrentAccount.IsLoggedIn = false;
                CurrentAccount.LogOut();
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
