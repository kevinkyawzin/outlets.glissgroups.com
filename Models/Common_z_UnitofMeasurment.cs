using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Common_z_UnitofMeasurment
    {
        public int id { get; set; }
        public string UOM { get; set; }        
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }     
    }
}