using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class CusSup_m_Logs
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Member")]
        public int cussupid { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Check In")]
        public DateTime? Checkin { get; set; }
        [Display(Name = "Check Out")]
        public DateTime? Checkout { get; set; }
    }
}