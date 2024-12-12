using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class MainViewVM : BaseVM
    {
        public ICommand ChangeViewCommand { get; set; }
        public ICommand ChangeViewCommandSpell { get; set; }
        public ICommand ValidateSelectionsCommand { get; set; }

        public ObservableCollection<Monster> Pokemons { get; set; }
        public ObservableCollection<Spell> Spells { get; set; }

        private Monster _selectedPokemon;
        public Monster SelectedPokemon
        {
            get => _selectedPokemon;
            set
            {
                if (_selectedPokemon != value)
                {
                    _selectedPokemon = value;
                    OnPropertyChanged(nameof(SelectedPokemon));
                    if (_selectedPokemon != null)
                    {
                        ChangeViewCommand.Execute(null);
                    }
                }
            }
        }

        private Spell _selectedSpell;
        public Spell SelectedSpell
        {
            get => _selectedSpell;
            set
            {
                if (_selectedSpell != value)
                {
                    _selectedSpell = value;
                    OnPropertyChanged(nameof(SelectedSpell));
                    if (_selectedSpell != null)
                    {
                        ChangeViewCommandSpell.Execute(null);
                    }
                }
            }
        }

        private Monster _selectedPokemonForFilter;
        public Monster SelectedPokemonForFilter
        {
            get => _selectedPokemonForFilter;
            set
            {
                if (_selectedPokemonForFilter != value)
                {
                    _selectedPokemonForFilter = value;
                    OnPropertyChanged(nameof(SelectedPokemonForFilter));
                    UpdateFilteredSpells();
                }
            }
        }

        private ObservableCollection<Spell> _filteredSpells;
        public ObservableCollection<Spell> FilteredSpells
        {
            get => _filteredSpells;
            set
            {
                _filteredSpells = value;
                OnPropertyChanged(nameof(FilteredSpells));
            }
        }

        private readonly ExerciceMonsterContext _context;

        public ObservableCollection<Monster> PlayerPokemonList { get; set; } = new();
        public ObservableCollection<Monster> EnemyPokemonList { get; set; } = new();

        private Monster _selectedPlayerPokemon;
        public Monster SelectedPlayerPokemon
        {
            get => _selectedPlayerPokemon;
            set
            {
                if (_selectedPlayerPokemon != value)
                {
                    _selectedPlayerPokemon = value;
                    OnPropertyChanged();
                    AddToPlayerSelection(value);
                }
            }
        }

        private Monster _selectedEnemyPokemon;
        public Monster SelectedEnemyPokemon
        {
            get => _selectedEnemyPokemon;
            set
            {
                if (_selectedEnemyPokemon != value)
                {
                    _selectedEnemyPokemon = value;
                    OnPropertyChanged();
                    AddToEnemySelection(value);
                }
            }
        }

        public MainViewVM(ExerciceMonsterContext context)
        {
            _context = context;
            Pokemons = new ObservableCollection<Monster>();
            Spells = new ObservableCollection<Spell>();
            FilteredSpells = new ObservableCollection<Spell>();
            LoadPokemons();
            LoadSpells();
            ChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            ChangeViewCommandSpell = new RelayCommand(HandleRequestChangeViewCommandSpell);
            ValidateSelectionsCommand = new RelayCommand(ValidateSelections);
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
            FilteredSpells = new ObservableCollection<Spell>(Spells);
        }

        private void UpdateFilteredSpells()
        {
            if (SelectedPokemonForFilter != null)
            {
                FilteredSpells = new ObservableCollection<Spell>(_context.Spells.Where(spell => spell.Monsters.Any(monster => monster.Id == SelectedPokemonForFilter.Id)));
            }
            else
            {
                FilteredSpells = new ObservableCollection<Spell>(Spells);
            }
        }

        private void HandleRequestChangeViewCommand()
        {
            if (SelectedPokemon != null)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new PokemonDetailsVM(SelectedPokemon, _context));
            }
        }

        private void HandleRequestChangeViewCommandSpell()
        {
            if (SelectedSpell != null)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new SpellDetailsVM(SelectedSpell, _context));
            }
        }

        private void AddToPlayerSelection(Monster selected)
        {
            if (selected != null && PlayerPokemonList.Count < 2 && !PlayerPokemonList.Contains(selected))
            {
                PlayerPokemonList.Add(selected);
            }
            else if (PlayerPokemonList.Contains(selected))
            {
                PlayerPokemonList.Remove(selected);
            }
        }

        private void AddToEnemySelection(Monster selected)
        {
            if (selected != null && EnemyPokemonList.Count < 2 && !EnemyPokemonList.Contains(selected))
            {
                EnemyPokemonList.Add(selected);
            }
            else if (EnemyPokemonList.Contains(selected))
            {
                EnemyPokemonList.Remove(selected);
            }
        }

        private void ValidateSelections()
        {
            if (PlayerPokemonList.Count == 2 && EnemyPokemonList.Count == 2)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new FightVM(PlayerPokemonList, EnemyPokemonList, _context));
            }
            else
            {
                MessageBox.Show("Tout le monde doit avoir au moins 2 pokémons");
            }
        }
    }
}
