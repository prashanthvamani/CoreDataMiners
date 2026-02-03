using DataminersDAL.DBConncetion;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersDAL.Repositories
{
    public class ErrorLogRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ErrorLogRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void SaveLogError(string errorMessage, string stackTrace, string controllerName, string actionName, string userName, string additionalInfo)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("USP_LogError", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ErrorMessage", errorMessage);
                    cmd.Parameters.AddWithValue("@StackTrace", stackTrace);
                    cmd.Parameters.AddWithValue("@ControllerName", controllerName);
                    cmd.Parameters.AddWithValue("@ActionName", actionName);
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    cmd.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
