using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class CusSup_m_CusFeedback
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "CusSup Id")]
        public int cussupid { get; set; }
        [Display(Name = "CusSup Name")]
        public int cussupname { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Feedback")]
        public string feedback { get; set; }
        [Display(Name = "Acknowledge by")]
        public int acknowledgeby { get; set; }
    }
}