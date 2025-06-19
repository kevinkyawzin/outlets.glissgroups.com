using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Mileage_m_rewards
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "CussupId")]
        public int cussupid { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "NRIC")]
        public string nric { get; set; }
        [Display(Name = "Receipt No")]
        public string receiptno { get; set; }
        [Display(Name = "Receipt Date")]
        public DateTime? receiptdate { get; set; }
        [Display(Name = "Amount")]
        public double? amount { get; set; }
        [Display(Name = "Reward bonus")]
        public double? rewardbonus{ get; set; }
        [Display(Name = "Parner ID")]
        public int partnerid { get; set; }
        [Display(Name = "Issued By")]
        public string issuedby { get; set; }
    }

}
