using g1_hangmanhero.Data;
using g1_hangmanhero.Models;
using g1_hangmanhero.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    internal class ScoreboardViewModel : ViewModelBase
    {
        private readonly HangmanHeroContext _context;
        private readonly Player _currentUser;
        private readonly Window _currentWindow;  // Reference to the current window

        private ObservableCollection<GameHistory> _gameHistories;
        public ObservableCollection<GameHistory> GameHistories
        {
            get => _gameHistories;
            set { _gameHistories = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); SearchFunction(); }  // Trigger search function when text changes
        }

        public ICommand ClearCommand { get; }
        public ICommand SortScoreAscCommand { get; }
        public ICommand SortScoreDescCommand { get; }

        public ICommand BackCommand { get; }
        public ScoreboardViewModel(Player loggedInPlayer, Window thisWindow)
        {
            _context = new HangmanHeroContext();
            LoadData();  // Load initial data

            _currentWindow = thisWindow;

            _currentUser = loggedInPlayer ?? throw new ArgumentNullException(nameof(loggedInPlayer));

            // Define commands
            ClearCommand = new RelayCommand(_ => ClearFunction());
            SortScoreAscCommand = new RelayCommand(_ => SortByScoreAscending());
            SortScoreDescCommand = new RelayCommand(_ => SortByScoreDescending());

            BackCommand = new RelayCommand(_ => Back());
        }

        private void LoadData()
        {
            // Initial data load
            var data = _context.GameHistories
                               .Include(g => g.Player)  // Include Player to access Username
                               .Include(g => g.Word)    // Include Word (if needed)
                               .ToList();

            GameHistories = new ObservableCollection<GameHistory>(data);
        }

        private void ClearFunction()
        {
            SearchText = string.Empty;
            Reload();
        }

        private void SearchFunction()
        {
            // If SearchText is null or empty, fetch all GameHistories
            var filteredData = string.IsNullOrWhiteSpace(SearchText)
                ? _context.GameHistories.Include(g => g.Player).ToList()
                : _context.GameHistories
                    .Include(g => g.Player)
                    .AsEnumerable()  // Fetch the data first, then apply filtering on the client side
                    .Where(g => g.Player.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            Reload(filteredData);
        }

        private void Reload(List<GameHistory> gameHistories = null)
        {
            GameHistories.Clear();  // Clear current list

            var historiesToLoad = gameHistories ?? _context.GameHistories.ToList();

            foreach (var item in historiesToLoad)
            {
                GameHistories.Add(item);  // Add each item to the ObservableCollection
            }
        }

        private void SortByScoreAscending()
        {
            GameHistories = new ObservableCollection<GameHistory>(
                GameHistories.OrderBy(g => g.Score).ToList()  // Sort by ascending score
            );
        }

        private void SortByScoreDescending()
        {
            GameHistories = new ObservableCollection<GameHistory>(
                GameHistories.OrderByDescending(g => g.Score).ToList()  // Sort by descending score
            );
        }
        private void Back()
        {
            var menu = new MainWindow(_currentUser);
            _currentWindow.Close();
            menu.Show();
        }
    }
}
