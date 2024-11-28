﻿using pokemon.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace pokemon.MVVM.View
{
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as InitViewVM;
            if (viewModel != null)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

    }
}