using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using g1_hangmanhero.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string StatusMessage { get; set; }

        public Player LoggedInUser { get; private set; }
        public ICommand LoginCommand { get; }

        private readonly Action onLoginSuccess;

      
        public LoginViewModel(Action onLoginSuccess = null)
        {
            this.onLoginSuccess = onLoginSuccess;
            LoginCommand = new RelayCommand(Login);

        }


        private void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = passwordBox?.Password;
            string hashedPassword = HashPassword(password);

            using (var context = new HangmanHeroContext())
            {
                var user = context.Players.FirstOrDefault(p => p.Username == Username && p.PasswordHash == hashedPassword);
                if (user != null)
                {
                    StatusMessage = "Login successful!";
                    this.LoggedInUser = user;
                    onLoginSuccess?.Invoke();
                }
                else
                {
                    StatusMessage = "Invalid username or password.";
                    this.LoggedInUser = null; // Đảm bảo là null nếu t
                }
            }

            OnPropertyChanged(nameof(StatusMessage));
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
