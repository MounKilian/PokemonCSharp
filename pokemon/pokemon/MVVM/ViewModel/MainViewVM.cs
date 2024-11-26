using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }  
        public MainViewVM() 
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
        }
        
        public void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new LoginVM());
        }
    }
}
