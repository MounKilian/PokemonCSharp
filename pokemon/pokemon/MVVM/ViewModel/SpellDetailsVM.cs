using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using pokemon.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class SpellDetailsVM : BaseVM
    {
        public Spell SelectedSpell { get; set; }
        public ExerciceMonsterContext _context { get; set; }
        public ICommand ReturnToMainViewCommand { get; }


        public SpellDetailsVM(Spell selectedSpell, ExerciceMonsterContext context)
        {
            _context = context;
            SelectedSpell = selectedSpell;
            ReturnToMainViewCommand = new RelayCommand(HandleReturnToMainView);
        }

        private void HandleReturnToMainView()
        {
            MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(_context));
        }
    }
}
