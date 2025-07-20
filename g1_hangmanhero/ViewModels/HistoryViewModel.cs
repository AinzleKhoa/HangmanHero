using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using g1_hangmanhero.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        private readonly HangmanHeroContext _context;
        private readonly Player _currentUser;
        private readonly Window _currentWindow;  // Reference to the current window
        public ObservableCollection<GameHistory> GameHistories { get; set; }

        private GameHistory _selectedFB;
        public GameHistory SelectedFB
        {
            get => _selectedFB;
            set
            {
                _selectedFB = value;
                OnPropertyChanged(nameof(SelectedFB));
            }
        }

        private int _totalGames;
        public int TotalGames
        {
            get => _totalGames;
            set
            {
                _totalGames = value;
                OnPropertyChanged(nameof(TotalGames));
            }
        }

        public ICommand BackCommand { get; }

        public HistoryViewModel(Player loggedInPlayer, Window thisWindow)
        {
            _context = new HangmanHeroContext();
            _context.Database.EnsureCreated();

            _currentWindow = thisWindow;

            _currentUser = loggedInPlayer ?? throw new ArgumentNullException(nameof(loggedInPlayer));

            // Load Game Histories for the logged-in player
            GameHistories = new ObservableCollection<GameHistory>(_context.GameHistories
                .Where(g => g.Player.PlayerId == loggedInPlayer.PlayerId)
                .Include(g => g.Player)
                .ToList());

            // Set the total number of games played
            TotalGames = GameHistories.Count;

            BackCommand = new RelayCommand(_ => Back());
        }

        private void Back()
        {
            var menu = new MainWindow(_currentUser);
            _currentWindow.Close();
            menu.Show();
        }
    }
}
