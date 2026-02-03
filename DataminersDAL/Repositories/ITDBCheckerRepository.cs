using DataminersDAL.DBConncetion;
using DataminersModel;
using DataminersModel.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataminersDAL.Repositories
{
    public class ITDBCheckerRepository : IITDBCheckerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseHelper _dbhelper;

        public ITDBCheckerRepository(IConfiguration configuration, DatabaseHelper dbhelper)
        {
            _configuration = configuration;
            _dbhelper = dbhelper;
        }

        public List<RequestDetailsOpenWIPViewModel> RequestDetailsOpenWIPs(string ntloginID)
        {
            var list = new List<RequestDetailsOpenWIPViewModel>();

            using (SqlConnection con = _dbhelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetOpenReq", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ntlogid", ntloginID);
                    //cmd.Parameters.AddWithValue("@Search", search ?? "");
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new RequestDetailsOpenWIPViewModel
                            {
                                RequestID = dr["RequestID"].ToString(),
                                ReqDateTime = Convert.ToDateTime(dr["ReqDateTime"]),
                                RequestSource = dr["RequestSource"].ToString(),
                                RequestDataFromDate = Convert.ToDateTime(dr["RequestDataFromDate"]),
                                RequestDataToDate = Convert.ToDateTime(dr["RequestDataToDate"]),
                                DataRequestType = dr["DataRequestType"].ToString(),
                                CustomerType = dr["CustomerType"].ToString(),
                                CustomerDesignation = dr["CustomerDesignation"].ToString(),
                                ContactNo = dr["ContactNo"].ToString(),
                                Requestor = dr["Requestor"].ToString(),
                                FileName = dr["FileName"].ToString(),
                                TAT = dr["TAT"].ToString(),
                                FinalStatus = dr["FinalStatus"].ToString(),
                                AssignTo = dr["AssginTo"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<RequestsearchViewModel> requestsearchViewModels(string requestID)
        {
            var list = new List<RequestsearchViewModel>();

            using (SqlConnection con = _dbhelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Getsearch1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Rqid", requestID);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new RequestsearchViewModel
                            {
                                RequestID = dr["RequestID"].ToString(),
                                RequestDate = Convert.ToDateTime(dr["RequestDate"]),
                                RequestSource = dr["RequestSource"].ToString(),
                                CustomerType = dr["CustomerType"].ToString(),
                                CustomerName = dr["CustomerName"].ToString(),
                                InitialStatus = dr["InitialStatus"].ToString(),
                                AssginTo = dr["AssginTo"].ToString(),
                                AssignDate = Convert.ToDateTime(dr["AssignDate"].ToString()),
                                FinalStatus = dr["FinalStatus"].ToString(),
                                CompletedDate = dr["CompletedDate"].ToString(),
                                VerifiedBy = dr["VerifiedBy"].ToString()
                            });
                        }
                    }
                }
                return list;
            }
        }
    }
}
