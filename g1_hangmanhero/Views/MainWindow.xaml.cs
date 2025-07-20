using System.Windows;
using g1_hangmanhero.Models;
using g1_hangmanhero.ViewModels;
using g1_hangmanhero.Views;

namespace g1_hangmanhero.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow(Player loggedInPlayer)
        {
            InitializeComponent();
             
            DataContext = new MainWindowViewModel(loggedInPlayer, this);
        }

        private void Logout_click(object sender, RoutedEventArgs e)
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}