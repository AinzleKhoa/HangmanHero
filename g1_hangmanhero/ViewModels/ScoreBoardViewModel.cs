using g1_hangmanhero.DTO;
using g1_hangmanhero.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace g1_hangmanhero.ViewModels
{
    internal class ScoreBoardViewModel : INotifyPropertyChanged
    {
        private readonly HangmanHeroContext _context;

        private ObservableCollection<GameHistoryView> _gameHistories;
        public ObservableCollection<GameHistoryView> GameHistories
        {
            get => _gameHistories;
            set { _gameHistories = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        // 👉 biến lưu trạng thái Sort
        private bool _isSortAscending = true;

        public ICommand SearchCommand { get; }
        public ICommand SortScoreAscCommand { get; }
        public ICommand SortScoreDescCommand { get; }


        public ScoreBoardViewModel()
        {
            _context = new HangmanHeroContext();
            LoadData();

            SearchCommand = new RelayCommand(_ => Search());

            SortScoreAscCommand = new RelayCommand(_ => SortByScoreAscending());
            SortScoreDescCommand = new RelayCommand(_ => SortByScoreDescending());
        }


        private void LoadData()
        {
            var data = _context.GameHistories
                               .Include(g => g.Player)
                               .Select(g => new GameHistoryView
                               {
                                   Username = g.Player.Username,
                                   Score = g.Score,
                                   Mistakes = g.Mistakes,


                                   TimeTaken = g.TimeTaken,


                                   PlayedAt = g.PlayedAt ?? new DateTime(1753, 1, 1)
                               })
                               .ToList();

            GameHistories = new ObservableCollection<GameHistoryView>(data);
        }


        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Nếu không nhập gì thì load lại toàn bộ
                LoadData();
                return;
            }

            var filtered = _context.GameHistories
                                   .Include(g => g.Player)
                                   .Where(g => g.Player.Username.Contains(SearchText))
                                   .Select(g => new GameHistoryView
                                   {
                                       Username = g.Player.Username,
                                       Score = g.Score,
                                       Mistakes = g.Mistakes,

                                       // ✅ TimeTaken là int → lấy trực tiếp
                                       TimeTaken = g.TimeTaken,

                                       // ✅ PlayedAt có thể null → fallback MinValue
                                       PlayedAt = g.PlayedAt ?? new DateTime(1753, 1, 1)
                                   })
                                   .ToList();

            GameHistories = new ObservableCollection<GameHistoryView>(filtered);

            if (!GameHistories.Any())
            {
                MessageBox.Show("No results found.", "Search", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void SortByScoreAscending()
        {
            GameHistories = new ObservableCollection<GameHistoryView>(
                GameHistories.OrderBy(g => g.Score)
            );
        }

        private void SortByScoreDescending()
        {
            GameHistories = new ObservableCollection<GameHistoryView>(
                GameHistories.OrderByDescending(g => g.Score)
            );
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
