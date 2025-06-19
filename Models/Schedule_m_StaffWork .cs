using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Schedule_m_StaffWork
    {

        public int id { get; set; }

        public int staffid { get; set; }

        public int workid { get; set; }

        public DateTime startdate { get; set; }

        public DateTime enddate { get; set; }

        public DateTime? createddate { get; set; }

        public DateTime? lastmodifieddate { get; set; }

        public string branchcode { get; set; }

        public string type { get; set; }

        public int departmentid { get; set; }

    }
}
