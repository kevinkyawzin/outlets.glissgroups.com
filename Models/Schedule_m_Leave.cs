using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_Leave
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        
        [Display(Name = "Leave Name")]
        public string leaveName { get; set; }

        [Display(Name = "Whole Day")]
        public int isWholeDay { get; set; }
        
        [Display(Name = "Start")]
        public string start { get; set; }
        
        [Display(Name = "End")]
        public string end { get; set; }
        
        [Display(Name = "Background Color")]
        public string backgroundColor { get; set; }

        [Display(Name = "Text Color")]
        public string textColor { get; set; }

        [Display(Name = "Date")]
        public DateTime createdDate { get; set; }

        [Display(Name = "Date")]
        public DateTime modifiedDate { get; set; }
        
    }
}
