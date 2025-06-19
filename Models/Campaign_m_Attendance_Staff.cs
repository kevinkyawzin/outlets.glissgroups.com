using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_m_Attendance_Staff
    {
        public int id { get; set; }
        public int campaignid { get; set; }
        public int groupid { get; set; }
        public string branchcode { get; set; }
        public string cocode { get; set; }
        public int staffid { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public string status { get; set; }

        //public string role="";
        [ForeignKey("groupid")]
        public virtual Campaign_z_Groups Group { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }

        [ForeignKey("staffid")]
        public virtual Common_m_Staff Staff { get; set; }

        public string gameDesc;
        public string DecryptedBranchName;

        
    }
}