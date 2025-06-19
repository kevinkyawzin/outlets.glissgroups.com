using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_Work
    {
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Staff ID")]
        public int staffid { get; set; }

        [Display(Name = "description")]
        public string description { get; set; }

        [Display(Name = "Start Time")]
        public string starttime { get; set; }

        [Display(Name = "End Time")]
        public string endtime { get; set; }

        [Display(Name = "Date From")]
        public DateTime datefrom { get; set; }

        [Display(Name = "Date To")]
        public DateTime dateto { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? createddate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? lastmodifieddate { get; set; }

        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }

        [Display(Name = "Days")]
        public string days { get; set; }

        [Display(Name = "DepartmentID")]
        public int departmentid { get; set; }

        [Display(Name = "type")]
        public string type { get; set; }

    }
}
