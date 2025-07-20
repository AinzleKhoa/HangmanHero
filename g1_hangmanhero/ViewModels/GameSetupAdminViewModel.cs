using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    public class GameSetupAdminViewModel : ViewModelBase
    {
        public ObservableCollection<Word> Words { get; set; }
        private readonly Player _adminUser;

        public string CurrentUserName
        {
            get => _adminUser.Username;
        }

        private Word _selectedWord;
        public Word SelectedWord
        {
            get => _selectedWord;
            set
            {
                _selectedWord = value;
                OnPropertyChanged();

                if (_selectedWord != null)
                {
                    NewWordText = _selectedWord.Text;
                    NewWordCategory = _selectedWord.Category;
                    NewWordDifficulty = _selectedWord.Difficulty;
                }
            }
        }

        private string _newWordText;
        public string NewWordText
        {
            get => _newWordText;
            set { _newWordText = value; OnPropertyChanged(); }
        }

        private string _newWordCategory;
        public string NewWordCategory
        {
            get => _newWordCategory;
            set { _newWordCategory = value; OnPropertyChanged(); }
        }

        private string _newWordDifficulty;
        public string NewWordDifficulty
        {
            get => _newWordDifficulty;
            set { _newWordDifficulty = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> AvailableDifficulties { get; set; }

        public ICommand AddWordCommand { get; }
        public ICommand UpdateWordCommand { get; }
        public ICommand DeleteWordCommand { get; }
        public ICommand LogoutCommand { get; }
        public GameSetupAdminViewModel(Player adminUser)
        {
            _adminUser = adminUser;

            AddWordCommand = new RelayCommand(ExecuteAddWord, CanExecuteAddWord);
            UpdateWordCommand = new RelayCommand(ExecuteUpdateWord, CanExecuteUpdateOrDelete);
            DeleteWordCommand = new RelayCommand(ExecuteDeleteWord, CanExecuteUpdateOrDelete);
            LogoutCommand = new RelayCommand(ExecuteLogout);
            LoadAvailableDifficulties();
            LoadWords();
        }

        public event Action RequestLogout;
        private void ExecuteLogout(object parameter)
        {
            RequestLogout?.Invoke();
        }

        // Validation for word
        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(NewWordText) &&
                   !string.IsNullOrWhiteSpace(NewWordCategory) &&
                   !string.IsNullOrWhiteSpace(NewWordDifficulty);
        }

        private bool CanExecuteAddWord(object parameter)
        {
            return IsValid();
        }

        private void LoadAvailableDifficulties()
        {
            try
            {
                using (var context = new HangmanHeroContext())
                {
                    var difficultiesList = context.Words
                                                  .Select(w => w.Difficulty)
                                                  .Distinct()
                                                  .ToList();
                    AvailableDifficulties = new ObservableCollection<string>(difficultiesList);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Could not load the list of difficulties: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                AvailableDifficulties = new ObservableCollection<string>();
            }
        }

        private void LoadWords()
        {
            try
            {
                using (var context = new HangmanHeroContext())
                {
                    Words = new ObservableCollection<Word>(context.Words.ToList());
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error loading words: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add a new word
        private void ExecuteAddWord(object parameter)
        {
            if (!IsValid())
            {
                MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newWord = new Word
            {
                Text = NewWordText.Trim(),
                Category = NewWordCategory.Trim(),
                Difficulty = NewWordDifficulty
            };

            try
            {
                using (var context = new HangmanHeroContext())
                {
                    context.Words.Add(newWord);
                    context.SaveChanges();
                    Words.Add(newWord);
                }

                NewWordText = string.Empty;
                NewWordCategory = string.Empty;
                NewWordDifficulty = null;

                MessageBox.Show("New word added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error adding word: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Update the selected word
        private void ExecuteUpdateWord(object parameter)
        {
            if (SelectedWord == null) return;

            try
            {
                using (var context = new HangmanHeroContext())
                {
                    var wordToUpdate = context.Words.Find(SelectedWord.WordId);
                    if (wordToUpdate != null)
                    {
                        wordToUpdate.Text = NewWordText.Trim();
                        wordToUpdate.Category = NewWordCategory.Trim();
                        wordToUpdate.Difficulty = NewWordDifficulty;
                        context.SaveChanges();

                        var index = Words.IndexOf(SelectedWord);
                        if (index >= 0)
                        {
                            Words[index] = wordToUpdate;
                        }

                        MessageBox.Show("Word updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error updating word: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Delete the selected word
        private void ExecuteDeleteWord(object parameter)
        {
            if (SelectedWord == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete the word '{SelectedWord.Text}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            try
            {
                using (var context = new HangmanHeroContext())
                {
                    var wordToDelete = context.Words.Find(SelectedWord.WordId);
                    if (wordToDelete != null)
                    {
                        context.Words.Remove(wordToDelete);
                        context.SaveChanges();
                        Words.Remove(SelectedWord);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error deleting word: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Can update or delete only when a word is selected
        private bool CanExecuteUpdateOrDelete(object parameter)
        {
            return SelectedWord != null;
        }
    }
}
