using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_m_CampaignDetails
    {
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int groupid { get; set; }
        public string branchcode { get; set; }
        public string cocode { get; set; }
        public int campaignid { get; set; }
        public int staffid { get; set; }
        public int categoryid { get; set; }
        public int categorytypeid { get; set; }
        public TimeSpan? time { get; set; }
        public double? salesactual { get; set; }
        public double? salesforecast { get; set; }
        public double? productactual { get; set; }
        public double? productforecast { get; set; }
        public int? userid { get; set; }
        public int? resourceid { get; set; }
        public int? resourcedetailid { get; set; }

        //public string role="";
        [ForeignKey("groupid")]
        public virtual Campaign_z_Groups Group { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }

        [ForeignKey("campaignid")]
        public virtual Campaign_m_Campaign Campaign { get; set; }

        [ForeignKey("staffid")]
        public virtual Common_m_Staff Staff { get; set; }

        [ForeignKey("categoryid")]
        public virtual Campaign_z_Category Category { get; set; }

        public string gameDesc;
    }

    public class CampaignBranchSales
    {
        public int groupid { get; set; }
        public string branchcode { get; set; }
        public int categoryid { get; set; }
        //public double actual { get; set; }
        //public double forecast { get; set; }
        public string category { get; set; }
        public double? salesactual { get; set; }
        public double? salesforecast { get; set; }
        public double? productactual { get; set; }
        public double? productforecast { get; set; }      
        
    }
}