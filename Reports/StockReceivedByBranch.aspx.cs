using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BigMac.Reports
{
    public partial class StockReceivedByBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport Report = rwModuleHC.GetReport();
            Report.ReportName = "Stock Received Listing By Branch ";
            wvModuleHC.Report = Report;
            wvModuleHC.PrintToPdf();
        }
    }
}