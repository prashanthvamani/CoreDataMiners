using DataminersDAL.DBConncetion;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersDAL.Repositories
{
    public class LoginRepository
    {
        private readonly DatabaseHelper _dbHelper;
        public LoginRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public DataSet Login(string empid)
        {
            using (SqlConnection con =  _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UserExists", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmpID", empid);
                        DataSet ds = new DataSet(); 
                        da.Fill(ds);
                        return ds;  
                    }

                }
            }
        }

        public bool UserLogInsert(string name, string employeeID, string role, string ipAddress, string browser)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("Preapproved_logged_info", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Uname", name);
                    cmd.Parameters.AddWithValue("@employeeid", employeeID);
                    cmd.Parameters.AddWithValue("@role", role);
                    //cmd.Parameters.AddWithValue("@region",);
                    cmd.Parameters.AddWithValue("@ip_address", ipAddress);
                    cmd.Parameters.AddWithValue("@browser_info", browser);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }

        public DataSet ITDBLogin(string ntLoginID, string password)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("select Empid from Processor_Credentials where NTloginid=@username and password=@password", con))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", ntLoginID);
                    cmd.Parameters.AddWithValue("@password", password);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        

    }
}
