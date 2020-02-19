using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSDKConn.Core.Domain.Loans
{
    public class LoanAttribute : BaseEntity
    {
        public int PointFieldID { get; set; }
        public string PointFieldName { get; set; }
    }
}
