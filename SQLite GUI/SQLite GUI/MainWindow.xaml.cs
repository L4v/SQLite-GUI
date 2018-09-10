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
using System.Windows.Data;
using System.Data.SqlClient;
using Microsoft.Win32;

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
        //private Database database = new Database("test.sqlite3");
        private Database database;
        private DataTable dataTable;
        private DataSet dataSet;
        private SQLiteDataAdapter dataAdapter;
        private SQLiteCommand cmd;
        
        #endregion



        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            // Disables all buttons, except for the LoadButton, until a database is loaded
            foreach (Button btn in this.LowerGrid.Children)
                if(btn.Name != "LoadButton")
                    btn.IsEnabled = false;

             foreach (Button btn in this.UpperGrid.Children)
                if (btn.Name != "LoadButton")
                    btn.IsEnabled = false;

            //NewGUI();
        }

        /// <summary>
        /// Creates a new instance of a GUI
        /// </summary>
        private void NewGUI()
        {
            // Clears input text
            //this.InputText.Document.Blocks.Clear();

            // Enables all buttons
            // Disables all buttons, except for the LoadButton, until a database is loaded
            foreach (Button btn in this.LowerGrid.Children)
                btn.IsEnabled = true;

            foreach (Button btn in this.UpperGrid.Children)
                btn.IsEnabled = true;

            // Clear message label
            this.MessageLabel.Content = String.Empty;

            // Update tables list
            UpdateTableList();

        }
        #endregion

        #region clicks and such

        /// <summary>
        /// Loads selected database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Load_Database_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            // Filters out which filetypes to use, TODO
            dialog.Filter = "Database files (*.sqlite3)|*.sqlite3";

            try
            {
                // Get selected file name
                if (dialog.ShowDialog() == true)
                    database = new Database(dialog.FileName);
            }catch(Exception ex)
            {
                // NEEDS IMPLEMENTING
                MessageLabel.Content = string.Format("Invalid database file! {0}", ex.Message);

                database = null;
            }

            // Doesn't actually show, 'cause it's overwritten on NewGUI() call
            MessageLabel.Content = string.Format("Loaded database!");

            if (database != null)
                NewGUI();

        }

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
        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow window = new PopUpWindow("Enter name of new table", "Create new table");
            window.Title = "New Table";

            // Shows PopUpWindow and waits for the user to input
            window.ShowDialog();

            string tableName = window.GetInput();

            try
            {


                if (tableName == "") {
                    throw new Exception();
                    return;
                }
                CreateTable(tableName);
            }catch(Exception ex)
            {
                MessageLabel.Content = String.Format("Invalid table name. \"{0}\"", tableName);
            }
        }

        /// <summary>
        /// Updates changes made to table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (GetSelectedItem() != "")
                UpdateTable(GetDataTable(GetSelectedItem()));
            return;
        }

        /// <summary>
        /// Drops selected table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            string item = GetSelectedItem();
            try
            {
                if (item == "")
                {
                    throw new Exception();
                    return;
                }
                DropTable(item);
            }catch(Exception ex)
            {
                MessageLabel.Content = string.Format("Invalid table to delete. \"{0}\"", item);
            }
            return;
        }

        private void New_Column_Button_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow window = new PopUpWindow("Enter:[column name] [column type]", "Create new column");
            window.Title = "New Column";

            // Shows PopUpWindow and waits for the user to input
            window.ShowDialog();
            string[] input = window.GetInput().Split();
            string table_name = GetSelectedItem();
            string col_name = input[0];
            string col_type = input[1];

            try
            {
                if(col_name == "")
                {
                    throw new Exception();
                    return;
                }

                AddColumn(col_name, col_type, table_name);

            }catch(Exception ex)
            {
                MessageLabel.Content = string.Format("Invalid column name. \"{0}\"", col_name);
            }
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

        #region Public helpers
        /// <summary>
        /// Creates a new table, if ne doesn't already exist
        /// </summary>
        /// <param name="name">Name for new table</param>
        public void CreateTable(string name)
        {
            try
            {
                // IRRELEVENT AL MEH
                /*if (TableExists(name))
                {
                    throw new Exception();
                    return;
                }*/

                ExecuteSQLite(string.Format("CREATE TABLE IF NOT EXISTS {0} (id INTEGER PRIMARY KEY AUTOINCREMENT);", name));
                UpdateTableList();
                MessageLabel.Content = string.Format("Table {0} successfully created.", name);

            }
            catch (Exception e)
            {
                this.MessageLabel.Content = string.Format("A table by that name already exists. {0}", name);
            }
        }

        /// <summary>
        /// Drops a table if it exists.
        /// </summary>
        /// <param name="name">Name of table to drop</param>
        public void DropTable(string name)
        {
            try
            {
                if (!TableExists(name))
                    throw new Exception();

                database.OpenConnection();
                ExecuteSQLite(string.Format("DROP TABLE {0}", name));
                database.CloseConnection();
                MessageLabel.Content = string.Format("Dropped table {0}", name);
                UpdateTableList();

            }
            catch(Exception e)
            {
                MessageLabel.Content = string.Format("Table doesn't exist. {0}", name);
            }
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
            dataAdapter = new SQLiteDataAdapter(query, database.connection);

            // Fill table
            dataAdapter.Fill(dataTable);

            // Sets default view for the table
            OutputGrid.ItemsSource = dataTable.DefaultView;

            // Info message
            this.MessageLabel.Content = "Showing table...";

            // HOW TO ADD A COLUMN
            //DataGridTextColumn textColumn = new DataGridTextColumn();
            //textColumn.Header = "First Name";
            //textColumn.Binding = new Binding("FirstName");
            //OutputGrid.Columns.Add(textColumn);

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
            cmd = new SQLiteCommand(query, database.connection);

            return cmd;
        }

        /// <summary>
        /// Returns selected item name
        /// </summary>
        /// <returns></returns>
        private String GetSelectedItem()
        {
            try
            {
                // Return name of item
                return this.TablesList.SelectedItem.ToString().Substring(37);
            }catch(Exception e)
            {
                MessageLabel.Content = "Nothing is selected. " + e.Message;
            }

            return "";
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
            SQLiteConnection connection = new SQLiteConnection(database.connection);

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

        /// <summary>
        /// Gets a DataTable via its name
        /// </summary>
        /// <param name="name">Name of DataTable to get</param>
        /// <returns></returns>
        private DataTable GetDataTable(string name)
        {
            // DataTable for returning
            DataTable dt = new DataTable();
            // Bogovi su poklonili ovu komandu

            /*// Open connection
            database.OpenConnection();

            // Fill dt with data adapter
            cmd = GetCommand("SELECT * FROM " + name);
            dataAdapter = new SQLiteDataAdapter(cmd);
            dataAdapter.Fill(dt);

            // Closes conection
            database.CloseConnection();*/

            OutputGrid.Items.Refresh();

            dt = ((DataView)OutputGrid.ItemsSource).ToTable();

            dt.TableName = name;

            // Return dt
            return dt;
        }

        #endregion

        #region helpers

        /// <summary>
        /// Executes an SQLite query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <returns></returns>
        private int ExecuteSQLite(string query)
        {
            // Open connection
            database.OpenConnection();

            // Command to execute
            cmd = GetCommand(query);

            // Int to check if row has been updated
            int row_updated;

            try
            {
                row_updated = cmd.ExecuteNonQuery();
            }catch(Exception e)
            {
                database.CloseConnection();
                return 0;
            }

            // Close connection
            database.CloseConnection();

            // Return if row updated
            return row_updated;

        }

        private Boolean TableExists(string name) {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                database.OpenConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = database.connection;
                    cmd.CommandText = "SELECT COUNT(*) AS QtRecords FROM sqlite_master WHERE type = 'table' AND name = @name";
                    cmd.Parameters.AddWithValue("@name", name);
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 0)
                {
                    database.CloseConnection();
                    return false;
                }
                else
                {
                    database.CloseConnection();
                    return true;
                }
            }
        }

        /// <summary>
        /// Updates table from edited gridview
        /// </summary>
        /// <param name="name">Name of table to update</param>
        private void UpdateTable(DataTable dt) {

            try
            {
                ExecuteSQLite(string.Format("TRUNCATE TABLE {0}", dt.TableName));
                ExecuteSQLite(string.Format("DELETE FROM {0}", dt.TableName));
                //ExecuteSQLite(string.Format("ALTER TABLE {0} AUTO_INCREMENT = 1;", dt.TableName));
                database.OpenConnection();
                cmd = new SQLiteCommand();
                cmd = GetCommand("SELECT * FROM " + dt.TableName);

                // DEBUG
                //MessageBox.Show(dt.TableName);

                dataAdapter = new SQLiteDataAdapter(cmd);
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(dataAdapter);

                // DEBUG
                //this.OutputGrid.ItemsSource = dt.DefaultView;
                //var modRow = dt.GetChanges(DataRowState.Modified);

                dataAdapter.Update(dt);

                database.CloseConnection();
            }
            catch (Exception e)
            {
                MessageLabel.Content = e.Message;
            }
        }

        /// <summary>
        /// Creates a column if the table exists
        /// </summary>
        /// <param name="col_name">Name of column</param>
        /// <param name="col_type">Type of column</param>
        /// <param name="table_name">Name of table to add the column to</param>
        private void AddColumn(string col_name, string col_type, string table_name)
        {
            try
            {
                if (!TableExists(table_name))
                {
                    throw new Exception();
                    return;
                }
                
                ExecuteSQLite(string.Format("ALTER TABLE {0} ADD COLUMN {1} {2};", table_name, col_name, col_type));

            }
            catch(Exception e)
            {
                MessageLabel.Content = string.Format("Invalid table name. \"{0}\")", table_name);
            }
        }

        #endregion
        //TODO: ZAVRISITI IF TABLE EXISTS
        // POPRAVITI DROP TABLE
        // POPRAVITI UPDATEOVANJE LISTE POSLE DROPOVANJA
        // NAMESTITI PROVERU LOSE DODATE BAZE

        /*DODATO:
         * LoadDatabase
         */

        // BAGOVI:
        /*
         * Posle brisanja tabele, baca exception kao da pokusava obrisati vec postojecu tabelu, iako je uspeo obrisati.
         * !!Konflikt je verovatno kod cinjenice kako radi prikazivanje tabele kada se selektuje iz liste, moracu naci
         * workaround
         * 
         * Kreiranje tabele cije je ime vec postojalo, iako je bila obrisana, stvara bug gde kaze da vec postoji,
         * ali je svejedno kreira.
         * 
         * Brisanje svih redova tabele pa ponovno pisanje je jedini nacin da radi update, ali AUTOINCREMENT ce povecati
         * PRIMARY_KEY za 1 posle svakog update, probao sam TRUNCATE bez uspeha, jedino mozda DROPovanje cele tabele pa
         * ponovno pisanje?
         * 
         * Mozda jos, ovo su glavni.
         */


    }
}