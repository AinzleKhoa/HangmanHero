using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g1_hangmanhero.Data;
using g1_hangmanhero.Models;

namespace g1_hangmanhero.Services
{
    public class GameEngine
    {
        private readonly HangmanHeroContext _context;
        private readonly Random _random = new Random();
        private Word _currentWord;
        private List<char> _guessedLetters;
        private int _lives;
        private int _score;
        private int _mistakes;
        private DateTime _startTime;
        private readonly int _maxLives = 6;

        public GameEngine(HangmanHeroContext context)
        {
            _context = context;
            _guessedLetters = new List<char>();
            _lives = _maxLives;
            _score = 0;
            _mistakes = 0;
        }

        public bool StartGame(int playerId, string difficulty, string category)
        {
            var words = _context.Words
                .Where(w => w.Difficulty == difficulty && w.Category == category)
                .ToList();

            if (!words.Any()) return false;

            _currentWord = words[_random.Next(words.Count)];
            _guessedLetters.Clear();
            _lives = _maxLives;
            _score = 0;
            _mistakes = 0;
            _startTime = DateTime.Now;
            return true;
        }

        public int GetRemainingLives()
        {
            return _lives;
        }

        public int GetScore()
        {
            return _score;
        }

        public bool IsGameWon()
        {
            return _currentWord.Text.All(c => _guessedLetters.Contains(char.ToUpper(c)));
        }

        public bool IsGameLost()
        {
            return _lives <= 0;
        }

        public (bool isValid, string ErrorMessage) ValidateGuess(char letter)
        {
            if (!char.IsLetter(letter))
                return (false, "Please enter a valid letter (A-Z).");
            letter = char.ToUpper(letter);
            if (_guessedLetters.Contains(letter))
                return (false, "This letter has already been guessed.");
            return (true, string.Empty);
        }

        public void VerifyLetterAndUpdate(char letter)
        {
            letter = char.ToUpper(letter);
            if (!_guessedLetters.Contains(letter))
            {
                _guessedLetters.Add(letter);
                bool isCorrect = _currentWord.Text.ToUpper().Contains(letter);

                if (isCorrect)
                {
                    _score += CalculateScoreForCorrectGuess(); // Increase score for correct guess
                }
                else
                {
                    _lives--;   // Decrease lives for incorrect guess
                    _mistakes++; // Increase mistake count for incorrect guess
                }
            }
        }

        private int CalculateScoreForCorrectGuess()
        {
            int basePoints = 10;
            return _currentWord.Difficulty switch
            {
                "Easy" => basePoints,
                "Medium" => basePoints * 2,
                "Hard" => basePoints * 3,
                _ => basePoints
            };
        }

        public string GetCurrentWordState()
        {
            return string.Concat(_currentWord.Text.Select(c =>
            _guessedLetters.Contains(char.ToUpper(c)) ? c : '_'));
        }

        //
        public void SaveGameResult(int playerId)
        {
            // Get the player from the database based on the playerId
            var player = _context.Players.FirstOrDefault(p => p.PlayerId == playerId);

            // If no player exists with the given playerId, handle the case (could throw error or return)
            if (player == null)
            {
                throw new Exception("Player not found.");
            }

            // Calculate time taken in seconds
            var timeTaken = (int)(DateTime.Now - _startTime).TotalSeconds;

            // Check if a game history already exists for this player
            var existingGameHistory = _context.GameHistories
                                              .FirstOrDefault(gh => gh.player.PlayerId == playerId);

            // If a game history exists, update it; otherwise, create a new one
            if (existingGameHistory != null)
            {
                // Update the existing game history record (if needed)
                existingGameHistory.Score = _score;
                existingGameHistory.Mistakes = _mistakes;
                existingGameHistory.TimeTaken = timeTaken;
                existingGameHistory.PlayedAt = DateTime.Now;

                // Save the changes
                _context.GameHistories.Update(existingGameHistory);
            }
            else
            {
                // Create a new GameHistory object
                var gameHistory = new GameHistory
                {
                    player = player, // Associate the player with the game history
                    word = _currentWord, // Assuming _currentWord is the word used in the game
                    Score = _score,
                    Mistakes = _mistakes,
                    TimeTaken = timeTaken,
                    PlayedAt = DateTime.Now
                };

                // Add the new game history
                _context.GameHistories.Add(gameHistory);
            }

            // Save the changes to the database
            _context.SaveChanges();
        }
    }
}
