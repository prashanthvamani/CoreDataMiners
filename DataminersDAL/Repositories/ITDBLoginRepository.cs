using DataminersDAL.DBConncetion;
using DataminersDAL.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersDAL.Repositories
{
    public class ITDBLoginRepository : IITDBLoginRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ITDBLoginRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;   
        }

        public DataSet ITDBLogin(string ntLoginID, string password)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("select NTloginid, Level from Processor_Credentials where NTloginid=@username and password=@password", con))
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
}
