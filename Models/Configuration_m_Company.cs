using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace BigMac.Models
{
    public class Configuration_m_Company
    {
        public int id { get; set; }
        [Display(Name = "Code")]
        public string cocode { get; set; }
        [Display(Name = "Name.")]
        public string name { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal Code")]
        public string postalcode { get; set; }
        [Display(Name = "Tel.")]
        public string tel { get; set; }
        [Display(Name = "Fax")]
        public string fax { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "website")]
        public string website { get; set; }
        [Display(Name = "Company Registration No")]
        public string coregno { get; set; }
        [Display(Name = "GST Registration No")]
        public string gstregno { get; set; }
        [Display(Name = "Facebook")]
        public string facebook { get; set; }
        [Display(Name = "Twitter")]
        public string twitter { get; set; }
        [Display(Name = "Google Map")]
        public string googlemap { get; set; }
        [Display(Name = "Mail User Name")]
        public string mailusername { get; set; }
        [Display(Name = "Mail Password")]
        public string mailpassword { get; set; }
        [Display(Name = "Mail Server")]
        public string mailserver { get; set; }
        [Display(Name = "Mail Server Port")]
        public string mailserverport { get; set; }
        [Display(Name = "Mail Display Name")]
        public string maildisplayname { get; set; }
        [Display(Name = "Smtp SSL")]
        public int mailsmtpssl { get; set; }
        [Display(Name = "Company logo")]
        public string companylogo { get; set; }
        [Display(Name = "Create date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Branch Sales")]
        public int branchsale { get; set; }
        [Display(Name = "Gst Reg")]
        public int gstreg { get; set; }
    }
}
