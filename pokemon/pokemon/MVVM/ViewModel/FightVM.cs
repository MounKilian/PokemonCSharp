using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace pokemon.MVVM.ViewModel
{
    public class FightVM : BaseVM
    {
        private readonly ExerciceMonsterContext _context;

        private bool _isPlayerTurn;
        private Monster _selectedPlayerPokemon;
        private Monster _selectedEnemyPokemon;
        public int HealthMaxPlayer { get; set; }
        public int HealthMaxEnnemy { get; set; }

        public Monster SelectedPlayerPokemon
        {
            get => _selectedPlayerPokemon;
            set
            {
                _selectedPlayerPokemon = value;
                LoadSpells();
                OnPropertyChanged();
            }
        }

        public Monster SelectedEnemyPokemon                                                                                                                                                                                 
        {
            get => _selectedEnemyPokemon;
            set
            {
                _selectedEnemyPokemon = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Monster> PlayerPokemonList { get; set; }
        public ObservableCollection<Monster> EnemyPokemonList { get; set; }
        public ObservableCollection<Spell> Spells { get; private set; }

        public RelayCommand<Spell> AttackCommand { get; set; }
        public RelayCommand<Monster> ChangePokemonCommand { get; set; }

        public FightVM(ObservableCollection<Monster> playerPokemonList, ObservableCollection<Monster> enemyPokemonList, ExerciceMonsterContext context)
        {
            _context = context;
            PlayerPokemonList = playerPokemonList ?? new ObservableCollection<Monster>();
            EnemyPokemonList = enemyPokemonList ?? new ObservableCollection<Monster>();

            SelectedPlayerPokemon = PlayerPokemonList.FirstOrDefault();
            SelectedEnemyPokemon = EnemyPokemonList.FirstOrDefault();
            HealthMaxPlayer = SelectedPlayerPokemon.Health;
            HealthMaxEnnemy = SelectedEnemyPokemon.Health;
            OnPropertyChanged(nameof(HealthMaxPlayer));
            OnPropertyChanged(nameof(HealthMaxEnnemy));

            _isPlayerTurn = true;
            AttackCommand = new RelayCommand<Spell>(ExecuteAttack, CanExecuteAttack);
            ChangePokemonCommand = new RelayCommand<Monster>(ChangePlayerPokemon);
        }

        private void LoadSpells()
        {
            if (SelectedPlayerPokemon == null) return;

            var spells = _context.Spells
                .Where(spell => spell.Monsters.Any(monster => monster.Id == SelectedPlayerPokemon.Id))
                .ToList();

            Spells = new ObservableCollection<Spell>(spells);
            OnPropertyChanged(nameof(Spells));
        }

        private async void ExecuteAttack(Spell selectedSpell)
        {
            if (selectedSpell == null || SelectedEnemyPokemon == null) return;

            _isPlayerTurn = false;
            AttackCommand.NotifyCanExecuteChanged();

            SelectedEnemyPokemon.Health -= selectedSpell.Damage;
            if (SelectedEnemyPokemon.Health <= 0)
            {
                SelectedEnemyPokemon.Health = 0;
            }
            OnPropertyChanged(nameof(SelectedEnemyPokemon));

            if (SelectedEnemyPokemon.Health <= 0)
            {
                EnemyPokemonList.Remove(SelectedEnemyPokemon);

                if (EnemyPokemonList.Any())
                {
                    SelectedEnemyPokemon = EnemyPokemonList.FirstOrDefault();
                    SelectedEnemyPokemon.Health = (int)(SelectedEnemyPokemon.Health * 1.05);
                    HealthMaxEnnemy = SelectedEnemyPokemon.Health;
                    OnPropertyChanged(nameof(HealthMaxEnnemy));
                }
                else
                {
                    MessageBox.Show("Vous avez gagné ! Tous les Pokémon ennemis ont été battus.");
                    return;
                }
            }

            await Task.Delay(1000);

            if (SelectedEnemyPokemon != null)
            {
                EnemyAttack();
            }

            if (!PlayerPokemonList.Any())
            {
                MessageBox.Show("Vous avez perdu ! Aucun Pokémon restant.");
                return;
            }

            _isPlayerTurn = true;
            AttackCommand.NotifyCanExecuteChanged();
        }



        private bool CanExecuteAttack(Spell selectedSpell) => _isPlayerTurn;

        private void EnemyAttack()
        {
            if (SelectedEnemyPokemon == null || SelectedPlayerPokemon == null) return;

            var enemySpell = _context.Spells
                .Where(spell => spell.Monsters.Any(monster => monster.Id == SelectedEnemyPokemon.Id))
                .FirstOrDefault();

            if (enemySpell != null)
            {
                SelectedPlayerPokemon.Health -= enemySpell.Damage;
                if (SelectedPlayerPokemon.Health <= 0)
                {
                    SelectedEnemyPokemon.Health = 0;
                }
                OnPropertyChanged(nameof(SelectedPlayerPokemon));

                if (SelectedPlayerPokemon.Health <= 0)
                {
                    PlayerPokemonList.Remove(SelectedPlayerPokemon);

                    if (PlayerPokemonList.Any())
                    {
                        SelectedPlayerPokemon = PlayerPokemonList.FirstOrDefault();
                        HealthMaxPlayer = SelectedPlayerPokemon.Health;
                        OnPropertyChanged(nameof(HealthMaxPlayer));
                        LoadSpells();
                    }
                    else
                    {
                        MessageBox.Show("Vous avez perdu ! Aucun Pokémon restant.");
                        return;
                    }
                }
            }
        }


        private void ChangePlayerPokemon(Monster newPokemon)
        {
            if (newPokemon == null || newPokemon == SelectedPlayerPokemon) return;

            SelectedPlayerPokemon = newPokemon;
            HealthMaxPlayer = SelectedPlayerPokemon.Health;
            OnPropertyChanged(nameof(HealthMaxPlayer));
            LoadSpells();
        }
    }
}
