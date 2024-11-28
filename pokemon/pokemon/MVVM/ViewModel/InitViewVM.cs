using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using pokemon.Model;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class InitViewVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public ICommand RequestSignInCommand { get; set; }
        public ICommand RequestDatabaseViewCommand { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database {  get; set; }

        private ExerciceMonsterContext _context;

        public InitViewVM()
        {
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);
            RequestSignInCommand = new RelayCommand(HandleRequestSignInCommand);
            RequestDatabaseViewCommand = new RelayCommand(HandleRequestDatabaseCommand);
        }

        public bool InsertUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);
            var existingUser = _context.Logins.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                MessageBox.Show("Ce nom d'utilisateur est déjà pris. Veuillez en choisir un autre.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                var user = new Login
                {
                    Username = username,
                    PasswordHash = hashedPassword
                };

                _context.Logins.Add(user);
                _context.SaveChanges();
                return true;
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool ValidateLogin()
        {
            var user = _context.Logins
                .FirstOrDefault(u => u.Username == Username && u.PasswordHash == HashPassword(Password));

            return user != null;
        }

        public void HandleRequestChangeViewCommand()
        {
            if (ValidateLogin())
            {
                MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM(_context));
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleRequestSignInCommand()
        {
            InsertUser(Username, Password);
            MessageBox.Show("L'utilisateur a été ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void HandleRequestDatabaseCommand()
        {
            if (!string.IsNullOrEmpty(Database))
            {
                _context = new ExerciceMonsterContext(Database);
                MessageBox.Show("La connexion à la base de données a été établie avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Veuillez entrer une chaîne de connexion valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
