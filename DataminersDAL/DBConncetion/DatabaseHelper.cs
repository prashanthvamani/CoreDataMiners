using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;

namespace DataminersDAL.DBConncetion
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public SqlConnection GetConnection()
        {
            string connectionString = _configuration.GetConnectionString("sqlCon");
            return new SqlConnection(connectionString);
        }
    }
}
