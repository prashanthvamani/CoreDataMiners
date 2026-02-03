using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersModel.ViewModels
{
    public class ITDBCloseViewModel
    {
        //public List<ITDBCloseModel.RequestDetailsOpenWIP> RequestDetails { get; set; }

        public List<RequestDetailsOpenWIPViewModel> RequestDetails {  get; set; }

        public List<RequestsearchViewModel> RequestIDSearch { get; set; }


        public ITDBCloseViewModel() 
        {
            RequestDetails = new List<RequestDetailsOpenWIPViewModel>();
            RequestIDSearch = new List<RequestsearchViewModel>(); 
        }

    }

    public class RequestDetailsOpenWIPViewModel
    {
        public string RequestID { get; set; }
        public DateTime? ReqDateTime { get; set; }
        public string RequestSource { get; set; }
        public DateTime? RequestDataFromDate { get; set; }
        public DateTime? RequestDataToDate { get; set; }
        public string DataRequestType { get; set; }
        public string CustomerType { get; set; }
        public string CustomerDesignation { get; set; }
        public string ContactNo { get; set; }
        public string Requestor { get; set; }
        public string FileName { get; set; }
        public string TAT { get; set; }
        public string FinalStatus { get; set; }
        public string AssignTo { get; set; }

    }

    public class RequestsearchViewModel
    {
        public string RequestID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string RequestSource { get; set; }
        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string InitialStatus { get; set; }
        public string AssginTo { get; set; }
        public DateTime? AssignDate { get; set; }
        public string FinalStatus { get; set; }
        public string CompletedDate { get; set; }
        public string VerifiedBy { get; set; }
    }

}
