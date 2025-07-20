using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using g1_hangmanhero.Views;

namespace g1_hangmanhero.ViewModels
{
    public class GameSetupViewModel : ViewModelBase
    {
        private readonly Player _currentUser;
        public List<string> AvailableDifficulties { get; private set; }
        public List<string> AvailableCategories { get; private set; }

        private readonly Window _currentWindow;  // Reference to the current window

        private string _selectedDifficulty;
        public string SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                _selectedDifficulty = value;
                OnPropertyChanged(); 
            }
        }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(); 
            }
        }

        public ICommand StartGameCommand { get; }

        public GameSetupViewModel(Player loggedInPlayer, Window thisWindow)
        {
            _currentUser = loggedInPlayer ?? throw new ArgumentNullException(nameof(loggedInPlayer));

            _currentWindow = thisWindow;

            StartGameCommand = new RelayCommand(ExecuteStartGame, CanExecuteStartGame);

            LoadGameOptions();
            LoadDefaultSettings();
        }

        private void LoadGameOptions()
        {
            try
            {
                using (var db = new HangmanHeroContext())
                {
                    AvailableDifficulties = db.Words.Select(w => w.Difficulty).Distinct().ToList();
                    AvailableCategories = db.Words.Select(w => w.Category).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load game settings: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AvailableDifficulties = new List<string>();
                AvailableCategories = new List<string>();
            }
        }
        private void LoadDefaultSettings()
        {
            SelectedDifficulty = AvailableDifficulties.Contains(_currentUser.DefaultDifficulty)
                ? _currentUser.DefaultDifficulty
                : AvailableDifficulties.FirstOrDefault();

            SelectedCategory = AvailableCategories.FirstOrDefault();
        }

        private bool CanExecuteStartGame(object parameter)
        {
            return !string.IsNullOrEmpty(SelectedDifficulty) && !string.IsNullOrEmpty(SelectedCategory);
        }

        private void ExecuteStartGame(object parameter)
        {
            try
            {
                using (var db = new HangmanHeroContext())
                {
                    var wordPool = db.Words
                        .Where(w => w.Difficulty == SelectedDifficulty && w.Category == SelectedCategory)
                        .ToList();

                    if (!wordPool.Any())
                    {
                        MessageBox.Show($"No words found for Difficulty: {SelectedDifficulty} and Category: {SelectedCategory}.", "No Words Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    var hangmanView = new HangmanView(_currentUser, _selectedCategory);
                    hangmanView.Show();
                    _currentWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while starting the game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}