using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Common_m_BranchDepartment
    {
        public int id { get; set; }
        public int departmentid { get; set; }
        public string branchcode { get; set; }
    }

}