using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class WinVM : BaseVM
    {
        public ObservableCollection<Monster> PlayerPokemonList { get; set; }
        public ObservableCollection<Monster> EnemyPokemonList { get; set; }
        public ExerciceMonsterContext _context { get; set; }
        public ICommand ReturnToMainViewCommandMenu { get; }
        public ICommand ReturnToMainViewCommandFight { get; }


        public WinVM(ExerciceMonsterContext context, ObservableCollection<Monster> playerPokemonList, ObservableCollection<Monster> enemyPokemonList)
        {
            _context = context;
            PlayerPokemonList = new ObservableCollection<Monster>(playerPokemonList);
            EnemyPokemonList = new ObservableCollection<Monster>(enemyPokemonList);
            ReturnToMainViewCommandFight = new RelayCommand(HandleReturnToFight);
            ReturnToMainViewCommandMenu = new RelayCommand(HandleReturnToMenu);
        }

        private void HandleReturnToFight()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new FightVM(PlayerPokemonList, EnemyPokemonList, _context));
        }

        private void HandleReturnToMenu()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(_context));
        }
    }
}
