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
using g1_hangmanhero.Models;
using g1_hangmanhero.ViewModels;

namespace g1_hangmanhero.Views
{
    /// <summary>
    /// Interaction logic for HangmanView.xaml
    /// </summary>
    public partial class HangmanView : Window
    {
        public HangmanView(Player loggedInPlayer, string category)
        {
            InitializeComponent();
            GuessedLetterTextBox.Focus();
            this.DataContext = new HangmanViewModel(loggedInPlayer, category, this);
        }

        // Focus on the TextBox when the view is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuessedLetterTextBox.Focus();  // Automatically focus the TextBox on load
        }
    }
}
