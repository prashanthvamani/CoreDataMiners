using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataminersModel
{
    public class CustomerReqModel
    {
        public class customerReq
        {
            public string RequestSource { get; set; }
            public string Reportname { get; set; }
            //public string Requesttype { get; set; }
            public string summarytype { get; set; }
            public string Description { get; set; }
            public string BussinessRequirement { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime FromDate { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime ToDate { get; set; }            
            //public string Noofcolumns { get; set; }
            public string CustomerUname { get; set; }
            public string UserID { get; set; }
            public string FileUpload { get; set; }
            public string customertype { get; set; }
            public string empid { get; set; }
            public string customerDesignation { get; set; }
            public string Contactnumber { get; set; }
            public string department { get; set; }
            public string UserEMailID { get; set; }
            public string Extenno { get; set; }
            public string mobilenumber { get; set; }
            public string RequestID { get; set; }
            public string AddCustomerEMailID { get; set; }
            public string AddCustomerName { get; set; }
            public DateTime InsertDate { get; set; }
        }

        public class CustomerAddtask
        {
            public string ReqID { get; set; }
            public DateTime? LoginDate { get; set; } 
            public string AssignTO { get; set; }
            public string InitialStatus { get; set; }
            public string DeilveryTime { get; set; }
        }

        public class Allowtowork
        {
            public string ReqID { get; set; }
            public DateTime? AssignDate { get; set; }
            public string Status { get; set; }
        }

        public class ChangeTask
        {   
            public string remarks { get; set; }
            public string AssignTO { get; set; }
            public string ReqID { get; set; }
            public string EstTime { get; set; }
            public string NtUsername { get; set; }

        }

        public class DMspLinkins
        {
            public string ReqID { get; set; }
            public string reqdate { get; set; }
            public string department { get; set; }
            public string reqtype { get; set; }
            public string AssignName { get; set; }
            public string UserMAIlid { get; set; }
            public string conatctno { get; set; }
            public string Splink { get; set; }
            public string ITDBchcckerMailiD { get; set; }

        }

        public class checkerReject
        {
            public string ReqID { get; set; }
            public string Status { get; set; }
            public string rejectremarks { get; set; }
            public string EMpID { get; set; }
        }
    }
}
