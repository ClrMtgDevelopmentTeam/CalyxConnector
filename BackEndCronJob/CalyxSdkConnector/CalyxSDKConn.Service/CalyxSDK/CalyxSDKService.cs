using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calyx.Point.Data.DataFolderServices;
using CalyxSDKConn.Core.Domain.ErrorLogging;
using CalyxSDKConn.Core.Domain.Loans;
using CalyxSDKConn.Core.Domain.Point;
using CalyxSDKConn.Service.ErrorLogging;
using CalyxSdkConnector.Core.Infrastructure;

namespace CalyxSDKConn.Service.CalyxSDK
{
    public partial class CalyxSDKService : ICalyxSDKService
    {
        #region variables

        IErrorLogService _errorLogService;
        private Calyx.Point.SDK.Results.PointUserLoginResults _loginResult;

        #endregion variables
        #region CTOR
        public CalyxSDKService(IErrorLogService errorLogService)
        {
            _errorLogService = errorLogService;
        }

        #endregion CTOR

        #region Methods
        private void ClientLogin(string userName, string password)
        {
            if (Calyx.Point.SDK.PointSDK.GetPDK().NeedCredentials())
                _loginResult = Calyx.Point.SDK.PointSDK.GetPDK().ClientLogin(userName, password);
            else
                _loginResult = Calyx.Point.SDK.PointSDK.GetPDK().ClientLogin();


        }
        public Dictionary<List<Loan>, string> Loans(string userName, string password, List<string> dataFolders, string selectedLoanType = "Borrower", string searchByType = "FileName", string searchOption = "BeginsWith", string searchContent = "")
        {
            Dictionary<List<Loan>, string> response = new Dictionary<List<Loan>, string>();
            List<Loan> officerLoans = new List<Loan>();
            try
            {

                this.ClientLogin(userName, password);
                Calyx.Point.SDK.Results.GetLoanResults apiResponse = this.GetLoansFromApi(dataFolders, selectedLoanType, searchByType, searchOption, searchContent);

                foreach (Calyx.Point.Data.DataFolderServices.LoanInfo item in apiResponse.Loans)
                {
                    officerLoans.Add(new Loan()
                    {
                        BorrowerBusinessPhone = item.Attributes.BorrowerBusinessPhone,
                        BorrowerFirstName = item.Attributes.BorrowerFirstName,
                        BorrowerHomePhone = item.Attributes.BorrowerHomePhone,
                        BorrowerLastName = item.Attributes.BorrowerLastName,
                        BorrowerPreferredName = item.Attributes.BorrowerPreferredName,
                        CoBorrowerBuisnessPhone = item.Attributes.CoBorrowerBuisnessPhone,
                        CoBorrowerFirstName = item.Attributes.CoBorrowerFirstName,
                        CoBorrowerHomePhone = item.Attributes.CoBorrowerHomePhone,
                        CoBorrowerLastName = item.Attributes.CoBorrowerLastName,
                        ContactDate = item.Attributes.ContactDate,
                        EstClose = item.Attributes.EstClose,
                        FileName = item.Attributes.FileName,
                        LoanRep = item.Attributes.LoanRep,
                        LoanStatus = item.Attributes.LoanStatus.ToString(),
                        LoanStatusDate = item.Attributes.LoanStatusDate,
                        PresentAddress = item.Attributes.PresentAddress,
                        Processor = item.Attributes.Processor,
                        RateLockExpiration = item.Attributes.RateLockExpiration,
                        SubjectPropertyAddress = item.Attributes.SubjectPropertyAddress,
                        TypeOfLoan = item.Attributes.TypeOfLoan.ToString()
                    });
                }
                response.Add(officerLoans, "");
            }
            catch (Exception exp)
            {
                _errorLogService.logerror("Calyx SDK : Failed to getting Loans.", exp.Message, ErrorType.GeneralException, ErrorSeverity.Error);
                response.Add(officerLoans, "Error occurred in getting Loans. Error Deatils: " + exp.Message);
            }
            return response;
        }
        public dynamic LoanInfo(string userName, string password, string selectedLoanType = "Borrower", string searchContent = "")
        {
            dynamic loanInfo = new System.Dynamic.ExpandoObject();
            try
            {
                this.ClientLogin(userName, password);
                Calyx.Point.SDK.Results.GetLoanResults apiResponse = this.GetLoansFromApi(null, selectedLoanType, SearchByType.FileName.ToString(), SearchOption.Equal.ToString(), searchContent);

                if (apiResponse.Loans == null || apiResponse.Loans.Count() == 0)
                    loanInfo.Error = "OOPS! Didn't Found Loan corresponding to FileName " + searchContent;
                else
                    loanInfo = BindUpLoanInfo(apiResponse.Loans.First());
            }
            catch (Exception exp)
            {
                _errorLogService.logerror("Calyx SDK : Failed to getting Loan Info.", exp.Message, ErrorType.GeneralException, ErrorSeverity.Error);
                loanInfo.Error = "Error occurred in getting Loan Info. Error Deatils: " + exp.Message;
            }
            return loanInfo;
        }

        #endregion Methods

        #region Utilities
        private Calyx.Point.SDK.Results.GetLoanResults GetLoansFromApi(List<string> dataFolders, string selectedLoanType, string searchByType, string searchOption, string searchContent = "")
        {
            SearchLoanType _selectedLoanType = SearchLoanType.Borrower;
            SearchByType _searchByType = SearchByType.FileName;
            SearchOption _searchOption = SearchOption.MatchesWith;
            Calyx.Point.SDK.Results.GetLoanResults loansResult = null;

            #region RefinFilters

            List<DataFolderInfo> _searchFolders = new List<DataFolderInfo>();
            if (dataFolders != null && dataFolders.Count() > 0)
            {
                foreach (var folder in dataFolders)
                {
                    foreach (var userFolder in _loginResult.UserInfo.DataFolders)
                    {
                        if (userFolder.Name == folder)
                        {
                            _searchFolders.Add(userFolder);
                            break;
                        }
                    }
                }
            }
            else
                _searchFolders = _loginResult.UserInfo.DataFolders.ToList();

            if (!Enum.TryParse(selectedLoanType, out _selectedLoanType))
                _selectedLoanType = SearchLoanType.Borrower;
            if (!Enum.TryParse(searchByType, out _searchByType))
                _searchByType = SearchByType.FileName;
            if (!Enum.TryParse(searchOption, out _searchOption))
                _searchOption = SearchOption.MatchesWith;


            #endregion  RefinFilters

            loansResult = _loginResult.UserInfo.GetLoans(_searchFolders, _selectedLoanType, _searchByType, _searchOption, searchContent, "");
            return loansResult;

        }

        private dynamic BindUpLoanInfo(LoanInfo loan)
        {
            dynamic loanInfo = new System.Dynamic.ExpandoObject();
            LoanFile loanFile = loan.Open(true);

            List<LoanAttribute> _attrs = CalyxConnEngine.Attributes;
            if (_attrs == null || _attrs.Count() == 0)
                loanInfo.Error = "OOPS! Failed to Bind Loan Fields.";
            else
            {
                loanInfo.Error = "";
                List<PointField> pointFields = new List<PointField>();
                foreach (LoanAttribute attr in _attrs)
                {
                    try
                    {
                        pointFields.Add(new PointField()
                        {
                            PointFieldID = attr.PointFieldID,
                            Description = attr.PointFieldName,
                            Data = PointFieldValue(loanFile.GetData(attr.PointFieldID, Calyx.Point.Data.Common.BorrowerSetPositionType.Borrower))
                        });
                    }
                    catch { }
                }
                loanInfo.PointFields = pointFields;
            }
            return loanInfo;
        }

        private dynamic PointFieldValue(object PointField)
        {
            if (PointField == null)
                PointField = "";

            return PointField;
        }


        #endregion Utilities


    }
}


