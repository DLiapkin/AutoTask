using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTask.Core;
using AutoTask.MVVM.Model;
using DomainModule.Model;

namespace AutoTask.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls Account View
    /// </summary>
    public class AccountViewModel : ObservableObject
    {
        private Account currentAccount;
        private User currentUser;

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

        public User CurrentUser
        {
            get 
            { 
                return currentUser; 
            }
            set 
            { 
                currentUser = value; 
                OnPropertyChanged(); 
            }
        }

        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }

        public AccountViewModel()
        {
            CurrentAccount = new Account();
            CurrentUser = currentAccount.User;

            EditCommand = new RelayCommand(o =>
            {
                CurrentAccount.UpdateInfo(CurrentUser);
            });

            DeleteCommand = new RelayCommand(o =>
            {
                CurrentAccount.DeleteAccount(CurrentUser.Id);
                CurrentAccount = new Account();
                CurrentUser = CurrentAccount.User;
            });
        }
    }
}
