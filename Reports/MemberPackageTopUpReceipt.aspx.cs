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
    public partial class MemberPackageTopUpReceipt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport Report = rwModuleHC.GetReport();
            Report.ReportName = "Package Top Up " + Request.QueryString["id"];
            wvModuleHC.Report = Report;
            StiReportResponse.ResponseAsPdf(this, wvModuleHC.Report, false);
          
        }
    }
}