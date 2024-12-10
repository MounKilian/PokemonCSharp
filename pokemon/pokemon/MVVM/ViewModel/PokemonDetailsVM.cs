using CommunityToolkit.Mvvm.Input;
using pokemon.Model;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace pokemon.MVVM.ViewModel
{
    public class PokemonDetailsVM : BaseVM
    {
        public Monster SelectedPokemon { get; set; }
        public ExerciceMonsterContext _context { get; set; }
        public ICommand ReturnToMainViewCommand { get; }

        public ICommand ChangeViewCommandSpell { get; set; }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Spell> Spells { get; set; }

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


        public PokemonDetailsVM(Monster selectedPokemon, ExerciceMonsterContext context)
        {
            _context = context;
            SelectedPokemon = selectedPokemon;

            LoadImageFromUrl(SelectedPokemon.ImageURL);
            LoadSpells();

            ReturnToMainViewCommand = new RelayCommand(HandleReturnToMainView);
            ChangeViewCommandSpell = new RelayCommand(HandleRequestChangeViewCommandSpell);
        }

        private void HandleReturnToMainView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(_context));
        }

        private void LoadImageFromUrl(string imageUrl)
        {
            Uri imageUri = new Uri(imageUrl, UriKind.Absolute);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = imageUri;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            ImageSource = bitmapImage;
        }

        private void LoadSpells()
        {
            var spells = _context.Spells
                .Where(spell => spell.Monsters.Any(monster => monster.Id == SelectedPokemon.Id))
                .ToList();
            Spells = new ObservableCollection<Spell>(spells);
        }

        private void HandleRequestChangeViewCommandSpell()
        {
            if (SelectedSpell != null)
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new SpellDetailsVM(SelectedSpell, _context));
            }
        }
    }
}
