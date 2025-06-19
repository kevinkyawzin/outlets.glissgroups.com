using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Campaign_m_Campaign
    {
         [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Game Description")]
        public string gamedesc { get; set; }
         [Display(Name = "Create By")]
        public int createdbyid { get; set; }
         [Display(Name = "Status")]
        public string status { get; set; }
         [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
         [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
    }
}