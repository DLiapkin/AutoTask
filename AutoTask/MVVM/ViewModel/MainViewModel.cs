using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoTask.Core;

namespace AutoTask.MVVM.ViewModel
{
    /// <summary>
    /// Represents View Model that controls Main Window
    /// </summary>
    class MainViewModel : ObservableObject
    {
        public ProcessViewModel ProcessViewModel { get; set; }
        //public TableViewModel TableViewModel { get; set; }

        public RelayCommand ProcessViewCommand { get; set; }
        public RelayCommand TableViewCommand { get; set; }

        private ObservableObject _currentView;

        public ObservableObject CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            ProcessViewModel = new ProcessViewModel();
            //TableViewModel = new TableViewModel();
            CurrentView = ProcessViewModel;

            ProcessViewCommand = new RelayCommand(o =>
            {
                CurrentView = ProcessViewModel;
            });
            TableViewCommand = new RelayCommand(o =>
            {
                //CurrentView = TableViewModel;
            });
        }
    }
}
