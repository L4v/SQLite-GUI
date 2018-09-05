using Microsoft.Win32;
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
    /// Interaction logic for LoadDatabaseWindow.xaml
    /// </summary>
    public partial class LoadDatabaseWindow : Window
    {
        // Save database to load
        private Database database;

        #region constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoadDatabaseWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region getters and setters

        /// <summary>
        /// Returns the loaded database
        /// </summary>
        /// <returns>Loaded database</returns>
        public Database GetDatabase()
        {
            return database;
        }
        #endregion


        #region clicks/buttons

        /// <summary>
        /// Loads the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadDatabase(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            // Filters out which filetypes to use, TODO
            dialog.Filter = "Database files (*.sqlite3)|*.sqlite3";

            if (dialog.ShowDialog() == true)
                database = new Database(dialog.FileName);

            Console.WriteLine(dialog.FileName);
            this.Close();
        }
        #endregion
    }
}
