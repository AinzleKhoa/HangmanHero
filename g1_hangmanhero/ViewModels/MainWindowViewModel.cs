using System.Windows;
using g1_hangmanhero.Views;

namespace g1_hangmanhero.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand NavigateToRegisterCommand { get; }
        public RelayCommand StartGameCommand { get; }

        public MainWindowViewModel()
        {
            NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
            StartGameCommand = new RelayCommand(StartGame);
        }

        private void NavigateToRegister(object parameter)
        {
            var registerWindow = new RegisterView();
            registerWindow.Show();
            Application.Current.MainWindow?.Close();
        }

        private void StartGame(object parameter)
        {
            // Placeholder for game view navigation
            MessageBox.Show("Game start functionality to be implemented!");
        }
    }
}