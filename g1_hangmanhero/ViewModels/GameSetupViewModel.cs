//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using System.Windows.Input;
//using g1_hangmanhero.Data;     // Required for HangmanHeroContext
//using g1_hangmanhero.Models;   // Required for Player, Word, and Enums

//namespace g1_hangmanhero.ViewModels
//{
//    // -----------------------------------------------------------------
//    //  MAIN VIEWMODEL: GameSetupViewModel
//    // -----------------------------------------------------------------
//    public class GameSetupViewModel : ViewModelBase
//    {
//        private readonly Player _currentUser;

//        // --- Properties for Data Binding ---
//        //public IEnumerable<Difficulty> AvailableDifficulties => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
//        public IEnumerable<Category> AvailableCategories => Enum.GetValues(typeof(Category)).Cast<Category>();

//        //private Difficulty _selectedDifficulty;
//        //public Difficulty SelectedDifficulty
//        //{
//        //    get => _selectedDifficulty;
//        //    set { _selectedDifficulty = value; OnPropertyChanged(); }
//        //}

//        private Category _selectedCategory;
//        public Category SelectedCategory
//        {
//            get => _selectedCategory;
//            set { _selectedCategory = value; OnPropertyChanged(); }
//        }

//        // --- Command ---
//        public ICommand StartGameCommand { get; }


//        // --- NEW CONSTRUCTOR (for testing) ---
//        public GameSetupViewModel()
//        {
//            // Create a fake player for testing purposes
//            _currentUser = new Player
//            {
//                PlayerId = 99,
//                Username = "TestUser",
//                DefaultDifficulty = "Medium"
//            };

//            // Initialize the rest of the ViewModel as normal
//            StartGameCommand = new RelayCommand(ExecuteStartGame);
//            LoadDefaultSettings();
//        }
//        // --- Constructor ---
//        public GameSetupViewModel(Player loggedInPlayer)
//        {
//            _currentUser = loggedInPlayer ?? throw new ArgumentNullException(nameof(loggedInPlayer));

//            StartGameCommand = new RelayCommand(ExecuteStartGame);
//            LoadDefaultSettings();
//        }

//        // --- Logic Methods ---
//        private void LoadDefaultSettings()
//        {
//            if (Enum.TryParse(_currentUser.DefaultDifficulty, true, out Difficulty defaultDifficulty))
//            {
//                SelectedDifficulty = defaultDifficulty;
//            }
//            else
//            {
//                SelectedDifficulty = Difficulty.Easy; // Fallback
//            }

//            SelectedCategory = AvailableCategories.FirstOrDefault();
//        }

//        private void ExecuteStartGame(object? parameter)
//        {
//            try
//            {
//                using (var db = new HangmanHeroContext())
//                {
//                    string difficultyString = SelectedDifficulty.ToString();
//                    string categoryString = SelectedCategory.ToString();

//                    var wordPool = db.Words
//                                     .Where(w => w.Difficulty == difficultyString && w.Category == categoryString)
//                                     .ToList();

//                    if (!wordPool.Any())
//                    {
//                        MessageBox.Show($"Không tìm thấy từ nào cho Độ khó: {SelectedDifficulty} và Chủ đề: {SelectedCategory}.", "Không có từ", MessageBoxButton.OK, MessageBoxImage.Information);
//                        return;
//                    }

//                    var random = new Random();
//                    var wordToGuess = wordPool[random.Next(wordPool.Count)];

//                    MessageBox.Show($"Sẵn sàng chơi với từ: '{wordToGuess.Text}'!", "Bắt đầu!");

//                    // TODO: Chuyển hướng đến màn hình chơi game (GamePlayView)
//                    // var gamePlayView = new GamePlayView();
//                    // gamePlayView.DataContext = new GamePlayViewModel(wordToGuess, _currentUser);
//                    // gamePlayView.Show();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
//            }
//        }}
//    }