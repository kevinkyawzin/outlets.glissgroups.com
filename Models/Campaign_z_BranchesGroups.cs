using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_z_BranchesGroups
    {
        public int id { get; set; }
        public int groupid { get; set; }
        public string branchcode { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

        [ForeignKey("groupid")]
        public virtual Campaign_z_Groups Group { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }

        //public IEnumerable<Configuration_m_Branches> Branches { get; set; }
    }
}