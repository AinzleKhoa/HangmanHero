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
using g1_hangmanhero.Data;
using g1_hangmanhero.ViewModels;

namespace g1_hangmanhero.Views
{
    /// <summary>
    /// Interaction logic for HangmanView.xaml
    /// </summary>
    public partial class HangmanView : Window
    {
        public HangmanView()
        {
            InitializeComponent();
            this.DataContext = new HangmanViewModel(new HangmanHeroContext()); // Set the DataContext
        }
    }
}
