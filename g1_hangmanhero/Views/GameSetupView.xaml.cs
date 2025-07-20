using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using g1_hangmanhero.Models;
using g1_hangmanhero.ViewModels;

namespace g1_hangmanhero.Views
{
    public partial class GameSetupView : Window
    {
        private readonly Player _currentUser;
        private readonly GameSetupViewModel _viewModel;
        public GameSetupView(Player loggedInPlayer)
        {
            _currentUser = loggedInPlayer;
            InitializeComponent();
            _viewModel = new GameSetupViewModel(loggedInPlayer, this);
            DataContext = _viewModel;
        }

        private void Logout_click(object sender, RoutedEventArgs e)
        {
            var menu = new MainWindow(_currentUser);
            menu.Show();
            this.Close();
        }
    }
}
