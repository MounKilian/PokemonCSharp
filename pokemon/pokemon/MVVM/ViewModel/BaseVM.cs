using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemon.MVVM.ViewModel
{
    abstract public class BaseVM : ObservableObject
    {
        public virtual void OnShowVM() { }
        public virtual void OnHideVM() { }
       

    }
}
