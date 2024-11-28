using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        public ICommand ChangeViewCommand { get; set; }
        public ObservableCollection<Monster> Pokemons { get; set; }
        public ObservableCollection<Spell> Spells { get; set; }

        private Monster _selectedPokemon;
        public Monster SelectedPokemon
        {
            get => _selectedPokemon;
            set
            {
                _selectedPokemon = value;
                OnPropertyChanged(nameof(SelectedPokemon));

                if (_selectedPokemon != null)
                {
                    ChangeViewCommand.Execute(null);
                }
            }
        }

        private readonly ExerciceMonsterContext _context;

        public MainViewVM(ExerciceMonsterContext context)
        {
            _context = context;
            Pokemons = new ObservableCollection<Monster>();
            Spells = new ObservableCollection<Spell>();
            LoadPokemons();
            LoadSpells();
            ChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
        }

        private void LoadPokemons()
        {
            var pokemons = _context.Monsters.ToList();
            foreach (var poke in pokemons)
            {
                Pokemons.Add(poke);
            }
        }

        private void LoadSpells()
        {
            var spells = _context.Spells.ToList();
            foreach (var spell in spells)
            {
                Spells.Add(spell);
            }
        }

        private void HandleRequestChangeViewCommand()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new PokemonDetailsVM(SelectedPokemon, _context));
        }
    }
}
