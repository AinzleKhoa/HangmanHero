using g1_hangmanhero.ViewModels;
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

namespace g1_hangmanhero.Views
{

    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            DataContext = new LoginViewModel(() =>
            {
                var viewModel = this.DataContext as LoginViewModel;

                if (viewModel?.LoggedInUser != null)
                {
                    if (viewModel.LoggedInUser.Role == 1) 
                    {
                        var gameSetupAdminView = new GameSetupAdminView(viewModel.LoggedInUser);
                        gameSetupAdminView.Show();
                    }
                    else
                    {
                        var mainWindoView = new MainWindow(viewModel.LoggedInUser);
                        mainWindoView.Show();
                    }

                    this.Close(); 
                }
            });
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
            this.Close();
        }
    }

}
