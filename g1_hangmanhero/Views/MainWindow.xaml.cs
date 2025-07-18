using System.Windows;
using g1_hangmanhero.ViewModels;
using g1_hangmanhero.Views;

namespace g1_hangmanhero
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
        }
    }
}