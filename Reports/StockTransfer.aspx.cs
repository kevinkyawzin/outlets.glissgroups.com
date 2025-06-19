using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BigMac.Reports
{
    public partial class StockTransfer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        Stimulsoft.Report.StiReport Report = rwModuleHC.GetReport();
        Report.ReportName = "Stock Transfer " + Request.QueryString["id"];
        wvModuleHC.Report = Report;
        wvModuleHC.PrintToPdf();
        }
    }
}