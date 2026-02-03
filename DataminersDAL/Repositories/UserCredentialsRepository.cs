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
    public class UserCredentialsRepository
    {
        private readonly DatabaseHelper _dbHelper;

        
        public UserCredentialsRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataSet GetUserCredentialDetails(string Empid)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Sp_GetuserDetails", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empid", Empid);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        }

        public int OpenCount()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_openCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    object result = cmd.ExecuteScalar(); // Get the count
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public int WIPCount()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_WIPCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    object result = cmd.ExecuteScalar(); // Get the count
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public int CloseCount()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_ClosedCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    object result = cmd.ExecuteScalar(); // Get the count
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }


        public int RejectCount()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_RejectCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    object result = cmd.ExecuteScalar(); // Get the count
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public int SelfCount(string NtLoginID)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Sp_SelfCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", NtLoginID);
                    object result = cmd.ExecuteScalar(); // Get the count
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

    }
}
