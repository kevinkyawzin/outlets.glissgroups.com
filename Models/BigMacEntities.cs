using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigMac.Models
{
    public class BigMacEntities : DbContext
    {
        //public BigMacEntities() : base("worldso1_info121") { }
       

        public DbSet<Access_m_Users> Users { get; set; }
        public DbSet<Access_z_Roles> Roles { get; set; }
        public DbSet<Access_z_Status> UserStauts { get; set; }

        public DbSet<Common_m_Staff> Staffs { get; set; }
        public DbSet<Common_m_StaffDepartment> Department { get; set; }
        public DbSet<Common_m_BranchDepartment> BranchDepartment { get; set; }
        public DbSet<Common_z_StaffPosition> Positions { get; set; }
        public DbSet<Common_z_StaffType> Types { get; set; }
        public DbSet<Common_m_StaffBranch> BranchStaff { get; set; }
        public DbSet<Common_m_Message> Message { get; set; }

        public DbSet<Access_z_AccessTypes> AccessTypes { get; set; }
        public DbSet<Configuration_m_Resource> Resources { get; set; }
        public DbSet<Access_m_AccessRights> AccessRights { get; set; }
        public DbSet<Access_m_RoleResources> RoleAccessRights { get; set; }

        //Campaign
        public DbSet<Campaign_z_Status> CampaignStatus { get; set; }
        public DbSet<Campaign_m_Campaign> Campaigns { get; set; }
        public DbSet<campaign_m_branchactivitytarget> CampaignBranchAcitivityTarget { get; set; }
        public DbSet<campaign_m_branchsalestarget> CampaignBranchSalesTarget { get; set; }
        public DbSet<Campaign_m_Attendance_Staff> CampaignAttendanceStaffs { get; set; }
        public DbSet<Campaign_z_Groups> Groups { get; set; }
        public DbSet<Campaign_z_BranchesGroups> BranchGroupLinks { get; set; }
        public DbSet<Campaign_m_Staffs> CampaignBranchStaff { get; set; }
        public DbSet<Campaign_z_Category> Categories { get; set; }
        public DbSet<Campaign_m_Category> CampaignCategories { get; set; }
        public DbSet<Campaign_m_CampaignDetails> CampaignDetails { get; set; }
        public DbSet<Campaign_z_Time> CampaignTimes { get; set; }
        public DbSet<Campaign_z_CategoryType> CategoryTypes { get; set; }
        public DbSet<campaign_z_staffdaytype> StaffDayType { get; set; }

        public DbSet<Configuration_m_Branches> Branches { get; set; }
        public DbSet<Configuration_m_Company> Companies { get; set; }
        public DbSet<Configuration_m_Default> ConfigDefault { get; set; }
        //Added by ZawZO on 8 Jan 2016
        public DbSet<Common_m_BranchAsset> BranchAsset { get; set; }

        public DbSet<Common_z_Status> CommonStatus { get; set; }
        public DbSet<Common_z_Currency> Currency { get; set; }
        public DbSet<Common_z_PaymentStatus> PaymentStatus { get; set; }
        public DbSet<Common_z_PaymentMode> PaymentModes { get; set; }

        //Message
        public DbSet<Message_m_Chats> MessageChats { get; set; }
        public DbSet<Message_m_ChatsUsers> MessageUsers { get; set; }
        public DbSet<Message_m_ChatsGroups> MessageChatGroups { get; set; }

        //CusSup
        public DbSet<CusSup_m_CusSup> CusSup { get; set; }
        public DbSet<CusSup_z_CusSupType> CusSupType { get; set; }
        public DbSet<CusSup_m_Cards> MemberCard { get; set; }
        public DbSet<CusSup_z_CardType> CardType { get; set; }
        public DbSet<CusSup_z_Status> CusSupStatus { get; set; }
        public DbSet<CusSup_z_Nationality> Nationality { get; set; }
        public DbSet<CusSup_z_Race> Race { get; set; }
        public DbSet<CusSup_z_NRICType> NRICType { get; set; }
        public DbSet<CusSup_m_CusRedemption> CusSupRedemption { get; set; }
        public DbSet<CusSup_m_CusFeedback> CusSupFeedback { get; set; }
        public DbSet<CusSup_m_Logs> CusSupLogs { get; set; }
        public DbSet<CusSup_m_Citibella> CusSupCiti { get; set; }
        public DbSet<CusSup_z_CitibellaConversionRate> CitiRate { get; set; }
        public DbSet<CusSup_m_CitibellaDupCustCode> CitiCustCode { get; set; }
        public DbSet<CusSup_m_CitibellaLiability> CitiLiability { get; set; }
        public DbSet<cussup_m_treatment> Treatments { get; set; }
        //product
        public DbSet<Common_z_UnitofMeasurment> UOM { get; set; }
        public DbSet<Product_z_Status> productStatus { get; set; }
        public DbSet<Product_z_Category> productCategory { get; set; }
        public DbSet<Product_z_CategorySub> productSubCategory { get; set; }
        public DbSet<Product_z_ProductBrand> productBrand { get; set; }
        public DbSet<Product_z_ProductRangesNSeries> productRNS { get; set; }
        public DbSet<Product_m_Product> products { get; set; }
        public DbSet<Product_m_ProductImage> productImages { get; set; }
        public DbSet<Product_m_ProductAltCode> productALTCode { get; set; }
        public DbSet<Product_m_ProductBundles> productBundle { get; set; }
        public DbSet<Product_z_ProductPriceType> pricetype { get; set; }
        public DbSet<Product_m_ProductPrice> productprices { get; set; }
        public DbSet<Product_m_ProductPromotion> productpromotions { get; set; }
        public DbSet<Product_m_Citibella> productCiti { get; set; }

        //Appointment
        public DbSet<Appointment_z_AppointmentType> AppointmentType { get; set; }
        public DbSet<Appointment_m_Appointment> Appointments { get; set; }

        //pruchase
        public DbSet<Purchase_m_Purchase> purchase { get; set; }
        public DbSet<Purchase_m_Purchase_Items> purchaseItems { get; set; }
        public DbSet<Purchase_m_Payment> purchasePayment { get; set; }

        //Sales
        public DbSet<Invoice_m_Invoice> sales { get; set; }
        public DbSet<Invoice_m_Invoice_Items> saleItems { get; set; }
        public DbSet<Invoice_m_Payment> invoicePayment { get; set; }
        public DbSet<Invoice_m_Invoice_SalesStaff> saleStaffs { get; set; }
        public DbSet<Invoice_z_discount> salesDiscount{ get; set; }
        public DbSet<Salesorder_m_salesorder> saleorders { get; set; }
        public DbSet<salesorder_m_detail> saleordersItems { get; set; }
        //Inventory
        //public DbSet<Stock_m_StockReceivedDetail> stockReceivedDetail { get; set; }
        public DbSet<Stock_m_StockReceived> stockReceived { get; set; }
        public DbSet<Stock_m_StockReceived_Items> stockReceivedItems { get; set; }
        public DbSet<stock_m_stockadjustment> stockAdjustment { get; set; }
        public DbSet<stock_m_stockadjustment_items> stockAdjustmentItems { get; set; }
        public DbSet<stock_m_stocktransfer> stockTransfer { get; set; }
        public DbSet<stock_m_stocktransfer_items> stockTransferItems { get; set; }
        public DbSet<stock_m_stockmovement> StockMovement { get; set; }
        //Logs
        public DbSet<Logs_m_Logs> logs { get; set; }
        public DbSet<Logs_m_ErrorLogs> Errorlogs { get; set; }
        public DbSet<Logs_z_LogType> logType { get; set; }
        //Attendance
        //Added by ZawZO on 22 May 2015
        public DbSet<attendance_m_attendancedetail> attnds{ get; set; }
        //Added by ZawZO on 22 May 2015
        public DbSet<Common_z_StaffSkills> staffskills { get; set; }
        //General
        public DbSet<rptEncryptValues> encryptdata { get; set; }
        //Added by ZawZO on 16 Feb 2016
        public DbSet<Mileage_m_rewards> rewards { get; set; }
        //Added by ZawZO 18 Feb 2016
        public DbSet<Common_m_Partnership> Partner { get; set; }

        //Added by Jayson Ocampo 02 Feb 2017
        public DbSet<Schedule_m_Leave> leave { get; set; }
        public DbSet<Schedule_m_Holiday> holiday { get; set; }
        public DbSet<Schedule_m_Staff> staffSchedule { get; set; }
        public DbSet<Schedule_m_Appointment> appointment { get; set; }
        public DbSet<Schedule_m_Work> workSchedule { get; set; }
        public DbSet<Schedule_m_StaffWork> staffWorkSchedule { get; set; }

    }
}
