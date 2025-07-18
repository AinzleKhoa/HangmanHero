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
            GuessedLetterTextBox.Focus();
            this.DataContext = new HangmanViewModel(new HangmanHeroContext()); // Set the DataContext
        }

        // This method will be triggered when the user types in the input field
        private void GuessedLetterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = this.DataContext as HangmanViewModel;
            if (viewModel != null)
            {
                // Execute the GuessCommand automatically when the text is changed
                viewModel.GuessCommand.Execute(null);
            }
        }

        // Focus on the TextBox when the view is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuessedLetterTextBox.Focus();  // Automatically focus the TextBox on load
        }
    }
}
