using System.Windows;
using System;

namespace pokemon.View
{
    public partial class DamagePopup : Window
    {
        public string AttackName { get; set; }
        public int Damage { get; set; }

        public DamagePopup(string attackName, int damage)
        {
            InitializeComponent();
            AttackName = attackName;
            Damage = damage;

            DataContext = this;
        }
    }
}
