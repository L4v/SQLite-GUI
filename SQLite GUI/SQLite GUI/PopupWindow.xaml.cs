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

namespace SQLite_GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PopUpWindow : Window
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="label"></param>
        /// <param name="button"></param>
        public PopUpWindow(string label, string button)
        {
            InitializeComponent();

            // Sets the window label text
            this.PopupText.Text = label;

            // Sets the button text
            this.PopupButton.Content = button;
        }

        /// <summary>
        /// Closes the PopUpWindow so MainWindow can get the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region getters setters

        /// <summary>
        /// Sets the popup text
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            this.PopupText.Text = text;
        }

        /// <summary>
        /// Sets the button text
        /// </summary>
        /// <param name="text"></param>
        public void SetButtonText(string text)
        {
            this.PopupButton.Content = text;
        }

        /// <summary>
        /// Returns the input
        /// </summary>
        /// <returns></returns>
        public string GetInput()
        {
            //string inputText = new TextRange(PopupInput.Document.ContentStart, PopupInput.Document.ContentEnd).Text;
            return PopupInput.Text;
        }

        #endregion

    }
}
