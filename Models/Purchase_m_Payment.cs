using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Purchase_m_Payment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Purchase ID")]
        public int purchaseid { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        [Display(Name = "Payment Mode")]
        public string paymentmode { get; set; }
        [Display(Name = "Received Amount")]
        public double amountrecd { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [Display(Name = "Payment By")]
        public int paymentid { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Cheque/Credit Card")]
        public string chequecreditcard { get; set; }
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }

        [ForeignKey("createid")]
        public virtual Common_m_Staff Create { get; set; }
    }
}