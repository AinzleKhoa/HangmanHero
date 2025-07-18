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
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        //public LoginView()
        //{
        //    InitializeComponent();

        //    DataContext = new LoginViewModel(() =>
        //    {
        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            new MainWindow().Show(); // 👈 mở cửa sổ chính sau đăng nhập
        //            this.Close(); // đóng cửa sổ đăng nhập
        //        });
        //    });
        //}

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            new RegisterView().Show();
            this.Close();
        }
    }

}
