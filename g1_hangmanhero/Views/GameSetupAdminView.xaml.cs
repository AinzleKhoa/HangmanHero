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
    public partial class GameSetupAdminView : Window
    {
        private readonly GameSetupAdminViewModel _viewModel;
        private readonly Player _currentUser;
        public GameSetupAdminView(Player adminUser)
        {
            _currentUser = adminUser;
            InitializeComponent();
            _viewModel = new GameSetupAdminViewModel(adminUser);
            DataContext = _viewModel;
        }
        private void Logout_click(object sender, RoutedEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
