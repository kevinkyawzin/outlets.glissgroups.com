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
    public partial class CheckAttendanceListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RewardsService.QWS rewardWebService = new RewardsService.QWS();
            bool result = rewardWebService.Report_AttendanceListTodayExist();

            if (result)
            {
                Response.Redirect("AttendanceListingByDay.aspx");
            }
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);

        }
    }
}