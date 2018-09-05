using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SQLite_GUI
{


    public class Database
    {
        public SQLiteConnection connection;

        /// <summary>
        /// Constructor without password
        /// </summary>
        /// <param name="database_name"></param>
        public Database(string database_name)
        {
            connection = new SQLiteConnection("Data Source=" + database_name);

            // Create a new database file if one doesn't exist
            try
            {
                if (File.Exists("./" + database_name))
                    throw new FileFormatException();

                SQLiteConnection.CreateFile(database_name);

            }
            catch (Exception e)
            {
                // TODO: IMPLEMENT ERROR
            }
        }
        
        /// <summary>
        /// Default constructor with password
        /// </summary>
        public Database(string database_name, string database_password) {
            // Creates a new connection
            connection = new SQLiteConnection("Data Source="+ database_name);

            // Sets the password for the new database
            connection.SetPassword(database_password);

            // Create a new database file if one doesn't exist
            try
            {
                if (File.Exists("./" + database_name))
                    throw new FileFormatException();

                SQLiteConnection.CreateFile(database_name);

            }catch(Exception e)
            {
                // TODO: IMPLEMENT ERROR
            }
        }

        /// <summary>
        /// If the database connection is closed, open it
        /// </summary>
        public void OpenConnection() {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            return;
        }

        /// <summary>
        /// If the database connection is open, close it
        /// </summary>
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
            return;
        }

        public void ChangePassword(string new_password)
        {
            connection.ChangePassword(new_password);
        }

    }
}
