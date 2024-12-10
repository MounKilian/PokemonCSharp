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

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Spell> Spells { get; set; }

        public PokemonDetailsVM(Monster selectedPokemon, ExerciceMonsterContext context)
        {
            _context = context;
            SelectedPokemon = selectedPokemon;

            LoadImageFromUrl(SelectedPokemon.ImageURL);
            LoadSpells();

            ReturnToMainViewCommand = new RelayCommand(HandleReturnToMainView);
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
    }
}
