using g1_hangmanhero.Models;
using g1_hangmanhero.ViewModels;
using System.Windows;

namespace g1_hangmanhero.Views
{
    public partial class SummaryView : Window
    {
        public SummaryView(GameHistory gameHistory)
        {
            InitializeComponent();
            DataContext = new SummaryViewModel(gameHistory, this);
        }
    }
}
