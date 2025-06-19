using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using System.Dynamic;
using MySql.Data.MySqlClient;
using System.Web.Configuration;

namespace BigMac.Controllers
{
    public class SalesController : Controller
    {
        //
        // GET: /Sales/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult TopUpIndex()
        {
            return View(db.sales.Include("Branch").Include("Create").OrderBy(x => new { x.status, x.id }).ToList());
        }

        public ActionResult InvoiceIndex()
        {
            //return View(db.sales.Include("Branch").Include("Create").OrderBy(x => new { x.status, x.id }).ToList());
            return View(db.sales.Include("Create").OrderBy(x => new { x.status, x.id }).ToList());
        }

        public JsonResult getInvoiceTreatmentList(string resourcecode = "")
        {
            try
            {
                var tList = db.Treatments.Where(x => x.resourcecode == resourcecode).Join(db.Staffs, t => t.staffid, staff => staff.id, (t, staff) => new { Treatments = t, Staffs = staff }).Select(x => new { x.Treatments.id, x.Treatments.createdate, x.Treatments.createby, createbyname = x.Staffs.name, x.Treatments.resourcedetailid, x.Treatments.description, keyid = "0", statusfield = "E", x.Treatments.type }).ToList();
                return Json(tList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getInvoiceSalesStaff(int invid = 0)
        {
            try
            {
                var sList = db.saleStaffs.Where(x => x.invoiceid == invid).ToList();
                return Json(sList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getReserveList(int memberid = 0, string rcode = "Redeem")
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    //MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                    //MySqlCommand cmd = new MySqlCommand("getMemberReservedList", conn);
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new MySqlParameter("memberid", memberid));
                    //cmd.Connection.Open();
                    //MySqlDataReader r = cmd.ExecuteReader();
                    //List<dynamic> pList = new List<dynamic>();
                    //while (r.Read())
                    //{
                    //    dynamic obj = new ExpandoObject();
                    //    obj.id = r.Getint("id");
                    //    obj.resourcecode = r.GetString("resourcecode");
                    //    obj.branchcode = r.GetString("branchcode");
                    //    obj.createid = r.Getint("createid");
                    //    obj.productid = r.Getint("productid");
                    //    obj.productcode = r.GetString("productcode");
                    //    obj.productdesc = r.GetString("productdesc");
                    //    obj.qty = r.Getint("qty");
                    //    obj.uom = r.GetString("uom");
                    //    obj.redeemdollars = r.GetDouble("redeemdollars");
                    //    obj.redeembonus = r.GetDouble("redeembonus");
                    //    obj.awarddollars = r.GetDouble("awarddollars");
                    //    obj.awardbonus = r.GetDouble("awardbonus");
                    //    pList.Add(obj);
                    //}

                    var pList = context.Database.SqlQuery<ReservedList>("call getMemberReservedList(" + memberid + ",'" + rcode + "')").ToList(); //.Select(x => new { x.id }).ToList();
                    //expandoObj.resourcecode, expandoObj.branchcode, expandoObj.createid, expandoObj.productid, expandoObj.productcode, expandoObj.productdesc, expandoObj.qty, expandoObj.uom, expandoObj.redeemdollars, expandoObj.redeembonus, expandoObj.awarddollars, expandoObj.awardbonus }).ToList();
                    return Json(pList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("FAIL", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getSalesKits()
        {
            try
            {
                //Product_m_Product ptmp = new Product_m_Product();
                //dynamic ptmp = new ExpandoObject();
                //ptmp.id = "-10";
                //ptmp.desc = "----- Selected Service/Product -----"; 
                //var PList = db.products.Where(x => x.category == "SALESKIT" && x.status == "Active").Select(x => new { x.id, x.desc }).ToList();
                //if(Session["userrole"].ToString()=="PARTNERS OUTLET")
                //{  
                string cocode = Session["cocode"].ToString();
                var PList = db.products.Where(x => x.category == "SALESKIT" && x.status == "Active" && x.cocode == cocode).Select(x => new { x.id, x.desc }).ToList();
                //}

                return Json(PList.OrderBy(x => x.desc), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public JsonResult getINVRedeemDetail(int id)
        {
            try
            {
                var PList = db.saleItems.Where(x => x.invoiceid == id).ToList();
                return Json(PList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        // Kyaw
        public JsonResult getINVRedeemDetailWithPaging(int pageno = 0, int id = 0)
        {
            try
            {
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                //ICollection<Invoice_m_Invoice_Itemsdtl> SList = (ICollection<Invoice_m_Invoice_Itemsdtl>)db.saleItems.Where(x => x.invoiceid == id).ToList();
                ICollection<Invoice_m_Invoice_Items> SList = db.saleItems.Where(x => x.invoiceid == id).ToList();
                var paginatedQSList = new PaginatedList<Invoice_m_Invoice_Items>(SList.AsQueryable(), pageno, pagesize);
                var paginatedSList = paginatedQSList.ToList();

                 Invoice_m_Invoice inv =   db.sales.Where(x => x.id == id ).FirstOrDefault();
                CusSup_m_CusRedemption red;

                int productId = 0;
                // Kyaw

                 if (paginatedSList != null)
                 {

                     for (int i = 0; i < paginatedSList.Count; i++)
                     {
                         if (paginatedSList.ElementAt(i).redemptiontype == "SQ")
                         {
                             productId = paginatedSList.ElementAt(i).productid;

                             red = db.CusSupRedemption.Where(x => x.cussupid == inv.cussupid && x.productid == productId && x.redemptiontype == "SQ")
                                 .OrderByDescending(x => x.id)
                                 .FirstOrDefault();

                             if (red == null)
                                 paginatedSList.ElementAt(i).balance = 0;
                             else
                                 paginatedSList.ElementAt(i).balance = red.balance;
                         }
                         else {
                             paginatedSList.ElementAt(i).balance = 0;
                         }
                     }

                    if (paginatedSList.Count > 0)
                        paginatedSList.ElementAt(0).TotalPages = paginatedQSList.TotalPages;
                    //totalpages = paginatedQPList.TotalPages;
                }

                return Json(paginatedSList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public ActionResult Redeem(int id = 0, int mid = 0, Int32 cardno = 0, string rcode = "REDEEM", string mcode = "", int soid = 0)
        {
            try
            {
                //ViewBag.StaffOptions = db.Staffs.Select(x=> new {x.id,x.name }).ToList();
                string bcode = Session["branchcode"].ToString();
                ViewBag.StaffOptions = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();
                if (id == 0 && mid == 0 && cardno == 0) cardno = Convert.ToInt32(Session["cardno"]);
                ViewBag.DiscountOptions = db.salesDiscount.OrderBy(x => x.value).ToList();
                ICollection<CusSup_m_CusSupdtl> mList;
                ViewBag.cardno = cardno;
                ViewBag.RCode = rcode;
                ViewBag.Role = Session["userrole"].ToString();
                ViewBag.CardNo = "";
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                //Added by ZawZO on 11 Nov 2015
                string CoCode = Session["cocode"].ToString();
                Configuration_m_Company co = db.Companies.Where(x => x.cocode == CoCode).FirstOrDefault();
                ViewBag.CoRegNo = cAESEncryption.getDecryptedString(co.coregno);

                ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(co.gstregno);
                Configuration_m_Branches br = db.Branches.Where(x => x.branchcode == bcode).FirstOrDefault();
                ViewBag.CoName = cAESEncryption.getDecryptedString(br.branchname);
                if (br.address != null)
                {
                    if (br.address == "")
                    {
                        ViewBag.CoAddress = "-";
                    }
                    else
                    {
                        ViewBag.CoAddress = cAESEncryption.getDecryptedString(br.address);
                    }
                }
                else
                {
                    ViewBag.CoAddress = "-";
                }
                if (br.tel != null)
                {
                    if (br.tel == "")
                    {
                        ViewBag.Tel = "-";
                    }
                    else
                    {
                        ViewBag.Tel = cAESEncryption.getDecryptedString(br.tel);
                    }
                }
                else
                {
                    ViewBag.Tel = "-";
                }
                if (br.fax != null)
                {
                    if (br.fax == "")
                    {
                        ViewBag.Fax = "-";
                    }
                    else
                    {
                        ViewBag.Fax = cAESEncryption.getDecryptedString(br.fax);
                    }
                }
                else
                {
                    ViewBag.Fax = "-";
                }
                if (br.email != null)
                {
                    if (br.email == "")
                    {
                        ViewBag.Email = "-";
                    }
                    else
                    {
                        ViewBag.Email = cAESEncryption.getDecryptedString(br.email);
                    }
                }
                else
                {
                    ViewBag.Email = "-";
                }
                int intGSTReg = br.gstreg;
                if (intGSTReg == 1)
                {
                    ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(br.gstregno);
                }
                else
                {
                    ViewBag.GSTRegNo = "-";
                }

                if (id > 0 || mid > 0 || cardno > 0 || mcode != "")
                {
                    mList = CusSupController.getMemberInfo("%", "", "", mid, mcode, cardno, "").ToList();
                    if (mList.Count > 0)
                    {
                        ViewBag.Member = mList.ElementAt(0);
                        mid = mList.ElementAt(0).id;
                        CusSup_m_Cards cardtmp = db.MemberCard.Where(x => x.cussupid == mid && x.status != "InActive" && x.status != "INACTIVE").OrderByDescending(x => x.id).FirstOrDefault();
                        if (cardtmp != null)
                        {
                            ViewBag.CardNo = cardtmp.cardno.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.Member = new CusSup_m_CusSupdtl();
                        mid = 0;
                    }
                }
                else
                {
                    //Added by ZawZO on 22 Jan 2016
                    if (soid > 0)
                    {
                        Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == soid).FirstOrDefault();
                        var cid = so.cussupid;
                        mList = CusSupController.getMemberInfo("%", "", "", cid, "", 0, "").ToList();
                        if (mList.Count > 0)
                        {
                            ViewBag.Member = mList.ElementAt(0);
                            mid = mList.ElementAt(0).id;
                            CusSup_m_Cards cardtmp = db.MemberCard.Where(x => x.cussupid == cid && x.status != "InActive" && x.status != "INACTIVE").OrderByDescending(x => x.id).FirstOrDefault();
                            if (cardtmp != null)
                            {
                                ViewBag.CardNo = cardtmp.cardno.ToString();
                            }
                            else
                            {
                                ViewBag.CardNo = "0";
                            }
                        }
                        else
                        {
                            ViewBag.Member = new CusSup_m_CusSupdtl();
                            mid = 0;
                        }
                    }
                    else
                    {
                        ViewBag.Member = new CusSup_m_CusSupdtl(); mid = 0;
                    }
                }

                ViewBag.obc = 0;
                ViewBag.obb = 0;
                if (id == 0)
                {
                    //Changed by ZawZO on 1 Feb 2016
                    if (soid == 0)
                    {
                        Invoice_m_Invoice topup = new Invoice_m_Invoice();
                        topup.cussupid = mid;
                        topup.status = "";
                        ViewBag.staffname = Session["staffname"].ToString();
                        int sid = 0;
                        int.TryParse(Session["staffid"].ToString(), out sid);
                        topup.staffid = sid;
                        return View(topup);
                    }
                    else
                    {
                        Invoice_m_Invoice topup = new Invoice_m_Invoice();
                        topup.status = "";
                        Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == soid).FirstOrDefault();
                        topup.cussupid = so.cussupid;
                        CusSup_m_CusSup cs = db.CusSup.Where(x => x.id == so.cussupid).FirstOrDefault();
                        ViewBag.CussupCode = cs.inhousecode;
                        ViewBag.SaleOrderID = soid;
                        Session["staffid"] = so.staffid;
                        int intstaffid = Convert.ToInt32(so.staffid);
                        Common_m_Staff staff = db.Staffs.Where(x => x.id == intstaffid).FirstOrDefault();
                        Session["staffname"] = staff.name;
                        ViewBag.staffname = staff.name;
                        return View(topup);
                    }
                }
                else
                {
                    Invoice_m_Invoice topup = db.sales.Find(id);
                    ViewBag.discremark = topup.remark;
                    ViewBag.discountpercent = topup.discountpercent;
                    try
                    {
                        CusSup_m_CusRedemption preCbal = db.CusSupRedemption.Where(x => x.cussupid == topup.cussupid && x.RefNo != topup.resourcecode && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                        if (preCbal != null)
                        {
                            ViewBag.obc = preCbal.balance;
                        }
                    }
                    catch (Exception e1)
                    { ViewBag.obc = 0; }
                    try
                    {
                        CusSup_m_CusRedemption preBbal = db.CusSupRedemption.Where(x => x.cussupid == topup.cussupid && x.RefNo != topup.resourcecode && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                        if (preBbal != null)
                        {
                            ViewBag.obb = preBbal.balance;
                        }
                    }
                    catch (Exception e2)
                    { ViewBag.obb = 0; }

                    if (topup.staffid > 0)
                    {
                        Common_m_Staff s = db.Staffs.Find(topup.staffid);
                        if (s != null)
                        { ViewBag.staffname = s.name; }
                        else
                            ViewBag.staffname = "";
                    }
                    else
                        ViewBag.staffname = "";
                    return View(topup);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        //Added by ZawZO on 22 Jan 2016
        public JsonResult getRedeemDetailFromFacilityOrderWithPaging(int pageno = 0, int soid = 0)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                    string strsql = "call getRedeemDetailFromFacilityOrder(" + soid.ToString() + ")";
                    var itemlist = context.Database.SqlQuery<Invoice_m_Invoice_Items>(strsql).ToList();
                    var SList = itemlist;
                    //ICollection<Invoice_m_Invoice_Items> SList = db.saleItems.Where(x => x.invoiceid == id).ToList();
                    var paginatedQSList = new PaginatedList<Invoice_m_Invoice_Items>(SList.AsQueryable(), pageno, pagesize);
                    var paginatedSList = paginatedQSList.ToList();

                    if (paginatedSList != null)
                    {
                        if (paginatedSList.Count > 0)
                            paginatedSList.ElementAt(0).TotalPages = paginatedQSList.TotalPages;
                    }

                    return Json(paginatedSList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }
        [HttpPost]
        public JsonResult RedeemVoid(int rid, string resource = "Redeem")
        {
            var returnStr = "FAIL";

            try
            {
                double? ctotal = 0;
                double? btotal = 0;
                double? rc = 0; double? ac = 0; double? rb = 0; double? ab = 0;
                Invoice_m_Invoice inv = db.sales.Where(x => x.id == rid).FirstOrDefault();

                if (inv != null)
                {
                    Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == inv.salesorderid).FirstOrDefault();

                    if (so != null)
                    {
                        if (so.type == "ORDER TAKING")
                        {
                            returnStr = "Please void this item in order taking module.";
                        }
                        else
                        {
                            Invoice_m_Invoice tmpobj = db.sales.Where(x => x.id == rid && x.status != "VOID").FirstOrDefault();

                            if (tmpobj != null)
                            {
                                //ICollection<Invoice_m_Invoice_Items> items = db.saleItems.Where(x => x.invoiceid == rid).ToList();
                                //rc = items.Sum(x => (x.redeemdollars * x.qty));
                                //ac = items.Sum(x => (x.awarddollars * x.qty));
                                //ctotal = ac - rc;
                                //rb = items.Sum(x => (x.redeembonus * x.qty));
                                //ab = items.Sum(x => (x.awardbonus * x.qty));
                                rc = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.redeemdollars * x.qty));
                                ac = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.awarddollars * x.qty));
                                ctotal = ac - rc;
                                rb = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.redeembonus * x.qty));
                                ab = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.awardbonus * x.qty));
                                btotal = ab - rb;
                                addToRedemptionTableForVoidItem(tmpobj, ctotal, "C$", "REDEEM");
                                addToRedemptionTableForVoidItem(tmpobj, btotal, "B$", "REDEEM");
                                tmpobj.status = "VOID";
                                db.SaveChanges();
                                returnStr = "SUCCESS";
                            }
                            else
                            { returnStr = "Void Fail. Status is invalid to void."; }
                        }
                    }
                    else
                    {
                        Invoice_m_Invoice tmpobj = db.sales.Where(x => x.id == rid && x.status != "VOID").FirstOrDefault();

                        if (tmpobj != null)
                        {
                            //ICollection<Invoice_m_Invoice_Items> items = db.saleItems.Where(x => x.invoiceid == rid).ToList();
                            //rc = items.Sum(x => (x.redeemdollars * x.qty));
                            //ac = items.Sum(x => (x.awarddollars * x.qty));
                            //ctotal = ac - rc;
                            //rb = items.Sum(x => (x.redeembonus * x.qty));
                            //ab = items.Sum(x => (x.awardbonus * x.qty));
                            rc = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.redeemdollars * x.qty));
                            ac = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.awarddollars * x.qty));
                            ctotal = ac - rc;
                            rb = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.redeembonus * x.qty));
                            ab = db.saleItems.Where(x => x.invoiceid == rid).Sum(x => (x.awardbonus * x.qty));
                            btotal = ab - rb;
                            addToRedemptionTableForVoidItem(tmpobj, ctotal, "C$", "REDEEM");
                            addToRedemptionTableForVoidItem(tmpobj, btotal, "B$", "REDEEM");
                            tmpobj.status = "VOID";
                            db.SaveChanges();
                            returnStr = "SUCCESS";
                        }
                        else
                        { returnStr = "Void Fail. Status is invalid to void."; }
                    }


                }

            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TopUpVoid(int tid, string resource = "TopUp")
        {
            var returnStr = "FAIL";

            try
            {
                double? ctotal = 0;
                double? btotal = 0;
                double? rc = 0; double? ac = 0; double? rb = 0; double? ab = 0;
                Invoice_m_Invoice tmpobj = db.sales.Where(x => x.id == tid && x.status != "VOID").FirstOrDefault();
                if (tmpobj != null)
                {
                    rc = db.saleItems.Where(x => x.invoiceid == tid).Sum(x => (x.redeemdollars * x.qty));
                    ac = db.saleItems.Where(x => x.invoiceid == tid).Sum(x => (x.awarddollars * x.qty));
                    ctotal = ac - rc;
                    rb = db.saleItems.Where(x => x.invoiceid == tid).Sum(x => (x.redeembonus * x.qty));
                    ab = db.saleItems.Where(x => x.invoiceid == tid).Sum(x => (x.awardbonus * x.qty));
                    btotal = ab - rb;
                    addToRedemptionTableForVoidItem(tmpobj, ctotal, "C$", "TOPUP");
                    addToRedemptionTableForVoidItem(tmpobj, btotal, "B$", "TOPUP");
                    tmpobj.status = "VOID";
                    db.SaveChanges();
                    returnStr = "SUCCESS";
                }
                else
                { returnStr = "Void Fail. Status is invalid to void."; }
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void addToRedemptionTableForVoidItem(Invoice_m_Invoice inv, double? total, string rtype, string ttype)
        {
            CusSup_m_CusRedemption obj = new CusSup_m_CusRedemption();
            int mid = inv.cussupid;
            CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == mid && x.redemptiontype == rtype).OrderByDescending(x => x.id).FirstOrDefault();
            double? prebalance = 0;
            if (redempttmp != null)
            { prebalance = redempttmp.balance; }

            if (total != 0)
            {
                obj.createdate = DateTime.Now;
                obj.lastmodifieddate = DateTime.Now;
                obj.invoiceitemid = 0;
                obj.cussupid = inv.cussupid;
                obj.productid = 0;
                obj.productdesc = "Void " + ttype + "# " + inv.resourcecode + " - Reverse back point";
                obj.remark = "Void " + ttype + "# " + inv.resourcecode;
                obj.resource = ttype + "VOID";
                obj.RefNo = inv.resourcecode;
                obj.branchcode = Convert.ToString(Session["branchcode"]);
                obj.cocode = Convert.ToString(Session["cocode"]);
                obj.redemptiontype = rtype;
                obj.createid = Convert.ToInt32(Session["userid"]);
                if (total > 0)
                {
                    obj.credit = 0;
                    obj.debit = total;
                }
                else
                {
                    obj.debit = 0;
                    obj.credit = (total * (-1));
                }

                obj.balance = prebalance + obj.credit - obj.debit;
                db.CusSupRedemption.Add(obj);
                db.SaveChanges();
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult RedeemSave(Invoice_m_Invoice redeem, string resource = "Redeem", string itemids = "", string deltIDs = "")
        {
            var returnStr = "FAIL";
            int itemcount = 0;
            if (Session["userid"] != null)
            {
                try
                {
                    string curr = "";
                    double exrate = 0;
                    ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                    if (config.Count > 0)
                        curr = config.ElementAt(0).value;
                    else
                        curr = "";

                    config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                    if (config.Count > 0)
                        exrate = Convert.ToDouble(config.ElementAt(0).value);
                    else
                        exrate = 0;

                    var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                    if (redeem.id == 0)
                    {
                        redeem.createdate = DateTime.Now;
                        redeem.lastmodifieddate = DateTime.Now;
                        redeem.cocode = Convert.ToString(Session["cocode"]);
                        redeem.createid = Convert.ToInt32(Session["userid"]);
                        redeem.branchcode = Convert.ToString(Session["branchcode"]);
                        redeem.currency = curr;
                        redeem.exchangerate = exrate;
                        redeem.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        redeem.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        redeem.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        redeem.total_amountrefund = 0;
                        redeem.total_amountvoid = 0;
                        redeem.total_discount = 0;
                        redeem.total_salestax = 0;

                        redeem.resourcecode = GeneralController.getGeneratedNewID("Invoice_m_Invoice", "resourcecode", "RDPREFIX", "INVREDEEM");
                        //Added by ZawZO on 31 Aug 2015
                        redeem.resourcedate = DateTime.Now;

                        db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, redeem.id, "CREATE", "Add New Redeem for cust id - " + redeem.cussupid.ToString() + ", Refno-" + redeem.resourcecode.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), "Redeem Ref no- " + redeem.resourcecode + ", ID- " + redeem.id.ToString());

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                SaveNewTopupDetailItem(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, redeem.resourcecode, rid, curr, exrate, redeem.status, false, redeem.cussupname);
                            }
                            //db.SaveChanges();
                        }
                        //Added by ZawZO on 2 Feb 2016
                        if (redeem.status == "CLOSE")
                        {
                            if (redeem.salesorderid > 0)
                            {
                                Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == redeem.salesorderid).FirstOrDefault();
                                so.status = "REDEEM";
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Invoice_m_Invoice Redeemtmp = db.sales.Find(redeem.id);
                        string from = ""; string to = "";
                        string prevStatus = Redeemtmp.status.ToUpper();
                        string newStatus = redeem.status.ToUpper();
                        from = "Status -" + Redeemtmp.status + ", Create By -" + Redeemtmp.staffid + ", total -" + Redeemtmp.total_total.ToString();
                        to = "Status -" + redeem.status + ", Create By -" + redeem.staffid + ", total -" + Redeemtmp.total_total.ToString() + ", items -" + itemids;
                        //Added by ZawZO on 31 Aug 2015
                        Redeemtmp.lastmodifieddate = DateTime.Now;
                        if (Redeemtmp.resourcedate == null) Redeemtmp.resourcedate = DateTime.Now;

                        Redeemtmp.cocode = Convert.ToString(Session["cocode"]);
                        Redeemtmp.branchcode = Convert.ToString(Session["branchcode"]);
                        Redeemtmp.currency = curr;
                        Redeemtmp.exchangerate = exrate;
                        Redeemtmp.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        Redeemtmp.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        Redeemtmp.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        Redeemtmp.total_amountrefund = 0;
                        Redeemtmp.total_amountvoid = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.total_salestax = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.status = redeem.status;
                        Redeemtmp.staffid = redeem.staffid;
                        //db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, Redeemtmp.id, "UPDATE", "UPDATE Redeem - refno -" + Redeemtmp.resourcecode + " , id - " + Redeemtmp.id.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from Invoice_m_Invoice_Items where invoiceid=" + redeem.id.ToString() + " and id not in (" + itemids + ")");
                            //return View(pList);
                        }

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                redeem.items.ElementAt(i).resourcecode = Redeemtmp.resourcecode;
                                if (redeem.items.ElementAt(i).id == 0)
                                {
                                    SaveNewTopupDetailItem(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, Redeemtmp.resourcecode, rid, curr, exrate, redeem.status, false, Redeemtmp.cussupname);
                                }
                                else
                                {
                                    UpdateTopupDetailItem(redeem.items.ElementAt(i), redeem.cussupid, resource, Redeemtmp.resourcecode, rid, prevStatus, newStatus, false, Redeemtmp.cussupname);
                                }
                            }
                            //db.SaveChanges();
                        }
                        //Added by ZawZO on 2 Feb 2016
                        if (redeem.status == "CLOSE")
                        {
                            if (Redeemtmp.salesorderid > 0)
                            {
                                Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == Redeemtmp.salesorderid).FirstOrDefault();
                                so.status = "REDEEM";
                                db.SaveChanges();
                            }
                        }
                        using (var context = new BigMacEntities())
                        {
                            string rescode = Redeemtmp.resourcecode.ToString();
                            deltIDs = deltIDs + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from cussup_m_treatment where resourcecode='" + rescode + "' and id  in (" + deltIDs + ")");
                            //return View(pList);
                        }
                    }

                    //returnStr = "SUCCESS";
                    returnStr = redeem.id.ToString();
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSIONEXPIRED";
            }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult RedeemSaveQty(Invoice_m_Invoice redeem, string resource = "Redeem", string itemids = "", string deltIDs = "")
        {
            var returnStr = "FAIL";
            int itemcount = 0;
            if (Session["userid"] != null)
            {
                try
                {
                    string curr = "";
                    double exrate = 0;
                    ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                    if (config.Count > 0)
                        curr = config.ElementAt(0).value;
                    else
                        curr = "";

                    config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                    if (config.Count > 0)
                        exrate = Convert.ToDouble(config.ElementAt(0).value);
                    else
                        exrate = 0;

                    var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                    if (redeem.id == 0)
                    {
                        redeem.createdate = DateTime.Now;
                        redeem.lastmodifieddate = DateTime.Now;
                        redeem.cocode = Convert.ToString(Session["cocode"]);
                        redeem.createid = Convert.ToInt32(Session["userid"]);
                        redeem.branchcode = Convert.ToString(Session["branchcode"]);
                        redeem.currency = curr;
                        redeem.exchangerate = exrate;
                        redeem.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        redeem.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        redeem.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        redeem.total_amountrefund = 0;
                        redeem.total_amountvoid = 0;
                        redeem.total_discount = 0;
                        redeem.total_salestax = 0;

                        redeem.resourcecode = GeneralController.getGeneratedNewID("Invoice_m_Invoice", "resourcecode", "RDPREFIX", "INVREDEEM");
                        //Added by ZawZO on 31 Aug 2015
                        redeem.resourcedate = DateTime.Now;

                        db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, redeem.id, "CREATE", "Add New Redeem for cust id - " + redeem.cussupid.ToString() + ", Refno-" + redeem.resourcecode.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), "Redeem Ref no- " + redeem.resourcecode + ", ID- " + redeem.id.ToString());

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                SaveNewTopupDetailItemQty(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, redeem.resourcecode, rid, curr, exrate, redeem.status, false, redeem.cussupname);
                            }
                            //db.SaveChanges();
                        }
                        //Added by ZawZO on 2 Feb 2016
                        if (redeem.status == "CLOSE")
                        {
                            if (redeem.salesorderid > 0)
                            {
                                Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == redeem.salesorderid).FirstOrDefault();
                                so.status = "REDEEM";
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Invoice_m_Invoice Redeemtmp = db.sales.Find(redeem.id);
                        string from = ""; string to = "";
                        string prevStatus = Redeemtmp.status.ToUpper();
                        string newStatus = redeem.status.ToUpper();
                        from = "Status -" + Redeemtmp.status + ", Create By -" + Redeemtmp.staffid + ", total -" + Redeemtmp.total_total.ToString();
                        to = "Status -" + redeem.status + ", Create By -" + redeem.staffid + ", total -" + Redeemtmp.total_total.ToString() + ", items -" + itemids;
                        //Added by ZawZO on 31 Aug 2015
                        Redeemtmp.lastmodifieddate = DateTime.Now;
                        if (Redeemtmp.resourcedate == null) Redeemtmp.resourcedate = DateTime.Now;

                        Redeemtmp.cocode = Convert.ToString(Session["cocode"]);
                        Redeemtmp.branchcode = Convert.ToString(Session["branchcode"]);
                        Redeemtmp.currency = curr;
                        Redeemtmp.exchangerate = exrate;
                        Redeemtmp.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        Redeemtmp.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        Redeemtmp.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        Redeemtmp.total_amountrefund = 0;
                        Redeemtmp.total_amountvoid = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.total_salestax = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.status = redeem.status;
                        Redeemtmp.staffid = redeem.staffid;
                        //db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, Redeemtmp.id, "UPDATE", "UPDATE Redeem - refno -" + Redeemtmp.resourcecode + " , id - " + Redeemtmp.id.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from Invoice_m_Invoice_Items where invoiceid=" + redeem.id.ToString() + " and id not in (" + itemids + ")");
                            //return View(pList);
                        }

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                redeem.items.ElementAt(i).resourcecode = Redeemtmp.resourcecode;
                                if (redeem.items.ElementAt(i).id == 0)
                                {
                                    SaveNewTopupDetailItemQty(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, Redeemtmp.resourcecode, rid, curr, exrate, redeem.status, false, Redeemtmp.cussupname);
                                }
                                else
                                {
                                    UpdateTopupDetailItemQty(redeem.items.ElementAt(i), redeem.cussupid, resource, Redeemtmp.resourcecode, rid, prevStatus, newStatus, false, Redeemtmp.cussupname);
                                }
                            }
                            //db.SaveChanges();
                        }
                        //Added by ZawZO on 2 Feb 2016
                        if (redeem.status == "CLOSE")
                        {
                            if (Redeemtmp.salesorderid > 0)
                            {
                                Salesorder_m_salesorder so = db.saleorders.Where(x => x.id == Redeemtmp.salesorderid).FirstOrDefault();
                                so.status = "REDEEM";
                                db.SaveChanges();
                            }
                        }
                        using (var context = new BigMacEntities())
                        {
                            string rescode = Redeemtmp.resourcecode.ToString();
                            deltIDs = deltIDs + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from cussup_m_treatment where resourcecode='" + rescode + "' and id  in (" + deltIDs + ")");
                            //return View(pList);
                        }
                    }

                    //returnStr = "SUCCESS";
                    returnStr = redeem.id.ToString();
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSIONEXPIRED";
            }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void SaveNewRedeemDetailItem(Invoice_m_Invoice_Items redeemDtl, int tid, string rcode, int memberid, string resource, string curr = "SGD", double exrate = 0)
        {
            //redeemDtl.currency = curr;
            //redeemDtl.exchangerate = exrate;
            //redeemDtl.createdate = DateTime.Today;
            //redeemDtl.lastmodifieddate = DateTime.Today;
            //redeemDtl.productid = tid;
            //redeemDtl.resourcecode = rcode;
            //redeemDtl.createid = Convert.Toint(Session["userid"]);
            //redeemDtl.discountamount = 0;
            //redeemDtl.taxamount = 0;
            //redeemDtl.lineamount = redeemDtl.qty * redeemDtl.unitprice;
            //redeemDtl.gstcode = "";
            //db.saleItems.Add(redeemDtl);

            //if (redeemDtl.awarddollars > 0)
            //{
            //    CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
            //    double prebalance = 0;
            //    if (redempttmp != null)
            //    { prebalance = redempttmp.balance; }

            //    CusSup_m_CusRedemption redempt = new CusSup_m_CusRedemption();
            //    redempt.cussupid = memberid;
            //    redempt.createdate = DateTime.Now;
            //    redempt.lastmodifieddate = DateTime.Now;
            //    redempt.RefNo = redeemDtl.resourcecode;
            //    redempt.productid = redeemDtl.productid;
            //    redempt.productdesc = redeemDtl.productdesc;
            //    redempt.debit = 0;
            //    redempt.credit = redeemDtl.awarddollars;
            //    redempt.redemptiontype = "C$";
            //    redempt.balance = prebalance + redempt.credit;
            //    redempt.createid = redeemDtl.createid;
            //    //redemp.remark = "Top Up";
            //    redempt.resource = resource;
            //    redempt.cocode = Convert.ToString(Session["cocode"]);
            //    redempt.branchcode = Convert.ToString(Session["branchcode"]);
            //    db.CusSupRedemption.Add(redempt);
            //}

            //if (redeemDtl.awardbonus > 0)
            //{
            //    CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
            //    double prebalance = 0;
            //    if (redempttmp != null)
            //    { prebalance = redempttmp.balance; }

            //    CusSup_m_CusRedemption redemptB = new CusSup_m_CusRedemption();
            //    redemptB.cussupid = memberid;
            //    redemptB.createdate = DateTime.Now;
            //    redemptB.lastmodifieddate = DateTime.Now;
            //    redemptB.RefNo = redeemDtl.resourcecode;
            //    redemptB.productid = redeemDtl.productid;
            //    redemptB.productdesc = redeemDtl.productdesc;
            //    redemptB.debit = 0;
            //    redemptB.credit = redeemDtl.awardbonus;
            //    redemptB.redemptiontype = "B$";
            //    redemptB.balance = prebalance + redemptB.credit;
            //    redemptB.createid = redeemDtl.createid;
            //    redemptB.resource = resource;
            //    redemptB.cocode = Convert.ToString(Session["cocode"]);
            //    redemptB.branchcode = Convert.ToString(Session["branchcode"]);
            //    db.CusSupRedemption.Add(redemptB);
            //}
            //db.SaveChanges();
        }

        public ActionResult TopUp(int id = 0, int mid = 0, Int32 cardno = 0, string rcode = "TOPUP", string mcode = "")
        {
            try
            {
                ViewBag.cardno = cardno;
                ViewBag.RCode = rcode;
                ViewBag.CardTypeOptions = db.CardType.AsEnumerable();
                ViewBag.CardNo = "";
                string bcode = Session["branchcode"].ToString();
                ViewBag.StaffOptions = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                var y = db.ConfigDefault.Where(x => x.key == "CEXPYEAR").FirstOrDefault().value;
                var yy = 0;
                if (y != "")
                    yy = Convert.ToInt32(y);
                ViewBag.ExpYear = DateTime.Today.AddYears(yy).Date;
                ViewBag.TempExpYear = DateTime.Today.AddMonths(1).Date;

                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                //Added by ZawZO on 12 Nov 2015
                string CoCode = Session["cocode"].ToString();
                Configuration_m_Company co = db.Companies.Where(x => x.cocode == CoCode).FirstOrDefault();
                ViewBag.CoRegNo = cAESEncryption.getDecryptedString(co.coregno);
                ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(co.gstregno);
                Configuration_m_Branches br = db.Branches.Where(x => x.branchcode == bcode).FirstOrDefault();
                ViewBag.CoName = cAESEncryption.getDecryptedString(br.branchname);
                if (br.address != null)
                {
                    if (br.address == "")
                    {
                        ViewBag.CoAddress = "-";
                    }
                    else
                    {
                        ViewBag.CoAddress = cAESEncryption.getDecryptedString(br.address);
                    }
                }
                else
                {
                    ViewBag.CoAddress = "-";
                }
                if (br.tel != null)
                {
                    if (br.tel == "")
                    {
                        ViewBag.Tel = "-";
                    }
                    else
                    {
                        ViewBag.Tel = cAESEncryption.getDecryptedString(br.tel);
                    }
                }
                else
                {
                    ViewBag.Tel = "-";
                }
                if (br.fax != null)
                {
                    if (br.fax == "")
                    {
                        ViewBag.Fax = "-";
                    }
                    else
                    {
                        ViewBag.Fax = cAESEncryption.getDecryptedString(br.fax);
                    }
                }
                else
                {
                    ViewBag.Fax = "-";
                }
                if (br.email != null)
                {
                    if (br.email == "")
                    {
                        ViewBag.Email = "-";
                    }
                    else
                    {
                        ViewBag.Email = cAESEncryption.getDecryptedString(br.email);
                    }
                }
                else
                {
                    ViewBag.Email = "-";
                }
                int intGSTReg = br.gstreg;
                if (intGSTReg == 1)
                {
                    ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(br.gstregno);
                }
                else
                {
                    ViewBag.GSTRegNo = "-";
                }

                if (id == 0 && mid == 0 && cardno == 0) cardno = Convert.ToInt32(Session["cardno"]);
                ICollection<CusSup_m_CusSupdtl> mList;
                if (id > 0 || mid > 0 || cardno > 0 || mcode != "")
                {
                    mList = CusSupController.getMemberInfo("%", "", "", mid, mcode, cardno, "").ToList();
                    if (mList.Count > 0)
                    {
                        ViewBag.Member = mList.ElementAt(0);
                        mid = mList.ElementAt(0).id;
                        CusSup_m_Cards cardtmp = db.MemberCard.Where(x => x.cussupid == mid && x.status != "InActive" && x.status != "INACTIVE").OrderByDescending(x => x.id).FirstOrDefault();
                        if (cardtmp != null)
                        {
                            ViewBag.CardNo = cardtmp.cardno.ToString();
                        }
                    }
                    else
                    {
                        ViewBag.Member = new CusSup_m_CusSupdtl();
                        mid = 0;
                    }

                }
                else
                {
                    ViewBag.Member = new CusSup_m_CusSupdtl(); mid = 0;
                }

                ViewBag.obc = 0;
                ViewBag.obb = 0;
                if (id == 0)
                {
                    Invoice_m_Invoice topup = new Invoice_m_Invoice();
                    topup.cussupid = mid;
                    ViewBag.staffname = Session["staffname"].ToString();
                    int sid = 0;
                    int.TryParse(Session["staffid"].ToString(), out sid);
                    topup.staffid = sid;
                    return View(topup);
                }
                else
                {
                    Invoice_m_Invoice topup = db.sales.Find(id);
                    ICollection<CusSup_m_CusRedemption> preCbal = db.CusSupRedemption.Where(x => x.cussupid == topup.cussupid && x.RefNo != topup.resourcecode && x.redemptiontype == "C$").OrderByDescending(x => x.id).ToList();
                    if (preCbal != null)
                    {
                        if (preCbal.Count > 0)
                            ViewBag.obc = preCbal.ElementAt(0).balance;
                    }

                    ICollection<CusSup_m_CusRedemption> preBbal = db.CusSupRedemption.Where(x => x.cussupid == topup.cussupid && x.RefNo != topup.resourcecode && x.redemptiontype == "B$").OrderByDescending(x => x.id).ToList();
                    if (preBbal != null)
                    {
                        if (preBbal.Count > 0)
                            ViewBag.obb = preBbal.ElementAt(0).balance;
                    }
                    if (topup.staffid > 0)
                    {
                        Common_m_Staff s = db.Staffs.Find(topup.staffid);
                        if (s != null)
                        {
                            ViewBag.staffname = s.name;
                        }
                        else
                            ViewBag.staffname = "";
                    }
                    else
                        ViewBag.staffname = "";
                    return View(topup);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public Boolean IsDuplicatePOSRef(int id = 0, string posno = "")
        {
            Boolean returnvalue = false;
            try
            {
                ICollection<Invoice_m_Invoice> inv = db.sales.Where(x => x.refno == posno && x.id != id).ToList();
                if (inv != null)
                {
                    if (inv.Count > 0)
                    {
                        returnvalue = true;
                    }
                }
            }
            catch (Exception e)
            { }
            return returnvalue;
        }

        [HttpPost]
        [Authorize]
        public JsonResult TopUpSave(Invoice_m_Invoice topup, string resource = "TopUp")
        {
            var returnStr = "FAIL";
            int itemcount = 0;
            if (Session["userid"] != null)
            {
                try
                {
                    if (IsDuplicatePOSRef(topup.id, topup.refno))
                    { returnStr = "DUPLICATEREF"; }
                    else
                    {
                        string curr = "";
                        double exrate = 0;
                        ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                        if (config.Count > 0)
                            curr = config.ElementAt(0).value;
                        else
                            curr = "";

                        config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                        if (config.Count > 0)
                            exrate = Convert.ToDouble(config.ElementAt(0).value);
                        else
                            exrate = 0;

                        var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                        int rid = 0;
                        if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                        if (topup.id == 0)
                        {
                            topup.createdate = DateTime.Now;
                            topup.lastmodifieddate = DateTime.Now;
                            topup.cocode = Convert.ToString(Session["cocode"]);
                            topup.createid = Convert.ToInt32(Session["userid"]);
                            topup.branchcode = Convert.ToString(Session["branchcode"]);
                            topup.currency = curr;
                            topup.exchangerate = exrate;
                            topup.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                            topup.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                            topup.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                            topup.total_amountrefund = 0;
                            topup.total_amountvoid = 0;
                            topup.total_discount = 0;
                            topup.total_salestax = 0;
                            topup.total_discount = 0;
                            //topup.resourcecode = GeneralController.getGeneratedNewID("Invoice_m_Invoice", "resourcecode", "INVPREFIX", "INIINV");
                            topup.resourcecode = GeneralController.getGeneratedNewID("Invoice_m_Invoice", "resourcecode", "TPPREFIX", "INVTOPUP");
                            //Added by ZawZO on 31 Aug 2015
                            topup.resourcedate = DateTime.Now;

                            db.sales.Add(topup);
                            db.SaveChanges();
                            if (topup.items != null) itemcount = topup.items.Count();
                            saveToLog(rid, topup.id, "CREATE", "Add New Top Up for cust id - " + topup.cussupid.ToString() + ", Ref-" + topup.resourcecode + ", Item Count -" + itemcount.ToString() + ", Total - " + topup.total_total.ToString(), "Topup no- " + topup.resourcecode + " Total -" + topup.total_total.ToString() + ", ID- " + topup.id.ToString());

                            if (topup.items != null)
                            {
                                for (int i = 0; i < (topup.items.Count()); i++)
                                {
                                    SaveNewTopupDetailItem(topup.items.ElementAt(i), topup.id, topup.cussupid, resource, topup.resourcecode, rid, curr, exrate, topup.status);
                                }
                                //db.SaveChanges();
                            }

                            if (topup.salestaffs != null)
                            {
                                for (int i = 0; i < (topup.salestaffs.Count()); i++)
                                {
                                    SaveNewServiceStaff(topup.salestaffs.ElementAt(i), topup.id, topup.resourcecode, rid);
                                }
                                db.SaveChanges();
                                //db.SaveChanges();
                            }

                            if (topup.status == "CLOSE")
                            {
                                AddSalesFigure(topup, rid);
                            }

                        }
                        else
                        {
                            Invoice_m_Invoice topuptmp = db.sales.Find(topup.id);
                            string from = ""; string to = "";
                            string prevStatus = topuptmp.status.ToUpper();
                            string newStatus = topup.status.ToUpper();
                            from = "topup status -" + topuptmp.status;
                            to = "topup status -" + topup.status;
                            //Added by ZawZO on 31 Aug 2015
                            topuptmp.lastmodifieddate = DateTime.Now;
                            if (topuptmp.resourcedate == null) topuptmp.resourcedate = DateTime.Now;

                            topuptmp.cocode = Convert.ToString(Session["cocode"]);
                            topuptmp.branchcode = Convert.ToString(Session["branchcode"]);
                            topuptmp.currency = curr;
                            topuptmp.exchangerate = exrate;
                            topuptmp.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                            topuptmp.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                            topuptmp.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                            topuptmp.total_amountrefund = 0;
                            topuptmp.total_amountvoid = 0;
                            topuptmp.total_discount = 0;
                            topuptmp.total_salestax = 0;
                            topuptmp.total_discount = 0;
                            topuptmp.status = topup.status;
                            //db.sales.Add(topup);
                            if (topup.items != null) itemcount = topup.items.Count();
                            db.SaveChanges();
                            if (topup.items != null) itemcount = topup.items.Count();
                            saveToLog(rid, topup.id, "UPDATE", "UPDATE TopUP - refno" + topup.resourcecode + ", id - " + topup.id.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + topup.total_total.ToString(), from, to);
                            if (topup.items != null)
                            {
                                for (int i = 0; i < (topup.items.Count()); i++)
                                {
                                    if (topup.items.ElementAt(i).id == 0)
                                    {
                                        SaveNewTopupDetailItem(topup.items.ElementAt(i), topup.id, topup.cussupid, resource, topuptmp.resourcecode, rid, curr, exrate, topup.status);
                                    }
                                    else
                                    {
                                        UpdateTopupDetailItem(topup.items.ElementAt(i), topup.cussupid, resource, topuptmp.resourcecode, rid, prevStatus, newStatus);
                                    }
                                }
                                //db.SaveChanges();
                            }
                            string sids = "";
                            if (topup.salestaffs != null)
                            {
                                for (int i = 0; i < (topup.salestaffs.Count()); i++)
                                {
                                    //sids = sids + topup.salestaffs.ElementAt(i).id + ",";
                                    if (topup.salestaffs.ElementAt(i).id == 0)
                                    {
                                        SaveNewServiceStaff(topup.salestaffs.ElementAt(i), topup.id, topup.resourcecode, rid);
                                        sids = sids + topup.salestaffs.ElementAt(i).id + ",";
                                    }
                                    else
                                    {
                                        UpdateServiceStaff(topup.salestaffs.ElementAt(i), topup.id, topup.resourcecode, rid);
                                        sids = sids + topup.salestaffs.ElementAt(i).id + ",";
                                        //UpdateTopupDetailItem(topup.items.ElementAt(i), topup.cussupid, resource, topuptmp.resourcecode, rid, prevStatus, newStatus);
                                    }
                                }
                                using (var context = new BigMacEntities())
                                {
                                    context.Database.ExecuteSqlCommand("call RemoveServiceStaff(" + topup.id.ToString() + ",'" + sids + "'," + rid.ToString() + "," + Convert.ToInt32(Session["userid"]).ToString() + ",'" + Session.SessionID + "','" + Session["cocode"].ToString() + "')");
                                }
                                db.SaveChanges();
                            }

                            if (newStatus == "CLOSE")
                            {
                                if (prevStatus == "ACTIVE")
                                {
                                    AddSalesFigure(topuptmp, rid);
                                }
                            }
                        }
                        //returnStr = "SUCCESS";

                        returnStr = topup.id.ToString();
                    }
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSIONEXPIRED";
            }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateServiceStaffList(IList<Invoice_m_Invoice_SalesStaff> slst, int tid = 0, string rcode = "TOPUP")
        {
            string returnStr = "";
            try
            {
                int i = 0;
                string from = ""; String to = ""; String tmpid = "";
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                for (i = 0; i < slst.Count; i++)
                {
                    if (slst.ElementAt(i).id == 0)
                    {
                        slst.ElementAt(i).createdate = DateTime.Now;
                        slst.ElementAt(i).lastmodifieddate = DateTime.Now;
                        slst.ElementAt(i).createid = Convert.ToInt32(Session["userid"]);
                        slst.ElementAt(i).invoiceid = tid;
                        //stmp.resourcecode = invresourcecode;
                        db.saleStaffs.Add(slst.ElementAt(i));
                        db.SaveChanges();
                        tmpid = tmpid + slst.ElementAt(i).id.ToString() + ",";
                        saveToLog(rid, tid, "CREATE", "Add New Service Staff - Invoice Code -" + tid.ToString(), "RefNo no- " + slst.ElementAt(i).resourcecode + ", ID- " + slst.ElementAt(i).id.ToString() + ",Staff ID -" + slst.ElementAt(i).staffid.ToString() + ", Percent -" + slst.ElementAt(i).percent.ToString());
                    }
                    else
                    {
                        Invoice_m_Invoice_SalesStaff stmp = db.saleStaffs.Find(slst.ElementAt(i).id);
                        if (stmp != null)
                        {
                            tmpid = tmpid + slst.ElementAt(i).id.ToString() + ",";
                            from = "Line No -" + stmp.lineno.ToString() + ",StaffID" + stmp.id.ToString() + ", Percent -" + stmp.percent.ToString();
                            to = "Line No -" + slst.ElementAt(i).lineno.ToString() + ",StaffID" + slst.ElementAt(i).id.ToString() + ", Percent -" + slst.ElementAt(i).percent.ToString();
                            stmp.staffid = slst.ElementAt(i).staffid;
                            stmp.lineno = slst.ElementAt(i).lineno;
                            stmp.percent = slst.ElementAt(i).percent;
                            stmp.lastmodifieddate = DateTime.Now;
                            db.SaveChanges();
                            saveToLog(rid, tid, "UPDATE", "Update Service Staff ID-" + stmp.id + ",- Invoice ID -" + tid.ToString(), from, to);

                        }
                    }
                }
                using (var context = new BigMacEntities())
                {
                    tmpid = tmpid + "0";
                    var value = context.Database.ExecuteSqlCommand("Delete from Invoice_m_Invoice_SalesStaff where invoiceid=" + tid.ToString() + " and id not in (" + tmpid + ")");
                    //return View(pList);
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void SaveNewServiceStaff(Invoice_m_Invoice_SalesStaff staff, int invid = 0, string invresourcecode = "", int rid = 0)
        {
            staff.createdate = DateTime.Now;
            staff.lastmodifieddate = DateTime.Now;
            staff.createid = Convert.ToInt32(Session["userid"]);
            staff.invoiceid = invid;
            staff.resourcecode = invresourcecode;
            db.saleStaffs.Add(staff);
            db.SaveChanges();
            saveToLog(rid, invid, "CREATE", "Add New Service Staff - Invoice Code -" + invid.ToString(), "RefNo no- " + invresourcecode + ", ID- " + staff.id.ToString() + ",Staff ID -" + staff.staffid.ToString() + ", Percent -" + staff.percent.ToString());
        }

        public void UpdateServiceStaff(Invoice_m_Invoice_SalesStaff s, int invid = 0, string invresourcecode = "", int rid = 0)
        {
            Invoice_m_Invoice_SalesStaff staff = db.saleStaffs.Find(s.id);
            string from = ""; String to = "";

            from = "Line No -" + staff.lineno.ToString() + ",StaffID" + staff.id.ToString() + ", Percent -" + staff.percent.ToString();
            to = "Line No -" + s.lineno.ToString() + ",StaffID" + s.id.ToString() + ", Percent -" + s.percent.ToString();
            staff.lastmodifieddate = DateTime.Now;
            staff.staffid = s.staffid;
            staff.percent = s.percent;
            staff.lineno = s.lineno;
            db.SaveChanges();
            saveToLog(rid, invid, "UPDATE", "Update Service Staff - Invoice ID -" + invid.ToString() + ", RefNo no- " + invresourcecode, from, to);
            //db.saleStaffs.Add(staff);
        }

        [Authorize]
        public void SaveNewTopupDetailItem(Invoice_m_Invoice_Items topupDtl, int tid, int memberid, string resource, string resourcecode, int rid = 0, string curr = "SGD", double exrate = 0, string status = "Close", Boolean pkgFlag = false, string membername = "")
        {
            topupDtl.currency = curr;
            topupDtl.exchangerate = exrate;
            topupDtl.createdate = DateTime.Now;
            topupDtl.lastmodifieddate = DateTime.Now;
            //topupDtl.productid = tid;
            topupDtl.resourcecode = resourcecode;
            topupDtl.createid = Convert.ToInt32(Session["userid"]);
            topupDtl.discountamount = 0;
            topupDtl.taxamount = 0;
            topupDtl.lineamount = topupDtl.qty * topupDtl.unitprice;
            topupDtl.gstcode = "";
            topupDtl.invoiceid = tid;
            
            db.saleItems.Add(topupDtl);
            db.SaveChanges();
            saveToLog(rid, topupDtl.id, "CREATE", "Add New Deatil Item - product ID -" + topupDtl.productid, "RefNo no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString());
            if (topupDtl.treatment != null)
                SaveTreatment(topupDtl, memberid, membername, rid);

            if (status.ToUpper() == "CLOSE")
            {
                if (pkgFlag)
                    AddPackageToRedemptionTable(topupDtl.id, topupDtl.productid, topupDtl.productdesc, topupDtl.uom, memberid, resource, resourcecode, rid, topupDtl.qty);
                else
                    AddToRedemptionTable(topupDtl, memberid, resource, resourcecode, rid);
            }

        }

        [Authorize]
        public void SaveNewTopupDetailItemQty(Invoice_m_Invoice_Items topupDtl, int tid, int memberid, string resource, string resourcecode, int rid = 0, string curr = "SGD", double exrate = 0, string status = "Close", Boolean pkgFlag = false, string membername = "")
        {
            topupDtl.currency = curr;
            topupDtl.exchangerate = exrate;
            topupDtl.createdate = DateTime.Now;
            topupDtl.lastmodifieddate = DateTime.Now;
            //topupDtl.productid = tid;
            topupDtl.resourcecode = resourcecode;
            topupDtl.createid = Convert.ToInt32(Session["userid"]);
            topupDtl.discountamount = 0;
            topupDtl.taxamount = 0;
            topupDtl.lineamount = topupDtl.qty * topupDtl.unitprice;
            topupDtl.gstcode = "";
            topupDtl.invoiceid = tid;
            db.saleItems.Add(topupDtl);
            db.SaveChanges();
            saveToLog(rid, topupDtl.id, "CREATE", "Add New Deatil Item - product ID -" + topupDtl.productid, "RefNo no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString());
            if (topupDtl.treatment != null)
                SaveTreatment(topupDtl, memberid, membername, rid);

            if (status.ToUpper() == "CLOSE")
            {
                if (pkgFlag)
                    AddPackageToRedemptionTable(topupDtl.id, topupDtl.productid, topupDtl.productdesc, topupDtl.uom, memberid, resource, resourcecode, rid, topupDtl.qty);
                else
                    AddToRedemptionTableQty(topupDtl, memberid, resource, resourcecode, rid);
            }

        }


        public void AddSalesFigure(Invoice_m_Invoice inv, int rid = 0)
        {
            string branchcode = Session["branchcode"].ToString();
            string cocode = Session["cocode"].ToString();
            int uid = Convert.ToInt32(Session["userid"].ToString());
            int campaignid = 1; int groupid = 0;
            double? prevactual = 0;
            var cmptmp = db.ConfigDefault.Where(x => x.key == "CAMPAIGNID").FirstOrDefault();
            if (cmptmp != null)
            { campaignid = Convert.ToInt16(cmptmp.value); }
            var bgtmp = db.BranchGroupLinks.Where(x => x.branchcode == branchcode).FirstOrDefault();
            if (bgtmp != null)
            { groupid = bgtmp.groupid; }
            var saleFig = db.CampaignBranchSalesTarget.Where(x => x.createdate == DateTime.Today && x.branchcode == branchcode && x.groupid == groupid && x.cocode == cocode && x.staffid == 0).FirstOrDefault();
            Campaign_m_CampaignDetails campaigndtl;
            if (saleFig != null)
            {
                prevactual = saleFig.salesactual;
                saleFig.salesactual = saleFig.salesactual + inv.total_total;
                saveToLog(rid, saleFig.id, "Update", "Update sales actual figure : -" + saleFig.id.ToString() + ", campaign id " + saleFig.campaignid + ", Group:" + saleFig.groupid.ToString() + ",Branch Code-" + saleFig.branchcode + ", Date " + saleFig.createdate.ToString(), "Previous Sales Actual- " + prevactual.ToString(), "New Actual - " + saleFig.salesactual.ToString());
            }
            else
            {
                saleFig = new campaign_m_branchsalestarget();
                saleFig.createdate = DateTime.Today;
                saleFig.cocode = cocode;
                saleFig.branchcode = branchcode;
                saleFig.groupid = groupid;
                saleFig.campaignid = campaignid;
                saleFig.staffid = 0;
                saleFig.salesactual = inv.total_total;
                saleFig.salesforecast = 0;
                saleFig.userid = uid;
                db.CampaignBranchSalesTarget.Add(saleFig);
                saveToLog(rid, saleFig.id, "CREATE", "CREATE SALES Fig: campaign id " + saleFig.campaignid + ", Group:" + saleFig.groupid.ToString() + ",Branch Code-" + saleFig.branchcode + ", Date " + saleFig.createdate.ToString() + " , Sales Actual - " + saleFig.salesactual.ToString());
            }
            db.SaveChanges();
            int invid = inv.id;
            var stafflist = db.saleStaffs.Where(x => x.invoiceid == invid).ToList();
            for (int i = 0; i < stafflist.Count; i++)
            {
                int? dtlid = stafflist.ElementAt(i).resourcedetailid;
                campaigndtl = new Campaign_m_CampaignDetails();
                campaigndtl.createdate = DateTime.Today;
                campaigndtl.cocode = cocode;
                campaigndtl.branchcode = branchcode;
                campaigndtl.groupid = groupid;
                campaigndtl.campaignid = campaignid;
                campaigndtl.staffid = stafflist.ElementAt(i).staffid;
                campaigndtl.salesactual = inv.total_total * stafflist.ElementAt(i).percent / 100.0;
                campaigndtl.salesforecast = 0;
                campaigndtl.resourceid = inv.id;
                campaigndtl.resourcedetailid = 0;
                campaigndtl.productactual = 0;
                campaigndtl.productforecast = 0;
                campaigndtl.categoryid = 0;
                campaigndtl.categorytypeid = 1;
                campaigndtl.userid = uid;
                campaigndtl.time = new TimeSpan(DateTime.Now.TimeOfDay.Hours, 0, 0);
                db.CampaignDetails.Add(campaigndtl);
                db.SaveChanges();
                saveToLog(rid, saleFig.id, "CREATE", "CREATE SALES Fig in campaigndetail : campaign id " + saleFig.campaignid + ", Group:" + saleFig.groupid.ToString() + ",Branch Code-" + saleFig.branchcode + ", Date " + saleFig.createdate.ToString() + ", Staff ID: " + saleFig.staffid.ToString() + " , Sales Actual - " + saleFig.salesactual.ToString());

                //int sid = stafflist.ElementAt(i).staffid;
                //saleFig = db.CampaignBranchSalesTarget.Where(x => x.createdate == DateTime.Today && x.branchcode == branchcode && x.groupid == groupid && x.cocode == cocode && x.staffid == sid).FirstOrDefault();
                //if (saleFig != null)
                //{
                //    prevactual = saleFig.salesactual;
                //    saleFig.salesactual = saleFig.salesactual + (inv.total_total * stafflist.ElementAt(i).percent / 100.0);
                //    saveToLog(rid, saleFig.id, "Update", "Update sales actual figure : -" + saleFig.id.ToString() + ", campaign id " + saleFig.campaignid + ", Group:" + saleFig.groupid.ToString() + ",Branch Code-" + saleFig.branchcode + ", Date " + saleFig.createdate.ToString() + ",Staffid -" + saleFig.staffid.ToString(), "Previous Sales Actual- " + prevactual.ToString(), "New Actual - " + saleFig.salesactual.ToString());
                //}
                //else
                //{
                //    saleFig = new campaign_m_branchsalestarget();
                //    saleFig.createdate = DateTime.Today;
                //    saleFig.cocode = cocode;
                //    saleFig.branchcode = branchcode;
                //    saleFig.groupid = groupid;
                //    saleFig.campaignid = campaignid;
                //    saleFig.staffid = stafflist.ElementAt(i).staffid;
                //    saleFig.salesactual =inv.total_total * stafflist.ElementAt(i).percent / 100.0;
                //    saleFig.salesforecast = 0;
                //    saleFig.userid = uid;
                //    db.CampaignBranchSalesTarget.Add(saleFig);
                //    saveToLog(rid, saleFig.id, "CREATE", "CREATE SALES Fig: campaign id " + saleFig.campaignid + ", Group:" + saleFig.groupid.ToString() + ",Branch Code-" + saleFig.branchcode + ", Date " + saleFig.createdate.ToString() + ", Staff ID: " + saleFig.staffid.ToString() + " , Sales Actual - " + saleFig.salesactual.ToString());
                //}
                //db.SaveChanges();
            }
        }

        public void SaveTreatment(Invoice_m_Invoice_Items item, int cussupid, string cussupname, int rid)
        {
            try
            {
                ICollection<cussup_m_treatment_Ops> tlist = item.treatment.ToList();
                int sid = 0;
                int.TryParse(Session["staffid"].ToString(), out sid);

                for (int i = 0; i < tlist.Count; i++)
                {
                    if (tlist.ElementAt(i).statusfield.Trim() == "NEW")
                    {
                        cussup_m_treatment t = new cussup_m_treatment();
                        t.createby = Convert.ToInt32(Session["userid"]);
                        t.staffid = sid;
                        t.createdate = DateTime.Now;
                        t.lastmodifieddate = DateTime.Now;
                        t.cussupid = cussupid;
                        t.cussupname = cussupname;
                        t.description = tlist.ElementAt(i).description ?? "";
                        t.remarks2 = tlist.ElementAt(i).remarks2 ?? "";
                        t.remarks3 = tlist.ElementAt(i).remarks3 ?? "";
                        t.remarks4 = tlist.ElementAt(i).remarks4 ?? "";
                        t.type = tlist.ElementAt(i).type ?? "";
                        t.productcode = item.productcode;
                        t.productdesc = item.productdesc;
                        t.productid = item.productid;
                        t.resourcecode = item.resourcecode;
                        t.resourcedate = item.createdate;
                        t.resourcedetailid = item.id;
                        db.Treatments.Add(t);
                        db.SaveChanges();
                        saveToLog(rid, t.id, "CREATE", "Add New Treatment - Member -" + cussupname, "RefNo no- " + item.resourcecode + ", Item ID- " + item.id.ToString());
                    }
                    else if (tlist.ElementAt(i).statusfield.Trim() == "UPDATE")
                    {
                        int tid = tlist.ElementAt(i).id;
                        cussup_m_treatment t = db.Treatments.Find(tid);
                        string from = ""; string to = "";
                        if (t != null)
                        {
                            from = "product code:" + t.productcode + ", Proudct desc :" + t.productdesc + ", desc:" + t.description;
                            to = "product code:" + item.productcode + ", Proudct desc :" + item.productdesc + ", desc:" + tlist.ElementAt(i).description;
                            t.cussupid = cussupid;
                            t.cussupname = cussupname;
                            t.description = tlist.ElementAt(i).description;
                            t.productcode = item.productcode;
                            t.productdesc = item.productdesc;
                            t.productid = item.productid;
                            db.SaveChanges();
                            saveToLog(rid, t.id, "UPDATE", "Update Detail Item ID-" + t.id.ToString() + " RefNo -" + item.resourcecode, from, to);
                        }
                    }
                }
            }
            catch (Exception e)
            { }
        }
        [Authorize]
        public void AddPackageToRedemptionTable(int itemid, int pid, string pdesc, string uom, int memberid, string resource, string resourcecode, int rid = 0, double qty = 0)
        {
            if (qty > 0)
            {
                CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.productid == pid && x.uom == uom && x.redemptiontype == "PACKAGE").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempt != null)
                { prebalance = redempt.balance; }

                CusSup_m_CusRedemption custRedemptTmp = new CusSup_m_CusRedemption();
                custRedemptTmp.invoiceitemid = itemid;
                custRedemptTmp.cussupid = memberid;
                custRedemptTmp.createdate = DateTime.Now;
                custRedemptTmp.lastmodifieddate = DateTime.Now;
                custRedemptTmp.RefNo = resourcecode; //posdtl.resourcecode;
                custRedemptTmp.productid = pid;
                custRedemptTmp.productdesc = pdesc;
                custRedemptTmp.uom = uom;
                if (resource.ToUpper().Contains("REDEEM") == true)
                {
                    custRedemptTmp.debit = qty;
                    custRedemptTmp.credit = 0;
                }
                else
                {
                    custRedemptTmp.debit = 0;
                    custRedemptTmp.credit = qty;
                }
                custRedemptTmp.redemptiontype = "PACKAGE";
                custRedemptTmp.balance = prebalance + custRedemptTmp.credit - custRedemptTmp.debit;
                custRedemptTmp.createid = Convert.ToInt32(Session["userid"]);// posdtl.createid;                
                custRedemptTmp.resource = resource;
                custRedemptTmp.cocode = Convert.ToString(Session["cocode"]);
                custRedemptTmp.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(custRedemptTmp);
                db.SaveChanges();
                saveToLog(rid, custRedemptTmp.id, "CREATE", "Add New Balance for Award Citi$ - ProductID -" + pid, "Ref no- " + resourcecode + ", Item ID- " + itemid + "Redemption ID -" + custRedemptTmp.id.ToString());
            }
        }

        public void AddToRedemptionTable(Invoice_m_Invoice_Items topupDtl, int memberid, string resource, string resourcecode, int rid = 0)
        {

          
             if (topupDtl.redemptiontype == "SQ")
             {
                 CusSup_m_CusRedemption red = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.productid == topupDtl.productid && x.redemptiontype == "SQ")
                                           .OrderByDescending(x => x.id)
                                           .FirstOrDefault();
                 
                 double? prebalance = red.balance;

                 if (prebalance > 0) {
                     CusSup_m_CusRedemption topuptmp = new CusSup_m_CusRedemption();
                     topuptmp.invoiceitemid = topupDtl.id;
                     topuptmp.cussupid = memberid;
                     topuptmp.createdate = DateTime.Now;
                     topuptmp.lastmodifieddate = DateTime.Now;
                     topuptmp.RefNo = resourcecode; //topupDtl.resourcecode;
                     topuptmp.productid = topupDtl.productid;
                     topuptmp.productdesc = topupDtl.productdesc;
                     topuptmp.debit = topupDtl.qty;
                     topuptmp.credit = 0;
                     topuptmp.redemptiontype = "SQ";
                     topuptmp.balance = prebalance - topupDtl.qty;
                     topuptmp.createid = Convert.ToInt32(Session["userid"]);// topupDtl.createid;
                     //redemp.remark = "Top Up";
                     topuptmp.resource = resource;
                     topuptmp.cocode = Convert.ToString(Session["cocode"]);
                     topuptmp.branchcode = Convert.ToString(Session["branchcode"]);
                     db.CusSupRedemption.Add(topuptmp);
                     db.SaveChanges();
                     saveToLog(rid, topuptmp.id, "CREATE", "Add New Balance for Service Quantity - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topuptmp.id.ToString());
                 }
             }

             if (topupDtl.redeemdollars > 0 && topupDtl.redemptiontype == "C$")
            {
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                CusSup_m_CusRedemption redempt = new CusSup_m_CusRedemption();
                redempt.invoiceitemid = topupDtl.id;
                redempt.cussupid = memberid;
                redempt.createdate = DateTime.Now;
                redempt.lastmodifieddate = DateTime.Now;
                redempt.RefNo = resourcecode; //topupDtl.resourcecode;
                redempt.productid = topupDtl.productid;
                redempt.productdesc = topupDtl.productdesc;
                redempt.credit = 0;
                redempt.debit = (topupDtl.redeemdollars * topupDtl.qty);
                redempt.redemptiontype = "C$";
                redempt.balance = prebalance - redempt.debit;
                redempt.createid = Convert.ToInt32(Session["userid"]);//topupDtl.createid;
                //redemp.remark = "Top Up";
                redempt.resource = resource;
                redempt.cocode = Convert.ToString(Session["cocode"]);
                redempt.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(redempt);
                db.SaveChanges();
                saveToLog(rid, redempt.id, "CREATE", "Add New Balance for Redeem Citi$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + redempt.id.ToString());
            }

             if (topupDtl.redeembonus > 0 && topupDtl.redemptiontype == "B$")
            {
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                CusSup_m_CusRedemption redemptB = new CusSup_m_CusRedemption();
                redemptB.invoiceitemid = topupDtl.id;
                redemptB.cussupid = memberid;
                redemptB.createdate = DateTime.Now;
                redemptB.lastmodifieddate = DateTime.Now;
                redemptB.RefNo = resourcecode; //topupDtl.resourcecode;
                redemptB.productid = topupDtl.productid;
                redemptB.productdesc = topupDtl.productdesc;
                redemptB.credit = 0;
                redemptB.debit = (topupDtl.redeembonus * topupDtl.qty);
                redemptB.redemptiontype = "B$";
                redemptB.balance = prebalance - redemptB.debit;
                redemptB.createid = Convert.ToInt32(Session["userid"]); //topupDtl.createid;
                redemptB.resource = resource;
                redemptB.cocode = Convert.ToString(Session["cocode"]);
                redemptB.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(redemptB);
                db.SaveChanges();
                saveToLog(rid, redemptB.id, "CREATE", "Add New Balance for Redeem Bonus$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + redemptB.id.ToString());
            }

             if (topupDtl.awarddollars > 0 && topupDtl.redemptiontype == "C$")
            {
                CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempt != null)
                { prebalance = redempt.balance; }

                CusSup_m_CusRedemption topuptmp = new CusSup_m_CusRedemption();
                topuptmp.invoiceitemid = topupDtl.id;
                topuptmp.cussupid = memberid;
                topuptmp.createdate = DateTime.Now;
                topuptmp.lastmodifieddate = DateTime.Now;
                topuptmp.RefNo = resourcecode; //topupDtl.resourcecode;
                topuptmp.productid = topupDtl.productid;
                topuptmp.productdesc = topupDtl.productdesc;
                topuptmp.debit = 0;
                topuptmp.credit = (topupDtl.awarddollars * topupDtl.qty);
                topuptmp.redemptiontype = "C$";
                topuptmp.balance = prebalance + topuptmp.credit; //- topupDtl.redeemdollars;
                topuptmp.createid = Convert.ToInt32(Session["userid"]);// topupDtl.createid;
                //redemp.remark = "Top Up";
                topuptmp.resource = resource;
                topuptmp.cocode = Convert.ToString(Session["cocode"]);
                topuptmp.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(topuptmp);
                db.SaveChanges();
                saveToLog(rid, topuptmp.id, "CREATE", "Add New Balance for Award Citi$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topuptmp.id.ToString());
            }

             if (topupDtl.awardbonus > 0 && topupDtl.redemptiontype == "B$")
             {
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                CusSup_m_CusRedemption topupB = new CusSup_m_CusRedemption();
                topupB.invoiceitemid = topupDtl.id;
                topupB.cussupid = memberid;
                topupB.createdate = DateTime.Now;
                topupB.lastmodifieddate = DateTime.Now;
                topupB.RefNo = resourcecode; //topupDtl.resourcecode;
                topupB.productid = topupDtl.productid;
                topupB.productdesc = topupDtl.productdesc;
                topupB.debit = 0;
                topupB.credit = (topupDtl.awardbonus * topupDtl.qty);
                topupB.redemptiontype = "B$";
                topupB.balance = prebalance + topupB.credit;//- topupDtl.redeembonus;
                topupB.createid = Convert.ToInt32(Session["userid"]); //topupDtl.createid;
                topupB.resource = resource;
                topupB.cocode = Convert.ToString(Session["cocode"]);
                topupB.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(topupB);
                db.SaveChanges();
                saveToLog(rid, topupB.id, "CREATE", "Add New Balance for Award Bonus$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topupB.id.ToString());
            }
        }

        public void AddToRedemptionTableQty(Invoice_m_Invoice_Items topupDtl, int memberid, string resource, string resourcecode, int rid = 0)
        {

            CusSup_m_CusRedemption red = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.productid == topupDtl.productid && x.redemptiontype == "SQ")
                .OrderByDescending(x => x.id)
                .FirstOrDefault();
            
            if (red.balance > 0)
            {
                double? prebalance = red.balance;
               
                CusSup_m_CusRedemption topuptmp = new CusSup_m_CusRedemption();
                topuptmp.invoiceitemid = topupDtl.id;
                topuptmp.cussupid = memberid;
                topuptmp.createdate = DateTime.Now;
                topuptmp.lastmodifieddate = DateTime.Now;
                topuptmp.RefNo = resourcecode; //topupDtl.resourcecode;
                topuptmp.productid = topupDtl.productid;
                topuptmp.productdesc = topupDtl.productdesc;
                topuptmp.debit = topupDtl.qty;
                topuptmp.credit = 0;
                topuptmp.redemptiontype = "SQ";
                topuptmp.balance = prebalance - topupDtl.qty;
                topuptmp.createid = Convert.ToInt32(Session["userid"]);// topupDtl.createid;
                //redemp.remark = "Top Up";
                topuptmp.resource = resource;
                topuptmp.cocode = Convert.ToString(Session["cocode"]);
                topuptmp.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(topuptmp);
                db.SaveChanges();
                saveToLog(rid, topuptmp.id, "CREATE", "Add New Balance for Service Quantity - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topuptmp.id.ToString());
            }

            if (topupDtl.awarddollars > 0)
            {
                CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempt != null)
                { prebalance = redempt.balance; }

                CusSup_m_CusRedemption topuptmp = new CusSup_m_CusRedemption();
                topuptmp.invoiceitemid = topupDtl.id;
                topuptmp.cussupid = memberid;
                topuptmp.createdate = DateTime.Now;
                topuptmp.lastmodifieddate = DateTime.Now;
                topuptmp.RefNo = resourcecode; //topupDtl.resourcecode;
                topuptmp.productid = topupDtl.productid;
                topuptmp.productdesc = topupDtl.productdesc;
                topuptmp.debit = 0;
                topuptmp.credit = (topupDtl.awarddollars * topupDtl.qty);
                topuptmp.redemptiontype = "C$";
                topuptmp.balance = prebalance + topuptmp.credit; // - topupDtl.redeemdollars;
                topuptmp.createid = Convert.ToInt32(Session["userid"]);// topupDtl.createid;
                //redemp.remark = "Top Up";
                topuptmp.resource = resource;
                topuptmp.cocode = Convert.ToString(Session["cocode"]);
                topuptmp.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(topuptmp);
                db.SaveChanges();
                saveToLog(rid, topuptmp.id, "CREATE", "Add New Balance for Award Citi$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topuptmp.id.ToString());
            }

            if (topupDtl.awardbonus > 0)
            {
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                CusSup_m_CusRedemption topupB = new CusSup_m_CusRedemption();
                topupB.invoiceitemid = topupDtl.id;
                topupB.cussupid = memberid;
                topupB.createdate = DateTime.Now;
                topupB.lastmodifieddate = DateTime.Now;
                topupB.RefNo = resourcecode; //topupDtl.resourcecode;
                topupB.productid = topupDtl.productid;
                topupB.productdesc = topupDtl.productdesc;
                topupB.debit = 0;
                topupB.credit = (topupDtl.awardbonus * topupDtl.qty);
                topupB.redemptiontype = "B$";
                topupB.balance = prebalance + topupB.credit; // - topupDtl.redeembonus;
                topupB.createid = Convert.ToInt32(Session["userid"]); //topupDtl.createid;
                topupB.resource = resource;
                topupB.cocode = Convert.ToString(Session["cocode"]);
                topupB.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(topupB);
                db.SaveChanges();
                saveToLog(rid, topupB.id, "CREATE", "Add New Balance for Award Bonus$ - ProductID -" + topupDtl.productid, "Ref no- " + resourcecode + ", Item ID- " + topupDtl.id.ToString() + "Redemption ID -" + topupB.id.ToString());
            }
        }


        public void AddToRedemptionTableForEditItem(Invoice_m_Invoice_Items tmp, Invoice_m_Invoice_Items topupDtl, int memberid, string resource, string resourcecode)
        {
            double? prevc = 0;
            double? prevb = 0;

            if (tmp.redeemdollars != topupDtl.redeemdollars)
            {
                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption redempt = new CusSup_m_CusRedemption(); //db.CusSupRedemption.Where(x => x.resourcecode == tmp.resourcecode && x.productid == tmp.productid && x.redemptiontype == "C$").FirstOrDefault();
                redempt.cussupid = memberid;
                redempt.createdate = DateTime.Now;
                redempt.lastmodifieddate = DateTime.Now;
                redempt.RefNo = resourcecode; // topupDtl.resourcecode;
                redempt.productid = topupDtl.productid;
                redempt.productdesc = topupDtl.productdesc;
                redempt.credit = 0;
                redempt.debit = (topupDtl.redeemdollars - tmp.redeemdollars);//topupDtl.awarddollars;
                redempt.redemptiontype = "C$";
                redempt.balance = prebalance - redempt.debit;
                redempt.createid = topupDtl.createid;
                redempt.remark = "Edit " + resource + " Citi$ from -" + tmp.redeemdollars + " - " + topupDtl.redeemdollars;
                redempt.resource = resource;
                redempt.cocode = Convert.ToString(Session["cocode"]);
                redempt.branchcode = Convert.ToString(Session["branchcode"]);
                prevc = redempt.debit;
                db.CusSupRedemption.Add(redempt);
            }

            if (tmp.redeembonus != topupDtl.redeembonus)
            {
                tmp.awardbonus = topupDtl.awardbonus;
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption redemptB = new CusSup_m_CusRedemption(); //db.CusSupRedemption.Where(x => x.resourcecode == tmp.resourcecode && x.productid == tmp.productid && x.redemptiontype == "C$").FirstOrDefault();
                redemptB.cussupid = memberid;
                redemptB.createdate = DateTime.Now;
                redemptB.lastmodifieddate = DateTime.Now;
                redemptB.RefNo = resourcecode; // topupDtl.resourcecode;
                redemptB.productid = topupDtl.productid;
                redemptB.productdesc = topupDtl.productdesc;
                redemptB.credit = 0;
                redemptB.debit = (topupDtl.awardbonus - tmp.awardbonus);//topupDtl.awarddollars;
                redemptB.redemptiontype = "B$";
                redemptB.balance = prebalance - redemptB.debit;
                redemptB.createid = topupDtl.createid;
                redemptB.remark = "Edit " + resource + " Bonus$ from -" + tmp.redeembonus + " - " + topupDtl.redeembonus;
                redemptB.resource = resource;
                redemptB.cocode = Convert.ToString(Session["cocode"]);
                redemptB.branchcode = Convert.ToString(Session["branchcode"]);
                prevb = redemptB.debit;
                db.CusSupRedemption.Add(redemptB);
            }
            if (tmp.awarddollars != topupDtl.awarddollars)
            {
                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption Aredempt = new CusSup_m_CusRedemption(); //db.CusSupRedemption.Where(x => x.resourcecode == tmp.resourcecode && x.productid == tmp.productid && x.redemptiontype == "C$").FirstOrDefault();
                Aredempt.cussupid = memberid;
                Aredempt.createdate = DateTime.Now;
                Aredempt.lastmodifieddate = DateTime.Now;
                Aredempt.RefNo = resourcecode; //topupDtl.resourcecode;
                Aredempt.productid = topupDtl.productid;
                Aredempt.productdesc = topupDtl.productdesc;
                Aredempt.debit = 0;
                Aredempt.credit = (topupDtl.awarddollars - tmp.awarddollars);//topupDtl.awarddollars;
                Aredempt.redemptiontype = "C$";
                Aredempt.balance = prebalance + Aredempt.credit - prevc;
                Aredempt.createid = topupDtl.createid;
                Aredempt.remark = "Edit " + resource + " Citi$ from -" + tmp.awarddollars + " - " + topupDtl.awarddollars;
                Aredempt.resource = resource;
                Aredempt.cocode = Convert.ToString(Session["cocode"]);
                Aredempt.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(Aredempt);
            }

            if (tmp.awardbonus != topupDtl.awardbonus)
            {
                tmp.awardbonus = topupDtl.awardbonus;
                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == memberid && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                tmp.awarddollars = topupDtl.awarddollars;
                CusSup_m_CusRedemption AredemptB = new CusSup_m_CusRedemption(); //db.CusSupRedemption.Where(x => x.resourcecode == tmp.resourcecode && x.productid == tmp.productid && x.redemptiontype == "C$").FirstOrDefault();
                AredemptB.cussupid = memberid;
                AredemptB.createdate = DateTime.Now;
                AredemptB.lastmodifieddate = DateTime.Now;
                AredemptB.RefNo = resourcecode; // topupDtl.resourcecode;
                AredemptB.productid = topupDtl.productid;
                AredemptB.productdesc = topupDtl.productdesc;
                AredemptB.debit = 0;
                AredemptB.credit = (topupDtl.awardbonus - tmp.awardbonus);//topupDtl.awarddollars;
                AredemptB.redemptiontype = "B$";
                AredemptB.balance = prebalance + AredemptB.credit - prevb;
                AredemptB.createid = topupDtl.createid;
                AredemptB.remark = "Edit " + resource + " Bonus$ from -" + tmp.awardbonus + " - " + topupDtl.awardbonus;
                AredemptB.resource = resource;
                AredemptB.cocode = Convert.ToString(Session["cocode"]);
                AredemptB.branchcode = Convert.ToString(Session["branchcode"]);
                db.CusSupRedemption.Add(AredemptB);
            }
        }

        public void UpdateTopupDetailItem(Invoice_m_Invoice_Items topupDtl, int memberid, string resource, string resourcecode, int rid, string prevStatus, string newStatus, Boolean pkgFlag = false, string membername = "")
        {
            Invoice_m_Invoice_Items tmp = db.saleItems.Find(topupDtl.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ",commission-" + tmp.servicecommission.ToString() + ",RD-" + tmp.redeemdollars.ToString() + ", RC -" + tmp.redeembonus.ToString() + ",AD -" + tmp.awarddollars.ToString() + ",AB-" + tmp.awardbonus.ToString();
                to = "proudctid -" + topupDtl.productid + ",product desc -" + topupDtl.productdesc + ",Price -" + topupDtl.unitprice.ToString() + ",commission-" + topupDtl.servicecommission.ToString() + ",RD-" + topupDtl.redeemdollars.ToString() + ", RC -" + topupDtl.redeembonus.ToString() + ",AD -" + topupDtl.awarddollars.ToString() + ",AB-" + topupDtl.awardbonus.ToString();
                if (newStatus == "CLOSE")
                {
                    if (prevStatus == "ACTIVE")
                    {
                        if (pkgFlag)
                            AddPackageToRedemptionTable(topupDtl.id, topupDtl.productid, topupDtl.productdesc, topupDtl.uom, memberid, resource, resourcecode, rid, topupDtl.qty);
                        else
                            AddToRedemptionTable(topupDtl, memberid, resource, resourcecode, rid);
                    }
                    else
                    {
                        AddToRedemptionTableForEditItem(tmp, topupDtl, memberid, resource, resourcecode);
                    }
                }
                tmp.redeemdollars = topupDtl.redeemdollars;
                tmp.redeembonus = topupDtl.redeembonus;
                tmp.awarddollars = topupDtl.awarddollars;
                tmp.awardbonus = topupDtl.awardbonus;
                tmp.servicecommission = topupDtl.servicecommission;
                tmp.unitprice = topupDtl.unitprice;
                tmp.productid = topupDtl.productid;
                tmp.productcode = topupDtl.productcode;
                tmp.productdesc = topupDtl.productdesc;
                tmp.qty = topupDtl.qty;
                tmp.lineamount = topupDtl.qty * topupDtl.unitprice;
                tmp.staffserviceid = topupDtl.staffserviceid;
                db.SaveChanges();
                saveToLog(rid, topupDtl.id, "UPDATE", "Update Detail Item ID-" + topupDtl.id.ToString() + " RefNo -" + resourcecode, from, to);
                if (topupDtl.treatment != null)
                    SaveTreatment(topupDtl, memberid, membername, rid);
            }
        }
      
        public void UpdateTopupDetailItemQty(Invoice_m_Invoice_Items topupDtl, int memberid, string resource, string resourcecode, int rid, string prevStatus, string newStatus, Boolean pkgFlag = false, string membername = "")
        {
            Invoice_m_Invoice_Items tmp = db.saleItems.Find(topupDtl.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ",commission-" + tmp.servicecommission.ToString() + ",RD-" + tmp.redeemdollars.ToString() + ", RC -" + tmp.redeembonus.ToString() + ",AD -" + tmp.awarddollars.ToString() + ",AB-" + tmp.awardbonus.ToString();
                to = "proudctid -" + topupDtl.productid + ",product desc -" + topupDtl.productdesc + ",Price -" + topupDtl.unitprice.ToString() + ",commission-" + topupDtl.servicecommission.ToString() + ",RD-" + topupDtl.redeemdollars.ToString() + ", RC -" + topupDtl.redeembonus.ToString() + ",AD -" + topupDtl.awarddollars.ToString() + ",AB-" + topupDtl.awardbonus.ToString();
                if (newStatus == "CLOSE")
                {
                    if (prevStatus == "ACTIVE")
                    {
                        if (pkgFlag)
                            AddPackageToRedemptionTable(topupDtl.id, topupDtl.productid, topupDtl.productdesc, topupDtl.uom, memberid, resource, resourcecode, rid, topupDtl.qty);
                        else
                            AddToRedemptionTableQty(topupDtl, memberid, resource, resourcecode, rid);
                    }
                    else
                    {
                        AddToRedemptionTableForEditItem(tmp, topupDtl, memberid, resource, resourcecode);
                    }
                }
                tmp.redeemdollars = topupDtl.redeemdollars;
                tmp.redeembonus = topupDtl.redeembonus;
                tmp.awarddollars = topupDtl.awarddollars;
                tmp.awardbonus = topupDtl.awardbonus;
                tmp.servicecommission = topupDtl.servicecommission;
                tmp.unitprice = topupDtl.unitprice;
                tmp.productid = topupDtl.productid;
                tmp.productcode = topupDtl.productcode;
                tmp.productdesc = topupDtl.productdesc;
                tmp.qty = topupDtl.qty;
                tmp.lineamount = topupDtl.qty * topupDtl.unitprice;
                tmp.staffserviceid = topupDtl.staffserviceid;
                db.SaveChanges();
                saveToLog(rid, topupDtl.id, "UPDATE", "Update Detail Item ID-" + topupDtl.id.ToString() + " RefNo -" + resourcecode, from, to);
                if (topupDtl.treatment != null)
                    SaveTreatment(topupDtl, memberid, membername, rid);
            }
        }


        [HttpPost]
        public JsonResult SaveToRedemptionTable(CusSup_m_CusRedemption r, string rcode = "Adjustment")
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == r.cussupid && x.redemptiontype == r.redemptiontype).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (redempttmp != null)
                { prebalance = redempttmp.balance; }

                r.cocode = Session["cocode"].ToString();
                r.branchcode = Session["branchcode"].ToString();
                r.createdate = DateTime.Now;
                r.createid = Convert.ToInt32(Session["userid"].ToString());
                r.RefNo = GeneralController.getGeneratedNewID("CusSup_m_CusRedemption", "RefNo", "ADJPREFIX", "ADJRDP");
                r.balance = (r.credit - r.debit) + prebalance;
                db.CusSupRedemption.Add(r);
                db.SaveChanges();
                saveToLog(rid, r.id, "CREATE", "Add Adjustment, Ref No-" + r.RefNo + ", Type -" + r.redemptiontype + ", Redemption ID - " + r.id.ToString() + ",Debit -" + r.debit.ToString() + ", Credit -" + r.credit.ToString());
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                returnStr = e.Message.ToString();
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public Boolean saveToLog(int resourceid, int resourcecode, string logtype, string description, string from = "", string to = "", string ip = "")
        {
            GeneralController gc = new GeneralController();
            int uid = Convert.ToInt32(Session["userid"].ToString());
            string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            return gc.SaveToLog(uid, Session["cocode"].ToString(), Session.SessionID, resourceid, resourcecode, logtype, description, from, to, visitorIPAddress);
        }

        public ActionResult TopUpOld(int id = 0)
        {
            //try
            //{
            //    int i = 0;
            //    string prddesc = "";
            //    ViewBag.CusSupOptions = db.CusSup.Where(x => x.status == "Active" && x.cussuptype == "Customer").ToList();
            //    ViewBag.StatusOptions = db.CommonStatus.ToList();
            //    ViewBag.PaymentModeOptions = db.PaymentModes.ToList();
            //    ViewBag.StaffOptions = db.Staffs.ToList();

            //    ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
            //    for (i = 0; i < branches.Count; i++)
            //    {
            //        branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            //    }
            //    ViewBag.BranchOptions = branches;
            //    ICollection<Configuration_m_Company> companies = db.Companies.ToList();
            //    for (i = 0; i < companies.Count; i++)
            //    {
            //        companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
            //    }
            //    ViewBag.CompanyOptions = companies;
            //    var uid = Convert.Toint(Session["userid"].ToString());
            //    //uid =db.Staffs.Where(x => x.userid == uid).FirstOrDefault().id; 
            //    var uname = db.Users.Where(x => x.id == uid).FirstOrDefault().username;
            //    ViewBag.UserName = cAESEncryption.getDecryptedString(uname);
            //    var sid = 0;
            //    try
            //    {
            //        sid = db.Staffs.Where(x => x.userid == uid).FirstOrDefault().id;
            //    } catch (Exception e1)
            //    { sid = 0;  }
            //    ViewBag.StaffID = sid;

            //    if (id == 0)
            //    {
            //        Invoice_m_Invoice s = new Invoice_m_Invoice();
            //        s.items = db.saleItems.Where(x => x.invoiceid == 0).OrderBy(x => x.lineno).AsEnumerable();
            //        s.createdate = DateTime.Today;
            //        s.createid = uid;

            //        //s.Create.name = db.Users.Where(x => x.id == s.createid).FirstOrDefault().username; 
            //        var b = db.ConfigDefault.Where(x => x.key == "BRID").FirstOrDefault().value;
            //        s.branchcode=b;                   
            //        s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
            //        var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;
            //        var pidtmp = db.ConfigDefault.Where(x => x.key == "CITI").FirstOrDefault().value;
            //        int pid = 0;

            //        if (pidtmp.Length > 0)  { pid = Convert.Toint(pidtmp); }
            //        prddesc = db.products.Where(x => x.id ==pid).FirstOrDefault().desc;
            //        ViewBag.proddesc = prddesc;
            //        if (ex.Length > 0)
            //            s.exchangerate = Convert.ToDouble(ex);
            //        else
            //            s.exchangerate = 0;

            //        //Invoice_m_Invoice_Items itm = new Invoice_m_Invoice_Items();
            //        //itm.productid = Convert.Toint(pid);
            //        //itm.productdesc = prddesc;
            //        //s.topup = itm;
            //        //s.items.Add(itm);

            //        return View(s);
            //    }
            //    else
            //    {
            //        Invoice_m_Invoice s = db.sales.Find(id);
            //        s.items = db.saleItems.Where(x => x.invoiceid == id).OrderBy(x => x.lineno).AsEnumerable();
            //        return View(s);
            //    }
            //}
            //catch (Exception e)
            //{
            //    return View();
            //}
            return View();
        }

        [HttpPost]
        public ActionResult TopUpOldSave(IList<MultiPayment> dtls, int invoiceid, string branchcode, int createid, int receiveid, int customerid, double amount)
        {
            //try
            //{
            //    int i=0;
            //    Invoice_m_Invoice s = new Invoice_m_Invoice();
            //    s.id = invoiceid;
            //    s.branchcode = branchcode;
            //    s.createid = createid;
            //    s.cussupid = customerid;
            //    s.total_total = amount;
            //    if (s.id ==0)
            //    {
            //        s.cussupname = db.CusSup.Where(x => x.id == s.cussupid).FirstOrDefault().cussupname;
            //        s.cocode = db.Branches.Where(x => x.branchcode == s.branchcode).FirstOrDefault().cocode;
            //        var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

            //        if (ex.Length > 0)
            //            s.exchangerate = Convert.ToDouble(ex);
            //        else
            //            s.exchangerate = 0;
            //        //s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
            //        s.createdate = DateTime.Now;
            //        s.lastmodifieddate = DateTime.Now;
            //        s.resourcecode ="00000";
            //        s.total_subtotal = s.total_total;
            //        s.total_salestax = 0;
            //        s.total_discount = 0;
            //        s.total_amountreceived = s.total_total;
            //        s.total_amountvoid = 0;
            //        s.total_amountrefund = 0;                    
            //        s.aracctid = db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
            //        s.salestaxacctid = db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
            //        s.discountacctid = db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
            //        s.status = "Active";
            //        s = db.sales.Add(s); 
            //        db.SaveChanges();                     
            //        Invoice_m_Invoice_Items itm = new Invoice_m_Invoice_Items();
            //        itm.lineno = 1;
            //        itm.resourcecode = s.resourcecode;
            //        itm.invoiceid = s.id;
            //        itm.unitprice = s.total_total;
            //        itm.discountamount = 0;
            //        itm.taxamount = 0;
            //        itm.lineamount = s.total_total;

            //        var pidtmp = db.ConfigDefault.Where(x => x.key == "CITI").FirstOrDefault().value;
            //        int pid = 0;
            //        if (pidtmp.Length > 0)  { pid = Convert.Toint(pidtmp); }
            //        Product_m_Product p= db.products.Where(x => x.id == pid).FirstOrDefault();                    
            //        itm.productid = pid;
            //        itm.productdesc = p.desc;
            //        itm.productcode = p.productcode;
            //        itm.createid = s.createid;
            //        itm.createdate = DateTime.Now;
            //        itm.lastmodifieddate = DateTime.Now;
            //        itm.qty = 1;
            //        itm.currency = s.currency;
            //        itm.exchangerate = s.exchangerate;
            //        itm.gstcode = db.ConfigDefault.Where(x => x.key == "IGST").FirstOrDefault().value;
            //        db.saleItems.Add(itm);
            //        db.SaveChanges();
            //        ViewBag.proddesc = itm.productdesc;

            //        for(i=0;i<dtls.Count ;i++)
            //        {
            //            Invoice_m_Payment pay = new Invoice_m_Payment();
            //            pay.createdate = DateTime.Now;
            //            pay.lastmodifieddate = DateTime.Now; 
            //            pay.invoiceid = s.id;
            //            pay.cocode = s.cocode;
            //            pay.branchcode = s.branchcode;
            //            pay.resourcecode = s.resourcecode;
            //            pay.paymentmode = dtls[i].paymentmode;
            //            pay.amountrecd = dtls[i].amountrecd;
            //            pay.createid = s.createid;
            //            pay.receivedid = s.createid;
            //            pay.status = "Paid";
            //            db.invoicePayment.Add(pay);
            //            db.SaveChanges();
            //        }


            //        CusSup_m_CusRedemption redemp = new CusSup_m_CusRedemption();
            //        redemp.cussupid = s.cussupid;
            //        redemp.createdate = DateTime.Now;
            //        redemp.lastmodifieddate = DateTime.Now;
            //        redemp.resourcecode = s.resourcecode;
            //        redemp.productid = pid;
            //        redemp.productdesc = itm.productdesc;
            //        redemp.credit = s.total_total;
            //        redemp.debit = 0;
            //        redemp.balance = redemp.credit - redemp.debit;
            //        redemp.createid = s.createid;
            //        //redemp.remark = "Top Up";
            //        redemp.resource = "Top Up";
            //        redemp.cocode = s.cocode;
            //        redemp.branchcode = s.branchcode;
            //        db.CusSupRedemption.Add(redemp);
            //        db.SaveChanges();
            //    }
            //}
            //catch (Exception e)
            //{

            //}
            return View();
        }
        //[HttpPost]
        //public ActionResult TopUp(Invoice_m_Invoice s)
        //{
        //    try
        //    {
        //        if (s.id ==0)
        //        {
        //            s.cussupname = db.CusSup.Where(x => x.id == s.cussupid).FirstOrDefault().cussupname;
        //            s.cocode = db.Branches.Where(x => x.id == s.branchcode).FirstOrDefault().cocode;
        //            var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

        //            if (ex.Length > 0)
        //                s.exchangerate = Convert.ToDouble(ex);
        //            else
        //                s.exchangerate = 0;
        //            //s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
        //            s.createdate = DateTime.Now;
        //            s.lastmodifieddate = DateTime.Now;
        //            s.resourcecode ="00000";
        //            s.total_subtotal = s.total_total;
        //            s.total_salestax = 0;
        //            s.total_discount = 0;
        //            s.total_amountreceived = s.total_total;
        //            s.total_amountvoid = 0;
        //            s.total_amountrefund = 0;                    
        //            s.aracctid = db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
        //            s.salestaxacctid = db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
        //            s.discountacctid = db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
        //            s.status = "Active";
        //            s = db.sales.Add(s); 
        //            db.SaveChanges();                     
        //            Invoice_m_Invoice_Items itm = new Invoice_m_Invoice_Items();
        //            itm.lineno = 1;
        //            itm.resourcecode = "00000";
        //            itm.invoiceid = s.id;
        //            itm.unitprice = s.total_total;
        //            itm.discountamount = 0;
        //            itm.taxamount = 0;
        //            itm.lineamount = s.total_total;

        //            var pidtmp = db.ConfigDefault.Where(x => x.key == "CITI").FirstOrDefault().value;
        //            int pid = 0;
        //            if (pidtmp.Length > 0)  { pid = Convert.Toint(pidtmp); }
        //            Product_m_Product p= db.products.Where(x => x.id == pid).FirstOrDefault();                    
        //            itm.productid = pid;
        //            itm.productdesc = p.desc;
        //            itm.productcode = p.productcode;
        //            itm.createid = s.createid;
        //            itm.createdate = DateTime.Now;
        //            itm.lastmodifieddate = DateTime.Now;
        //            itm.qty = 1;
        //            itm.currency = s.currency;
        //            itm.exchangerate = s.exchangerate;
        //            itm.gstcode = db.ConfigDefault.Where(x => x.key == "IGST").FirstOrDefault().value;
        //            db.saleItems.Add(itm);
        //            db.SaveChanges();
        //            ViewBag.proddesc = itm.productdesc;

        //            Invoice_m_Payment pay = new Invoice_m_Payment();
        //            pay.createdate = DateTime.Now;
        //            pay.lastmodifieddate = DateTime.Now; 
        //            pay.invoiceid = s.id;
        //            pay.cocode = s.cocode;
        //            pay.branchcode = s.branchcode;
        //            pay.resourcecode = "00000";
        //            pay.paymentmode = "Cash";
        //            pay.amountrecd = s.total_total;
        //            pay.createid = s.createid;
        //            pay.receivedid = s.createid;
        //            pay.status = "Paid";
        //            db.invoicePayment.Add(pay);
        //            db.SaveChanges();

        //            CusSup_m_CusRedemption redemp = new CusSup_m_CusRedemption();
        //            redemp.cussupid = s.cussupid;
        //            redemp.createdate = DateTime.Now;
        //            redemp.lastmodifieddate = DateTime.Now;
        //            redemp.productid = pid;
        //            redemp.productdesc = itm.productdesc;
        //            redemp.credit = s.total_total;
        //            redemp.debit = 0;
        //            redemp.balance = redemp.credit - redemp.debit;
        //            redemp.createid = s.createid;
        //            redemp.remark = "Top Up";
        //            redemp.resource = "00000";
        //            redemp.cocode = s.cocode;
        //            redemp.branchcode = s.branchcode;
        //            db.CusSupRedemption.Add(redemp);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    int i = 0;
        //    ViewBag.CusSupOptions = db.CusSup.Where(x => x.status == "Active" && x.cussuptype == "Customer").ToList();
        //    ViewBag.StatusOptions = db.CommonStatus.ToList();
        //    ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
        //    for (i = 0; i < branches.Count; i++)
        //    {
        //        branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
        //    }
        //    ViewBag.BranchOptions = branches;
        //    ICollection<Configuration_m_Company> companies = db.Companies.ToList();
        //    for (i = 0; i < companies.Count; i++)
        //    {
        //        companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
        //    }
        //    ViewBag.CompanyOptions = companies;
        //    var uid = Convert.Toint(Session["userid"].ToString());
        //    var uname = db.Users.Where(x => x.id == uid).FirstOrDefault().username;
        //    ViewBag.StaffName = cAESEncryption.getDecryptedString(uname); 
        //    return View(s);

        //}
        public ActionResult Invoice(int id = 0)
        {
            try
            {
                int i = 0;
                ViewBag.CusSupOptions = db.CusSup.Where(x => x.status == "Active").ToList();
                ViewBag.StatusOptions = db.CommonStatus.ToList();
                ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
                for (i = 0; i < branches.Count; i++)
                {
                    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                }
                ViewBag.BranchOptions = branches;
                ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                for (i = 0; i < companies.Count; i++)
                {
                    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = companies;

                if (id == 0)
                {
                    Invoice_m_Invoice s = new Invoice_m_Invoice();
                    s.items = db.saleItems.Where(x => x.invoiceid == 0).OrderBy(x => x.lineno).ToList();
                    s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
                    var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;
                    if (ex.Length > 0)
                        s.exchangerate = Convert.ToDouble(ex);
                    else
                        s.exchangerate = 0;
                    return View(s);
                }
                else
                {
                    Invoice_m_Invoice s = db.sales.Find(id);
                    s.items = db.saleItems.Where(x => x.invoiceid == id).OrderBy(x => x.lineno).ToList();
                    return View(s);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult Invoice(Invoice_m_Invoice s)
        {
            int i = 0;
            ViewBag.CusSupOptions = db.CusSup.Where(x => x.status == "Active").ToList();
            ViewBag.StatusOptions = db.CommonStatus.ToList();
            ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;
            ICollection<Configuration_m_Company> companies = db.Companies.ToList();
            for (i = 0; i < companies.Count; i++)
            {
                companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
            }
            ViewBag.CompanyOptions = companies;

            return View(s);
        }

        //public ActionResult InvoiceDetail(int invoiceid = 0)
        //{
        //    try
        //    {
        //        ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
        //        ViewBag.UOMOptions = db.UOM.ToList();
        //        ViewBag.INVItemOptions = db.saleItems.Where(x => x.invoiceid == invoiceid).OrderBy(x => x.lineno).ToList();
        //        Invoice_m_Invoice_Items s = new Invoice_m_Invoice_Items();
        //        s.invoiceid = invoiceid;
        //        s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
        //        s.gstcode = db.ConfigDefault.Where(x => x.key == "IGST").FirstOrDefault().value;
        //        var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

        //        if (ex.Length > 0)
        //            s.exchangerate = Convert.ToDouble(ex);
        //        else
        //            s.exchangerate = 0;
        //        return View(s);

        //    }
        //    catch (Exception e)
        //    {
        //        return View();
        //    }
        //}

        public ActionResult InvoiceDetail(int itemid = 0, int invoiceid = 0)
        {
            try
            {
                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
                ViewBag.UOMOptions = db.UOM.ToList();
                //ViewBag.POItemOptions = db.purchaseItems.Where(x => x.purchaseid == purchaseid).OrderBy(x => x.lineno).ToList();
                Invoice_m_Invoice_Items s;
                if (itemid == 0)
                {
                    s = new Invoice_m_Invoice_Items();
                    s.invoiceid = invoiceid;
                    s.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
                    s.gstcode = db.ConfigDefault.Where(x => x.key == "IGST").FirstOrDefault().value;
                    var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

                    if (ex.Length > 0)
                        s.exchangerate = Convert.ToDouble(ex);
                    else
                        s.exchangerate = 0;
                }
                else
                {
                    s = db.saleItems.Find(itemid);
                }
                return View(s);

            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult SaveInvoiceDetail(Invoice_m_Invoice_Items s)
        {
            return View(s);
        }

        public ActionResult InvoicePayment(int invoiceid = 0)
        {
            try
            {
                int i = 0;
                ViewBag.PaymentStatusOptions = db.PaymentStatus.ToList();
                ViewBag.ResourceOptions = db.Resources.ToList();
                ViewBag.PaymentItemOptions = db.invoicePayment.Where(x => x.invoiceid == invoiceid).OrderBy(x => x.id).ToList();
                ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
                for (i = 0; i < branches.Count; i++)
                {
                    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                }
                ViewBag.BranchOptions = branches;
                ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                for (i = 0; i < companies.Count; i++)
                {
                    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = companies;
                Invoice_m_Payment p = new Invoice_m_Payment();
                p.invoiceid = invoiceid;
                return View(p);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SaveInvoicePayment(Invoice_m_Payment p)
        {
            return View(p);
        }

        //Redeem Packaage
        [Authorize]
        public ActionResult RedeemPackage(int id = 0, int cardno = 0, string rcode = "PACKAGE REDEEM")
        {
            Invoice_m_Invoice topup = new Invoice_m_Invoice();
            try
            {
                //ViewBag.StaffOptions = db.Staffs.Select(x=> new {x.id,x.name }).ToList();
                string bcode = Session["branchcode"].ToString();
                ViewBag.StaffOptions = db.BranchStaff.Join(db.Staffs, bs => bs.staffid, s => s.id, (bs, s) => new { BranchStaff = bs, Staffs = s }).Where(x => x.BranchStaff.branchcode == bcode).Select(x => new { x.Staffs.id, x.Staffs.name }).ToList();

                //ICollection<CusSup_m_CusSupdtl> mList;

                ViewBag.RCode = rcode;
                ViewBag.CardNo = "";
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                topup.cussupid = 0;
                string CoCode = Session["cocode"].ToString();
                Configuration_m_Company co = db.Companies.Where(x => x.cocode == CoCode).FirstOrDefault();
                ViewBag.CoRegNo = cAESEncryption.getDecryptedString(co.coregno);
                ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(co.gstregno);
                Configuration_m_Branches br = db.Branches.Where(x => x.branchcode == bcode).FirstOrDefault();
                ViewBag.CoName = cAESEncryption.getDecryptedString(br.branchname);
                if (br.address != null)
                {
                    if (br.address == "")
                    {
                        ViewBag.CoAddress = "-";
                    }
                    else
                    {
                        ViewBag.CoAddress = cAESEncryption.getDecryptedString(br.address);
                    }
                }
                else
                {
                    ViewBag.CoAddress = "-";
                }
                if (br.tel != null)
                {
                    if (br.tel == "")
                    {
                        ViewBag.Tel = "-";
                    }
                    else
                    {
                        ViewBag.Tel = cAESEncryption.getDecryptedString(br.tel);
                    }
                }
                else
                {
                    ViewBag.Tel = "-";
                }
                if (br.fax != null)
                {
                    if (br.fax == "")
                    {
                        ViewBag.Fax = "-";
                    }
                    else
                    {
                        ViewBag.Fax = cAESEncryption.getDecryptedString(br.fax);
                    }
                }
                else
                {
                    ViewBag.Fax = "-";
                }
                if (br.email != null)
                {
                    if (br.email == "")
                    {
                        ViewBag.Email = "-";
                    }
                    else
                    {
                        ViewBag.Email = cAESEncryption.getDecryptedString(br.email);
                    }
                }
                else
                {
                    ViewBag.Email = "-";
                }
                int intGSTReg = br.gstreg;
                if (intGSTReg == 1)
                {
                    ViewBag.GSTRegNo = cAESEncryption.getDecryptedString(br.gstregno);
                }
                else
                {
                    ViewBag.GSTRegNo = "-";
                }

             
                if (cardno > 0)
                {
                    var cardtmp = db.MemberCard.Where(x => x.cardno == cardno && x.expirydate >= DateTime.Today).FirstOrDefault();
                    if (cardtmp != null)
                    { topup.cussupid = cardtmp.cussupid; }
                }

                if (id == 0)
                {
                    //Invoice_m_Invoice topup = new Invoice_m_Invoice();                    
                    topup.status = "";
                    topup.resourcecode = "";
                    ViewBag.staffname = Session["staffname"].ToString();
                    int sid = 0;
                    int.TryParse(Session["staffid"].ToString(), out sid);
                    topup.staffid = sid;
                    return View(topup);
                }
                else
                {
                    //Invoice_m_Invoice topup = db.sales.Find(id);
                    topup = db.sales.Find(id);
                    int mid = topup.cussupid;
                    string tpno = topup.resourcecode;
                    try
                    {
                        CusSup_m_CusRedemption preCbal = db.CusSupRedemption.Where(x => x.cussupid == mid && x.RefNo != tpno && x.redemptiontype == "C$").OrderByDescending(x => x.id).FirstOrDefault();
                        if (preCbal != null)
                        {
                            ViewBag.obc = preCbal.balance;
                        }
                    }
                    catch (Exception e1)
                    { ViewBag.obc = 0; }

                    try
                    {
                        CusSup_m_CusRedemption preBbal = db.CusSupRedemption.Where(x => x.cussupid == topup.cussupid && x.RefNo != topup.resourcecode && x.redemptiontype == "B$").OrderByDescending(x => x.id).FirstOrDefault();
                        if (preBbal != null)
                        {
                            ViewBag.obb = preBbal.balance;
                        }

                    }
                    catch (Exception e2)
                    { ViewBag.obb = 0; }

                    if (topup.staffid > 0)
                    {
                        Common_m_Staff s = db.Staffs.Find(topup.staffid);
                        if (s != null)
                        { ViewBag.staffname = s.name; }
                        else
                            ViewBag.staffname = "";
                    }
                    else
                        ViewBag.staffname = "";

                    return View(topup);
                }
            }
            catch (Exception e)
            {
                return View(topup);
            }
        }

        public JsonResult getMemberRedeemPackageList(int mid = 0)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var pList = context.Database.SqlQuery<pkgRedeemProduct>("select custredempt.id,custredempt.productid, prod.productcode , prod.desc productdesc, custredempt.uom, custredempt.balance, price.sellprice, price.servicecommission from (select C.id,C.productid, uom,C.balance,FIND_IN_SET(C.id, (select group_concat(redem.id order by redem.id desc)from CusSup_m_CusRedemption redem where redem.productid = C.productid and redem.uom = C.uom and redem.cussupid = " + mid + " and redem.redemptiontype = 'PACKAGE')) rowno from CusSup_m_CusRedemption C where cussupid = " + mid + " and c.redemptiontype  = 'PACKAGE') as  custredempt inner join product_m_product prod on custredempt.productid = prod.id inner join product_m_productprice price on custredempt.productid = price.productid and custredempt.uom = price.uom where rowno =1 and balance > 0").ToList(); //.Select(x => new { x.id }).ToList();

                    //expandoObj.resourcecode, expandoObj.branchcode, expandoObj.createid, expandoObj.productid, expandoObj.productcode, expandoObj.productdesc, expandoObj.qty, expandoObj.uom, expandoObj.redeemdollars, expandoObj.redeembonus, expandoObj.awarddollars, expandoObj.awardbonus }).ToList();
                    return Json(pList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        [Authorize]
        public JsonResult RedeemPackageSave(Invoice_m_Invoice redeem, string resource = "PACKAGE REDEEM", string itemids = "")
        {
            var returnStr = "FAIL";
            int itemcount = 0;
            if (Session["userid"] != null)
            {
                try
                {
                    string curr = "";
                    double exrate = 0;
                    ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                    if (config.Count > 0)
                        curr = config.ElementAt(0).value;
                    else
                        curr = "";

                    config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                    if (config.Count > 0)
                        exrate = Convert.ToDouble(config.ElementAt(0).value);
                    else
                        exrate = 0;

                    var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                    int rid = 0;
                    if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                    if (redeem.id == 0)
                    {
                        redeem.createdate = DateTime.Now;
                        redeem.lastmodifieddate = DateTime.Now;
                        redeem.cocode = Convert.ToString(Session["cocode"]);
                        redeem.createid = Convert.ToInt32(Session["userid"]);
                        redeem.branchcode = Convert.ToString(Session["branchcode"]);
                        redeem.currency = curr;
                        redeem.exchangerate = exrate;
                        redeem.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        redeem.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        redeem.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        redeem.total_amountrefund = 0;
                        redeem.total_amountvoid = 0;
                        redeem.total_discount = 0;
                        redeem.total_salestax = 0;
                        redeem.total_discount = 0;
                        redeem.resourcecode = GeneralController.getGeneratedNewID("Invoice_m_Invoice", "resourcecode", "RDPREFIX", "INVREDEEM");
                        db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, redeem.id, "CREATE", "Add New Redeem Package for cust id - " + redeem.cussupid.ToString() + ", Refno-" + redeem.resourcecode.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), "Redeem Ref no- " + redeem.resourcecode + ", ID- " + redeem.id.ToString());

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                SaveNewTopupDetailItem(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, redeem.resourcecode, rid, curr, exrate, redeem.status, true);
                            }
                            //db.SaveChanges();
                        }

                    }
                    else
                    {
                        Invoice_m_Invoice Redeemtmp = db.sales.Find(redeem.id);
                        string from = ""; string to = "";
                        string prevStatus = Redeemtmp.status.ToUpper();
                        string newStatus = redeem.status.ToUpper();
                        from = "Status -" + Redeemtmp.status + ", Create By -" + Redeemtmp.staffid + ", total -" + Redeemtmp.total_total.ToString();
                        to = "Status -" + redeem.status + ", Create By -" + redeem.staffid + ", total -" + Redeemtmp.total_total.ToString() + ", items -" + itemids;
                        Redeemtmp.cocode = Convert.ToString(Session["cocode"]);
                        Redeemtmp.branchcode = Convert.ToString(Session["branchcode"]);
                        Redeemtmp.currency = curr;
                        Redeemtmp.exchangerate = exrate;
                        Redeemtmp.aracctid = ""; // db.ConfigDefault.Where(x => x.key == "ARID").FirstOrDefault().value;
                        Redeemtmp.salestaxacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARGD").FirstOrDefault().value;
                        Redeemtmp.discountacctid = ""; // db.ConfigDefault.Where(x => x.key == "ARDD").FirstOrDefault().value;
                        Redeemtmp.total_amountrefund = 0;
                        Redeemtmp.total_amountvoid = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.total_salestax = 0;
                        Redeemtmp.total_discount = 0;
                        Redeemtmp.status = redeem.status;
                        Redeemtmp.staffid = redeem.staffid;
                        redeem.resourcecode = Redeemtmp.resourcecode;
                        //db.sales.Add(redeem);
                        db.SaveChanges();
                        if (redeem.items != null) itemcount = redeem.items.Count();
                        saveToLog(rid, Redeemtmp.id, "UPDATE", "UPDATE Redeem Package- refno -" + Redeemtmp.resourcecode + " , id - " + Redeemtmp.id.ToString() + ", Item Count -" + itemcount.ToString() + ", Total - " + redeem.total_total.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from Invoice_m_Invoice_Items where invoiceid=" + redeem.id.ToString() + " and id not in (" + itemids + ")");
                            //return View(pList);
                        }

                        if (redeem.items != null)
                        {
                            for (int i = 0; i < (redeem.items.Count()); i++)
                            {
                                if (redeem.items.ElementAt(i).id == 0)
                                {
                                    SaveNewTopupDetailItem(redeem.items.ElementAt(i), redeem.id, redeem.cussupid, resource, Redeemtmp.resourcecode, rid, curr, exrate, redeem.status, true);
                                }
                                else
                                {
                                    UpdateTopupDetailItem(redeem.items.ElementAt(i), redeem.cussupid, resource, Redeemtmp.resourcecode, rid, prevStatus, newStatus, true);
                                }
                            }
                            //db.SaveChanges();
                        }
                    }
                    //returnStr = "SUCCESS";
                    returnStr = "SUCCESS" + "," + redeem.id.ToString() + "," + redeem.resourcecode.ToString() + "," + redeem.status;
                    //returnStr = redeem.id.ToString();
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSIONEXPIRED";
            }
            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult RedeemPackageSavetoCussupRedemption(CusSup_m_CusRedemption redeem, string resource = "PACKAGE REDEEM")
        {
            var returnStr = "FAIL";
            if (Session["userid"] != null)
            {


             //   return Json("No allow more than max redeemable quantity", JsonRequestBehavior.AllowGet);
                
                try
                {
                    string curr = "";
                    double exrate = 0;
                    ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                    if (config.Count > 0)
                        curr = config.ElementAt(0).value;
                    else
                        curr = "";

                    config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                    if (config.Count > 0)
                        exrate = Convert.ToDouble(config.ElementAt(0).value);
                    else
                        exrate = 0;

                    var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                    int rid = 0;
                    if (redeem.id == 0)
                    {
                        CusSup_m_CusRedemption redempt = db.CusSupRedemption.Where(x => x.cussupid == redeem.cussupid && x.productid == redeem.productid && x.redemptiontype == "PQ" && x.uom == redeem.uom && x.resource == "PACKAGEREDEEM" || x.resource == "CustomerTransfer").OrderByDescending(x => x.id).FirstOrDefault();
                        double? prebalance = 0;
                        if (redempt != null)
                        { prebalance = redempt.balance; }
                        
                        redeem.createdate = DateTime.Now;
                        redeem.lastmodifieddate = DateTime.Now;
                        redeem.cocode = Convert.ToString(Session["cocode"]);
                        redeem.createid = Convert.ToInt32(Session["userid"]);
                        redeem.branchcode = Convert.ToString(Session["branchcode"]);
                        redeem.invoiceitemid = 0;
                        redeem.balance = prebalance - redeem.debit;
                        db.CusSupRedemption.Add(redeem);
                        db.SaveChanges();
                        saveToLog(rid, redeem.id, "CREATE", "Add New Redeem Package for cust id - " + redeem.cussupid.ToString() + ", ID- " + redeem.id.ToString());
                    }
                    else
                    {
                        CusSup_m_CusRedemption Redeemtmp = db.CusSupRedemption.Find(redeem.id);
                        Redeemtmp.cocode = Convert.ToString(Session["cocode"]);
                        Redeemtmp.branchcode = Convert.ToString(Session["branchcode"]);
                        Redeemtmp.invoiceitemid = 0;
                        db.SaveChanges();
                    }
                    returnStr = "SUCCESS";
                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSIONEXPIRED";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 29 Jan 2016
        public JsonResult getPackageRedeemDetail(int mid)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Redempt_Package> pptmp = context.Database.SqlQuery<Redempt_Package>("call getCussupPackageItemList(" + mid + ")").ToList();
                    return Json(pptmp, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 29 Jan 2016
        public JsonResult getIncompletePackageRedeemDetail(jQueryDataTableParamModel param)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    ICollection<Incompleted_Package_Item> plist = context.Database.SqlQuery<Incompleted_Package_Item>("call getCussupIncompletePackageItemList()").ToList();
                    var searchValue = "";
                    if (param.sSearch == null)
                        searchValue = "";
                    else
                        searchValue = param.sSearch;
                    var filterPList = plist.Where(x => x.cussupid.ToString() == searchValue).ToList();
                    return Json(filterPList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 2 Dec 2015
        [Authorize]
        public ActionResult getProductListWithPaging(jQueryDataTableParamModel param)
        {
            var ptype = Convert.ToString(Request["ptype"]);
            var filter = Convert.ToString(Request["filter"]);
            var cat = Convert.ToString(Request["cat"]);
            var brand = Convert.ToString(Request["brand"]);

            try
            {
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                ICollection<Product_m_Productdtl> PList = ProductController.ProductList("%", "%", 0, "", "", "", 0, 0, 1);
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterPList = PList.Where(x => x.desc.ToUpper().Contains(searchValue.ToUpper())).ToList();
                var paginatedQPList = filterPList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = PList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterPList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        [Authorize]
        public Boolean chkUserAccessRightForRedeemPackageVoid(string uname, string pwd)
        {
            Boolean returnValue = false;
            try
            {
                ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x => x.status.ToUpper() == "ACTIVE").ToList();
                Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();
                if (u != null)
                {
                    if (u.roldid == 1)
                    { returnValue = true; }
                    else
                    {
                        var aList = db.RoleAccessRights.Where(x => x.roleid == u.roldid && x.resource == "PACKAGE REDEEM" && x.voidField == true).Select(x => new { x.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == u.id && x.resource == "POS" && x.voidField == true).Select(x => new { x.Resources.resource }).Distinct()).ToList();
                        if (aList != null)
                        {
                            if (aList.Count > 0)
                                returnValue = true;
                        }
                    }
                }
            }
            catch (Exception e)
            { }

            return returnValue;
        }

        [Authorize]
        public Boolean addToRedemptionTableForVoidPackage(string refno, string ttype)
        {

            Boolean returnValue = false;
            try
            {
                ICollection<CusSup_m_CusRedemption> cusRedList = db.CusSupRedemption.Where(x => x.RefNo == refno && x.redemptiontype == "PACKAGE").ToList();
                for (int i = 0; i < cusRedList.Count; i++)
                {
                    CusSup_m_CusRedemption obj = new CusSup_m_CusRedemption();
                    int mid = cusRedList.ElementAt(i).cussupid;
                    int pid = cusRedList.ElementAt(i).productid;
                    CusSup_m_CusRedemption redempttmp = db.CusSupRedemption.Where(x => x.cussupid == mid && x.productid == pid && x.redemptiontype == "PACKAGE").OrderByDescending(x => x.id).FirstOrDefault();
                    double? prebalance = 0;
                    if (redempttmp != null)
                    { prebalance = redempttmp.balance; }
                    double? total = cusRedList.ElementAt(i).credit - cusRedList.ElementAt(i).debit;
                    if (total != 0)
                    {
                        obj.createdate = DateTime.Now;
                        obj.lastmodifieddate = DateTime.Now;
                        obj.invoiceitemid = cusRedList.ElementAt(i).invoiceitemid;
                        obj.cussupid = cusRedList.ElementAt(i).cussupid;
                        obj.productid = pid;
                        obj.productdesc = cusRedList.ElementAt(i).productdesc;
                        obj.remark = "Void " + ttype + "# " + refno;
                        obj.resource = ttype + "VOID";
                        obj.RefNo = refno;
                        obj.branchcode = Convert.ToString(Session["branchcode"]);
                        obj.cocode = Convert.ToString(Session["cocode"]);
                        obj.redemptiontype = "PACKAGE";
                        obj.createid = Convert.ToInt32(Session["userid"]);
                        obj.uom = cusRedList.ElementAt(i).uom;
                        if (total > 0)
                        {
                            obj.credit = 0;
                            obj.debit = total;
                        }
                        else
                        {
                            obj.debit = 0;
                            obj.credit = (total * (-1));
                        }

                        obj.balance = prebalance + obj.credit - obj.debit;
                        db.CusSupRedemption.Add(obj);
                        db.SaveChanges();
                        returnValue = true;
                    }
                }
            }
            catch (Exception e)
            {
            }
            return returnValue;
        }

        [Authorize]
        public JsonResult RedeemPackageVOID(int id = 0, string uname = "", string pwd = "", string resource = "REDEEM PACKAGE")
        {
            var returnStr = "FAIL";
            Boolean redFlag = true;

            if (Session["userid"] != null)
            {
                try
                {
                    if (chkUserAccessRightForRedeemPackageVoid(uname.ToUpper(), pwd) == true)
                    {
                        var ridtmp = db.Resources.Where(x => x.resource == resource).FirstOrDefault().id;
                        int rid = 0;
                        if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                        var ptmp = db.sales.Where(x => x.id == id).FirstOrDefault();
                        if (ptmp != null)
                        {

                            saveToLog(rid, ptmp.id, "Update", "Void Redeem Package refid" + ptmp.id + " , Redeem# " + ptmp.resourcecode);
                            if (ptmp.status.ToUpper() == "CLOSE")
                            {
                                //var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice, x.productprices.servicecommission }).Distinct().ToList();
                                //var itemlist = db.saleItems.Where(x => x.invoiceid == posid).Join(db.CusSupRedemption, sales => sales.id, red => red.invoiceitemid, (sales, red) => new { saleItems = sales, CusSupRedemption = red }).Select(x => new { x.saleItems.invoiceid}).ToList();
                                addToRedemptionTableForVoidPackage(ptmp.resourcecode, "REDEEM PACKAGE");
                            }

                            ptmp.status = "VOID";
                            db.SaveChanges();
                            returnStr = "SUCCESS";

                        }// End of checking pos header                      
                    }// End of checking valid user
                    else
                        returnStr = "Wrong username and password or user doesn't have access to void.";

                }
                catch (Exception e)
                { returnStr = e.Message.ToString(); }
            }
            else
            {
                Response.RedirectToRoute(new { controller = "Access", action = "Login" });
                returnStr = "SESSION EXPIRED";
            }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //Added by ZawZO on 3 Dec 2015
        [Authorize]
        public JsonResult ChangeRedeemPackageItem(int mid = 0, string pkgcode = "", int oldpid = 0, string pdesc = "", string resource = "REDEEM PACKAGE")
        {
            var returnStr = "FAIL";
            try
            {
                CusSup_m_CusRedemption c = db.CusSupRedemption.Where(x => x.cussupid == mid && x.packagecode == pkgcode && x.productid == oldpid && x.credit > 0 && x.redemptiontype == "PQ").FirstOrDefault();
                if (c != null)
                {
                    Product_m_Product p = db.products.Where(x => x.desc == pdesc).FirstOrDefault();
                    if (p != null)
                    {
                        c.productid = p.id;
                        c.productdesc = p.desc;
                        db.SaveChanges();
                    }
                    returnStr = "SUCCESS";
                }
                else
                    returnStr = "FAILED";
            }
            catch (Exception e)
            { returnStr = "FAILED"; }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RedeemPackageListing(string rcode = "PACKAGE REDEEM")
        {
            ViewBag.Rcode = rcode;
            return View();
        }

        [Authorize]
        public ActionResult getRedeemPackageListWithPaging(jQueryDataTableParamModel param, string rcode = "POS")
        {
            try
            {
                ICollection<Invoice_m_Invoice> pList = db.sales.Where(x => x.type == rcode || (x.status == "Active" && x.createdate >= DateTime.Today)).OrderByDescending(x => x.createdate).ToList();
                //ICollection<CusSup_m_CusSupdtl> mList = getMemberInfo(filter, mobile, name, mid, mcode, cardno, nric);
                //ICollection<CusSup_m_CusSupdtl> mList = getMemberInfo("", mobile, name, mid, mcode, 0, nric);

                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterPList = pList.Where(x => string.Format("dd/MM/yyyy", x.createdate).Contains(searchValue) || x.resourcecode.ToUpper().Contains(searchValue.ToUpper()) || x.branchcode.ToUpper().Contains(searchValue.ToUpper()) || x.cussupname.ToUpper().Contains(searchValue.ToUpper())).OrderByDescending(x => x.createdate).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"]; //asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterPList = filterPList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderBy(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filterPList = filterPList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filterPList = filterPList.OrderBy(x => x.total_discount).ToList();
                    else if (sortColumnIndex == 6)
                        filterPList = filterPList.OrderBy(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filterPList = filterPList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterPList = filterPList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderByDescending(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filterPList = filterPList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filterPList = filterPList.OrderByDescending(x => x.total_discount).ToList();
                    else if (sortColumnIndex == 6)
                        filterPList = filterPList.OrderByDescending(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filterPList = filterPList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedPList = filterPList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = pList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterPList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedPList
                },
                JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult VoidRedeem(int id = 0)
        {
            string returnStr = "";
            try
            {
                Invoice_m_Invoice inv = db.sales.Where(x => x.id == id).FirstOrDefault();
                inv.status = "CANCEL";
                db.SaveChanges();

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }




    }
}
