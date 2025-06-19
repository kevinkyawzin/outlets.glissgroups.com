using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_Appointment
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        
        [Display(Name = "Customer Name")]
        public string customername { get; set; }

        [Display(Name = "Customer Code")]
        public string customercode { get; set; }

        [Display(Name = "Mobile")]
        public string mobile { get; set; }

        [Display(Name = "NRIC")]
        public string nric { get; set; }

        [Display(Name = "Staff ID")]
        public int staffid { get; set; }

        [Display(Name = "Branch Asset ID")]
        public int branchassetid { get; set; }

        [Display(Name = "Appointment Date")]
        public DateTime appointmentdate { get; set; }

        [Display(Name = "Start Time")]
        public string starttime { get; set; }

        [Display(Name = "End Time")]
        public string endtime { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Channel")]
        public int channel { get; set; }

        [Display(Name = "Booked By")]
        public int bookedby { get; set; }

        [Display(Name = "Source")]
        public int source { get; set; }

        [Display(Name = "Queue Number")]
        public string queuenumber { get; set; }

        [Display(Name = "Sales Order Number")]
        public string sonumber { get; set; }

        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }

        [Display(Name = "Created Date")]
        public DateTime createddate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime modifieddate { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "DepartmentId")]
        public int departmentid { get; set; }

        [Display(Name = "filename")]
        public string filename { get; set; }

        [Display(Name = "signature")]
        public byte[] signature { get; set; }

        [Display(Name = "nationality")]
        public string nationality { get; set; }

        [Display(Name = "transactiontype")]
        public string transactiontype { get; set; }

    }
}
