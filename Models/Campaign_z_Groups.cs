using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BigMac.Models
{
    public class Campaign_z_Groups
    {
        public int id { get; set; }
        public string group { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Create By")]
        public int createdbyid { get; set; }
    }
}