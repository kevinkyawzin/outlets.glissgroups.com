using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class cussup_m_treatment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public int staffid { get; set; }
        public int createby { get; set; }
        [DisplayFormat(NullDisplayText = "", DataFormatString = " {0:dd/MM/yyyy}")]
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Resource Code")]
        public string resourcecode { get; set; }
        public DateTime? resourcedate { get; set; }
        public int resourcedetailid { get; set; }
        [Display(Name = "Member")]
        public int cussupid { get; set; }
        [Display(Name = "Member Name")]
        public string cussupname { get; set; }
        public string description { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }
        [Display(Name = "Product Code")]
        public string productcode { get; set; }
        [Display(Name = "Product")]
        public string productdesc { get; set; }
        [Display(Name = "Start Time")]
        public string starttime { get; set; }
        [Display(Name = "End Time")]
        public string endtime { get; set; }
        public string remarks2 { get; set; }
        public string remarks3 { get; set; }
        public string remarks4 { get; set; }
        public string type { get; set; }



    }

    public class cussup_m_treatment_Ops
    {
        public int id { get; set; }
        public int staffid { get; set; }
        public int createby { get; set; }
        public string createbyname { get; set; }
        public DateTime? createdate { get; set; }
        public int resourcedetailid { get; set; }
        public string resourcecode { get; set; }
        public string description { get; set; }
        public string remarks2 { get; set; }
        public string remarks3 { get; set; }
        public string remarks4 { get; set; }
        public string type { get; set; }
        public string keyid { get; set; }
        public string statusfield { get; set; }
    }
}