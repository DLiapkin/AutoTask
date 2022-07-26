using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutoTask.MVVM.ViewModel;

namespace AutoTask.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        public EditTaskWindow(ProcessViewModel processViewModel)
        {
            InitializeComponent();
            this.DataContext = processViewModel;
        }
    }
}
