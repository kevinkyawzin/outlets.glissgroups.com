using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigMac.Models
{
    public class CusSup_m_CusSup
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, HtmlEncode = false, NullDisplayText = "")]
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Inhouse Code")]
        public string inhousecode { get; set; }
        [Display(Name = "Name")]
        public string cussupname { get; set; }
        [Display(Name = "Type")]
        public string cussuptype { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal Code")]
        public string postalcode { get; set; }
        [Display(Name = "NRIC")]
        public string nric { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, HtmlEncode = false, NullDisplayText = "")]
        public DateTime? dateofbirth { get; set; }
        [Display(Name = "Occupation")]
        public string occupation { get; set; }
        [Display(Name = "Telephone")]
        public string Tel { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "User ID")]
        public string userid { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Photo")]
        public string photo { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Race")]
        public string race { get; set; }
        [Display(Name = "Nationality")]
        public string nationality { get; set; }
        [Display(Name = "NRIC Type")]
        public string nrictype { get; set; }

        [Display(Name = "Department")]
        public string departmentname { get; set; }

        [Display(Name = "Department ID")]
        public string departmentid { get; set; }

        [Display(Name = "E-Mail?")]
        public int canemail { get; set; }
        [Display(Name = "SMS?")]
        public int cansms { get; set; }
        [Display(Name = "Call?")]
        public int cancall { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }
        public IEnumerable<CusSup_m_Cards> cards { get; set; }

        public double CBalance = 0;
        public double BBalance = 0;
        public double CReserved = 0;
        public double BReserved = 0;
        public double CAvailable = 0;
        public double BAvailable = 0;

    }

    public class CusSup_m_CusSupdtl
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        [Display(Name = "Create Date")]
        public DateTime? createdate { get; set; }
        [Display(Name = "Last Modified Date")]
        public DateTime? lastmodifieddate { get; set; }
        [Display(Name = "Inhouse Code")]
        public string inhousecode { get; set; }
        [Display(Name = "Name")]
        public string cussupname { get; set; }
        [Display(Name = "Type")]
        public string cussuptype { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Postal Code")]
        public string postalcode { get; set; }
        [Display(Name = "NRIC")]
        public string nric { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? dateofbirth { get; set; }
        [Display(Name = "Occupation")]
        public string occupation { get; set; }
        [Display(Name = "Telephone")]
        public string Tel { get; set; }
        [Display(Name = "Mobile")]
        public string mobile { get; set; }
        [Display(Name = "E-Mail")]
        public string email { get; set; }
        [Display(Name = "User ID")]
        public string userid { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }
        [Display(Name = "Branch")]
        public string branchcode { get; set; }
        [Display(Name = "Company")]
        public string cocode { get; set; }
        [Display(Name = "Photo")]
        public string photo { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Race")]
        public string race { get; set; }
        [Display(Name = "Nationality")]
        public string nationality { get; set; }
        [Display(Name = "NRIC Type")]
        public string nrictype { get; set; }
        [Display(Name = "E-Mail?")]
        public int canemail { get; set; }
        [Display(Name = "SMS?")]
        public int cansms { get; set; }
        [Display(Name = "Call?")]
        public int cancall { get; set; }

        [Display(Name = "Department ID")]
        public string departmentid { get; set; }
        [Display(Name = "Department Name")]
        public string departmentname { get; set; }

        [ForeignKey("branchcode")]
        public virtual Configuration_m_Branches Branch { get; set; }
        public IEnumerable<CusSup_m_Cards> cards { get; set; }

        public double CBalance { get; set; }
        public double BBalance { get; set; }
        public double CReserved { get; set; }
        public double BReserved { get; set; }
        public double CAvailable { get; set; }
        public double BAvailable { get; set; }
        public string cardno { get; set; }
        public int TotalPages;
    }

    public class CusSup_m_Citibella
    {
        [Display(Name = "ID")]
        public int id { get; set; }
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, HtmlEncode = false, NullDisplayText = "")]
        [Display(Name = "Join Date")]
        public string Cust_JoinDate { get; set; }
        [Display(Name = "Customer No")]
        public string Cust_No { get; set; }
        [Display(Name = "Customer Code")]
        public string Cust_code { get; set; }
        [Display(Name = "Name")]
        public string Cust_name { get; set; }
        [Display(Name = "Address")]
        public string Cust_address { get; set; }
        [Display(Name = "Address1")]
        public string Cust_address1 { get; set; }
        [Display(Name = "PostalCode")]
        public string Cust_PostCode { get; set; }
        [Display(Name = "Phone1")]
        public string Cust_phone1 { get; set; }
        [Display(Name = "Phone2")]
        public string Cust_phone2 { get; set; }
        [Display(Name = "E-Mail")]
        public string Cust_email { get; set; }
        [Display(Name = "Remark")]
        public string Cust_Remark { get; set; }        
        [Display(Name = "NRIC")]
        public string Cust_nric { get; set; }
        [Display(Name = "Date of Birth")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true, HtmlEncode = false, NullDisplayText = "")]
        public string Cust_DOB { get; set; }
        [Display(Name = "Site_Code")]
        public string Site_Code { get; set; }
        [Display(Name = "Saluation")]
        public string Saluation { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Race")]
        public string race { get; set; }
        [Display(Name = "Nationality")]
        public string nationality { get; set; }
        [Display(Name = "NRIC Type")]
        public string NRICType { get; set; }
        [Display(Name = "EMail")]
        public string Email { get; set; }
        [Display(Name = "SMS")]
        public string SMS { get; set; }
        [Display(Name = "Call")]
        public string Call { get; set; }
  
        public string Status { get; set; }
        public int? memberid { get; set; }        

        public double CBalance;
        public double BBalance;
        public double CReserved;
        public double BReserved;
        public double CAvailable;
        public double BAvailable;

        public IEnumerable<CusSup_m_CitibellaDupCustCode> CustCode { get; set; }
        public IEnumerable<CusSup_m_CitibellaLiability> Liability { get; set; }

    }

}