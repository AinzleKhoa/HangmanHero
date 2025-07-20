using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using g1_hangmanhero.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    public class SummaryViewModel : ViewModelBase
    {
        // Readonly field for GameHistory passed from HangmanViewModel
        private readonly GameHistory _gameHistory;
        private readonly Window _currentWindow;

        // Properties to bind to the SummaryView
        private string _wordUsed;
        public string WordUsed
        {
            get => _wordUsed;
            set
            {
                _wordUsed = value;
                OnPropertyChanged();
            }
        }

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        private int _mistakes;
        public int Mistakes
        {
            get => _mistakes;
            set
            {
                _mistakes = value;
                OnPropertyChanged();
            }
        }

        private int _timeTaken;
        public int TimeTaken
        {
            get => _timeTaken;
            set
            {
                _timeTaken = value;
                OnPropertyChanged();
            }
        }

        private DateTime _playedAt;
        public DateTime PlayedAt
        {
            get => _playedAt;
            set
            {
                _playedAt = value;
                OnPropertyChanged();
            }
        }

        // Command to export the game summary to a CSV
        public ICommand ExportCommand { get; }
        public ICommand BackCommand { get; }

        // Constructor to initialize the SummaryViewModel with GameHistory
        public SummaryViewModel(GameHistory gameHistory, Window currentWindow)
        {
            _gameHistory = gameHistory ?? throw new ArgumentNullException(nameof(gameHistory));

            _currentWindow = currentWindow;
            // Initialize properties from GameHistory object
            Score = _gameHistory.Score;
            Mistakes = _gameHistory.Mistakes;
            TimeTaken = _gameHistory.TimeTaken;
            PlayedAt = _gameHistory.PlayedAt ?? DateTime.MinValue;

            // Initialize ExportCommand
            ExportCommand = new RelayCommand(_ => ExportToCsv());
            BackCommand = new RelayCommand(_ => Back());
        }

        private void Back()
        {
            var menu = new MainWindow(_gameHistory.Player);
            menu.Show();
            _currentWindow.Close();
        }

        // Export the summary to a CSV file
        private void ExportToCsv()
        {
            string filePath = $"GameSummary_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Score,Mistakes,TimeTaken,PlayedAt");
                writer.WriteLine($"{Score},{Mistakes},{TimeTaken},{PlayedAt}");
            }

            MessageBox.Show($"Exported to {filePath}", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
