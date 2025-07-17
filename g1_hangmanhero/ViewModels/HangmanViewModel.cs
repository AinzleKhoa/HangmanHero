using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using g1_hangmanhero.Data;
using g1_hangmanhero.Services;

namespace g1_hangmanhero.ViewModels
{
    public class HangmanViewModel : ViewModelBase
    {
        private readonly GameEngine _gameEngine;
        private readonly HangmanHeroContext _context;
        private int _playerId = 4;
        private string _difficulty;
        private string _category;

        public HangmanViewModel(HangmanHeroContext context)
        {
            _context = context;
            _gameEngine = new GameEngine(context);

            GuessCommand = new RelayCommand(_ => OnGuess(), CanGuess);
            StartNewGameCommand = new RelayCommand(_ => StartNewGame(), CanStartNewGame);
        }
        public ICommand GuessCommand { get; }
        public ICommand StartNewGameCommand { get; }


        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private string _guessedLetter;

        public string GuessedLetter
        {
            get { return _guessedLetter; }
            set
            {
                _guessedLetter = value;
                OnPropertyChanged();
                ErrorMessage = string.Empty;
            }
        }

        private string _currentWordState;

        public string CurrentWordState
        {
            get { return _currentWordState; }
            set { _currentWordState = value; OnPropertyChanged(); }
        }

        private int _remainingLives;

        public int RemainingLives
        {
            get { return _remainingLives; }
            set { _remainingLives = value; OnPropertyChanged(); }
        }

        private int _score;

        public int Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged(); }
        }

        private bool _isGameOver;

        public bool IsGameOver
        {
            get { return _isGameOver; }
            set { _isGameOver = value; OnPropertyChanged(); }
        }

        private string _gameResultMessage;

        public string GameResultMessage
        {
            get { return _gameResultMessage; }
            set { _gameResultMessage = value; OnPropertyChanged(); }
        }


        private void OnGuess()
        {
            // Make sure the whole textbox  it is not empty
            if (string.IsNullOrEmpty(GuessedLetter) || GuessedLetter.Length != 1)
            {
                _errorMessage = "Please enter a single letter.";
                return;
            }

            // Validation a character
            char letter = GuessedLetter[0];
            var (isValid, errorMessage) = _gameEngine.ValidateGuess(letter);
            if (!isValid)
            {
                ErrorMessage = errorMessage;
                return;
            }

            // Verify is it correct and update
            _gameEngine.VerifyLetterAndUpdate(letter);

            CurrentWordState = _gameEngine.GetCurrentWordState();
            RemainingLives = _gameEngine.GetRemainingLives();
            Score = _gameEngine.GetScore();
            GuessedLetter = string.Empty; // Restart guessedletter

            if (_gameEngine.IsGameWon())
            {
                IsGameOver = true;
                GameResultMessage = "Congratulations! You Won!";
                _gameEngine.SaveGameResult(_playerId);
            }
            else if (_gameEngine.IsGameLost())
            {
                IsGameOver = true;
                GameResultMessage = "Game Over! You lost.";
                _gameEngine.SaveGameResult(_playerId);
            }
        }

        private void StartNewGame()
        {
            //bool started = _gameEngine.StartGame(_playerId, _difficulty, _category);
            bool started = _gameEngine.StartGame(4, "easy", "animal");
            if (!started)
            {
                GameResultMessage = "No words available for the selected difficulty and category!";
                IsGameOver = true;
                return;
            }

            CurrentWordState = _gameEngine.GetCurrentWordState();
            RemainingLives = _gameEngine.GetRemainingLives();
            Score = _gameEngine.GetScore();
            ErrorMessage = string.Empty;
            IsGameOver = false;
            GameResultMessage = string.Empty;
            GuessedLetter = string.Empty;
        }

        private bool CanGuess(object parameter) => !IsGameOver && !string.IsNullOrEmpty(GuessedLetter);

        private bool CanStartNewGame(object parameter) => true;
    }
}
