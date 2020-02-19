using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CalyxSDKConn.Service.CalyxSDK;
using CalyxSDKConn.Service.ErrorLogging;
using CalyxSDKConn.Core.Domain.Loans;
using Newtonsoft;
using Newtonsoft.Json;
using CalyxSdkConnector.Models;
using System.Text.RegularExpressions;

namespace CalyxSdkConnector
{
    public partial class LoanSearch : Form
    {
        #region Variables
        private string _action = "";
        private string _userName = "";
        private string _password = "";
        private string _fileID = "";
        private string _dataFolder = "";
        private string _searchLoanType = "";
        private string _searchByType = "";
        private string _searchOption = "";
        private string _searchContent = "";

        #endregion Variables
        public LoanSearch(string[] args)
        {

            InitializeComponent();

            try
            {
                #region args
                _action = args[0];
                _userName = args[1];
                _password = args[2];
                _fileID = args[3];
                _dataFolder = args[4];
                _searchLoanType = args[5];
                _searchByType = args[6];
                _searchOption = args[7];
                _searchContent = args[8];
                #endregion args

                SearchLoans();
            }
            catch
            {
                
            }
        }

        #region Methods
        public void SearchLoans()
        {
            List<string> _dataFolders = new List<string>();
            if (_dataFolder != "")
                _dataFolders.Add(_dataFolder);
            ErrorLogService _errLogService = new ErrorLogService();
            CalyxSDKService _calyxSdk = new CalyxSDKService(_errLogService);
            if (_action == "search")
            {
                Dictionary<List<Loan>, string> response = _calyxSdk.Loans(_userName, _password, _dataFolders, _searchLoanType, _searchByType, _searchOption, _searchContent);
                string result = JsonConvert.SerializeObject(new LoanApiResponse() { Loans = response.FirstOrDefault().Key, Error = response.FirstOrDefault().Value });
                System.IO.File.WriteAllText(Application.StartupPath + "/LoanFiles/" + _fileID + ".json", result);
            }
            else if (_action == "LoanInfo")
            {
                dynamic response = _calyxSdk.LoanInfo(_userName, _password, _searchLoanType, _searchContent);
                string result = JsonConvert.SerializeObject(response);
                System.IO.File.WriteAllText(Application.StartupPath + "/LoanInfo/" + _fileID + ".json", result);
            }
        }

        #endregion Methods

        #region Events
        private void LoanSearch_Shown(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Events
    }
}
