using System;
using System.Linq;
using System.Windows;
using g1_hangmanhero.Models;
using g1_hangmanhero.Data;
using g1_hangmanhero.Views;

namespace g1_hangmanhero.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _errorMessage;
        private bool _hasError;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); HasError = !string.IsNullOrEmpty(value); }
        }

        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(); }
        }

        public RelayCommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private void Register(object parameter)
        {
            // Reset error message
            ErrorMessage = string.Empty;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password cannot be empty.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            using (var db = new HangmanHeroContext())
            {
                // Check for duplicate username
                if (db.Players.Any(p => p.Username == Username))
                {
                    ErrorMessage = "Username already exists.";
                    return;
                }

                // Hash the password

                // Create a new player
                var player = new Player
                {
                    Username = Username,
                    JoinDate = DateTime.Now
                };

                // Save to database
                try
                {
                    db.Players.Add(player);
                    db.SaveChanges();

                    // Navigate to GameView (placeholder)

                    // Close the RegisterView
                    Application.Current.Windows.OfType<RegisterView>().FirstOrDefault()?.Close();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Registration failed: {ex.Message}";
                }
            }
        }
    }
}
