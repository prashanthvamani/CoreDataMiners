using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersModel
{
    public class ITDBCloseModel
    {
        public class CloseData
        {
            public string RequestID { get; set; }
            public string Status { get; set; }
            public string SpendTime { get; set; }
            public string Rejectremarks { get; set; }
            public string mkrempid { get; set; }

        }

        
        public class InsertPRoc
        {
            public string RequestID { get; set; }
            public string Dept { get; set; }
            public string Splink { get; set; }
            public string QueryLink { get; set; }

            public string spname { get; set; }

        } 

    }
}
