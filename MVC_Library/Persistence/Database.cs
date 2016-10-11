using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MVC_LIbrary.Persistence
{
    /// <summary>
    /// Class to handle the access to database
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Holder for created connection to database
        /// </summary>
        private static SqlConnection sqlConnection { get; set; }

        /// <summary>
        /// Initilization of this class, singleton usage
        /// </summary>
        static Database()
        {
            if (sqlConnection == null)
            {
                sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            }
        }

        /// <summary>
        /// Get the connection to database
        /// </summary>
        /// <returns>
        /// Connection to database
        /// </returns>
        public static SqlConnection GetConnection()
        {
            if (sqlConnection.State != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

            return sqlConnection;
        }
    }
}