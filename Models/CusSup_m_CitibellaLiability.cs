using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 

namespace BigMac.Models
{
    public class CusSup_m_CitibellaLiability
    {
        public int id { get; set; }
        public string Branch { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string ReceiptNo { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustContact { get; set; }
        public double Total { get; set; }
        public double Done { get; set; }
        //Added by ZawZO on 13 Jul 2016
        public double Cancel { get; set; }
        public double Open { get; set; }

        public double UnitPrice { get; set; }
        public double TotalAmt { get; set; }
        public double OutstandingAmt { get; set; }
        public double Liability { get; set; }
        public double FromAmount { get; set; }
        public double ToCitiDollar { get; set; }
        public double ToRewardDollar { get; set; }
        public double AdmissionFees { get; set; }
        public DateTime? transferdate { get; set; }
        public DateTime? transactiondate { get; set; }
        public string transfernumber { get; set; }
        public string staff  { get; set; }
        public string Status { get; set; }
        public int? memberid { get; set; }
        public int? rateid { get; set; }
    }
    //Added by ZawZO on 12 Jul 2016
    public class CitibellaMemberLiability
    {
        public int LiabilityID { get; set; }
        public string BranchCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductDescription2 { get; set; }
        public string ReceiptNo { get; set; }
        public string CustID { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string CustContact { get; set; }
        public double Total { get; set; }
        public double Done { get; set; }
        public double OpenBal { get; set; }
        public double UnitPrice { get; set; }
        public double TotalAmt { get; set; }
        public double OutstandingAmt { get; set; }
        public double Liability { get; set; }
        public string TransferStatus { get; set; }
        public string TransferDate { get; set; }
        public string TransactionDate { get; set; }
        
    }
}
