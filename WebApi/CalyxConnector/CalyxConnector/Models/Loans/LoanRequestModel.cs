using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalyxConnector.Models.Loans
{
    public class LoanRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public List<string> DataFolders { get; set; }
        public string SearchLoanType { get; set; }
        public string SearchByType { get; set; }
        public string SearchOption { get; set; }
        public string SearchContent { get; set; }

    }
    public class LoanInfoRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public List<string> DataFolders { get; set; }
        public string SearchLoanType { get; set; }
        public string SearchByType { get; set; }
        public string SearchOption { get; set; }
        public string LoanFileName { get; set; }
    }
}