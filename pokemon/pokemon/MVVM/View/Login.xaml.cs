using pokemon.MVVM.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pokemon.MVVM.View
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TabPage1View_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as LoginVM;
            if (viewModel != null)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        private void ActionText_Click(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (LoginVM)DataContext;  
            viewModel.ToggleViewMode(); 
        }

    }
}
