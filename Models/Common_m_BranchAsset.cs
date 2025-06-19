using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigMac.Models
{
    public class Common_m_BranchAsset
    {
        public int id { get; set; }
        [Display(Name = "Branch ID")]
        public int branchid { get; set; }
        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Asset Type ID")]
        public int assettypeid { get; set; }
        [Display(Name = "Qty")]
        public int qty { get; set; }
        [Display(Name = "Remarks")]
        public string remark { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}