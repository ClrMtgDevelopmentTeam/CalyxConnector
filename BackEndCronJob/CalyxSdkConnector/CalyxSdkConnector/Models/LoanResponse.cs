using CalyxSDKConn.Core.Domain.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSdkConnector.Models
{
    public class LoanApiResponse
    {
        public List<Loan> Loans { get; set; }
        public string Error { get; set; }
    }
}
