using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private int _round;
        private DateTime _startTime;
        private readonly int _maxLives = 6;

        public GameEngine(HangmanHeroContext context)
        {
            _context = context;
            _guessedLetters = new List<char>();
            _lives = _maxLives;
            _score = 0;
            _mistakes = 0;
            _round = 0;
        }

        public bool StartGame(string difficulty, string category)
        {
            RandomGeneration(difficulty, category);
            Init();
            _round = 1;  // Set the initial round to 1
            return true;
        }

        public void StartNewRound(string difficulty, string category)
        {
            RandomGeneration(difficulty, category);
            _guessedLetters.Clear();  // Clear guessed letters for the new round
            _round++;  // Increment the round
        }

        private void RandomGeneration(string difficulty, string category)
        {
            var words = _context.Words
                .Where(w => w.Difficulty == difficulty && w.Category == category)
                .ToList();

            if (!words.Any()) throw new Exception("No words available for this difficulty and category.");

            _currentWord = words[_random.Next(words.Count)];
        }

        private void Init()
        {
            _guessedLetters.Clear();
            _lives = _maxLives;
            _score = 0;
            _mistakes = 0;
            _startTime = DateTime.Now;
        }

        public int GetRound() => _round;

        public int AddOneMoreRound() => _round++;

        public int GetRemainingLives() => _lives;

        public int GetScore() => _score;

        public bool IsGameOver() => _lives <= 0;

        public bool isAllCorrect() => _currentWord.Text.All(c => _guessedLetters.Contains(char.ToUpper(c)));

        public (bool isValid, string errorMessage) ValidateGuess(char letter)
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
                    _score += CalculateScoreForCorrectGuess();
                }
                else
                {
                    _lives--;
                    _mistakes++;
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

        // Get the current word state (e.g., "_ _ _ a")
        public string GetCurrentWordState()
        {
            return string.Join(" ", _currentWord.Text.Select(c =>
                _guessedLetters.Contains(char.ToUpper(c)) ? c.ToString() : "_"));
        }

        public int GetMistakes() => _mistakes;

        public DateTime GetStartTime() => _startTime;

        public Word GetCurrentWord() => _currentWord;
    }
}
