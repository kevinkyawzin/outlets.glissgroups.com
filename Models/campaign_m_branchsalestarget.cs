using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class campaign_m_branchsalestarget
    {
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string cocode { get; set; }
        public int? campaignid { get; set; }
        public int? groupid { get; set; }
        public string branchcode { get; set; }
        public int? staffid { get; set; }
        public int? userid { get; set; }
        public double? salesforecast { get; set; }
        public double? salesactual { get; set; }
        public string staffdaytype { get; set; }
    }
}