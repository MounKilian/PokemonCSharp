﻿using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using pokemon.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class FightVM : BaseVM
    {
        private readonly ExerciceMonsterContext _context;

        private bool _isPlayerTurn;
        private bool _isFirstEnemyDied;
        public int EnemyScore { get; set; }
        public int PlayerScore { get; set; }

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

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Monster> PlayerPokemonList { get; set; }
        public ObservableCollection<Monster> EnemyPokemonList { get; set; }
        private ObservableCollection<Monster> PlayerPokemonListMemory { get; set; }
        private ObservableCollection<Monster> EnemyPokemonListMemory { get; set; }
        public ObservableCollection<Spell> Spells { get; private set; }
        public RelayCommand<Spell> AttackCommand { get; set; }
        public RelayCommand<Monster> ChangePokemonCommand { get; set; }

        public FightVM(ObservableCollection<Monster> playerPokemonList, ObservableCollection<Monster> enemyPokemonList, ExerciceMonsterContext context)
        {
            _context = context;
            _isFirstEnemyDied = false;
            PlayerScore = 0;
            EnemyScore = 0;

            PlayerPokemonList = new ObservableCollection<Monster>(playerPokemonList?.Select(pokemon => ClonePokemon(pokemon)) ?? new List<Monster>());
            EnemyPokemonList = new ObservableCollection<Monster>(enemyPokemonList?.Select(pokemon => ClonePokemon(pokemon)) ?? new List<Monster>());
            PlayerPokemonListMemory = new ObservableCollection<Monster>(playerPokemonList?.Select(pokemon => ClonePokemon(pokemon)) ?? new List<Monster>());
            EnemyPokemonListMemory = new ObservableCollection<Monster>(enemyPokemonList?.Select(pokemon => ClonePokemon(pokemon)) ?? new List<Monster>());

            SelectedPlayerPokemon = PlayerPokemonList.FirstOrDefault();
            SelectedEnemyPokemon = EnemyPokemonList.FirstOrDefault();

            HealthMaxPlayer = SelectedPlayerPokemon?.Health ?? 0;
            HealthMaxEnnemy = SelectedEnemyPokemon?.Health ?? 0;

            OnPropertyChanged(nameof(HealthMaxPlayer));
            OnPropertyChanged(nameof(HealthMaxEnnemy));

            _isPlayerTurn = true;

            AttackCommand = new RelayCommand<Spell>(ExecuteAttack, CanExecuteAttack);
            ChangePokemonCommand = new RelayCommand<Monster>(ChangePlayerPokemon, CanChangeMonster);
        }

        private Monster ClonePokemon(Monster original)
        {
            if (original == null) return null;

            return new Monster
            {
                Id = original.Id,
                Name = original.Name,
                Health = original.Health,
                ImageURL = original.ImageURL,
            };
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
            ChangePokemonCommand.NotifyCanExecuteChanged();

            SelectedEnemyPokemon.Health -= selectedSpell.Damage;
            if (SelectedEnemyPokemon.Health <= 0)
            {
                SelectedEnemyPokemon.Health = 0;
            }
            OnPropertyChanged(nameof(SelectedEnemyPokemon));

            if (SelectedEnemyPokemon.Health <= 0)
            {
                _isFirstEnemyDied = true;
                EnemyPokemonList.Remove(SelectedEnemyPokemon);
           
                PlayerScore += 1;
                OnPropertyChanged(nameof(PlayerScore));

                if (EnemyPokemonList.Any())
                {
                    SelectedEnemyPokemon = EnemyPokemonList.FirstOrDefault();
                    SelectedEnemyPokemon.Health = (int)Math.Ceiling(SelectedEnemyPokemon.Health * 1.10);
                    HealthMaxEnnemy = SelectedEnemyPokemon.Health;

                    StatusMessage = "Boost de stats pour le Pokémon ennemi !";

                    OnPropertyChanged(nameof(HealthMaxEnnemy));
                    OnPropertyChanged(nameof(SelectedEnemyPokemon));
                }
            }

            var popup = new DamagePopup(SelectedPlayerPokemon.Name, selectedSpell.Name, selectedSpell.Damage);
            popup.Show();
            await Task.Delay(1000);
            popup.Close();

            if (!PlayerPokemonList.Any())
            {
                await Task.Delay(1000);
                MainWindowVM.OnRequestVMChange?.Invoke(new LoseVM(_context, PlayerPokemonListMemory, EnemyPokemonListMemory));
            }
            else if (!EnemyPokemonList.Any())
            {
                await Task.Delay(1000);
                MainWindowVM.OnRequestVMChange?.Invoke(new WinVM(_context, PlayerPokemonListMemory, EnemyPokemonListMemory));
            }
            else if (SelectedEnemyPokemon != null)
            {
                EnemyAttack();
                await Task.Delay(1000);
            }

            _isPlayerTurn = true;
            AttackCommand.NotifyCanExecuteChanged();
            ChangePokemonCommand.NotifyCanExecuteChanged();
        }

        private bool CanExecuteAttack(Spell selectedSpell) => _isPlayerTurn;
        private bool CanChangeMonster(Monster selectMonster) => _isPlayerTurn;

        private async void EnemyAttack()
        {
            if (SelectedEnemyPokemon == null || SelectedPlayerPokemon == null) return;

            Random rnd = new();

            var enemySpellArray = _context.Spells
                .Where(spell => spell.Monsters.Any(monster => monster.Id == SelectedEnemyPokemon.Id)).ToArray();

            int index = rnd.Next(enemySpellArray.Length);
            var enemySpell = enemySpellArray[index];

            if (enemySpell != null)
            {
                int ennemyDammage;
                if (_isFirstEnemyDied)
                {
                    ennemyDammage = (int)(enemySpell.Damage * 1.05);
                    SelectedPlayerPokemon.Health -= ennemyDammage;
                }
                else
                {
                    ennemyDammage = enemySpell.Damage;
                    SelectedPlayerPokemon.Health -= ennemyDammage;
                }

                if (SelectedPlayerPokemon.Health <= 0)
                {
                    SelectedPlayerPokemon.Health = 0;
                }
                OnPropertyChanged(nameof(SelectedPlayerPokemon));

                var popup = new DamagePopup(SelectedEnemyPokemon.Name, enemySpell.Name, ennemyDammage);
                popup.Show();
                await Task.Delay(1000);
                popup.Close();

                if (SelectedPlayerPokemon.Health <= 0)
                {
                    PlayerPokemonList.Remove(SelectedPlayerPokemon);

                    EnemyScore += 1;
                    OnPropertyChanged(nameof(EnemyScore));

                    if (PlayerPokemonList.Any())
                    {
                        SelectedPlayerPokemon = PlayerPokemonList.FirstOrDefault();
                        HealthMaxPlayer = SelectedPlayerPokemon.Health;
                        OnPropertyChanged(nameof(HealthMaxPlayer));
                        LoadSpells(); 
                    }
                    else
                    {
                        await Task.Delay(1000);
                        MainWindowVM.OnRequestVMChange?.Invoke(new LoseVM(_context, PlayerPokemonListMemory, EnemyPokemonListMemory));
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
