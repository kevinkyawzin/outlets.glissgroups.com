using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Support_m_Support
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "User")]
        public int userid { get; set; }
        [Display(Name = "Type")]
        public string supporttype { get; set; }
        [Display(Name = "Subject")]
        public string subject { get; set; }
        [Display(Name = "Priority")]
        public string priority { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Remark")]
        public string remarks { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

        public IEnumerable<Support_m_SupportDetail> items { get; set; }

    }
}