using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_m_Staffs
    {
        public int id { get; set; }
        public string branchcode { get; set; }
        public int staffid { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

        //[ForeignKey("branchcode")]
        //public virtual Campaign_z_BranchesGroups BranchGroup { get; set; }
        //public virtual Campaign_z_BranchesGroups BranchGroup { get; set; }

        [ForeignKey("staffid")]
        public virtual Common_m_Staff Staff { get; set; }
    }
}