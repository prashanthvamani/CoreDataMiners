using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersModel
{
    public class ErrorLog
    {
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
