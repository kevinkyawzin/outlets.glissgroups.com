using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BigMac.Models;

namespace BigMac.Reports
{
    public partial class PrintCardListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {        
        Stimulsoft.Report.StiReport Report = rwModuleHC.GetReport();
        Report.ReportName = "Card List To Print ";
        wvModuleHC.Report = Report;
        //wvModuleHC.PrintToPdf();
        }
    }
}