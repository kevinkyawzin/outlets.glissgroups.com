using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Common_m_StaffBranch
    {
        public int id { get; set; }
        public string branchcode { get; set; }
        public int staffid { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int departmentid { get; set; }
        [ForeignKey("staffid")]
        public virtual Common_m_Staff Staff { get; set; }
    }

    public class Common_m_StaffBranchdtl
    {
        public int id { get; set; }
        public string branchcode { get; set; }
        public int staffid { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string stafftype { get; set; }
        public int departmentid { get; set; }
        public string departmentname { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

    }
}