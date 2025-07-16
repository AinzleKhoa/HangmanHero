using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace g1_hangmanhero.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string StatusMessage { get; set; }

        public ICommand RegisterCommand { get; }
        private readonly Action onRegisterSuccess;

        public RegisterViewModel(Action onRegisterSuccess = null)
        {
            this.onRegisterSuccess = onRegisterSuccess;
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register(object parameter)
        {
            if (parameter is PasswordBox passwordBox &&
                passwordBox.Tag is string passwordTag &&
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive) is Window currentWindow)
            {
                var confirmBox = FindControlByTag<PasswordBox>(currentWindow, "ConfirmPassword");
                string password = passwordBox.Password;
                string confirmPassword = confirmBox?.Password;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    StatusMessage = "All fields are required.";
                }
                else if (password != confirmPassword)
                {
                    StatusMessage = "Passwords do not match.";
                }
                else
                {
                    using (var context = new HangmanHeroContext())
                    {
                        if (context.Players.Any(p => p.Username == Username))
                        {
                            StatusMessage = "Username already exists.";
                        }
                        else
                        {
                            var newPlayer = new Player
                            {
                                Username = Username,
                                PasswordHash = HashPassword(password),
                                JoinDate = DateTime.Now,
                                DefaultDifficulty = "Easy"
                            };

                            context.Players.Add(newPlayer);
                            context.SaveChanges();

                            StatusMessage = "Registration successful!";
                            onRegisterSuccess?.Invoke();
                        }
                    }
                }

                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private T FindControlByTag<T>(DependencyObject parent, string tag) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && (string)element.Tag == tag)
                    return element;

                var result = FindControlByTag<T>(child, tag);
                if (result != null)
                    return result;
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
