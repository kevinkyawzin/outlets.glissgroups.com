using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_Staff
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        
        [Display(Name = "Staff ID")]
        public int staffId { get; set; }

        [Display(Name = "Leave ID")]
        public int leaveId { get; set; }

        [Display(Name = "Date")]
        public DateTime startDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? createdDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? modifiedDate { get; set; }

        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "DepartmentID")]
        public int departmentid { get; set; }

    }
}
