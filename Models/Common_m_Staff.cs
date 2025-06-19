using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations; 

namespace BigMac.Models
{
    public class Common_m_Staff
    {
        public int id { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
         [Display(Name = "Address")]
        public string address { get; set; } 
        [Display(Name = "Postal Code")]
        public string postalcode { get; set; }
        [Display(Name = "NRIC")]
        public string nric { get; set; }
        [Display(Name = "Tel.")]
        public string tel { get; set; }
        [Display(Name = "Fax")]
        public string fax { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "Position")]
        public string position { get; set; }
        [Display(Name = "Staff Type")]
        public string stafftype { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        public int userid { get; set; }
        //Added by ZawZO on 13 May 2015
        [Display(Name = "Discount Percent")]
        public int maxdiscpc { get; set; }
        //public virtual ICollection<Common_z_StaffType> Types { get; set; }
        //public static IEnumerable<Common_z_StaffType> StaffTypeOptions;
        //public static IEnumerable<Common_z_StaffPosition> PositionOptions;//= new List<Common_z_StaffPosition> {  };// { get; set; }
        
    }

}
