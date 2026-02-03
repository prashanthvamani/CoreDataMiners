using DataminersDAL.DBConncetion;
using DataminersModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersDAL.Repositories
{
    public class RequestFormRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public RequestFormRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public bool RequestFormInsert(CustomerReqModel.customerReq req)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("AddUserReq1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ReqSource", req.RequestSource ?? "");
                    cmd.Parameters.AddWithValue("@rptname", req.Reportname ?? "");
                    cmd.Parameters.AddWithValue("@DataReqType", req.summarytype ?? "");
                    cmd.Parameters.AddWithValue("@DescOfReq", req.Description ?? "");
                    cmd.Parameters.AddWithValue("@BusinessReq", req.BussinessRequirement ?? "");
                    cmd.Parameters.AddWithValue("@FromDate",  req.FromDate);
                    cmd.Parameters.AddWithValue("@Todate", req.ToDate);
                    cmd.Parameters.AddWithValue("@CustomerType", req.customertype ?? "");
                    cmd.Parameters.AddWithValue("@CustomerName", req.CustomerUname ?? "");
                    cmd.Parameters.AddWithValue("@customerDesign", req.customerDesignation ?? "");
                    cmd.Parameters.AddWithValue("@department", req.department ?? "");
                    cmd.Parameters.AddWithValue("@contactno", req.Contactnumber ?? "");
                    cmd.Parameters.AddWithValue("@UserId", req.UserID ?? "");
                    cmd.Parameters.AddWithValue("@Filename", req.FileUpload);
                    cmd.Parameters.AddWithValue("@Employid", req.empid ?? "");
                    cmd.Parameters.AddWithValue("@Useremailid", req.UserEMailID ?? "");
                    cmd.Parameters.AddWithValue("@Extensionno", req.Extenno ?? "");
                    cmd.Parameters.AddWithValue("@Mobileno", req.mobilenumber ?? "");
                    cmd.Parameters.AddWithValue("@Addcontactname", req.AddCustomerName ?? "");
                    cmd.Parameters.AddWithValue("@Addusermailid", req.AddCustomerEMailID ?? "");
                    cmd.Parameters.AddWithValue("@Customer_mail", req.UserEMailID ?? "");
                    cmd.Parameters.AddWithValue("@InsertDate", req.InsertDate);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }

        public DataSet GetCustomerUniqID()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetNTId", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        public DataSet GetTatTime()
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetTime", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
        }


        public bool AddTask(CustomerReqModel.CustomerAddtask addtask)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                
                using (SqlCommand cmd = new SqlCommand("AddTasks1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ReqId", addtask.ReqID ?? "");
                    cmd.Parameters.AddWithValue("@AddDate", addtask.LoginDate);
                    cmd.Parameters.AddWithValue("@AssignTo", addtask.AssignTO ?? "");
                    cmd.Parameters.AddWithValue("@initialstatus", addtask.InitialStatus ?? "");
                    cmd.Parameters.AddWithValue("@deliverytime", addtask.DeilveryTime ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool UpdateUserDetails(CustomerReqModel.customerReq updatedetails)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UpdateUserDetails1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@emailid", updatedetails.UserEMailID);
                    cmd.Parameters.AddWithValue("@ExtNo", updatedetails.Extenno ?? "");
                    cmd.Parameters.AddWithValue("@Mobilno", updatedetails.mobilenumber);
                    cmd.Parameters.AddWithValue("@ntloginid", updatedetails.UserID);
                    cmd.Parameters.AddWithValue("@EMPid", updatedetails.empid);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
                return true;
        }


        public bool AllowtoWork(CustomerReqModel.Allowtowork allowtowork)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("[AllotworkPraTest]", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestId", allowtowork.ReqID);
                    cmd.Parameters.AddWithValue("@Assigndate", allowtowork.AssignDate);
                    cmd.Parameters.AddWithValue("@status", allowtowork.Status);
                    
                    con.Open(); 
                    cmd.ExecuteNonQuery();
                }
            }

                return true;
        }
    }
}
