using Calyx.Point.Data.DataFolderServices;
using CalyxSDKConn.Core.Domain.Loans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalyxSDKConn.Service.CalyxSDK
{
    public interface ICalyxSDKService
    {
        Dictionary<List<Loan>, string>Loans(string userName, string password, List<string> dataFolders = null, string selectedLoanType = "Borrower", string searchByType = "FileName", string searchOption = "BeginsWith", string searchContent="");
        dynamic LoanInfo(string userName, string password, string selectedLoanType = "Borrower", string searchContent="");
    }
}
