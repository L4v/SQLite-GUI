using System;
using System.Windows;
using System.Data.SQLite;
using System.Data;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace SQLite_GUI

    // TODO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private helpers
        // Create new Database object
        private Database database = new Database("test.sqlite3");
        private DataTable dataTable;
        private SQLiteDataAdapter dataAdapter;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGUI();
        }

        /// <summary>
        /// Creates a new instance of a GUI
        /// </summary>
        private void NewGUI()
        {
            // Clears input text
            //this.InputText.Document.Blocks.Clear();

            // Clear message label
            this.MessageLabel.Content = String.Empty;

            // Update tables list
            UpdateTableList();

        }
        #endregion

        #region clicks and such
        /// <summary>
        /// Processes command inside textbox
        /// </summary>
        /// <param name="sender">Button which was clicked</param>
        /// <param name="e">Event object</param>
        /* private void RunButton_Click(object sender, RoutedEventArgs e)
         {
             // Open connection
             database.OpenConnection();

             // Create new data table
             dataTable = new DataTable();

             // Input text to make a commands
             string query = new TextRange(InputText.Document.ContentStart, InputText.Document.ContentEnd).Text;

             // Create new data adapter
             dataAdapter = new SQLiteDataAdapter(query, database.myConnection);

             // Fill table
             dataAdapter.Fill(dataTable);

             // Sets default view for the table
             OutputGrid.ItemsSource = dataTable.DefaultView;

             // Sample message
             this.MessageLabel.Content = "Command executed";

             UpdateTableList();

             // Close connection
             database.CloseConnection();
         }
         */

        /// <summary>
        /// Creates a new table if one doesn't exist already
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Shows selected table in ListBox 
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void TablesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Take the name from selected item in ListBox, the name starts at the 37th character
            string name = GetSelectedItem();
            ShowTable(name);
        }
        #endregion

        /// <summary>
        /// Update tables list
        /// </summary>
        private void UpdateTableList() {

            // Clear list
            this.TablesList.Items.Clear();

            // A listboxitem object to be added
            ListBoxItem item;

            // Creates a new listbox item object and adds it to the TablesList listbox
            foreach (string name in GetTableNames())
            {
                item = new ListBoxItem();
                item.Content = name;
                TablesList.Items.Add(item);
            }
            
        }

        /// <summary>
        /// Shows a table if exists
        /// </summary>
        /// <param name="name">Name of table to show</param>
        private void ShowTable(string name) {
            // Open the connection
            database.OpenConnection();

            string query = "SELECT * FROM " + name; 

            // Create new DataTable
            dataTable = new DataTable();

            // Create new data adapter
            dataAdapter = new SQLiteDataAdapter(query, database.myConnection);

            // Fill table
            dataAdapter.Fill(dataTable);

            // Sets default view for the table
            OutputGrid.ItemsSource = dataTable.DefaultView;

            // Info message
            this.MessageLabel.Content = "Showing table...";

            // Close the connection
            database.CloseConnection();
        }



        #region getters

        /// <summary>
        /// Takes a query as a string and returns a SQLite command object
        /// </summary>
        /// <param name="query">Query as a string for the command</param>
        /// <returns></returns>
        private SQLiteCommand GetCommand(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, database.myConnection);

            return command;
        }

        /// <summary>
        /// Returns selected item name
        /// </summary>
        /// <returns></returns>
        private String GetSelectedItem()
        {
            // Return name of item
            return this.TablesList.SelectedItem.ToString().Substring(37);
        }

        /// <summary>
        /// Gets a list of table names
        /// </summary>
        /// <returns></returns>
        private List<string> GetTableNames()
        {
            // List of string names
            List<string> names = new List<string>();

            // Open connection
            database.OpenConnection();

            // Create new connection
            SQLiteConnection connection = new SQLiteConnection(database.myConnection);

            // Sqlite data reader
            SQLiteDataReader reader = GetCommand("SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1").ExecuteReader();

            // TODO: DODATI DOUBLE CLICK NA LIST ITEM

            while (reader.Read())
            {
                names.Add((string)reader["name"]);
            }

            // Close connection
            database.CloseConnection();

            return names;
        }

        #endregion

        #region helpers

        private Boolean TableExists(string name) {
            return false;
        }

        #endregion
    }
}
