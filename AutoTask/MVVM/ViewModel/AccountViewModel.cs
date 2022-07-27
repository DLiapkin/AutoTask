using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTask.Core;
using DomainModule.Model;

namespace AutoTask.MVVM.ViewModel
{
    public class AccountViewModel : ObservableObject
    {
        private User currentUser;

        public AccountViewModel(User currentUser)
        {
            this.currentUser = currentUser;
        }
    }
}
