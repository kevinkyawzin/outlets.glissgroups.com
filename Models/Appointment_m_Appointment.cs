using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Appointment_m_Appointment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Type")]
        public string appointmenttype { get; set; }
        [Display(Name = "Customer/Supplier")]
        public int cussupid { get; set; }
        [Display(Name = "Name")]
        public string cussupname { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "Create By")]
        public int createid { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime? appointmentdate { get; set; }
        [DataType(DataType.Time)]        
        [Display(Name = "Time")]
        public TimeSpan? appointmenttime { get; set; }
        [Display(Name = "Staff")]
        public int staffid { get; set; }
        [Display(Name = "Product")]
        public int productid { get; set; }        
        public string productdesc { get; set; }
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [ForeignKey("staffid")]
        public virtual Common_m_Staff Staff { get; set; }
    }
}