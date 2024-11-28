using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class PokemonDetailsVM : BaseVM
    {
        public Monster SelectedPokemon { get; set; }
        public ExerciceMonsterContext _context { get; set; }
        public ICommand ReturnToMainViewCommand { get; }

        public PokemonDetailsVM(Monster selectedPokemon, ExerciceMonsterContext context)
        {
            _context = context;
            SelectedPokemon = selectedPokemon;

            ReturnToMainViewCommand = new RelayCommand(HandleReturnToMainView);
        }

        private void HandleReturnToMainView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(_context));
        }
    }
}
