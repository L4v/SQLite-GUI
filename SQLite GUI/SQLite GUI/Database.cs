using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace SQLite_GUI
{


    class Database
    {
        public SQLiteConnection myConnection;


        /// <summary>
        /// Default constructor
        /// </summary>
        public Database(String database_name) {
            // Creates a new connection
            myConnection = new SQLiteConnection("Data Source="+ database_name);

            // Create a new database file if one doesn't exist
            if (!File.Exists("./"+database_name))
                 SQLiteConnection.CreateFile(database_name);
        }

        /// <summary>
        /// If the database connection is closed, open it
        /// </summary>
        public void OpenConnection() {
            if (myConnection.State != System.Data.ConnectionState.Open)
                myConnection.Open();
            return;
        }

        /// <summary>
        /// If the database connection is open, close it
        /// </summary>
        public void CloseConnection()
        {
            if (myConnection.State == System.Data.ConnectionState.Open)
                myConnection.Close();
            return;
        }

    }
}
