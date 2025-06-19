using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_Holiday
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        
        [Display(Name = "Holiday Name")]
        public string holidayName { get; set; }

        [Display(Name = "Date")]
        public DateTime holidayDate { get; set; }

        [Display(Name = "Date")]
        public DateTime? createdDate { get; set; }

        [Display(Name = "Date")]
        public DateTime? modifiedDate { get; set; }

    }
}
