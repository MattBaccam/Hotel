using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal static class SqlConnectionProvider
    {
        // use a factor method to ensure that there is only one method in the entire project that has the database connection string
        //Connection string always include the server, database, credentials

        private static string connectionString = @"Data Source=localhost;Initial Catalog=Hotel;Integrated Security=True;";

        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection(connectionString);

            return connection;
        }
    }
}
