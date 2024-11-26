using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using pokemon.Model;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace pokemon.MVVM.ViewModel
{
    public class LoginVM : BaseVM
    {
        public ICommand RequestChangeViewCommand { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private readonly ExerciceMonsterContext _context;

        private string _titleAndButtonText;

        public string TitleAndButtonText
        {
            get => _titleAndButtonText;
            set
            {
                _titleAndButtonText = value;
                OnPropertyChanged();
            }
        }

        public LoginVM()
        {
            _context = new ExerciceMonsterContext(new DbContextOptions<ExerciceMonsterContext>());
            RequestChangeViewCommand = new RelayCommand(HandleRequestChangeViewCommand);

            TitleAndButtonText = "Login";
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

        public void ToggleViewMode()
        {
            if (TitleAndButtonText == "Sign Up")
            {
                TitleAndButtonText = "Login";
            }
            else
            {
                TitleAndButtonText = "Sign Up";
            }
        }

        public void HandleRequestChangeViewCommand()
        {
            if (TitleAndButtonText == "Sign Up") 
            {
                bool isCreated = InsertUser(Username, Password);
                if (isCreated == true)
                {
                    MessageBox.Show("Utilisateur créé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else 
            {
                if (ValidateLogin())
                {
                    MainWindowVM.OnRequestVMChange?.Invoke(new MainViewVM());
                }
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
