using CalyxSDKConn.Core.Domain.ErrorLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSDKConn.Service.ErrorLogging
{
    public partial class ErrorLogService : IErrorLogService
    {
        public void logerror(string message, string details, ErrorType errorType, ErrorSeverity errorSeverity)
        {
            //throw new NotImplementedException();
        }
    }
}
