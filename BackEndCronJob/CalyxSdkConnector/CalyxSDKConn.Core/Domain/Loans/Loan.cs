using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSDKConn.Core.Domain.Loans
{
    public partial class Loan : BaseEntity
    {
        public Int64 LoanID { get; set; }
        public string FileName { get; set; }
        public string LoanRep { get; set; }
        public string TypeOfLoan { get; set; }
        public DateTime? LoanStatusDate { get; set; }
        public DateTime? ContactDate { get; set; }
        public DateTime? EstClose { get; set; }
        public string LoanStatus { get; set; }

        public string CoBorrowerBuisnessPhone { get; set; }
        public string CoBorrowerHomePhone { get; set; }
        public string CoBorrowerLastName { get; set; }
        public string CoBorrowerFirstName { get; set; }
        public string BorrowerBusinessPhone { get; set; }
        public string BorrowerHomePhone { get; set; }
        public string PresentAddress { get; set; }
        public string SubjectPropertyAddress { get; set; }
        public string Processor { get; set; }
        public string BorrowerFirstName { get; set; }


        public string BorrowerLastName { get; set; }
        public string BorrowerPreferredName { get; set; }
        public DateTime? RateLockExpiration { get; set; }

    }
}
