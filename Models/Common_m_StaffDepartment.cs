using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Common_m_StaffDepartment
    {
        public int id { get; set; }
        public string departmentname { get; set; }
        public string description { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? lastmodifieddate { get; set; }
 
    }

}