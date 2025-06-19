using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class Access_m_Users
    {
         [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Role")]
         public int roldid { get; set; }

         [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string username { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

         [Display(Name = "E-Mail")]
        public string email { get; set; }

         [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }

         [Display(Name = "Status")]
        public string status { get; set; }

         [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }

         [Display(Name = "First Login")]
        public int firstlogin { get; set; }

         public string cocode { get; set; }

        //public string role="";
        [ForeignKey("roldid")]
        public virtual Access_z_Roles Role { get; set; }
        
        public IEnumerable<Access_m_AccessRights> accessRights { get; set; }
        public int staffid = 0;
        //public IList<Access_m_AccessRights> accessRights { get; set; }
        //public virtual int staffid { get; set; }
        //public virtual string staffname { get;set;}
        //[Display(Name = "Remember on this computer")]
        //public bool RememberMe { get; set; }
        
        //public bool IsValid(string _username, string _password)
        //{
        //    Boolean flag = false;

        //    return flag;
        //}
    }
}