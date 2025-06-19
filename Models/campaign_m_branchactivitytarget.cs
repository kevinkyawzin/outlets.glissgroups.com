using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class campaign_m_branchactivitytarget
    {
        public int id { get; set; }        
        public DateTime? createdate { get; set; }        
        public DateTime? lastmodifieddate { get; set; }
        public string cocode { get; set; }
        public int? campaignid { get; set; }
        public int? groupid { get; set; }
        public string branchcode { get; set; }
        public int? categoryid { get; set; }
        public int? categorytypeid { get; set; }
        public double? forecast { get; set; }
        public int? userid { get; set; }
    }
}