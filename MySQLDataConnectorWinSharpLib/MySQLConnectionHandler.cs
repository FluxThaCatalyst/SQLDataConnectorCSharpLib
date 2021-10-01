//---SQL Connection Library---//
//---Created by: Louis P.---//

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MySQLDataConnectorWinSharpLib
{
    /*================================================================
                     SQL Queries
     _______________________________________________________________
     Insert: INSERT tableinfo (name, age) VALUES('John Smith', '33')
     Update: UPDATE tableinfo SET name='Joe',age='22' WHERE name='John Smith'
     Delete: DELETE From tableinfo WHERE name='John Smith'
     Select: SELECT * FROM tableinfo
     Count:  SELECT Count(*) FROM tableinfo
     =================================================================*/

    /// <summary>
    /// This is a MySQL Automation tool
    /// </summary>
    public class MySQLConnectionHandler
    {
        private MySqlConnection Connection;
        private string Server { get; set; } = "localhost";
        private string DataBase { get; set; } = null;
        private string UID { get; set; } = "username";
        private string Password { get; set; } = "passw0rd1";
        private static string Table { get; set; }
        private static string[] Fields { get; set; }
        //Constructor
        public MySQLConnectionHandler()
        {
            Initialize();
        }

        /// <summary>
        /// Return a Connection String
        /// </summary>
        /// <param name="server">Defaults to localhost</param>
        /// <param name="dataBase">Defaults to null</param>
        /// <param name="uid">Defaults to username</param>
        /// <param name="password">Defaults to passw0rd1</param>
        /// <returns></returns>
        public void GetConnectionString(string server, string dataBase, string uid, string password)
        {
            this.Server = server;
            this.DataBase = dataBase;
            this.UID = uid;
            this.Password = password;
        }

        /// <summary>
        /// Set the Connection String
        /// </summary>
        /// <returns>Returns connection string</returns>
        private MySqlConnection SetConnectionString()
        {
            string connectionString;
            connectionString = $"SERVER={this.Server};" +
                                $"DATABASE={this.DataBase};" +
                                $"UID={this.UID};" +
                                $"PASSWORD={this.Password};";
            return this.Connection = new MySqlConnection(connectionString);
        }

        //Initializer
        private void Initialize()
        {
            SetConnectionString();
        }

        /// <summary>
        /// Open Connection to DataBase
        /// </summary>
        /// <returns></returns>
        private bool OpenConnection()
        {
            try
            {
                this.Connection.Open();
            }
            catch (MySqlException sqlex)
            {

                //0: Cannot connect to server
                //1045: Incorrect Username or Password
                switch (sqlex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot Connect to Server. Contact Admin", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 1045:
                        MessageBox.Show("Incorrect Username/Password, please try again", null, MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                        break;
                }
            }
            return false;
        }

        /// <summary>
        ///  Close Connection
        /// </summary>
        /// <returns></returns>
        private bool CloseConnection()
        {
            try
            {
                this.Connection.Close();
                return true;
            }
            catch (MySqlException sqlex)
            {

                MessageBox.Show(sqlex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /*==============================================
         *              Table Information
         ===============================================*/
        public string SetTableName(string tableinfo)
        {
            return Table = tableinfo;
        }

        public string GetTableName()
        {
            if (String.IsNullOrEmpty(Table)) { return "Table name has not been updated"; }
            else return Table;
        }

        public string[] SetTableFields(string[] fields)
        {
            return Fields = fields;
        }

        public string GetTableFields()
        {
            if (Fields == null || Fields.Length == 0) { return "Fields have not been updated"; }
            else return string.Join(",", Fields);
        }
        /*==================================================
                    Insert into table commmands
         ===================================================*/
        /// <summary>
        /// Insert statement *Use SetTableName() before using this or use override
        /// </summary>
        /// <param name="tableinfo">Fields you like to insert to *format as name, age</param>
        /// <param name="values">Field insert value *format as *format as name,age</param>
        public void Insert(string tableinfo, string values)
        {
            if (!String.IsNullOrEmpty(Table))
            {
                string query = $"INSERT INTO {Table} ({tableinfo}) VALUES({values})";

                //open connection
                if (this.OpenConnection() == true)
                {
                    MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);

                    //Execute command
                    sqlCommand.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                }
            }
            else { MessageBox.Show("Table name is not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>
        /// Insert Statement Override
        /// </summary>
        /// <param name="table">Table Name</param>
        /// <param name="tableinfo">Fields you like to insert to *format as name, age</param>
        /// <param name="values">Field insert value *format as *format as name,age</param>
        public void Insert(string table, string tableinfo, string values)
        {
            string query = $"INSERT INTO {table} ({tableinfo}) VALUES({values})";

            //open connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);

                //Execute command
                sqlCommand.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        /*============================================
                        Update Commands
         ============================================*/
        /// <summary>
        /// Update Statement
        /// </summary>
        /// <param name="set">Format as: name='Joe'</param>
        /// <param name="where">Format as: name='Joe'</param>
        public void Update(string set, string where)
        {
            if (!String.IsNullOrEmpty(Table))
            {
                string query = $"UPDATE {Table} SET {set} WHERE {where}";

                if (this.OpenConnection() == true)
                {
                    MySqlCommand sqlCommand = new MySqlCommand();
                    sqlCommand.CommandText = query;
                    sqlCommand.Connection = this.Connection;
                    sqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            else { MessageBox.Show("Table name is not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        /// <summary>x
        /// Update Statement Override
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="set">Format as: name='Joe'</param>
        /// <param name="where">Format as: name='Joe'</param>
        public void Update(string table, string set, string where)
        {
            string query = $"UPDATE {table} SET {set} WHERE {where}";
            if (this.OpenConnection() == true)
            {
                MySqlCommand sqlCommand = new MySqlCommand();
                sqlCommand.CommandText = query;
                sqlCommand.Connection = this.Connection;
                sqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        /*========================================
         *          Delete Commands
         ==========================================*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        public void Delete(string where)
        {
            if (!String.IsNullOrEmpty(Table))
            {
                /*DELETE FROM tableinfo WHERE name='John Smith'*/
                string query = $"DELETE FROM {Table} WHERE {where}";

                if (this.OpenConnection() == true)
                {
                    MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                    sqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            else { MessageBox.Show("Table name is not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        public void Delete(string table, string where)
        {
            /*DELETE FROM tableinfo WHERE name='John Smith'*/
            string query = $"DELETE FROM {table} WHERE {where}";

            if (this.OpenConnection() == true)
            {
                MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                sqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        /*=============================================
         *              Select Statement
         ==============================================*/
        /// <summary>
        /// Select Statement Base
        /// </summary>
        /// <returns>List of Strings</returns>
        public List<string>[] Select()
        {
            if (!String.IsNullOrEmpty(Table))
            {
                if (Fields == null || Fields.Length == 0)
                {
                    string query = $"SELECT * FROM {Table}";

                    List<string>[] list = new List<string>[Fields.Length];

                    for (int i = 0; i < Fields.Length; i++)
                    {
                        list[i] = new List<string>();
                    }

                    //Open Connection
                    if (this.OpenConnection() == true)
                    {
                        MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                        MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            int count = 0;
                            foreach (string field in Fields)
                            {
                                list[count].Add(sqlDataReader[field] + "");
                                count++;
                            }
                        }

                        sqlDataReader.Close();
                        this.CloseConnection();
                        return list;
                    }
                    else return list;
                }
                else
                {
                    MessageBox.Show("Table Fields are not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Table name is not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Select Statement Override 1
        /// </summary>
        /// <param name="table">Table Name as String</param>
        /// <returns>List of Strings</returns>
        public List<string>[] Select(string table)
        {
            if (Fields == null || Fields.Length == 0)
            {
                string query = $"SELECT * FROM {table}";

                List<string>[] list = new List<string>[Fields.Length];

                for (int i = 0; i < Fields.Length; i++)
                {
                    list[i] = new List<string>();
                }

                //Open Connection
                if (this.OpenConnection() == true)
                {
                    MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                    MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        int count = 0;
                        foreach (string field in Fields)
                        {
                            list[count].Add(sqlDataReader[field] + "");
                            count++;
                        }
                    }

                    sqlDataReader.Close();
                    this.CloseConnection();
                    return list;
                }
                else return list;
            }
            else
            {
                MessageBox.Show("Table Fields are not defined", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Select Statement Override 2
        /// </summary>
        /// <param name="table">Table Name as String</param>
        /// <param name="fields">Table Fields as Array of Strings</param>
        /// <returns>List of Strings</returns>
        public List<string>[] Select(string table, string[] fields)
        {
            string query = $"SELECT * FROM {table}";

            List<string>[] list = new List<string>[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                list[i] = new List<string>();
            }

            //Open Connection
            if (this.OpenConnection() == true)
            {
                MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    int count = 0;
                    foreach (string field in fields)
                    {
                        list[count].Add(sqlDataReader[field] + "");
                        count++;
                    }
                }

                sqlDataReader.Close();
                this.CloseConnection();
                return list;
            }
            else return list;
        }

        /*===============================================
         *              Count Statement
         ================================================*/
        /// <summary>
        /// Count Statement
        /// </summary>
        /// <returns>Count as Interger</returns>
        public int Count()
        {
            if (!String.IsNullOrEmpty(Table))
            {
                string query = $"SELECT Count(*) FROM {Table}";
                int count = -1;

                if (this.OpenConnection() == true)
                {
                    MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                    count = int.Parse(sqlCommand.ExecuteScalar() + "");
                    this.CloseConnection();
                    return count;
                }
                else return count;
            }
            else return 0;
        }
        /// <summary>
        /// Count Statement
        /// </summary>
        /// <param name="table">Table Name as String</param>
        /// <returns>Count as Interger</returns>
        public int Count(string table)
        {
            string query = $"SELECT Count(*) FROM {table}";
            int count = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand sqlCommand = new MySqlCommand(query, this.Connection);
                count = int.Parse(sqlCommand.ExecuteScalar() + "");
                this.CloseConnection();
                return count;
            }
            else return count;
        }

        /// <summary>
        /// Backup Data
        /// </summary>
        /*public void Backup() { }*/        //Zombie code handles backup function 

        /// <summary>
        /// Retore Data
        /// </summary>
        /*public void Retore() { }*/        //Zobie code handles restores function
    }
}