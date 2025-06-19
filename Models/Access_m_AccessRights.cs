using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Access_m_AccessRights
    {
        //[ForeignKey("roldid")]
        public int id { get; set; }
        public int userid { get; set; }

        //[Display(Name = "User name")]
        //public string username { get; set; }
        public string resource { get; set; }
        [Display(Name = "Resource")]
        public int userresourceid { get; set; }
        [Display(Name = "Create")]
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
        [Display(Name = "Access Type")]
        public int accesstypeid { get; set; }               
        public DateTime? createdate { get; set; }
        //public int firstlogin { get; set; }

        [ForeignKey("userresourceid")]
        public virtual Configuration_m_Resource Resources { get; set; }
        [ForeignKey("accesstypeid")]
        public virtual Access_z_AccessTypes accessTypes { get; set; }

        public string username;
    }
}