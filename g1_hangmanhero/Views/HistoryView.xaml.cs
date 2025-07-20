using g1_hangmanhero.Models;
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
    public partial class HistoryView : Window
    {
        public HistoryView(Player loggedInPlayer)
        {
            InitializeComponent();
            DataContext = new HistoryViewModel(loggedInPlayer, this);
        }
    }
}
