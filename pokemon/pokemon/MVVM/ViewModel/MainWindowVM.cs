namespace pokemon.MVVM.ViewModel
{
    public class MainWindowVM : BaseVM
    {

        static public  Action<BaseVM> OnRequestVMChange;

        #region Commands
        #endregion
        #region Variables

        private BaseVM _currentVM;
        public BaseVM CurrentVM
        {
            get => _currentVM;
            set
            {
                SetProperty(ref _currentVM, value);
                OnPropertyChanged(nameof(CurrentVM));
            }
        }

        #endregion

        public MainWindowVM()
        {
            MainWindowVM.OnRequestVMChange += HandleRequestViewChange;
            MainWindowVM.OnRequestVMChange?.Invoke(new InitViewVM());
        }



        public void HandleRequestViewChange(BaseVM a_VMToChange)
        {
            CurrentVM = a_VMToChange;
        }
      
    }
}
