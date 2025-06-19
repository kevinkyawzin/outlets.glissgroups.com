using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace BigMac.Models
{
    //Added by ZawZO on 4 Feb 2016
    public class Common_m_Partnership
    {
        public int id { get; set; }
        [Display(Name = "Inhouse Code")]
        public string inhousecode { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "mobile")]
        public string mobile { get; set; }
        [Display(Name = "Fax")]
        public string fax { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal Code")]
        public string postalcode { get; set; }
        [Display(Name = "Contact Person")]
        public string contactperson { get; set; }
        [Display(Name = "Contact Person Post")]
        public string contactpersonpost { get; set; }
        [Display(Name = "Award Bonus")]
        public double? awardbonus { get; set; }
        [Display(Name = "Invoice Amount")]
        public double? invoiceamount { get; set; }
        [Display(Name = "Max Invoice Amount")]
        public double? maxinvoiceamount { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastupdatedate { get; set; }
        public int? mainuserid { get; set; }
        public int? otheruser1 { get; set; }
        public int? otheruser2 { get; set; }
    }
}
