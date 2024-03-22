using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//installing mysql.data to project
using MySql.Data.MySqlClient;

namespace C1.Models
{
    public class SchoolDbContext
    {
        //These are readonly "secret" properties 
        //Only the SchoolDbContext class can use time
        //matching it to local school database
        private static string User { get { return "root"; } }

        private static string Password { get { return "root"; } }

        private static string Database { get { return "school"; } }

        private static string Server { get { return "localhost"; } }

        private static string port { get { return "3306"; } }

        // ConnectionString is a series of credientials used to connect to the database 
        protected static string ConnectionString
        {
            get
            {
                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + port
                    + "; password = " + Password;
            }
        }
        //this is the method used to get the database
        /// <summary>
        /// Returns a connection to the school database
        /// </summary>
        /// <example>
        /// private SchoolDbContext School = new SchoolDbContext();
        /// MySqlConnection Conn = School.AccessDatabase();
        /// </example>
        /// <returns>A MySqlConnection Object</returns>
        public MySqlConnection AccessDatabase()
        {
            //intantiating the MySqlConnection Class to create an object
            //the object is a specific connection to our school database on port 3306 of localhost
            return new MySqlConnection(ConnectionString);
        }
    }
}