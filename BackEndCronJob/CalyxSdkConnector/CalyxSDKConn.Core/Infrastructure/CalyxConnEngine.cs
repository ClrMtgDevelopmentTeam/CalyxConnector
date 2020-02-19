
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Reflection;
using CalyxSDKConn.Core.Domain.Loans;

namespace CalyxSdkConnector.Core.Infrastructure
{
    public static class CalyxConnEngine
    {
        private static List<LoanAttribute> _attributes;
        public static List<LoanAttribute> Attributes
        {
            get
            {
                return _attributes == null ? LoadAttributesFromFile() : _attributes;
            }
        }
        private static List<LoanAttribute> LoadAttributesFromFile()
        {
            int pointFieldID = 0;
            List<LoanAttribute> loanAttrs = new List<LoanAttribute>();

            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            if (File.Exists(dirPath + "/LoanAttribute/Attribute.txt"))
            {
                string[] lines = File.ReadAllLines(dirPath + "/LoanAttribute/Attribute.txt");
                foreach (var line in lines)
                {
                    string[] attr = line.Split('\t');
                    int.TryParse(attr[0], out pointFieldID);
                    if (pointFieldID != 0 && attr.Count() == 2)
                    {
                        try
                        {
                            loanAttrs.Add(new LoanAttribute() { PointFieldID = pointFieldID, PointFieldName = attr[1] });
                        }
                        catch { }
                    }
                    else
                    {

                    }
                    
                }
            }
           
            _attributes = loanAttrs;
            return loanAttrs;
        }
    }
}
