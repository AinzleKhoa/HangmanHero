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
using g1_hangmanhero.ViewModels;

namespace g1_hangmanhero.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();

            DataContext = new RegisterViewModel(() =>
            {
                // Callback khi đăng ký thành công
                Application.Current.Dispatcher.Invoke(() =>
                {
                    new LoginView().Show();
                    this.Close();
                });
            });
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            new LoginView().Show();
            this.Close();
        }
    }
}
