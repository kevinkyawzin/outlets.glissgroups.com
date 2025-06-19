using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace BigMac.Models
{
    public class Configuration_m_Branches
    {
        public int id { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Key]
        [Display(Name = "Branch Code")]
        public string branchcode { get; set; }
        [Display(Name = "Branch Name")]
        public string branchname { get; set; }
        [Display(Name = "Prefix")]
        public string prefix { get; set; }
        [Display(Name = "Postfix")]
        public string postfix { get; set; }
        [Display(Name = "Tel.")]
        public string tel { get; set; }
        [Display(Name = "Fax")]
        public string fax { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal")]
        public string postalcode { get; set; }
        [Display(Name = "website")]
        public byte[] website { get; set; }
        [Display(Name = "facebook")]
        public byte[] facebook { get; set; }
        [Display(Name = "Twitter")]
        public byte[] twitter { get; set; }
        [Display(Name = "googlemap")]
        public byte[] googlemap { get; set; }
        //Added by ZawZO on 27 Aug 2015
        [Display(Name = "GST Reg No.")]
        public string gstregno { get; set; }
        [Display(Name = "GST Reg")]
        public int gstreg { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? lastmodifieddate { get; set; }

        public string DecryptedBranchName; 

        public string getBranchName()
        {
            //string dstr = "";
            //var MKey = "";
            //var IVKey = "";
            //var MKeyHex = "";
            //var IVKeyHex = "";
            //var EncryptHex = "";
            //cAESEncryption.AESDecryption(branchname, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr); 
            //return dstr;
            return cAESEncryption.getDecryptedString(branchname);
        }
    }
}
