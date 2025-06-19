using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class attendance_m_attendancedetail
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        
        [Display(Name = "Branch ID")]
        public int? branchid { get; set; }

        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }

        [Display(Name = "Co. Code")]
        public string cocode { get; set; }
               
        [Display(Name = "Staff ID")]
        public string staffid { get; set; }

        [Display(Name = "Staff Name")]
        public string staffname { get; set; }

        [Display(Name = "Type")]
        public string type { get; set; }

        [Display(Name = "Time In")]
        public DateTime attendancein { get; set; }

        [Display(Name = "Time Out")]
        public DateTime? attendanceout { get; set; }

        [Display(Name = "Create Date")]
        public DateTime createdate { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime lastmodifieddate { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Last Modified By")]
        public string lastmodifiedby { get; set; } 

    }
}