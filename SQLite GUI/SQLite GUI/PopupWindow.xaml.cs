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

        private string InputText;

        public PopUpWindow(string label, string button, int type)
        {
            InitializeComponent();

            this.PopupText.Text = label;
            this.PopupButton.Content = button;
            try
            {
                    }catch(Exception e)
            {
                throw new InvalidOperationException($"Invalid type of windows selected " + e.Message);
            }
        }

        /// <summary>
        /// Calls function base on which type of window it is
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = GetInput();
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

        public string GetText()
        {
            return InputText;
        }

        #endregion

    }
}
