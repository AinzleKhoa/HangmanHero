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
    /// <summary>
    /// Interaction logic for ScoreboardView.xaml
    /// </summary>
    public partial class ScoreboardView : Window
    {
        public ScoreboardView(Player loggedInPlayer)
        {
            InitializeComponent();
            this.DataContext = new ScoreboardViewModel(loggedInPlayer, this);
        }
    }
}
