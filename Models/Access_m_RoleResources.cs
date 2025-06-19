using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Access_m_RoleResources
    {
        public int id { get; set; }
        public int roleid { get; set; }

        //[Display(Name = "User name")]
        //public string username { get; set; }
        [Display(Name = "Resource")]
        public int resourceid { get; set; }
        public string resource { get; set; }
        public DateTime? createdate { get; set; }
        //public int firstlogin { get; set; }
        public DateTime? lastmodifieddate { get; set; }
        //public int firstlogin { get; set; }   
        public bool create { get; set; }
        [Display(Name = "Read")]
        public bool read { get; set; }
        [Display(Name = "Update")]
        public bool update { get; set; }
        [Display(Name = "Delete")]
        public bool delete { get; set; }
        [Display(Name = "Void")]
        public bool voidField { get; set; }
        [Display(Name = "UnVoid")]
        public bool unvoid { get; set; }
        [Display(Name = "Print")]
        public bool print { get; set; }
        
        [ForeignKey("resourceid")]
        public virtual Configuration_m_Resource Resources { get; set; }
    }
}