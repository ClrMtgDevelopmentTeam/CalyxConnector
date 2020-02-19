using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.IO;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CalyxConnector.Models.Loans;

namespace CalyxConnector.Controllers
{
    [RoutePrefix("api/Loans")]
    public class LoansController : ApiController
    {
        public LoansController()
        {
        }
        [HttpPost]
        [Route("search")]
        public HttpResponseMessage Search(LoanRequestModel request)
        {
            string resonse = "";
            string fileID = Guid.NewGuid().ToString();
            bool isProcessStart = false;
            if (ModelState.IsValid)
            {
                try
                {
                    #region Start Child Process

                    Process prc = new Process();
                    prc.StartInfo.Arguments = "\"search\" \"" + request.UserName + "\" \"" + request.Password + "\" \"" + fileID + "\" \"" + (request.DataFolders == null ? "" : request.DataFolders.FirstOrDefault())
                        + "\" \"" + (request.SearchLoanType == null ? "" : request.SearchLoanType) + "\" \""
                        + (request.SearchByType == null ? "" : request.SearchByType) + "\" \"" + (request.SearchOption == null ? "" : request.SearchOption) + "\" \""
                        + (request.SearchContent == null ? "" : request.SearchContent) + "\"";
                    prc.StartInfo.FileName = ConfigurationManager.AppSettings["sdkConnectorPath"] + "/CalyxSdkConnector.exe";
                    isProcessStart = prc.Start();

                    #endregion Start Child Process

                    if (isProcessStart)
                    {
                        System.Threading.Thread.Sleep(3000);

                        int tryCount = 0;
                        do
                        {
                            if (File.Exists(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanFiles/" + fileID + ".json"))
                            {
                                resonse = File.ReadAllText(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanFiles/" + fileID + ".json");
                                File.Delete(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanFiles/" + fileID + ".json");
                                break;
                            }
                     
                            System.Threading.Thread.Sleep(1000);
                        } while (tryCount < 25 && resonse == "");
             
                        if (resonse == "")
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, "SDK Connector failed to get data.");

                        else
                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.DeserializeObject(resonse));
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed To Run SDK Connector.");
                    }



                }
                catch (Exception exp)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed To Run SDK Connector. Exception " + exp.Message);

                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
        [HttpPost]
        [Route("loaninfo")]
        public HttpResponseMessage LoanInfo(LoanInfoRequestModel request)
        {
            string resonse = "";
            string fileID = Guid.NewGuid().ToString();
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");         
            bool isProcessStart = false;
            if (ModelState.IsValid && !string.IsNullOrEmpty(request.LoanFileName))
            {
               
                fileID = rgx.Replace(request.LoanFileName, "") + "-" + fileID;
                try
                {

                    #region Start Child Process

                    Process prc = new Process();
                    prc.StartInfo.Arguments = "\"LoanInfo\" \"" + request.UserName + "\" \"" + request.Password + "\" \"" + fileID + "\" \"" + (request.DataFolders == null ? "" : request.DataFolders.FirstOrDefault())
                        + "\" \"" + (request.SearchLoanType == null ? "" : request.SearchLoanType) + "\" \""
                        + (request.SearchByType == null ? "" : request.SearchByType) + "\" \"" + (request.SearchOption == null ? "" : request.SearchOption) + "\" \""
                        + (request.LoanFileName == null ? "" : request.LoanFileName) + "\"";
                    prc.StartInfo.FileName = ConfigurationManager.AppSettings["sdkConnectorPath"] + "/CalyxSdkConnector.exe";
                    isProcessStart = prc.Start();

                    #endregion Start Child Process
                    
                    if (isProcessStart)
                    {
                        System.Threading.Thread.Sleep(5000);
                        int tryCount = 0;
                        do
                        {                                                                  
                            if (File.Exists(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanInfo/" + fileID + ".json"))
                            {
                                resonse = File.ReadAllText(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanInfo/" + fileID + ".json");
                                File.Delete(ConfigurationManager.AppSettings["sdkConnectorPath"] + "/LoanInfo/" + fileID + ".json");
                                break;        
                            }  
                            System.Threading.Thread.Sleep(1000);
                        } while (tryCount < 35 && resonse == "");

                        if (resonse =="")
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, "SDK Connector failed to get data.");

                        else
                            return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.DeserializeObject(resonse));
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed To Run SDK Connector.");
                    }
                }                                                                                                                                
                catch (Exception exp)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Failed To Run SDK Connector. Exception " + exp.Message);
                }
            }                                                     
            else
            {
                if (string.IsNullOrEmpty(request.LoanFileName))
                    ModelState.AddModelError("LoanFileName", "The LoanFileName field is required.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
