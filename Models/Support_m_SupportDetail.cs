using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Support_m_SupportDetail
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "supportid")]
        public int supportid { get; set; }
        [Display(Name = "User")]
        public int userid { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
    }
}