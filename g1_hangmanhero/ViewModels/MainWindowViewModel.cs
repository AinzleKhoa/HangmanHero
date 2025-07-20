using System.Windows;
using g1_hangmanhero.Models;
using System.Windows.Input;
using g1_hangmanhero.Views;
using g1_hangmanhero.Data;

namespace g1_hangmanhero.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly HangmanHeroContext _context;
        public Player _currentUser { get; }
        public ICommand NavigateToGameSetupCommand { get; }
        public ICommand NavigateToHistoryCommand { get; }
        public ICommand NavigateToScoreboardCommand { get; }
        public ICommand ExitApplicationCommand { get; }

        private readonly Window _currentWindow;  // Reference to the current window

        public MainWindowViewModel(Player loggedInPlayer, Window currentWindow)
        {
            _context = new HangmanHeroContext();
            _currentUser = loggedInPlayer;
            _currentWindow = currentWindow;  // Set the reference to the current window

            NavigateToGameSetupCommand = new RelayCommand(OpenGameSetup);
            NavigateToHistoryCommand = new RelayCommand(OpenHistory);
            NavigateToScoreboardCommand = new RelayCommand(OpenScoreboard);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }

        private void OpenGameSetup(object? parameter)
        {
            var gameSetupView = new GameSetupView(_currentUser);
            _currentWindow.Close();
            gameSetupView.Show();
        }

        private void OpenHistory(object? parameter)
        {
            var historyView = new HistoryView(_currentUser);
            _currentWindow.Close();
            historyView.Show();
        }

        private void OpenScoreboard(object? parameter)
        {
            var scoreboardView = new ScoreboardView(_currentUser);
            _currentWindow.Close();
            scoreboardView.Show();
        }

        private void ExitApplication(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}