using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using System.Web.Configuration;

namespace BigMac.Controllers
{
    public class InventoryController : Controller
    {
        //
        // GET: /Inventory/
        private BigMacEntities db = new BigMacEntities();
        private GeneralController gc = new GeneralController();
        [Authorize]
        public ActionResult getProductListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                ICollection<Product_m_Productdtl> PList = ProductController.ProductList("%", "%", 0, "", "", "", 0, 0, 1);
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterPList = PList.Where(x => x.category.ToUpper() != "PACKAGE").ToList();
                filterPList = filterPList.Where(x => x.desc.ToUpper().Contains(searchValue.ToUpper()) || x.categorysub.ToUpper().Contains(searchValue.ToUpper()) || x.brand.ToUpper().Contains(searchValue.ToUpper()) || x.productcode.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderBy(x => x.productcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderBy(x => x.category).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderBy(x => x.categorysub).ToList();
                    else if (sortColumnIndex == 4)
                        filterPList = filterPList.OrderBy(x => x.brand).ToList();
                    else if (sortColumnIndex == 5)
                        filterPList = filterPList.OrderBy(x => x.desc).ToList();
                    else if (sortColumnIndex == 6)
                        filterPList = filterPList.OrderBy(x => x.redeemdollars).ToList();
                    else if (sortColumnIndex == 7)
                        filterPList = filterPList.OrderBy(x => x.redeembonus).ToList();
                    else if (sortColumnIndex == 8)
                        filterPList = filterPList.OrderBy(x => x.sellprice).ToList();
                }
                else
                {
                    if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderByDescending(x => x.productcode).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderByDescending(x => x.category).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderByDescending(x => x.categorysub).ToList();
                    else if (sortColumnIndex == 4)
                        filterPList = filterPList.OrderByDescending(x => x.brand).ToList();
                    else if (sortColumnIndex == 5)
                        filterPList = filterPList.OrderByDescending(x => x.desc).ToList();
                    else if (sortColumnIndex == 6)
                        filterPList = filterPList.OrderByDescending(x => x.redeemdollars).ToList();
                    else if (sortColumnIndex == 7)
                        filterPList = filterPList.OrderByDescending(x => x.redeembonus).ToList();
                    else if (sortColumnIndex == 8)
                        filterPList = filterPList.OrderByDescending(x => x.sellprice).ToList();
                }

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

        //New Stock Received Code
        //----------(START)-----------
        public ActionResult StockReceiveOverview(string rcode = "Stock Received", string acode = "")
        {
            int i = 0;
            ViewBag.SupplierOptions = db.CusSup.Where(x => x.cussuptype.Contains("SUPPLIER") || x.cussuptype.Contains("SUPPLIER")).OrderBy(x => x.cussupname).Select(x => new { x.id, x.cussupname, x.inhousecode }).ToList();
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x=>x.branchcode !="MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;

            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }

        [Authorize]
        public JsonResult getStockReceiveListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                var solist = db.stockReceived.Select(x => new { x.id, x.uniquecode, x.resourcecode, x.branchcode, x.cussupname, x.total_salestax, x.total_total, x.status, x.createdate, x.resourcedate }).ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch.ToUpper();

                var filtertpList = solist.Where(x => x.branchcode.ToUpper().Contains(searchValue) || x.resourcecode.ToUpper().Contains(searchValue) || x.cussupname.ToUpper().Contains(searchValue)).OrderBy(x => x.createdate).ToList();
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderBy(x => x.resourcedate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderBy(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderBy(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filtertpList = filtertpList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderByDescending(x => x.resourcedate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderByDescending(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filtertpList = filtertpList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedtpList = filtertpList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = solist.Count,    //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filtertpList.Count,   //paginatedQPList.TotalCount,
                    aaData = paginatedtpList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult CreateStockReceive(int custid, string custname, string rcode = "Stock Received")
        {
            var returnStr = "FAIL";
            int rid = 0;
            try
            {
                string curr = "";
                double exrate = 0;

                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

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

                var uniquecode = "";
                uniquecode = DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + DateTime.Now.Hour.ToString().Trim() + DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim();
                Stock_m_StockReceived stmp = new Stock_m_StockReceived();
                stmp.cussupid = custid;
                stmp.cussupname = custname;
                stmp.createdate = DateTime.Now;
                stmp.lastmodifieddate = DateTime.Now;
                stmp.createid = Convert.ToInt32(Session["userid"]);
                stmp.cocode = Convert.ToString(Session["cocode"]);
                stmp.branchcode = Convert.ToString(Session["branchcode"]);
                stmp.status = "Active";
                stmp.currency = curr;
                stmp.exchangerate = exrate;
                stmp.resourcecode = GeneralController.getGeneratedNewID("stock_m_stockreceived", "resourcecode", "STKRECPREF", "STKREC");
                db.stockReceived.Add(stmp);
                db.SaveChanges();
                saveToLog(rid, stmp.id, "CREATE", "Add New Sales Order for cust id - " + stmp.cussupid.ToString(), "Redeem Ref no- " + stmp.resourcecode + ", ID- " + stmp.id.ToString());
                returnStr = stmp.resourcecode.ToString();
            }
            catch (Exception e)
            {
                returnStr = "ERROR";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        //Stock Received
        [Authorize]
        public ActionResult StockReceive(string uniquecode = "", string rcode = "StockReceived Listing", string acode = "",string productlist = "")
        {
            try
            {
                string curr = "";
                double exrate = 0;
                ViewBag.RCode = rcode;
                ViewBag.UserName = "";
                ViewBag.productlist = productlist;

                string bcode = Convert.ToString(Session["branchcode"]);
                ViewBag.BranchOptions = db.Branches.Where(x=>x.branchcode!="MILEAGE").Select(x => new { x.branchcode }).OrderBy(x => x.branchcode).ToList();
                //Added by ZawZO on 26 Oct 2015
                ViewBag.SupplierOptions = db.CusSup.Where(x => x.cussuptype.Contains("SUPPLIER") || x.cussuptype.Contains("SUPPLIER")).OrderBy(x => x.cussupname).Select(x => new { x.id, x.cussupname, x.inhousecode }).ToList();
                ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
                ViewBag.CategoryOptions = db.productCategory.ToList();
                ViewBag.SubCategoryOptions = db.productSubCategory.ToList();
                ViewBag.RangesNSeriesOptions = db.productRNS.ToList();
                ViewBag.BrandOptions = db.productBrand.ToList();
                
                //Added by ZawZO on 1 Sep 2015
                ViewBag.BranchCode = Convert.ToString(Session["branchcode"]);
                ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
                ViewBag.SupplierOptions = db.CusSup.Where(x => x.cussuptype.Contains("Supplier") || x.cussuptype.Contains("SUPPLIER")).OrderBy(x => x.cussupname).ToList();

                var gst = db.ConfigDefault.Where(x => x.key == "INVGSTPER").FirstOrDefault();
                if (gst != null){ 
                    ViewBag.gstpercent = Convert.ToInt32(gst.value); 
                }
                else { 
                    ViewBag.gstpercent = 0; 
                }
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

                if (uniquecode.Trim() == "")
                {
                    Stock_m_StockReceived stkrec = new Stock_m_StockReceived();
                    stkrec.branchcode = Convert.ToString(Session["branchcode"]);
                    stkrec.currency = curr;
                    stkrec.exchangerate = exrate;
                    stkrec.resourcedate = DateTime.Today;
                    stkrec.status = "ACTIVE";
                    ViewBag.UserName = Convert.ToString(User.Identity.Name);
                    return View(stkrec);
                }
                else
                {
                    Stock_m_StockReceived stkrec = db.stockReceived.Where(x => x.uniquecode== uniquecode).FirstOrDefault(); 
                    if (stkrec == null) stkrec = new Stock_m_StockReceived();
                    int uid = stkrec.createid;
                    var tmp = db.Users.Where(x => x.id == uid).FirstOrDefault();
                    if (tmp != null)
                        ViewBag.UserName = cAESEncryption.getDecryptedString(tmp.name);
                    ViewBag.BranchCode = stkrec.branchcode;

                    return View(stkrec);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult StockReceiveSave(Stock_m_StockReceived stkrec, string rcode = "Stock Receive", string itemids = "")
        {
            var returnStr = "FAIL,";
            if (Session["userid"] != null)
            {
                try
                {

                    int rid = gc.getResourceID(rcode);

                    if (stkrec.id == 0)
                    {
                        var uniquecode = "";
                        uniquecode = DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + DateTime.Now.Hour.ToString().Trim() + DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim();
                        stkrec.uniquecode = uniquecode;
                        stkrec.createdate = DateTime.Now;
                        stkrec.lastmodifieddate = DateTime.Now;
                        stkrec.cocode = Convert.ToString(Session["cocode"]);
                        stkrec.createid = Convert.ToInt32(Session["userid"]);
                        stkrec.resourcecode = GeneralController.getGeneratedNewID("stock_m_stockreceived", "resourcecode", "STKRECPREF", "STKREC");
                        db.stockReceived.Add(stkrec);
                        db.SaveChanges();
                        saveToLog(rid, stkrec.id, "CREATE", "Add New Stock Received  Ref no- " + stkrec.resourcecode + ", ID- " + stkrec.id.ToString());

                        if (stkrec.items != null)
                        {
                            for (int i = 0; i < (stkrec.items.Count()); i++)
                            {
                                 SaveNewStockReceiveItem(stkrec.items.ElementAt(i), stkrec.branchcode, stkrec.id, rcode, stkrec.resourcecode, rid, "ACTIVE", stkrec.status.ToUpper());
                            }
                        }
                        returnStr = "SUCCESS," + stkrec.uniquecode + "," + stkrec.id.ToString() + "," + stkrec.resourcecode.Trim() + "," + stkrec.status;
                    }
                    else
                    {
                        Stock_m_StockReceived itemtmp = db.stockReceived.Find(stkrec.id);
                        string from = ""; string to = "";
                        string prevStatus = "";
                        from = "Status -" + itemtmp.status + ", Create By -" + itemtmp.createid + ", total -" + itemtmp.total_total.ToString() + ",status " + itemtmp.status;
                        to = "Status -" + stkrec.status + ", Create By -" + Session["userid"].ToString() + ", total -" + stkrec.total_total.ToString() + "," + itemids + ",status " + stkrec.status;
                        itemtmp.branchcode = stkrec.branchcode; 
                        itemtmp.currency = stkrec.currency;
                        itemtmp.exchangerate = stkrec.exchangerate;
                        itemtmp.resourcedate = stkrec.resourcedate;
                        itemtmp.refno = stkrec.refno;
                        itemtmp.suprefno = stkrec.suprefno;
                        itemtmp.remark = stkrec.remark;
                        itemtmp.total_subtotal = stkrec.total_subtotal;
                        itemtmp.total_salestax = stkrec.total_salestax;
                        itemtmp.currency = stkrec.currency;
                        itemtmp.exchangerate = stkrec.exchangerate;
                        itemtmp.total_total = stkrec.total_total;
                        prevStatus = itemtmp.status.ToUpper();
                        itemtmp.status = stkrec.status;
                        itemtmp.post = stkrec.post;
                        if (prevStatus == "ACTIVE" && stkrec.post == 1)
                        {
                            itemtmp.postdate = DateTime.Now;
                            itemtmp.postid = Convert.ToInt32(Session["userid"]);
                        }
                        db.SaveChanges();
                        saveToLog(rid, itemtmp.id, "UPDATE", "UPDATE Transfer - refno -" + itemtmp.resourcecode + ", id - " + itemtmp.id.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from stock_m_stockreceived_items where stockreceivedid=" + stkrec.id.ToString() + " and id not in (" + itemids + ")");
                        }

                        if (stkrec.items != null)
                        {
                            for (int i = 0; i < (stkrec.items.Count()); i++)
                            {
                                if (stkrec.items.ElementAt(i).id == 0)
                                {
                                    SaveNewStockReceiveItem(stkrec.items.ElementAt(i), stkrec.branchcode, stkrec.id, rcode, itemtmp.resourcecode, rid, prevStatus, stkrec.status.ToUpper());
                                }
                                else
                                {
                                    UpdateStockReceiveitem(stkrec.items.ElementAt(i), stkrec.branchcode, rcode, itemtmp.resourcecode, rid, prevStatus, stkrec.status.ToUpper());
                                }
                            }
                        }
                        returnStr = "SUCCESS," + itemtmp.uniquecode + "," + stkrec.id.ToString() + "," + itemtmp.resourcecode.Trim() + "," + stkrec.status;
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
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        //----------(END)-----------
        public void SaveNewStockReceiveItem(Stock_m_StockReceived_Items item, string branchcode, int headerid, string resource, string resourcecode, int rid = 0, string pstatus = "", string status = "Active")
        {
            //Changed by ZawZO on 21 Oct 2015
            if (item.productcode != null)
            {
                item.createdate = DateTime.Now;
                item.lastmodifieddate = DateTime.Now;
                item.resourcecode = resourcecode;
                item.createid = Convert.ToInt32(Session["userid"]);
                item.stockreceivedid = headerid;
                db.stockReceivedItems.Add(item);
                db.SaveChanges();
                saveToLog(rid, item.id, "CREATE", "Add New Receive Item - product ID -" + item.productid, "RefNo no- " + resourcecode + ", Item ID- " + item.id.ToString());
                if (pstatus.ToUpper() == "ACTIVE" && status.ToUpper() == "CLOSE")
                {
                    AddToMovementTable(item, branchcode, "Receive", resourcecode, rid);
                }
            }
        }

        public void UpdateStockReceiveitem(Stock_m_StockReceived_Items item, string branchcode, string resource, string resourcecode, int rid, string prevStatus, string newStatus)
        {
            Stock_m_StockReceived_Items tmp = db.stockReceivedItems.Find(item.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ", Qty -" + tmp.qty.ToString() + ", UOM" + tmp.uom + ",status " + prevStatus;
                to = "proudctid -" + item.productid + ",product desc -" + item.productdesc + ",Price -" + item.unitprice.ToString() + ", Qty -" + item.qty.ToString() + ", UOM" + item.uom + ",status " + newStatus;

                if (newStatus == "CLOSE")
                {
                    if (prevStatus == "ACTIVE")
                    {
                        AddReceiveToMovementTable(item, branchcode, "Receive", resourcecode, rid);
                    }
                }
                tmp.currency = item.currency;
                tmp.exchangerate = item.exchangerate;
                tmp.productid = item.productid;
                tmp.productdesc = item.productdesc;
                tmp.productcode = item.productcode;
                tmp.unitprice = item.unitprice;
                tmp.qty = item.qty;
                tmp.uom = item.uom;
                tmp.taxamount = item.taxamount;
                tmp.discountamount = item.discountamount;
                tmp.lineamount = item.lineamount; 
                tmp.lineno = item.lineno;
                db.SaveChanges();
                saveToLog(rid, item.id, "UPDATE", "Update Detail item ID-" + item.id.ToString() + " RefNo -" + resourcecode, from, to);
            }
        }
        //Stock Received
        public void SaveNewStockReceivedDetailItem(Stock_m_StockReceived_Items sodtl, int rid = 0)
        {
            sodtl.createdate = DateTime.Now;
            sodtl.lastmodifieddate = DateTime.Now;
            sodtl.createid = Convert.ToInt32(Session["userid"]);
            db.stockReceivedItems.Add(sodtl);
            db.SaveChanges();
            saveToLog(rid, sodtl.id, "CREATE", "Add New Deatil Item - product ID -" + sodtl.productid, "RefNo no- " + sodtl.resourcecode + ", Item ID- " + sodtl.id.ToString());
        }

        public void UpdateStockReceivedDetailItem(Stock_m_StockReceived_Items sodtl, int rid)
        {
            Stock_m_StockReceived_Items tmp = db.stockReceivedItems.Find(sodtl.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ",Qty -" + tmp.qty.ToString() +  ",Price-" + tmp.unitprice.ToString() + ", Disc -" + tmp.discountamount.ToString() + ",Tax -" + tmp.taxamount.ToString() + ",LineAmt-" + tmp.lineamount.ToString();
                to = "proudctid -" + sodtl.productid + ",product desc -" + sodtl.productdesc + ",Price -" + sodtl.unitprice.ToString() + ",Qty -" + tmp.qty.ToString() + ",Price-" + sodtl.unitprice.ToString() + ", Disc -" + sodtl.discountamount.ToString() + ",Tax -" + sodtl.taxamount.ToString() + ",LineAmt-" + sodtl.lineamount.ToString();

                tmp.productid = sodtl.productid;
                tmp.lineno = sodtl.lineno;
                tmp.productcode = sodtl.productcode;
                tmp.productdesc = sodtl.productdesc;
                tmp.qty = sodtl.qty;
                tmp.uom = sodtl.uom;
                tmp.unitprice = sodtl.unitprice;
                tmp.discountamount = sodtl.discountamount;
                tmp.taxamount = sodtl.taxamount;
                tmp.lineamount = sodtl.lineamount;
                tmp.resourcecode = sodtl.resourcecode;
                tmp.currency = sodtl.currency;
                tmp.exchangerate = sodtl.exchangerate;
                tmp.lastmodifieddate = DateTime.Now;
                db.SaveChanges();
                saveToLog(rid, sodtl.id, "UPDATE", "Update Detail Item ID-" + sodtl.id.ToString() + " RefNo -" + sodtl.resourcecode, from, to);
            }
        }

        
        public JsonResult getStockReceivedDetailWithPaging(int pageno = 0, int id = 0)
        {
            try
            {
                ICollection<Stock_m_StockReceived_Items> SList = db.stockReceivedItems.Where(x => x.stockreceivedid == id).ToList();
                return Json(SList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public ActionResult StockReceivedListing(string rcode = "Stock Received", string acode = "")
        {
            int i = 0;
            ViewBag.CustomerOptions = db.CusSup.Where(x => x.status == "Active").ToList();
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;

            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }
               

        [Authorize]
        public JsonResult getStockReceivedListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                var solist = db.stockReceived.ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch.ToUpper();
                
                var filtertpList = solist.Where(x => x.branchcode.ToUpper().Contains(searchValue) || x.resourcecode.ToUpper().Contains(searchValue) || x.cussupname.ToUpper().Contains(searchValue)).OrderBy(x => x.createdate).ToList();
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderBy(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderBy(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filtertpList = filtertpList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderByDescending(x => x.cussupname).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderByDescending(x => x.total_total).ToList();
                    else if (sortColumnIndex == 7)
                        filtertpList = filtertpList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedtpList = filtertpList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = solist.Count,    //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filtertpList.Count,   //paginatedQPList.TotalCount,
                    aaData = paginatedtpList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        

        public ActionResult StockReceivedIndex()
        {
            return View(db.stockReceived.Include("Branch").Include("Create").Where(x => x.status == "Active").ToList());
        }
               

        public ActionResult StockRecedivedDetail(int srid = 0)
        {
            try
            {
                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
                ViewBag.UOMOptions = db.UOM.ToList();
                ViewBag.SRItemOptions = db.stockReceivedItems.Where(x => x.stockreceivedid == srid).OrderBy(x => x.lineno).ToList();
                Stock_m_StockReceived_Items sitem = new Stock_m_StockReceived_Items();
                sitem.stockreceivedid = srid;
                sitem.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;                
                var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

                if (ex.Length > 0)
                    sitem.exchangerate = Convert.ToDouble(ex);
                else
                    sitem.exchangerate = 0;

                return View(sitem);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult StockRecedivedDetail(Stock_m_StockReceived_Items sitem)
        {
            return View(sitem);
        }
        //End of Stock Received 
        //Start of Stock Adjustment
        public ActionResult StockAdjustmentOverview(string rcode = "Stock Adjustment", string acode = "")
        {
            int i = 0;
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;
            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }

        [Authorize]
        public JsonResult getStockAdjustmentListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                var salist = db.stockAdjustment.Select(x => new { x.id, x.uniquecode, x.resourcedate, x.resourcecode, x.branchcode,x.reason, x.refno, x.postdate, x.status,x.createdate }).ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch.ToUpper();

                var filtertpList = salist.Where(x => x.branchcode.ToUpper().Contains(searchValue) || x.resourcecode.ToUpper().Contains(searchValue)).OrderBy(x => x.createdate).ToList();
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderBy(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderBy(x => x.reason).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderBy(x => x.refno).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderBy(x => x.postdate).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filtertpList = filtertpList.OrderByDescending(x => x.createdate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertpList = filtertpList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertpList = filtertpList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertpList = filtertpList.OrderByDescending(x => x.reason).ToList();
                    else if (sortColumnIndex == 4)
                        filtertpList = filtertpList.OrderByDescending(x => x.refno).ToList();
                    else if (sortColumnIndex == 5)
                        filtertpList = filtertpList.OrderByDescending(x => x.postdate).ToList();
                    else if (sortColumnIndex == 6)
                        filtertpList = filtertpList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedtpList = filtertpList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = salist.Count,    
                    iTotalDisplayRecords = filtertpList.Count,   
                    aaData = paginatedtpList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }
                
        [Authorize]
        public ActionResult StockAdjustment(string uniquecode = "", string rcode = "StockAdjustment Listing", string acode = "", string productlist = "")
        {
            try
            {
                ViewBag.RCode = rcode;
                ViewBag.productlist = productlist;

                ViewBag.BranchOptions = db.Branches.Where(x=>x.branchcode!="MILEAGE").Select(x => new { x.branchcode }).OrderBy(x => x.branchcode).ToList();
                ViewBag.SupplierOptions = db.CusSup.Where(x => x.cussuptype.Contains("SUPPLIER") || x.cussuptype.Contains("SUPPLIER")).OrderBy(x => x.cussupname).Select(x => new { x.id, x.cussupname, x.inhousecode }).ToList();
                ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
                ViewBag.CategoryOptions = db.productCategory.ToList();
                ViewBag.SubCategoryOptions = db.productSubCategory.ToList();
                ViewBag.RangesNSeriesOptions = db.productRNS.ToList();
                ViewBag.BrandOptions = db.productBrand.ToList();
                
                if (uniquecode == "")
                {
                    stock_m_stockadjustment sa = new stock_m_stockadjustment();
                    //Added by ZawZO on 4 Sep 2015
                    sa.createdate = DateTime.Now;
                    sa.lastmodifieddate = DateTime.Now;
                    sa.createid = Convert.ToInt32(Session["userid"]);
                    sa.cocode = Convert.ToString(Session["cocode"]);
                    sa.branchcode = Convert.ToString(Session["branchcode"]);
                    sa.resourcedate= DateTime.Now;
                    ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();
                    string curr = "";
                    double exrate = 0;
                    if (config.Count > 0)
                        curr = config.ElementAt(0).value;
                    else
                        curr = "";
                    
                    config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                    if (config.Count > 0)
                        exrate = Convert.ToDouble(config.ElementAt(0).value);
                    else
                        exrate = 0;

                    sa.currency = curr;
                    sa.exchangerate = exrate;
                    sa.status = "Active";

                    return View(sa);
                }
                else
                {
                    stock_m_stockadjustment stockadj = db.stockAdjustment.Where(x => x.uniquecode==uniquecode).FirstOrDefault();
                    return View(stockadj);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }
        
        [HttpPost]
        public JsonResult StockAdjustmentSave(stock_m_stockadjustment stkadj, string rcode = "Stock Adjustment", string itemids = "")
        {
            var returnStr = "FAIL,";
            if (Session["userid"] != null)
            {
                try
                {
                    int rid = gc.getResourceID(rcode);
                    if (stkadj.id == 0)
                    {
                        var uniquecode = "";
                        uniquecode = DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + DateTime.Now.Hour.ToString().Trim() + DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim();
                        stkadj.uniquecode = uniquecode;
                        stkadj.createdate = DateTime.Now;
                        stkadj.lastmodifieddate = DateTime.Now;
                        stkadj.cocode = Convert.ToString(Session["cocode"]);
                        stkadj.createid = Convert.ToInt32(Session["userid"]);
                        stkadj.resourcecode = GeneralController.getGeneratedNewID("stock_m_stockadjustment", "resourcecode", "STKADJPREF", "STKADJ");
                        db.stockAdjustment.Add(stkadj);
                        db.SaveChanges();
                        saveToLog(rid, stkadj.id, "CREATE", "Add New Stock Adjustment  Ref no- " + stkadj.resourcecode + ", ID- " + stkadj.id.ToString());

                        if (stkadj.items != null)
                        {
                            for (int i = 0; i < (stkadj.items.Count()); i++)
                            {
                                SaveNewStockAdjustmentItem(stkadj.items.ElementAt(i), stkadj.branchcode, stkadj.id, rcode, stkadj.resourcecode, rid, "ACTIVE", stkadj.status.ToUpper());
                            }
                        }
                        returnStr = "SUCCESS," + stkadj.uniquecode + "," + stkadj.id.ToString() + "," + stkadj.resourcecode.Trim() + "," + stkadj.status;
                    }
                    else
                    {
                        stock_m_stockadjustment itemtmp = db.stockAdjustment.Find(stkadj.id);
                        string from = ""; string to = "";
                        string prevStatus = "";
                        
                        from = "Status -" + itemtmp.status + ", Create By -" + itemtmp.createid + ", total -" + itemtmp.total_total.ToString();
                        to = "Status -" + stkadj.status + ", Create By -" + Session["userid"].ToString() + ", total -" + stkadj.total_total.ToString() + itemids;
                        itemtmp.branchcode = stkadj.branchcode; 
                        itemtmp.currency = stkadj.currency;
                        itemtmp.exchangerate = stkadj.exchangerate;
                        itemtmp.resourcedate = stkadj.resourcedate;
                        itemtmp.refno = stkadj.refno;
                        itemtmp.reason = stkadj.reason;
                        itemtmp.total_subtotal = stkadj.total_subtotal;
                        itemtmp.total_salestax = stkadj.total_salestax;
                        itemtmp.currency = stkadj.currency;
                        itemtmp.exchangerate = stkadj.exchangerate;
                        itemtmp.total_total = stkadj.total_total;
                        prevStatus = itemtmp.status.ToUpper();
                        itemtmp.status = stkadj.status;
                        itemtmp.post = stkadj.post;
                        if (prevStatus == "ACTIVE" && stkadj.post == 1)
                        {
                            itemtmp.postdate = DateTime.Now;
                            itemtmp.postid = Convert.ToInt32(Session["userid"]);
                        }
                        db.SaveChanges();
                        saveToLog(rid, itemtmp.id, "UPDATE", "UPDATE Transfer - refno -" + itemtmp.resourcecode + ", id - " + itemtmp.id.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from stock_m_stockadjustment_items where stockadjustmentid=" + stkadj.id.ToString() + " and id not in (" + itemids + ")");
                        }

                        if (stkadj.items != null)
                        {
                            for (int i = 0; i < (stkadj.items.Count()); i++)
                            {
                                if (stkadj.items.ElementAt(i).id == 0)
                                {
                                    SaveNewStockAdjustmentItem(stkadj.items.ElementAt(i), stkadj.branchcode, stkadj.id, rcode, itemtmp.resourcecode, rid, prevStatus, stkadj.status.ToUpper());
                                }
                                else
                                {
                                    UpdateStockAdjustmentitem(stkadj.items.ElementAt(i), stkadj.branchcode, rcode, itemtmp.resourcecode, rid, prevStatus, stkadj.status.ToUpper());
                                }
                            }
                        }
                        returnStr = "SUCCESS," + itemtmp.uniquecode + "," + stkadj.id.ToString() + "," + itemtmp.resourcecode.Trim() + "," + stkadj.status;
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
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void SaveNewStockAdjustmentItem(stock_m_stockadjustment_items item, string branchcode, int headerid, string resource, string resourcecode, int rid = 0, string pstatus = "", string status = "Active")
        {
            //Changed by ZawZO on 21 Oct 2015
            if (item.productcode != null)
            {
                item.createdate = DateTime.Now;
                item.lastmodifieddate = DateTime.Now;
                item.resourcecode = resourcecode;
                item.createid = Convert.ToInt32(Session["userid"]);
                item.stockadjustmentid = headerid;
                item.reason = item.reason ?? "";
                db.stockAdjustmentItems.Add(item);
                db.SaveChanges();
                saveToLog(rid, item.id, "CREATE", "Add New Deatil Item - product ID -" + item.productid, "RefNo no- " + resourcecode + ", Item ID- " + item.id.ToString());
                if (pstatus.ToUpper() == "ACTIVE" && status.ToUpper() == "CLOSE")
                {
                    AddToMovementTableAdjustment(item, branchcode, resource, resourcecode, rid);
                }
            }
        }

        public void UpdateStockAdjustmentitem(stock_m_stockadjustment_items item, string branchcode, string resource, string resourcecode, int rid, string prevStatus, string newStatus)
        {
            stock_m_stockadjustment_items tmp = db.stockAdjustmentItems.Find(item.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ", Qty -" + tmp.qty.ToString() + ", UOM" + tmp.uom;
                to = "proudctid -" + item.productid + ",product desc -" + item.productdesc + ",Price -" + item.unitprice.ToString() + ", Qty -" + item.qty.ToString() + ", UOM" + item.uom;

                if (newStatus == "CLOSE")
                {
                    if (prevStatus == "ACTIVE")
                    {
                        AddToMovementTableAdjustment(item, branchcode, "Adjustment", resourcecode, rid);
                    }
                }
                tmp.currency = item.currency;
                tmp.exchangerate = item.exchangerate;
                tmp.productid = item.productid;
                tmp.productdesc = item.productdesc;
                tmp.productcode = item.productcode;
                tmp.unitprice = item.unitprice;
                tmp.qty = item.qty;
                tmp.reason = item.reason ?? "";
                tmp.uom = item.uom;
                tmp.taxamount = item.taxamount;
                tmp.discountamount = item.discountamount;
                tmp.lineamount = item.lineamount; 
                tmp.lineno = item.lineno;
                db.SaveChanges();
                saveToLog(rid, item.id, "UPDATE", "Update Detail item ID-" + item.id.ToString() + " RefNo -" + resourcecode, from, to);
            }
        }

        public void AddReceiveToMovementTable(dynamic item, string branchcode, string resource, string resourcecode, int rid = 0)
        {
            int pid = item.productid;
            if (item.qty > 0)
            {
                stock_m_stockmovement tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (tmp != null)
                    prebalance = tmp.productbalance;

                stock_m_stockmovement mov = new stock_m_stockmovement();
                mov.resourcecode = resourcecode; 
                mov.productid = item.productid;
                mov.productcode = item.productcode;
                mov.productdesc = item.productdesc;
                mov.uom = item.uom;
                mov.unitprice = item.unitprice;
                mov.currency = item.currency;
                mov.exchangerate = item.exchangerate;
                mov.createid = Convert.ToInt32(Session["userid"]);
                mov.stockmoduletype = resource;
                mov.cocode = Convert.ToString(Session["cocode"]);
                mov.branchcode = branchcode; 
                mov.stockrefid = 0;
                mov.createdate = DateTime.Now;
                mov.lastmodifieddate = DateTime.Now;
                mov.qty = item.qty;
                mov.lastbalance = mov.qty;
                mov.taxamount = item.taxamount;
                mov.discountamount = item.discountamount;
                mov.lineamount = item.lineamount;
                mov.productbalance = prebalance + mov.qty;
                db.StockMovement.Add(mov);
                db.SaveChanges();
                saveToLog(rid, mov.id, "CREATE", "Add New Balance  ProductID -" + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
            }
        }



        public JsonResult getStockAdjustmentDetailWithPaging(int pageno = 0, int id = 0)
        {
            try
            {
            
                ICollection<stock_m_stockadjustment_items> SList = db.stockAdjustmentItems.Where(x => x.stockadjustmentid == id).ToList();
                return Json(SList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public ActionResult StockAdjustmentListing(string rcode = "Stock Adjustment", string acode = "")
        {
            int i = 0;
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x=>x.branchcode !="MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;

            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }
        public ActionResult StockAdjustmentIndex()
        {
            return View(db.stockAdjustment.Include("Branch").Include("Create").Where(x => x.status == "Active").ToList());
        }

        public ActionResult StockAdjustmentDetail(int srid = 0)
        {
            try
            {
                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
                ViewBag.UOMOptions = db.UOM.ToList();
                ViewBag.SRItemOptions = db.stockReceivedItems.Where(x => x.stockreceivedid == srid).OrderBy(x => x.lineno).ToList();
                stock_m_stockadjustment_items sitem = new stock_m_stockadjustment_items();
                sitem.stockadjustmentid = srid;
                sitem.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
                var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

                if (ex.Length > 0)
                    sitem.exchangerate = Convert.ToDouble(ex);
                else
                    sitem.exchangerate = 0;

                return View(sitem);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult StockAdjustmentDetail(stock_m_stockadjustment_items sitem)
        {
            return View(sitem);
        }
        //End of stock adjustment
        //Start of stock transfer
        public ActionResult StockTransferOverview(string rcode = "Stock Transfer", string acode = "")
        {
            int i = 0;
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;
            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }

        [Authorize]
        public JsonResult getStockTransferListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                var stlist = db.stockTransfer.Select(x => new { x.id, x.uniquecode, x.resourcedate, x.resourcecode, x.branchcode, x.tobranchcode, x.total_total, x.total_salestax, x.status }).ToList();
                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch.ToUpper();

                var filtertstList = stlist.Where(x => x.branchcode.ToUpper().Contains(searchValue) ||  x.resourcecode.ToUpper().Contains(searchValue)).OrderByDescending(x => x.id).ToList();
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filtertstList = filtertstList.OrderBy(x => x.resourcedate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertstList = filtertstList.OrderBy(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertstList = filtertstList.OrderBy(x => x.tobranchcode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertstList = filtertstList.OrderBy(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 4)
                        filtertstList = filtertstList.OrderBy(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertstList = filtertstList.OrderBy(x => x.total_total).ToList();
                    else if (sortColumnIndex == 6)
                        filtertstList = filtertstList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filtertstList = filtertstList.OrderByDescending(x => x.resourcedate).ToList();
                    else if (sortColumnIndex == 1)
                        filtertstList = filtertstList.OrderByDescending(x => x.branchcode).ToList();
                    else if (sortColumnIndex == 2)
                        filtertstList = filtertstList.OrderByDescending(x => x.tobranchcode).ToList();
                    else if (sortColumnIndex == 3)
                        filtertstList = filtertstList.OrderByDescending(x => x.resourcecode).ToList();
                    else if (sortColumnIndex == 4)
                        filtertstList = filtertstList.OrderByDescending(x => x.total_salestax).ToList();
                    else if (sortColumnIndex == 5)
                        filtertstList = filtertstList.OrderByDescending(x => x.total_total).ToList();
                    else if (sortColumnIndex == 6)
                        filtertstList = filtertstList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedstList = filtertstList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = stlist.Count,    
                    iTotalDisplayRecords = filtertstList.Count,   
                    aaData = paginatedstList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        public JsonResult CreateStockTransfer(string bcode, string tobcode, string rcode = "Stock Transfer")
        {
            var returnStr = "FAIL";
            int rid = 0;
            try
            {
                string curr = "";
                double exrate = 0;

                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

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

                var uniquecode = "";
                uniquecode = DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + DateTime.Now.Hour.ToString().Trim() + DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim();
                stock_m_stocktransfer stmp = new stock_m_stocktransfer();
                stmp.uniquecode = uniquecode;
                stmp.createdate = DateTime.Now;
                stmp.lastmodifieddate = DateTime.Now;
                stmp.createid = Convert.ToInt32(Session["userid"]);
                stmp.cocode = Convert.ToString(Session["cocode"]);
                stmp.branchcode = bcode;
                stmp.tobranchcode = tobcode;
                stmp.status = "Active";
                stmp.currency = curr;
                stmp.exchangerate = exrate;
                stmp.resourcecode = GeneralController.getGeneratedNewID("stock_m_stocktransfer", "resourcecode", "STKTRSF", "STKTRSF");
                db.stockTransfer.Add(stmp);
                db.SaveChanges();
                saveToLog(rid, stmp.id, "CREATE", "Add New Sales Order for cust id - " + stmp.branchcode.ToString(), "Redeem Ref no- " + stmp.resourcecode + ", ID- " + stmp.id.ToString());
                returnStr = stmp.uniquecode.ToString();
            }
            catch (Exception e)
            {
                returnStr = "ERROR";
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult StockTransfer(string uniquecode = "", string rcode = "StockTransfer Listing", string acode = "", string productlist = "")
        {
            try
            {
                string curr = "";
                double exrate = 0;
                ViewBag.Rcode = rcode;
                ViewBag.Acode = acode;
                ViewBag.productlist = productlist;


                string bcode = Session["branchcode"].ToString();
                ViewBag.BranchOptions = db.Branches.Where(x=>x.branchcode!="MILEAGE").Select(x => new { x.branchcode }).OrderBy(x => x.branchcode).ToList();
                ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
                ViewBag.CategoryOptions = db.productCategory.ToList();
                ViewBag.SubCategoryOptions = db.productSubCategory.ToList();
                ViewBag.RangesNSeriesOptions = db.productRNS.ToList();
                ViewBag.BrandOptions = db.productBrand.ToList();

                ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();
                var gst = db.ConfigDefault.Where(x => x.key == "INVGSTPER").FirstOrDefault();
                if (gst != null)
                { ViewBag.gstpercent = Convert.ToInt32(gst.value); }
                else
                { ViewBag.gstpercent = 0; }

                if (config.Count > 0)
                    curr = config.ElementAt(0).value;
                else
                    curr = "";

                config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                if (config.Count > 0)
                    exrate = Convert.ToDouble(config.ElementAt(0).value);
                else
                    exrate = 0;

                if (uniquecode == "")
                {
                    stock_m_stocktransfer stktrn = new stock_m_stocktransfer();
                    stktrn.branchcode = Convert.ToString(Session["branchcode"]);
                    stktrn.currency = curr;
                    stktrn.exchangerate = exrate;
                    stktrn.resourcedate = DateTime.Today;
                    stktrn.status = "ACTIVE";
                    return View(stktrn);
                }
                else
                {
                    stock_m_stocktransfer stktrn = db.stockTransfer.Where(x => x.uniquecode==uniquecode).FirstOrDefault(); 
                    return View(stktrn);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult StockListing(string rcode = "STOCK LISTING", string acode = "acode", string UniqueCode = "")
        {
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            ViewBag.UniqueCode = UniqueCode;

            ViewBag.BrandOptions = db.productBrand.OrderBy(x => x.value).AsEnumerable();
            ViewBag.SubCategoryOptions = db.productSubCategory.OrderBy(x => x.Category).AsEnumerable();
            ViewBag.CategoryOptions = db.productCategory.OrderBy(x => x.Category).AsEnumerable();

            ICollection<Configuration_m_Branches> bcol = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
            Configuration_m_Branches btmp = new Configuration_m_Branches();

            foreach (Configuration_m_Branches br in bcol)
            {
                br.tel = "B";
            }

            btmp.id = 0;
            btmp.tel = "A";
            btmp.branchcode = "ALL";
            bcol.Add(btmp);
            ViewBag.BranchOptions = bcol.OrderBy(x => x.tel).ThenBy(x => x.branchcode).ToList();

            ViewBag.StockYrOptions = db.StockMovement.Select(x => new { stockYr = x.createdate.Value.Year }).Distinct().ToList();

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            items.Add(new SelectListItem { Text = "January", Value = "1" });
            items.Add(new SelectListItem { Text = "February", Value = "2" });
            items.Add(new SelectListItem { Text = "March", Value = "3" });
            items.Add(new SelectListItem { Text = "April", Value = "4" });
            items.Add(new SelectListItem { Text = "May", Value = "5" });
            items.Add(new SelectListItem { Text = "June", Value = "6" });
            items.Add(new SelectListItem { Text = "July", Value = "7" });
            items.Add(new SelectListItem { Text = "August", Value = "8" });
            items.Add(new SelectListItem { Text = "September", Value = "9" });
            items.Add(new SelectListItem { Text = "October", Value = "10" });
            items.Add(new SelectListItem { Text = "November", Value = "11" });
            items.Add(new SelectListItem { Text = "December", Value = "12" });
            ViewBag.StockMthOptions = items.ToList();

            var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
            var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
            ViewBag.CitiDesc = c;
            ViewBag.BonusDesc = b;

            return View();
        }

        [Authorize]
        public JsonResult getStockMovementListWithPaging(jQueryDataTableParamModel param, string branchcode, string category, string brand, string filter)
        {
            try
            {
                var stockMovementList = StockMovementList();

                if (branchcode == "ALL")
                {
                    stockMovementList = AllStockMovementList();
                }

                var searchValue = "";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch.ToUpper();

                var filterMovementList = stockMovementList.ToList();

                if (branchcode != "ALL")
                {
                    filterMovementList = filterMovementList.Where(x => x.branchcode == branchcode).ToList();
                }

                if (category != "ALL")
                {
                    filterMovementList = filterMovementList.Where(x => x.categorysub == category).ToList();
                }

                if (brand != "ALL")
                {
                    filterMovementList = filterMovementList.Where(x => x.brand == brand).ToList();
                }

                if (filter != "")
                {
                    filterMovementList = filterMovementList.Where(x => x.productcode.Contains(filter) || x.productdesc.Contains(filter)).ToList();
                }

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var sortDirection = Request["sSortDir_0"];  // asc or desc
                if (branchcode != "ALL")
                {
                    if (sortDirection == "asc")
                    {
                        if (sortColumnIndex == 1)
                            filterMovementList = filterMovementList.OrderBy(x => x.branchcode).ToList();
                        else if (sortColumnIndex == 2)
                            filterMovementList = filterMovementList.OrderBy(x => x.productdesc).ToList();
                        else if (sortColumnIndex == 3)
                            filterMovementList = filterMovementList.OrderBy(x => x.productcode).ToList();
                        else if (sortColumnIndex == 4)
                            filterMovementList = filterMovementList.OrderBy(x => x.category).ToList();
                        else if (sortColumnIndex == 5)
                            filterMovementList = filterMovementList.OrderBy(x => x.categorysub).ToList();
                        else if (sortColumnIndex == 6)
                            filterMovementList = filterMovementList.OrderBy(x => x.brand).ToList();
                        else if (sortColumnIndex == 7)
                            filterMovementList = filterMovementList.OrderBy(x => x.productbalance).ToList();
                        else if (sortColumnIndex == 8)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom).ToList();
                    }
                    else
                    {
                        if (sortColumnIndex == 1)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.branchcode).ToList();
                        else if (sortColumnIndex == 2)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.productdesc).ToList();
                        else if (sortColumnIndex == 3)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.productcode).ToList();
                        else if (sortColumnIndex == 4)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.category).ToList();
                        else if (sortColumnIndex == 5)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.categorysub).ToList();
                        else if (sortColumnIndex == 6)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.brand).ToList();
                        else if (sortColumnIndex == 7)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.productbalance).ToList();
                        else if (sortColumnIndex == 8)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom).ToList();
                    }
                }
                else
                {
                    if (sortDirection == "asc")
                    {
                        if (sortColumnIndex == 1)
                            filterMovementList = filterMovementList.OrderBy(x => x.productdesc).ToList();
                        else if (sortColumnIndex == 2)
                            filterMovementList = filterMovementList.OrderBy(x => x.productcode).ToList();
                        else if (sortColumnIndex == 3)
                            filterMovementList = filterMovementList.OrderBy(x => x.category).ToList();
                        else if (sortColumnIndex == 4)
                            filterMovementList = filterMovementList.OrderBy(x => x.categorysub).ToList();
                        else if (sortColumnIndex == 5)
                            filterMovementList = filterMovementList.OrderBy(x => x.brand).ToList();
                        else if (sortColumnIndex == 6)
                            filterMovementList = filterMovementList.OrderBy(x => x.b1).ToList();
                        else if (sortColumnIndex == 7)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom1).ToList();
                        else if (sortColumnIndex == 8)
                            filterMovementList = filterMovementList.OrderBy(x => x.b2).ToList();
                        else if (sortColumnIndex == 9)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom2).ToList();
                        else if (sortColumnIndex == 10)
                            filterMovementList = filterMovementList.OrderBy(x => x.b3).ToList();
                        else if (sortColumnIndex == 11)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom3).ToList();
                        else if (sortColumnIndex == 12)
                            filterMovementList = filterMovementList.OrderBy(x => x.b4).ToList();
                        else if (sortColumnIndex == 13)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom4).ToList();
                        else if (sortColumnIndex == 14)
                            filterMovementList = filterMovementList.OrderBy(x => x.b5).ToList();
                        else if (sortColumnIndex == 15)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom5).ToList();
                        else if (sortColumnIndex == 16)
                            filterMovementList = filterMovementList.OrderBy(x => x.b6).ToList();
                        else if (sortColumnIndex == 17)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom6).ToList();
                        else if (sortColumnIndex == 18)
                            filterMovementList = filterMovementList.OrderBy(x => x.b7).ToList();
                        else if (sortColumnIndex == 19)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom7).ToList();
                        else if (sortColumnIndex == 20)
                            filterMovementList = filterMovementList.OrderBy(x => x.b8).ToList();
                        else if (sortColumnIndex == 21)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom8).ToList();
                        else if (sortColumnIndex == 22)
                            filterMovementList = filterMovementList.OrderBy(x => x.b9).ToList();
                        else if (sortColumnIndex == 23)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom9).ToList();
                        else if (sortColumnIndex == 24)
                            filterMovementList = filterMovementList.OrderBy(x => x.b10).ToList();
                        else if (sortColumnIndex == 25)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom10).ToList();
                        else if (sortColumnIndex == 26)
                            filterMovementList = filterMovementList.OrderBy(x => x.b11).ToList();
                        else if (sortColumnIndex == 27)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom11).ToList();
                        else if (sortColumnIndex == 28)
                            filterMovementList = filterMovementList.OrderBy(x => x.b12).ToList();
                        else if (sortColumnIndex == 29)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom12).ToList();
                        else if (sortColumnIndex == 30)
                            filterMovementList = filterMovementList.OrderBy(x => x.b13).ToList();
                        else if (sortColumnIndex == 31)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom13).ToList();
                        else if (sortColumnIndex == 32)
                            filterMovementList = filterMovementList.OrderBy(x => x.b14).ToList();
                        else if (sortColumnIndex == 33)
                            filterMovementList = filterMovementList.OrderBy(x => x.uom14).ToList();

                    }
                    else
                    {
                        if (sortColumnIndex == 1)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.productdesc).ToList();
                        else if (sortColumnIndex == 2)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.productcode).ToList();
                        else if (sortColumnIndex == 3)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.category).ToList();
                        else if (sortColumnIndex == 4)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.categorysub).ToList();
                        else if (sortColumnIndex == 5)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.brand).ToList();
                        else if (sortColumnIndex == 6)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b1).ToList();
                        else if (sortColumnIndex == 7)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom1).ToList();
                        else if (sortColumnIndex == 8)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b2).ToList();
                        else if (sortColumnIndex == 9)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom2).ToList();
                        else if (sortColumnIndex == 10)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b3).ToList();
                        else if (sortColumnIndex == 11)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom3).ToList();
                        else if (sortColumnIndex == 12)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b4).ToList();
                        else if (sortColumnIndex == 13)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom4).ToList();
                        else if (sortColumnIndex == 14)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b5).ToList();
                        else if (sortColumnIndex == 15)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom5).ToList();
                        else if (sortColumnIndex == 16)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b6).ToList();
                        else if (sortColumnIndex == 17)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom6).ToList();
                        else if (sortColumnIndex == 18)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b7).ToList();
                        else if (sortColumnIndex == 19)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom7).ToList();
                        else if (sortColumnIndex == 20)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b8).ToList();
                        else if (sortColumnIndex == 21)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom8).ToList();
                        else if (sortColumnIndex == 22)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b9).ToList();
                        else if (sortColumnIndex == 23)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom9).ToList();
                        else if (sortColumnIndex == 24)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b10).ToList();
                        else if (sortColumnIndex == 25)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom10).ToList();
                        else if (sortColumnIndex == 26)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b11).ToList();
                        else if (sortColumnIndex == 27)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom11).ToList();
                        else if (sortColumnIndex == 28)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b12).ToList();
                        else if (sortColumnIndex == 29)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom12).ToList();
                        else if (sortColumnIndex == 30)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b13).ToList();
                        else if (sortColumnIndex == 31)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom13).ToList();
                        else if (sortColumnIndex == 32)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.b14).ToList();
                        else if (sortColumnIndex == 33)
                            filterMovementList = filterMovementList.OrderByDescending(x => x.uom14).ToList();
                    }

                }



                var paginatedMovementList = filterMovementList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = stockMovementList.Count,
                    iTotalDisplayRecords = filterMovementList.Count,
                    aaData = paginatedMovementList
                },
                JsonRequestBehavior.AllowGet);




            }
            catch (Exception e)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        public static ICollection<stock_m_stockmovementlist> StockMovementList()
        {
            using (var context = new BigMacEntities())
            {
                var pList = context.Database.SqlQuery<stock_m_stockmovementlist>("call getStockMovementList()").ToList();
                return pList;
            }
        }

        public static ICollection<stock_m_stockmovementlist> AllStockMovementList()
        {
            using (var context = new BigMacEntities())
            {
                var pList = context.Database.SqlQuery<stock_m_stockmovementlist>("call getAllStockMovementList()").ToList();
                return pList;
            }
        }

        [HttpPost]
        public JsonResult StockTransferSave(stock_m_stocktransfer stktrn, string rcode = "Stock Transfer", string itemids = "")
        {
            var returnStr = "FAIL,";
            if (Session["userid"] != null)
            {
                try
                {

                    int rid = gc.getResourceID(rcode);

                    if (stktrn.id == 0)
                    {
                        var uniquecode = "";
                        uniquecode = DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim() + DateTime.Now.Hour.ToString().Trim() + DateTime.Now.Minute.ToString().Trim() + DateTime.Now.Second.ToString().Trim();
                        stktrn.uniquecode = uniquecode;
                        stktrn.createdate = DateTime.Now;
                        stktrn.lastmodifieddate = DateTime.Now;
                        stktrn.cocode = Convert.ToString(Session["cocode"]);
                        stktrn.createid = Convert.ToInt32(Session["userid"]);
                        stktrn.resourcecode = GeneralController.getGeneratedNewID("stock_m_stocktransfer", "resourcecode", "STKTRNPREF", "STKTRN");
                        db.stockTransfer.Add(stktrn);
                        db.SaveChanges();
                        saveToLog(rid, stktrn.id, "CREATE", "Add New Stock Transfer  Ref no- " + stktrn.resourcecode + ", ID- " + stktrn.id.ToString());

                        if (stktrn.items != null)
                        {
                            for (int i = 0; i < (stktrn.items.Count()); i++)
                            {
                                SaveNewStockTransferDetailItem(stktrn.items.ElementAt(i), stktrn.branchcode, stktrn.tobranchcode, stktrn.id, rcode, stktrn.resourcecode, rid, stktrn.status.ToUpper());
                            }
                        }
                        returnStr = "SUCCESS," + stktrn.uniquecode + "," + stktrn.id.ToString() + "," + stktrn.resourcecode.Trim() + "," + stktrn.status;
                    }
                    else
                    {
                        stock_m_stocktransfer itemtmp = db.stockTransfer.Find(stktrn.id);
                        string from = ""; string to = "";
                        string prevStatus = "";
                        from = "Status -" + itemtmp.status + ", Create By -" + itemtmp.createid + ", total -" + itemtmp.total_total.ToString();
                        to = "Status -" + stktrn.status + ", Create By -" + Session["userid"].ToString() + ", total -" + stktrn.total_total.ToString() + itemids;
                        itemtmp.branchcode = stktrn.branchcode; 
                        itemtmp.currency = stktrn.currency;
                        itemtmp.exchangerate = stktrn.exchangerate;
                        itemtmp.resourcedate = stktrn.resourcedate;
                        itemtmp.refno = stktrn.refno;
                        itemtmp.tobranchcode = stktrn.tobranchcode;
                        itemtmp.total_subtotal = stktrn.total_subtotal;
                        itemtmp.total_salestax = stktrn.total_salestax;
                        itemtmp.currency = stktrn.currency;
                        itemtmp.exchangerate = stktrn.exchangerate;
                        itemtmp.total_total = stktrn.total_total;
                        prevStatus = itemtmp.status.ToUpper();
                        itemtmp.status = stktrn.status;
                        itemtmp.post = stktrn.post;
                        if (prevStatus == "ACTIVE" && stktrn.post == 1)
                        {
                            itemtmp.postdate = DateTime.Now;
                            itemtmp.postid = Convert.ToInt32(Session["userid"]);
                        }
                        db.SaveChanges();
                        saveToLog(rid, itemtmp.id, "UPDATE", "UPDATE Stock Transfer - refno -" + itemtmp.resourcecode + ", id - " + itemtmp.id.ToString(), from, to);
                        using (var context = new BigMacEntities())
                        {
                            itemids = itemids + "0";
                            var value = context.Database.ExecuteSqlCommand("Delete from stock_m_stocktransfer_items where stocktransferid=" + stktrn.id.ToString() + " and id not in (" + itemids + ")");
                        }

                        if (stktrn.items != null)
                        {
                            for (int i = 0; i < (stktrn.items.Count()); i++)
                            {
                                if (stktrn.items.ElementAt(i).id == 0)
                                {
                                    SaveNewStockTransferDetailItem(stktrn.items.ElementAt(i), stktrn.branchcode, stktrn.tobranchcode, stktrn.id, rcode, itemtmp.resourcecode, rid, stktrn.status.ToUpper());
                                }
                                else
                                {
                                    UpdateStockTransferDetailitem(stktrn.items.ElementAt(i), stktrn.branchcode, stktrn.tobranchcode, rcode, itemtmp.resourcecode, rid, prevStatus, stktrn.status.ToUpper());
                                }
                            }
                        }
                        returnStr = "SUCCESS," + itemtmp.uniquecode + "," + stktrn.id.ToString() + "," + itemtmp.resourcecode.Trim() + "," + stktrn.status;
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
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public void SaveNewStockTransferDetailItem(stock_m_stocktransfer_items item, string frombranchcode, string tobranchcode, int headerid, string resource, string resourcecode, int rid = 0, string status = "Active")
        {
            //Changed by ZawZO on 21 Oct 2015
            if (item.productcode != null)
            {
                item.createdate = DateTime.Now;
                item.lastmodifieddate = DateTime.Now;
                item.resourcecode = resourcecode;
                item.createid = Convert.ToInt32(Session["userid"]);
                item.stocktransferid = headerid;
                db.stockTransferItems.Add(item);
                db.SaveChanges();
                saveToLog(rid, item.id, "CREATE", "Add New Deatil Item - product ID -" + item.productid, "RefNo no- " + resourcecode + ", Item ID- " + item.id.ToString());
                if (status.ToUpper() == "CLOSE")
                {
                    AddTransferTomovementTable(item, frombranchcode, tobranchcode, "Transfer", resourcecode, rid);
                }
            }
        }

        public void UpdateStockTransferDetailitem(stock_m_stocktransfer_items item, string frombranchcode, string tobranchcode, string resource, string resourcecode, int rid, string prevStatus, string newStatus)
        {
            stock_m_stocktransfer_items tmp = db.stockTransferItems.Find(item.id);
            string from = ""; string to = "";
            if (tmp != null)
            {
                from = "proudctid -" + tmp.productid + ",product desc -" + tmp.productdesc + ",Price -" + tmp.unitprice.ToString() + ", Qty -" + tmp.qty.ToString() + ",uom" + tmp.uom;
                to = "proudctid -" + item.productid + ",product desc -" + item.productdesc + ",Price -" + item.unitprice.ToString() + ", Qty -" + item.qty.ToString() + ",uom" + item.uom;

                if (newStatus == "CLOSE")
                {
                    if (prevStatus == "ACTIVE")
                    {
                        AddTransferTomovementTable(item, frombranchcode, tobranchcode, "Transfer", resourcecode, rid);
                    }
                }
                tmp.currency = item.currency;
                tmp.exchangerate = item.exchangerate;
                tmp.productid = item.productid;
                tmp.productdesc = item.productdesc;
                tmp.productcode = item.productcode;
                tmp.unitprice = item.unitprice;
                tmp.qty = item.qty;
                tmp.uom = item.uom;
                tmp.taxamount = item.taxamount;
                tmp.discountamount = item.discountamount;
                tmp.lineamount = item.lineamount;
                tmp.lineno = item.lineno;
                db.SaveChanges();
                saveToLog(rid, item.id, "UPDATE", "Update Detail item ID-" + item.id.ToString() + " RefNo -" + resourcecode, from, to);
            }
        }

        public void AddTransferTomovementTable(stock_m_stocktransfer_items item, string frombranchcode, string tobranchcode, string resource, string resourcecode, int rid = 0)
        {
            int pid = item.productid;
            if (item.qty != 0)
            {
                stock_m_stockmovement tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == frombranchcode).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (tmp != null)
                    prebalance = tmp.productbalance;

                stock_m_stockmovement mov1 = new stock_m_stockmovement();
                stock_m_stockmovement mov2 = new stock_m_stockmovement();
                mov1.lastbalance = 0;
                mov1.productid = item.productid;
                mov1.productcode = item.productcode;
                mov1.productdesc = item.productdesc;
                mov1.resourcecode = resourcecode;
                mov1.uom = item.uom;
                mov1.unitprice = item.unitprice;
                mov1.currency = item.currency;
                mov1.exchangerate = Convert.ToDouble(item.exchangerate);
                mov1.createid = Convert.ToInt32(Session["userid"]);
                mov1.stockmoduletype = resource;
                mov1.cocode = Convert.ToString(Session["cocode"]);
                mov1.branchcode = frombranchcode;

                double? pqty = 0;
                double? tmpbalance = item.qty;
                Boolean bflag = true;
                while (tmpbalance > 0 && bflag)
                {
                    mov1.lastbalance = 0;
                    //Finish receive last balace count
                    tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == frombranchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                    if (tmp != null)
                    {
                        double? plbal = 0;
                        pqty = 0;
                        if (tmp.lastbalance >= tmpbalance)
                        {
                            pqty = tmpbalance;
                            tmp.lastbalance = tmp.lastbalance - tmpbalance;
                            tmpbalance = 0;
                        }
                        else
                        {
                            pqty = tmp.lastbalance;
                            tmpbalance = tmpbalance - tmp.lastbalance;
                            tmp.lastbalance = 0;
                        }
                        db.SaveChanges();
                        saveToLog(rid, tmp.id, "Update", "Update LastBalance for Adjustment ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                        //Finish receive last balace count
                        mov1.stockrefid = tmp.id;
                        mov1.createdate = DateTime.Now;
                        mov1.lastmodifieddate = DateTime.Now;
                        mov1.qty = pqty * (-1); 
                        mov1.taxamount = (item.taxamount / item.qty) * pqty;
                        mov1.discountamount = (item.discountamount / item.qty) * pqty;
                        mov1.lineamount = (item.lineamount / item.qty) * pqty;
                        mov1.productbalance = prebalance + mov1.qty;
                        prebalance = mov1.productbalance;
                        db.StockMovement.Add(mov1);
                        db.SaveChanges();
                        saveToLog(rid, mov1.id, "CREATE", "Add New Balance ProductID -" + item.productid + ",Branch Code-" + frombranchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov1.id.ToString());
                        if (tmpbalance > 0)
                        {
                            tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == frombranchcode).OrderByDescending(x => x.id).FirstOrDefault();
                            if (tmp != null)
                                prebalance = tmp.productbalance;
                        }
                    }
                    else
                        bflag = false;
                }

                if (bflag == false && tmpbalance > 0)
                {
                    mov1.lastbalance = tmpbalance * (-1);
                    mov1.stockrefid = 0;
                    mov1.createdate = DateTime.Now;
                    mov1.lastmodifieddate = DateTime.Now;
                    mov1.qty = tmpbalance * (-1);
                    mov1.taxamount = (item.taxamount / item.qty) * tmpbalance;
                    mov1.discountamount = (item.discountamount / item.qty) * tmpbalance;
                    mov1.lineamount = (item.lineamount / item.qty) * tmpbalance;
                    mov1.productbalance = prebalance + mov1.qty;
                    db.StockMovement.Add(mov1);
                    db.SaveChanges();
                    saveToLog(rid, mov1.id, "CREATE", "Add New Balance ProductID -" + item.productid + ",Branch Code-" + tobranchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov1.id.ToString());
                }

                mov2 = mov1;
                tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == tobranchcode).OrderByDescending(x => x.id).FirstOrDefault();
                prebalance = 0;
                if (tmp != null)
                    prebalance = tmp.productbalance;

                tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == tobranchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                if (tmp != null)
                {
                    double? plbal = 0;
                    plbal = tmp.lastbalance;
                    tmp.lastbalance = tmp.lastbalance + item.qty;
                    db.SaveChanges();
                    saveToLog(rid, tmp.id, "Update", "Update LastBalance for Transfer ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                    mov2.stockrefid = tmp.id;
                    mov2.lastbalance = 0;
                }
                else
                {
                    mov2.stockrefid = 0;
                    mov2.lastbalance = item.qty;
                }


                mov2.qty = item.qty;
                mov2.branchcode = tobranchcode;
                mov2.taxamount = item.taxamount;
                mov2.discountamount = item.discountamount;
                mov2.lineamount = item.lineamount; 
                mov2.productbalance = prebalance + mov1.qty;
                db.StockMovement.Add(mov2);
                db.SaveChanges();
                saveToLog(rid, mov2.id, "CREATE", "Add Transfer To Branch -" + tobranchcode + ",ProductID -" + item.productid + ", Qty" + item.qty + ",UOM -" + item.uom, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov2.id.ToString());
            }

        }


        public JsonResult getStockTransferDetailWithPaging(int pageno = 0, int id = 0)
        {
            try
            {
                //Changed by ZawZO on 21 Oct 2015
                //int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                //ICollection<stock_m_stocktransfer_items> SList = db.stockTransferItems.Where(x => x.stocktransferid== id).ToList();
                //var paginatedQSList = new PaginatedList<stock_m_stocktransfer_items>(SList.AsQueryable(), pageno, pagesize);
                //var paginatedSList = paginatedQSList.ToList();

                //if (paginatedSList != null)
                //{
                //    if (paginatedSList.Count > 0)
                //        paginatedSList.ElementAt(0).TotalPages = paginatedQSList.TotalPages;
                //}

                //return Json(paginatedSList, JsonRequestBehavior.AllowGet);
                ICollection<stock_m_stocktransfer_items> SList = db.stockTransferItems.Where(x => x.stocktransferid == id).ToList();
                return Json(SList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public ActionResult StockTransferListing(string rcode = "Stock Transfer", string acode = "")
        {
            int i = 0;
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x => x.branchcode != "MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            ViewBag.BranchOptions = branches;

            ViewBag.CurrencyOptions = db.Currency.Select(x => new { x.value, x.ExchangeRate }).OrderBy(x => x.value).ToList();
            ViewBag.Rcode = rcode;
            ViewBag.Acode = acode;
            return View();
        }
        public ActionResult StockTransferIndex()
        {
            return View(db.stockAdjustment.Include("Branch").Include("Create").Where(x => x.status == "Active").ToList());
        }

        public ActionResult StockTransferDetail(int stid = 0)
        {
            try
            {
                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
                ViewBag.UOMOptions = db.UOM.ToList();
                ViewBag.SRItemOptions = db.stockTransferItems.Where(x => x.stocktransferid== stid).OrderBy(x => x.lineno).ToList();
                stock_m_stocktransfer_items sitem = new stock_m_stocktransfer_items();
                sitem.stocktransferid = stid;
                sitem.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
                var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

                if (ex.Length > 0)
                    sitem.exchangerate = Convert.ToDouble(ex);
                else
                    sitem.exchangerate = 0;

                return View(sitem);
            }
            catch (Exception e)
            {
                return View();
            }

        }


        [Authorize]
        public JsonResult getProductDetails(string productlist = "")
        {
            try
            {

                string[] products = productlist.Split(',');
                List<Product_m_Productdtl> selectedProductList = new List<Product_m_Productdtl>();

                for (int i = 0; i < products.Length; i++)
                {
                    ICollection<Product_m_Productdtl> productdata = ProductController.ProductList("%", "%", Convert.ToInt32(products[i]), "", "", "", 0, 0, 1);

                    if (productdata.Count > 0)
                    {
                        Product_m_Productdtl p = new Product_m_Productdtl();
                        p.id = productdata.Select(x => x.id).FirstOrDefault();
                        p.productid = productdata.Select(x => x.id).FirstOrDefault();
                        p.productcode = productdata.Select(x => x.productcode).FirstOrDefault();
                        p.desc = productdata.Select(x => x.desc).FirstOrDefault();
                        p.uom = productdata.Select(x => x.uom).FirstOrDefault();
                        p.sellprice = productdata.Select(x => x.sellprice).FirstOrDefault();
                        p.servicecommission = productdata.Select(x => x.servicecommission).FirstOrDefault();

                        List<string> stock = GetProductCurrentStock(WebConfigurationManager.AppSettings["DefaultBranch"].ToString(), p.id);

                        if (stock.Count > 0)
                        {
                            p.stockqty = Convert.ToDouble(stock[0].ToString());
                        }
                        else
                        {
                            p.stockqty = 0;
                        }

                        selectedProductList.Add(p);
                    }
                }

                return Json(selectedProductList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        [Authorize]
        public JsonResult getProductDetailsWithNoStock(string productlist = "")
        {
            try
            {

                string[] products = productlist.Split(',');
                List<Product_m_Productdtl> selectedProductList = new List<Product_m_Productdtl>();

                for (int i = 0; i < products.Length; i++)
                {
                    ICollection<Product_m_Productdtl> productdata = ProductController.ProductList("%", "%", Convert.ToInt32(products[i]), "", "", "", 0, 0, 1);

                    if (productdata.Count > 0)
                    {
                        Product_m_Productdtl p = new Product_m_Productdtl();
                        p.id = productdata.Select(x => x.id).FirstOrDefault();
                        p.productid = productdata.Select(x => x.id).FirstOrDefault();
                        p.productcode = productdata.Select(x => x.productcode).FirstOrDefault();
                        p.desc = productdata.Select(x => x.desc).FirstOrDefault();
                        p.uom = productdata.Select(x => x.uom).FirstOrDefault();
                        p.sellprice = productdata.Select(x => x.sellprice).FirstOrDefault();
                        p.servicecommission = productdata.Select(x => x.servicecommission).FirstOrDefault();
                        selectedProductList.Add(p);
                    }
                }

                return Json(selectedProductList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }



        [Authorize]
        public JsonResult getBranchProductStock(string branchcode = "", int productid = 0)
        {
            try
            {

                string stockqty = "0";

                List<string> stock = GetProductCurrentStock(branchcode, productid);

                if (stock.Count > 0)
                {
                    stockqty = stock[0].ToString();
                }

                return Json(stockqty.Replace(".00", ""), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }

        }

        public static List<string> GetProductCurrentStock(string branchcode, int productid)
        {
            using (var context = new BigMacEntities())
            {
                var pList = context.Database.SqlQuery<string>("call getProductCurrentStock('" + branchcode + "'," + productid + ")").ToList();
                return pList;
            }
        }


        [HttpPost]
        public ActionResult StockTransferDetail(stock_m_stocktransfer_items sitem)
        {
            return View(sitem);
        }

        public void AddToMovementTable(dynamic item, string branchcode, string resource, string resourcecode, int rid = 0)
        {
            int pid = item.productid;
            if (item.qty != 0)
            {
                stock_m_stockmovement tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (tmp != null)
                    prebalance = tmp.productbalance;

                stock_m_stockmovement mov = new stock_m_stockmovement();
                mov.resourcecode = resourcecode; 
                mov.productid = item.productid;
                mov.productcode = item.productcode;
                mov.productdesc = item.productdesc;
                mov.uom = item.uom;
                mov.unitprice = item.unitprice;
                mov.currency = item.currency;
                mov.exchangerate = item.exchangerate;
                mov.createid = Convert.ToInt32(Session["userid"]);
                mov.stockmoduletype = resource;
                mov.cocode = Convert.ToString(Session["cocode"]);
                mov.branchcode = branchcode; 

                if (item.qty > 0)
                {
                    tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                    if (tmp != null)
                    {
                        double? plbal = 0;
                        plbal = tmp.lastbalance;
                        tmp.lastbalance = tmp.lastbalance + item.qty;
                        db.SaveChanges();
                        saveToLog(rid, tmp.id, "Update", "Update LastBalance for Adjustment ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                        mov.stockrefid = tmp.id;
                        mov.lastbalance = 0;
                    }
                    else
                        mov.lastbalance = item.qty;

                    mov.createdate = DateTime.Now;
                    mov.lastmodifieddate = DateTime.Now;
                    mov.qty = item.qty;
                    mov.taxamount = item.taxamount;
                    mov.discountamount = item.discountamount;
                    mov.lineamount = item.lineamount;
                    mov.productbalance = prebalance + mov.qty;
                    db.StockMovement.Add(mov);
                    db.SaveChanges();
                    saveToLog(rid, mov.id, "CREATE", "Add New Balance  ProductID -" + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                }
                else
                {
                    double? pqty = 0;
                    double? tmpbalance = item.qty * (-1);
                    Boolean bflag = true;
                    while (tmpbalance > 0 && bflag)
                    {
                        mov.lastbalance = 0;
                        tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                        if (tmp != null)
                        {
                            double? plbal = 0;
                            plbal = tmp.lastbalance;
                            if (tmp.lastbalance >= tmpbalance)
                            {
                                pqty = tmpbalance;
                                tmp.lastbalance = tmp.lastbalance - tmpbalance;
                                tmpbalance = 0;
                            }
                            else
                            {
                                pqty = tmp.lastbalance;
                                tmpbalance = tmpbalance - tmp.lastbalance;
                                tmp.lastbalance = 0;
                            }
                            db.SaveChanges();
                            saveToLog(rid, tmp.id, "Update", "Update LastBalance for Adjustment ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                            mov.stockrefid = tmp.id;
                            mov.createdate = DateTime.Now;
                            mov.lastmodifieddate = DateTime.Now;
                            mov.qty = pqty * (-1); 
                            mov.taxamount = (item.taxamount / item.qty) * pqty;
                            mov.discountamount = (item.discountamount / item.qty) * pqty;
                            mov.lineamount = (item.lineamount / item.qty) * pqty;
                            mov.productbalance = prebalance + mov.qty;
                            prebalance = mov.productbalance;
                            db.StockMovement.Add(mov);
                            db.SaveChanges();
                            saveToLog(rid, mov.id, "CREATE", "Add New Balance ProductID -" + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                            if (tmpbalance > 0)
                            {
                                tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode).OrderByDescending(x => x.id).FirstOrDefault();
                                if (tmp != null)
                                    prebalance = tmp.productbalance;
                            }
                        }
                        else
                            bflag = false;
                    }
                                        
                    if (bflag == false && tmpbalance > 0)
                    {
                        mov.lastbalance = tmpbalance * (-1);
                        mov.stockrefid = 0;
                        mov.createdate = DateTime.Now;
                        mov.lastmodifieddate = DateTime.Now;
                        mov.qty = tmpbalance * (-1);
                        mov.taxamount = (item.taxamount / item.qty) * tmpbalance;
                        mov.discountamount = (item.discountamount / item.qty) * tmpbalance;
                        mov.lineamount = (item.lineamount / item.qty) * tmpbalance;
                        mov.productbalance = prebalance + mov.qty;
                        db.StockMovement.Add(mov);
                        db.SaveChanges();
                        saveToLog(rid, mov.id, "CREATE", "Add New Balance ProductID - " + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                    }
                } //End for Issue Quantity
            }
        }

        public void AddToMovementTableAdjustment(dynamic item, string branchcode, string resource, string resourcecode, int rid = 0)
        {
            int pid = item.productid;
            if (item.qty != 0)
            {
                stock_m_stockmovement tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode).OrderByDescending(x => x.id).FirstOrDefault();
                double? prebalance = 0;
                if (tmp != null)
                    prebalance = tmp.productbalance;

                stock_m_stockmovement mov = new stock_m_stockmovement();
                mov.resourcecode = resourcecode;
                mov.productid = item.productid;
                mov.productcode = item.productcode;
                mov.productdesc = item.productdesc;
                mov.uom = item.uom;
                mov.unitprice = item.unitprice;
                mov.currency = item.currency;
                mov.exchangerate = item.exchangerate;
                mov.createid = Convert.ToInt32(Session["userid"]);
                mov.stockmoduletype = resource;
                mov.cocode = Convert.ToString(Session["cocode"]);
                mov.branchcode = branchcode;

                if (item.qty > 0)
                {
                    //tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                    //if (tmp != null)
                    //{
                    //    double? plbal = 0;
                    //    plbal = tmp.lastbalance;
                    //    tmp.lastbalance = tmp.lastbalance + item.qty;
                    //    db.SaveChanges();
                    //    saveToLog(rid, tmp.id, "Update", "Update LastBalance for Adjustment ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                    //    mov.stockrefid = tmp.id;
                    //    mov.lastbalance = 0;
                    //}
                    //else
                        mov.lastbalance = item.qty;

                    mov.createdate = DateTime.Now;
                    mov.lastmodifieddate = DateTime.Now;
                    mov.qty = item.qty - item.previousqty; 
                    mov.taxamount = item.taxamount;
                    mov.discountamount = item.discountamount;
                    mov.lineamount = item.lineamount;
                    mov.productbalance = prebalance + mov.qty;
                    db.StockMovement.Add(mov);
                    db.SaveChanges();
                    saveToLog(rid, mov.id, "CREATE", "Add New Balance  ProductID -" + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                }
                else
                {
                    double? pqty = 0;
                    double? tmpbalance = item.qty * (-1);
                    Boolean bflag = true;
                    while (tmpbalance > 0 && bflag)
                    {
                        mov.lastbalance = 0;
                        tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode && x.lastbalance > 0).OrderBy(x => x.id).FirstOrDefault();
                        if (tmp != null)
                        {
                            double? plbal = 0;
                            plbal = tmp.lastbalance;
                            if (tmp.lastbalance >= tmpbalance)
                            {
                                pqty = tmpbalance;
                                tmp.lastbalance = tmp.lastbalance - tmpbalance;
                                tmpbalance = 0;
                            }
                            else
                            {
                                pqty = tmp.lastbalance;
                                tmpbalance = tmpbalance - tmp.lastbalance;
                                tmp.lastbalance = 0;
                            }
                            db.SaveChanges();
                            saveToLog(rid, tmp.id, "Update", "Update LastBalance for Adjustment ProductID -" + pid + ",Branch Code-" + tmp.branchcode, "Ref no- " + tmp.resourcecode + ", Item ID- " + tmp.id.ToString() + ", Qty-" + item.qty + ", Old last balance -" + plbal.ToString() + ", New lastbalance-" + tmp.lastbalance.ToString());
                            mov.stockrefid = tmp.id;
                            mov.createdate = DateTime.Now;
                            mov.lastmodifieddate = DateTime.Now;
                            mov.qty = pqty * (-1);
                            mov.taxamount = (item.taxamount / item.qty) * pqty;
                            mov.discountamount = (item.discountamount / item.qty) * pqty;
                            mov.lineamount = (item.lineamount / item.qty) * pqty;
                            mov.productbalance = prebalance + mov.qty;
                            prebalance = mov.productbalance;
                            db.StockMovement.Add(mov);
                            db.SaveChanges();
                            saveToLog(rid, mov.id, "CREATE", "Add New Balance ProductID -" + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                            if (tmpbalance > 0)
                            {
                                tmp = db.StockMovement.Where(x => x.productid == pid && x.branchcode == branchcode).OrderByDescending(x => x.id).FirstOrDefault();
                                if (tmp != null)
                                    prebalance = tmp.productbalance;
                            }
                        }
                        else
                            bflag = false;
                    }

                    if (bflag == false && tmpbalance > 0)
                    {
                        mov.lastbalance = tmpbalance * (-1);
                        mov.stockrefid = 0;
                        mov.createdate = DateTime.Now;
                        mov.lastmodifieddate = DateTime.Now;
                        mov.qty = tmpbalance * (-1);
                        mov.taxamount = (item.taxamount / item.qty) * tmpbalance;
                        mov.discountamount = (item.discountamount / item.qty) * tmpbalance;
                        mov.lineamount = (item.lineamount / item.qty) * tmpbalance;
                        mov.productbalance = prebalance + mov.qty;
                        db.StockMovement.Add(mov);
                        db.SaveChanges();
                        saveToLog(rid, mov.id, "CREATE", "Add New Balance ProductID - " + item.productid + ",Branch Code-" + branchcode, "Ref no- " + resourcecode + ", Item ID- " + item.id.ToString() + "Movement ID -" + mov.id.ToString());
                    }
                } //End for Issue Quantity
            }
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
    }
}
