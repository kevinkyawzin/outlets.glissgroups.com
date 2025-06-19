using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class stock_m_stockadjustment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public DateTime? resourcedate { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Ref. no:")]
        public string refno { get; set; }
        [Display(Name = "Reason")]
        public string reason { get; set; }
        [Display(Name = "Post")]
        public int? post { get; set; }
        [Display(Name = "Post ID")]
        public int? postid { get; set; }
        [Display(Name = "Post Date")]
        public DateTime? postdate { get; set; }
        [Display(Name = "Currency")]
        public string currency { get; set; }
        [Display(Name = "Exchange Rate")]
        public double? exchangerate { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Sub Total")]
        public double? total_subtotal { get; set; }
        [Display(Name = "Tax")]
        public double? total_salestax { get; set; }
        [Display(Name = "Total")]
        public double? total_total { get; set; }
        public string uniquecode { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }
        [ForeignKey("createid")]
        public virtual Common_m_Staff Create { get; set; }

        public IEnumerable<stock_m_stockadjustment_items> items { get; set; }
    }
}
