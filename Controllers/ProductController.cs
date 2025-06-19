using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;
using MySql.Data.MySqlClient;
using System.Web.Configuration;

namespace BigMac.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        private BigMacEntities db = new BigMacEntities();

        [Authorize]
        public ActionResult getProductInfo(string prodInfo = "", int count = 10)
        {
            try
            {
                try
                {
                    var ptmplist = db.products.Where(x => x.status == "Active" || x.status == "ACTIVE").ToList();
                    //var listwithalt = ptmplist.Join(db.productALTCode, prod => prod.id, alt => alt.productid, (prod, alt) => new { prodlist = prod, productalt = alt }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productalt.altcode }).Distinct().ToList();



                   var altCodeList = db.productALTCode.ToList();
                   var listwithalt =  (from product in ptmplist
                                       join alt in altCodeList on product.id equals alt.productid into prod
                                       from result in prod.DefaultIfEmpty()
                        select new {
                           id = product.id,
                           desc = product.desc,
                           category = product.category,
                           categorysub = product.categorysub,
                           brand = product.brand,
                           RangesNSeries = product.RangesNSeries,
                           productcode = product.productcode,
                           altcode = (result != null) ? result.altcode : ""
               
                        }
                        ).Distinct().ToList();

                    var prodlist = listwithalt.Where(x => x.productcode.ToUpper().Contains(prodInfo.ToUpper()) || x.desc.ToUpper().Contains(prodInfo.ToUpper()) || x.altcode.ToUpper().Contains(prodInfo.ToUpper())).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).ToList();
                    //var prodlist = ptmplist.Where(x => x.productcode.ToUpper().Contains(prodInfo.ToUpper()) || x.desc.ToUpper().Contains(prodInfo.ToUpper())).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).ToList();

                    var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice, x.productprices.servicecommission, awardbonus = x.productprices.awardbonus == null ? 0 : x.productprices.awardbonus, awarddollars = x.productprices.awarddollars == null ? 0 : x.productprices.awarddollars, serviceallowance = x.productprices.serviceallowance == null ? 0 : x.productprices.serviceallowance }).Distinct().ToList();
                    plist = plist.OrderBy(x => x.desc).Take(count).ToList();
                    return Json(plist, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
                
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public ActionResult getProductInfoByCode(string code)
        {
            try
            {
                try
                {
                    var ptmplist = db.products.Where(x => x.status == "Active" || x.status == "ACTIVE").ToList();
                    var prodlist = ptmplist.Where(x => x.productcode == code).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).ToList();
                    var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice, x.productprices.servicecommission, awardbonus = x.productprices.awardbonus == null ? 0 : x.productprices.awardbonus, awarddollars = x.productprices.awarddollars == null ? 0 : x.productprices.awarddollars, serviceallowance = x.productprices.serviceallowance == null ? 0 : x.productprices.serviceallowance }).Distinct().ToList();
                    plist = plist.OrderBy(x => x.desc).ToList();
                    return Json(plist, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }

            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        [Authorize]
        public ActionResult getProductInfoAppointment(string prodInfo = "", int count = 10)
        {
            try
            {
                try
                {
                    var ptmplist = db.products.Where(x => x.status == "Active" || x.status == "ACTIVE").ToList();
                    var prodlist = ptmplist.Where(x => x.productcode.ToUpper().Contains(prodInfo.ToUpper()) || x.desc.ToUpper().Contains(prodInfo.ToUpper())).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).ToList();
                    //Changed by ZawZO on 19 Aug 2016
                    var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice, x.productprices.servicecommission, awardbonus = x.productprices.awardbonus == null ? 0 : x.productprices.awardbonus, awarddollars = x.productprices.awarddollars == null ? 0 : x.productprices.awarddollars,redeemdollars = x.productprices.redeemdollars ?? 0, redeembonus = x.productprices.redeembonus ?? 0, serviceallowance = x.productprices.serviceallowance == null ? 0 : x.productprices.serviceallowance }).Distinct().ToList();
                    plist = plist.OrderBy(x => x.desc).Take(count).ToList();
                    return Json(plist, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }

            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }


        //Added by ZawZO on 3 Dec 2015
        [Authorize]
        public ActionResult getProductInfoByAltCode(string altCode = "", int count = 10)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    //var searchfilter = "%" + altCode + "%";
                    var prodlist = context.Database.SqlQuery<Product_m_Product>("call getProductListWithAltCode('" + altCode + "')").ToList();
                    //Changed by ZawZO on 19 Aug 2016
                    var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice, x.productprices.servicecommission, awardbonus = x.productprices.awardbonus == null ? 0 : x.productprices.awardbonus, awarddollars = x.productprices.awarddollars == null ? 0 : x.productprices.awarddollars }).Distinct().ToList();                                
                    plist = plist.OrderBy(x => x.desc).Take(count).ToList();
                    return Json(plist, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        [Authorize]
        public ActionResult getProductInfoByFilter(string ptype = "Package", string pcategory = "", string pbrand = "", string prnc = "", int count = 10)
        {
            try
            {
                ptype = ptype.ToUpper();
                pcategory = pcategory.ToUpper();
                pbrand = pbrand.ToUpper();
                prnc = prnc.ToUpper();
                var ptmplist = db.products.Where(x => x.status == "Active" || x.status == "ACTIVE").ToList();
                var prodlist = ptmplist.Where(x => x.category.ToUpper() == ptype && (x.categorysub.ToUpper() == pcategory || pcategory == "") && (x.brand.ToUpper() == pbrand || pbrand == "") && (x.RangesNSeries.ToUpper() == prnc || prnc == "")).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).ToList();
                //var prodlist = ptmplist.Where(x => x.productcode.ToUpper().Contains(prodInfo.ToUpper()) || x.desc.ToUpper().Contains(prodInfo.ToUpper())).Select(x => new { x.id, x.productcode, x.desc, x.category, x.categorysub, x.brand, x.RangesNSeries }).OrderBy(x => new { x.desc, x.productcode }).ToList();
                var plist = prodlist.Join(db.productprices, prod => prod.id, price => price.productid, (prod, price) => new { prodlist = prod, productprices = price }).Select(x => new { x.prodlist.id, x.prodlist.desc, x.prodlist.category, x.prodlist.categorysub, x.prodlist.brand, x.prodlist.RangesNSeries, x.prodlist.productcode, x.productprices.uom, x.productprices.sellprice,x.productprices.servicecommission }).Distinct().ToList();                                
                //plist = plist.OrderBy(x => x.desc).Take(count).ToList();
                return Json(plist.OrderBy(x=>x.desc), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult ProductListing(string ptype = "Product", string filter = "")
        {
            try
            {
                ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                //ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                ViewBag.PType = ptype;
                ViewBag.Filter = filter;

                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
            //    using (var context = new BigMacEntities())
            //    {
            //        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductList(0,'')").ToList() ;
            //        //return View(pList);
            //    }
                
            }
            catch (Exception e)
            { 
            //    //return View(db.products.ToList()); 
            }
            //return View(db.products.ToList());
            return View();
        }

        public JsonResult getProducts(string ptype = "Package", string pcategory = "", string pbrand = "", string prnc="")
        {
            try
            {
                ICollection<Product_m_Product> pl = db.products.Where(x => x.category == ptype && x.status == "Active" && (x.categorysub == pcategory || pcategory=="") && (x.brand == pbrand || pbrand == "") && (x.RangesNSeries== prnc || prnc == "")).OrderBy(x => x.desc).ToList();
                return Json(pl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public static ICollection<Product_m_Productdtl> ProductList(string ptype = "Product", string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0, int stkflag =0)
        {
                using (var context = new BigMacEntities())
                {
                    var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductList('" + ptype + "','" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + ")").ToList();
                    return pList;
                }
        }
        //Added by ZawZO on 5 Dec 2015
        [Authorize]
        public JsonResult getProductCatSubOptions(string pCat = "")
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var pList = context.Database.SqlQuery<Product_z_CategorySub>("call getcategorysublistByFilter('" + pCat + "')").ToList();
                    return Json(pList, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 5 Dec 2015
        [Authorize]
        public JsonResult getProductBrandOptions(string pCat = "", string pSubCat = "")
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var pList = context.Database.SqlQuery<Product_z_ProductBrand>("call getbrandlistByFilter('" + pCat + "','" + pSubCat + "')").ToList();
                    return Json(pList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 5 Dec 2015
        [Authorize]
        public JsonResult getProductRangeNSeriesOptions(string pCat = "", string pSubCat = "", string pBrand = "")
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    var pList = context.Database.SqlQuery<Product_z_ProductBrand>("call getrangeNSerieslistByFilter('" + pCat + "','" + pSubCat + "','" + pBrand + "')").ToList();
                    return Json(pList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed" + e.InnerException.Message, JsonRequestBehavior.AllowGet); }
        }
        public static ICollection<Product_m_Productdtl> ProductRedeemList(string pfltrFld, string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0, int stkflag = 0, int cussupid = 0)
        {
            using (var context = new BigMacEntities())
            {
                //Changed by ZawZO on 21 Aug 2015
                //var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemList('" + pfltrFld + "','" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + ")").ToList();
                //return pList;
                if (filter=="%%"){
                    var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemList('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                    return pList;
                }
                else
                {
                    if (pfltrFld == "Product_m_product.brand")
                    {
                        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemListbybrand('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                        return pList;
                    }
                    else if (pfltrFld == "Product_m_product.categorysub")
                    {
                        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemListbysubcat('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                        return pList;
                    }
                    else if (pfltrFld == "Product_m_product.category")
                    {
                        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemListbycat('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                        return pList;
                    }
                    else if (pfltrFld == "Product_m_product.desc")
                    {
                        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemList('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                        return pList;
                    }
                    else
                    {
                        var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductRedeemListbyRangesNSeries('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + "," + cussupid + ")").ToList();
                        return pList;
                    }
                }
            }
        }
        //Added by ZawZO on 22 Aug 2015
        public JsonResult getProductTopupList( string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0)
        {
            try
            {
                var PList = ProductTopupList(filter, pid, pcode, cat, brand, firstUOM, skid);
                return Json(PList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 22 Aug 2015
        public static ICollection<Product_m_Productdtl> ProductTopupList( string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0, int stkflag = 0)
        {
            using (var context = new BigMacEntities())
            {
                var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductTopupList('" + filter + "'," + pid + ",'" + pcode + "','" + cat + "','" + brand + "'," + firstUOM + "," + skid + "," + stkflag + ")").ToList();
                return pList;
            }
        }

        public JsonResult getProductList(string ptype = "Product", string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0)
        {
            try
            {
                //ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                //ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                //using (var context = new BigMacEntities())
                //{
                //    var pList = context.Database.SqlQuery<Product_m_Productdtl>("call getProductList('" + ptype + "','" + filter + "'," + pid + ",'" + pcode + "','" + cat +"','" + brand + "'," + firstUOM + ")").ToList();                    
                //    return Json(pList, JsonRequestBehavior.AllowGet);
                //}
                var PList = ProductList(ptype, filter, pid, pcode, cat, brand, firstUOM,skid);
                return Json(PList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }
        //Added by ZawZO on 20 Aug 2015
        public JsonResult getRedeemProductWithPaging(int pageno = 0, string pfltrfld = "Product_m_product.category", string filter = "%", int firstUOM = 0, int skid = 0, int cussupid = 0)
        {
            try
            {
                
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                //var PList = ProductList(ptype, filter, 0, "", "", "", firstUOM, skid);
               // ProductRedeemList(string pfltrFld, string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0, int stkflag = 0, int cussupid = 0)

                var PList = ProductRedeemList(pfltrfld, filter, 0, "", "", "", firstUOM, skid, 0 , cussupid);
                string cocode = Session["cocode"].ToString();
                PList = PList.Where(x => x.cocode == cocode).ToList();
                var paginatedQPList = new PaginatedList<Product_m_Productdtl>(PList.AsQueryable(), pageno, pagesize);
                var paginatedPList = paginatedQPList.ToList();

                if (paginatedPList != null)
                {
                    if (paginatedPList.Count > 0)
                        paginatedPList.ElementAt(0).TotalPages = paginatedQPList.TotalPages;
                    //totalpages = paginatedQPList.TotalPages;
                }

                return Json(paginatedPList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult getProductListWithPagingOld(ref int totalpages, int pageno = 0, string ptype = "Product", string filter = "%", int pid = 0, string pcode = "", string cat = "", string brand = "", int firstUOM = 0, int skid = 0)
        {
            try
            {
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                ICollection<Product_m_Productdtl> PList = ProductList(ptype, filter, pid, pcode, cat, brand, firstUOM, skid);
                var paginatedQPList = new PaginatedList<Product_m_Productdtl>(PList.AsQueryable(), pageno, pagesize);
                var paginatedPList = paginatedQPList.ToList();

                if (paginatedPList != null)
                {
                    if (paginatedPList.Count > 0)
                        paginatedPList.ElementAt(0).TotalPages = paginatedQPList.TotalPages;
                    totalpages = paginatedQPList.TotalPages;
                }

                return Json(paginatedPList, JsonRequestBehavior.AllowGet);
     
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public ActionResult getPromotionListWithPaging(jQueryDataTableParamModel param)
        {
            //return View(db.productpromotions.OrderByDescending(x => new { x.startdate }).OrderBy(x => x.product.desc).ToList());
            try
            {
                //int pagesize = Convert.Toint(WebConfigurationManager.AppSettings["pagesize"]);
                ICollection<Product_m_ProductPromotion> PList = db.productpromotions.OrderByDescending(x => new { x.startdate, x.enddate }).OrderBy(x => x.product.desc).ToList();
                //var paginatedQPList = new PaginatedList<Product_m_Productdtl>(PList.AsQueryable(), param.iDisplayStart, pagesize);
                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterPList = PList.Where(x => x.product.desc.ToUpper().Contains(searchValue.ToUpper())).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                //Func<Product_m_ProductPromotion, DateTime> dateOrder = (x => sortColumnIndex == 2 ? x.startdate :
                //                                                        x.enddate);
                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterPList = filterPList.OrderBy(x=> x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderBy(x=>x.product.desc).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderBy(x => x.startdate).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderBy(x => x.enddate).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterPList = filterPList.OrderByDescending(x => x.id).ToList();
                    else if (sortColumnIndex == 1)
                        filterPList = filterPList.OrderByDescending(x => x.product.desc).ToList();
                    else if (sortColumnIndex == 2)
                        filterPList = filterPList.OrderByDescending(x => x.startdate).ToList();
                    else if (sortColumnIndex == 3)
                        filterPList = filterPList.OrderByDescending(x => x.enddate).ToList();
                }

                var paginatedQPList = filterPList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
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

        public ActionResult  getProductListWithPaging(jQueryDataTableParamModel param)
        {
            var ptype = Convert.ToString(Request["ptype"]);
            var filter = "%"+Convert.ToString(Request["filter"])+"%";
            var cat = Convert.ToString(Request["cat"]);
            var brand = Convert.ToString(Request["brand"]);           

            try
            {
                int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                ICollection<Product_m_Productdtl> PList = ProductList(ptype, filter,0, "", cat, brand,0, 0);
                //var paginatedQPList = new PaginatedList<Product_m_Productdtl>(PList.AsQueryable(), param.iDisplayStart, pagesize);
                var searchValue ="";
                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;
                var filterPList = PList.Where(x => x.desc.ToUpper().Contains(searchValue.ToUpper())).ToList();
                var paginatedQPList = filterPList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                //return Json(paginatedQPList, JsonRequestBehavior.AllowGet);
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
            //return View(db.products.ToList());
        }

        public JsonResult getProductBundle(string ptype = "Package", int pid = 0, string pcode = "")
        {
            try
            {
                ICollection<Product_m_ProductBundles> bundle = db.productBundle.Include("Product").Where(x => x.productid == pid).OrderBy(x=>x.lineno).ToList();
                return Json(bundle, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }
        //Added by ZawZO on 13 May 2015
        public JsonResult getProductBundleDetails(int pid = 0)
        {
            try
            {
                var itemlist = db.productBundle.Where(x => x.productid == pid).ToList();
                var plist = itemlist.Join(db.products, item => item.itemid, prod => prod.id, (item, prod) => new { itemlist = item, products = prod }).Select(x => new { lineno = x.itemlist.lineno,description = x.products.desc, uom = x.products.uom , qty= x.itemlist.qty }).OrderBy(x => x.lineno).ToList();
                return Json(plist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getProductBundleDetailsWithBalance(int pid = 0,int mid = 0)
        {
            try
            {
               
           
                using (var context = new BigMacEntities())
                {
                    var itemlist = db.productBundle.Where(x => x.productid == pid).ToList();
                    var plist = itemlist.Join(db.products, item => item.itemid, prod => prod.id, (item, prod) => new { itemlist = item, products = prod }).Select(x => new { lineno = x.itemlist.lineno, productid = x.products.id, description = x.products.desc, uom = x.products.uom, qty = x.itemlist.qty, productcode = x.products.productcode }).OrderBy(x => x.lineno).ToList();
                
                    ICollection<Redempt_Package> pptmp = context.Database.SqlQuery<Redempt_Package>("call getCussupPackageItemList(" + mid + ")").ToList();
                    var outstandingPackageList = (from product in pptmp join package in db.products on product.packagecode equals package.productcode select new { id = package.id, packagecode = product.packagecode, packagedesc = product.packagedesc, productdesc = product.productdesc, productid = product.productid, credit = product.credit, debit = product.debit, uom = product.uom, balance = product.balance }).Where(x=>x.balance > 0).ToList();

                    var productList = (from product in plist
                                     join package in outstandingPackageList on product.productid equals package.productid into prod
                                     from pack in prod.DefaultIfEmpty()
                                     select new { 
                                          lineno = product.lineno,
                                          productid = product.productid,
                                          productcode = product.productcode,
                                          description = product.description,
                                          uom = product.uom,
                                          qty = product.qty,
                                          balance = (pack != null) ? pack.balance : 0
                                         
                                      }).ToList();

                    var finalList = (from product in productList
                                     join price in db.productprices on new { product.productid, product.uom } equals new { price.productid, price.uom } into prod
                                     from productprice in prod.DefaultIfEmpty()
                                     select new
                                     {
                                         lineno = product.lineno,
                                         productid = product.productid,
                                         productcode = product.productcode,
                                         description = product.description,
                                         uom = product.uom,
                                         qty = product.qty,
                                         balance = product.balance,
                                         serviceallowance = productprice.serviceallowance ?? 0.00

                                     }).ToList();

                    return Json(finalList, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        //Added by ZawZO on 13 May 2015
        public JsonResult getProductCostPrice(int pid = 0)
        {
            try
            {
                var prlist = db.productprices.Where(x => x.productid == pid).ToList();
                return Json(prlist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        public JsonResult getProductAlternateCode(int pid = 0)
        {
            try
            {
                ICollection<Product_m_ProductAltCode> altcodes = db.productALTCode.Where(x => x.productid == pid).ToList();
                return Json(altcodes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult getProductCategory(string ptype = "")
        {
            try
            {
                ICollection<Product_z_Category> pl = db.productCategory.Where(x => x.Category == ptype || ptype == "").OrderBy(x => x.id).ToList();
                return Json(pl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult getProductCategoryForIndex()
        {
            try
            {
                ICollection<Product_z_Category> pl = db.productCategory.Where(x => x.Category != "PACKAGE" &&  x.Category != "SALESKIT").OrderBy(x => x.id).ToList();
                return Json(pl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }
        
        public ActionResult PriceIndex()
        {
            return View(db.products.Where(x=>x.status =="Active").ToList());
        }

        public ActionResult ProductSave(int id = 0, string ptype = "Product",string rcode ="Product")
        {
            try
            {
                int i = 0;
                ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                ViewBag.RangesNSeriesOptions = db.productRNS.AsEnumerable();
                ViewBag.StatusOptions = db.productStatus.AsEnumerable();
                ViewBag.UOMOptions = db.UOM.AsEnumerable();
                ViewBag.RCode = rcode;
                //if (ptype.ToUpper() == "PACKAGE")
                //{
                //    ICollection<Product_m_Product> pl = db.products.Where(x => (x.category == "Product" || x.category == "Service" || x.category == "TopUp") && x.status == "Active").OrderBy(x => x.desc).ToList();
                //    ViewBag.ProductOptions = pl;
                //}
                //ViewBag.PriceTypeOptions = db.pricetype.AsEnumerable();
                //ViewBag.ProductName = db.products.Where(x => x.id == id).FirstOrDefault().desc;
                //ViewBag.prc = prc;
                //Product_m_ProductPrice prcItem = new Product_m_ProductPrice();
                //prcItem.productid = id;                
                //prcItem.currency = ViewBag.Currency;
                //prcItem.exchangerate = Convert.ToDouble(ViewBag.ExchangeRate);

                //ICollection<Configuration_m_Branches> branches = db.Branches.ToList();
                //for (i = 0; i < branches.Count; i++)
                //{
                //    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                //}
                //ViewBag.BranchOptions = branches;
                //ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                //for (i = 0; i < companies.Count; i++)
                //{
                //    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                //}
                //ViewBag.CompanyOptions = companies;

                if (id == 0)
                {
                    Product_m_Product p = new Product_m_Product();
                    p.prices = db.productprices.Where(x => x.productid == 0).ToList();
                    //p.priceItem = prcItem;
                    ViewBag.ProductImage = "";
                    p.category = ptype;
                    //p.qty = 1;
                    return View(p);
                }
                else
                {
                    Product_m_Product p = db.products.Find(id);
                    //p.RangesNSeries
                    p.prices = db.productprices.Where(x => x.productid == id).ToList();
                    var pimg = db.productImages.Where(x => x.productid == id && x.defaultflag == 1).ToList().DefaultIfEmpty(new Product_m_ProductImage { image = "" }).FirstOrDefault().image;
                    ViewBag.ProductImage = pimg;
                    //p.priceItem = prcItem;
                    return View(p);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        public static string getGeneratedNewID(string cat = "Product", string subcat = "SPA", string brand = "InHouse",string rnc="")
        {
            string returnvalue = "";
            using (var context = new BigMacEntities())
            {
                MySqlConnection conn = new MySqlConnection(context.Database.Connection.ConnectionString.ToString());
                try
                {
                    MySqlCommand cmd = new MySqlCommand("getGenerateProductID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("cat", cat));
                    cmd.Parameters.Add(new MySqlParameter("subcat", subcat));
                    cmd.Parameters.Add(new MySqlParameter("brand", brand));
                    cmd.Parameters.Add(new MySqlParameter("rnc", rnc));
                    var Result = new MySqlParameter();
                    Result.ParameterName = "pcode";
                    Result.MySqlDbType = MySqlDbType.VarChar;
                    Result.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(Result);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();                                       
                    returnvalue = Result.Value.ToString();
                    //cmd.Connection.Close();
                    //var Result = new Object();
                    //var val = context.Database.ExecuteSqlCommand("call getGenerateNewID('" + tblname + "','" + colname + "','" + prekeyvalue + "','" + startnokeyvalue + "'," + Result + ")");

                }
                catch (Exception e)
                { }
                finally
                {

                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

            return returnvalue;
        }

        [HttpPost]
        public JsonResult UploadProductImage()
        {
            var returnStr = "FAIL";
            string fileName = "";
            try
            {
                string cat = Request.QueryString["cat"].ToString();
                string hr = Request.QueryString["hr"].ToString();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file                   
                    fileName = file.FileName;
                    UploadImage(file, cat);
                    returnStr = fileName;
                    //int fileSize = file.ContentLength;
                    //string mimeType = file.ContentType;
                    //System.IO.Stream fileContent = file.InputStream;
                    ////To save file, use SaveAs method
                    //file.SaveAs(Server.MapPath("~/assets/img/Members/") + fileName); //File will be saved in application root
                }                
                //return Json("Uploaded " + Request.Files.Count + " files");
            }
            catch (Exception e)
            { }
            finally
            {

            }
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public Boolean UploadImage(HttpPostedFileBase file,string cat="Product")
        {
            Boolean returnvalue = false;
            try {
                string fileName = "";
                int fileSize = file.ContentLength;
                fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                //To save file, use SaveAs method
                file.SaveAs(Server.MapPath("~/assets/img/" + cat  + "/") + fileName); //File will be saved in application root
                returnvalue= true;
            }
            catch(Exception e)
            {  }
            return returnvalue;
            
        }

        [HttpPost]
        public JsonResult ProductBundleSave(Product_m_ProductBundles pb,string rcode="PRODUCT", string pcode="" )
        {
            var returnStr = "FAIL";
            
            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                string from = ""; string to = "";

                if (pb.id == 0)
                {
                    //pb.productid = p.id;
                    pb.createdate = DateTime.Now;
                    pb.lastmodifieddate = DateTime.Now;
                    db.productBundle.Add(pb);
                    db.SaveChanges();
                    saveToLog(rid, pb.id , "CREATE", "Add New Bundle item to " + pcode, pb.itemid.ToString());
                }
                else
                {
                    Product_m_ProductBundles bdltmp = db.productBundle.Find(pb.id);
                    from = "Line No-" + bdltmp.lineno.ToString()  + "Product id -" + bdltmp.itemid.ToString() +  ", Qty -" + bdltmp.qty.ToString();
                    to = "Line No-" + pb.lineno.ToString() + "Product id -" + pb.itemid.ToString() + ", Qty -" + pb.qty.ToString();
                    bdltmp.productid = pb.productid;
                    bdltmp.lineno = pb.lineno;
                    bdltmp.itemid = pb.itemid;
                    bdltmp.qty = pb.qty;
                    //bdltmp.lastmodifieddate
                    db.SaveChanges();
                    saveToLog(rid, pb.id, "Update", "Update Product Bundle - ProductID - " + pb.productid + ", Product Code -" + pcode + " , ID - " + pb.id.ToString(),from, to);
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductBundleRemove(int pbid, string rcode = "PRODUCT", string pcode = "")
        {
            var returnStr = "FAIL";

            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                string from = "";

                Product_m_ProductBundles bdltmp = db.productBundle.Find(pbid);
                from = "Line No-" + bdltmp.lineno.ToString() + "Product id -" + bdltmp.itemid.ToString() + ", Qty -" + bdltmp.qty.ToString();
                db.productBundle.Remove(bdltmp);
                db.SaveChanges();
                saveToLog(rid, bdltmp.productid, "DELETE", "Remove Bundle - Prodct Code -" + pcode + ",Product ID -" + bdltmp.productid + ", Bundle ID -" + pbid.ToString(), from);
                returnStr = "SUCCESS";                
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductALTCodeSave(Product_m_ProductAltCode palt, string rcode ="PRODUCT",string pcode="" )
        {
            var returnStr = "FAIL";

            try
            {
                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
                string from =""; string to ="";
                if (palt.id == 0)
                {
                    //pb.productid = p.id;
                    palt.createdate = DateTime.Now;
                    palt.lastmodifieddate = DateTime.Now;
                    db.productALTCode.Add(palt);
                    db.SaveChanges();
                    saveToLog(rid, palt.id, "CREATE", "Add new ALTCode - Product ID -" + palt.productid + ", Product Code -"+ pcode, palt.altcode);
                }
                else
                {
                    Product_m_ProductAltCode palttmp = db.productALTCode.Find(palt.id);
                    from = palttmp.altcode ;
                    to = palt.altcode ;
                    palttmp.altcode = palt.altcode;                    
                    db.SaveChanges();
                    saveToLog(rid, palt.id, "UPDATE", "Update ALTCode - Product ID -" + palt.productid + ", Product Code - " + pcode + ", Alt ID -" + palt.id.ToString(), from, to);
                }
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductALTCodeRemove(int paltid, string rcode ="PRODUCT",string pcode="" )
        {
            var returnStr = "FAIL";
            try
            {
                var ridtmp = db.Resources.Where(x=>x.resource == rcode).FirstOrDefault().id;
                int rid =0;
                if (ridtmp != null)  rid = Convert.ToInt32(ridtmp);
                string from ="";

                Product_m_ProductAltCode palttmp = db.productALTCode.Find(paltid);
                from = palttmp.altcode ;
                db.productALTCode.Remove(palttmp);
                db.SaveChanges();
                saveToLog(rid, palttmp.productid, "DELETE", "Delete - ProductCode - " + pcode + ",ALT code - " + palttmp.altcode + ", ID - " + paltid.ToString(), from); 
                returnStr = "SUCCESS";
                
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
            
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProductSaveNewBundle(Product_m_Product p,Product_m_ProductBundles pb, string priceItemids = "", string bundleItemids = "", string altcodes = "", string rcode = "Product")
        {

            var returnStr = "0";

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
              

                var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null) rid = Convert.ToInt32(ridtmp);

                if (p.id == 0)
                {
                    p.createdate = DateTime.Now;
                    p.lastmodifieddate = DateTime.Now;
                    p.cocode = Convert.ToString(Session["cocode"]);
                    p.productcode = getGeneratedNewID(p.category, p.categorysub, p.brand, p.RangesNSeries);
                    db.products.Add(p);
                    db.SaveChanges();
                    using (var context = new BigMacEntities())
                    {
                        var blogNames = context.Database.ExecuteSqlCommand("Update Product_z_CategorySub set lastnumber = IFNULL(lastnumber,0) + 1 where Category ='" + p.categorysub + "'");
                    }
                
                    //if (p.prices != null)
                    //{
                    //    for (int i = 0; i < (p.prices.Count()); i++)
                    //    {
                            Product_m_ProductPrice pr = new Product_m_ProductPrice();
                            pr.currency = curr;
                            pr.exchangerate = exrate;
                            pr.createdate = DateTime.Now;
                            pr.lastmodifieddate = DateTime.Now;
                            pr.productid = p.id;
                            pr.pricetype = "Selling Price";
                            pr.stockreceivedref = 0;
                            pr.sellprice = 0.00;
                            pr.uom = p.uom;
                            pr.redeembonus = 0.00;
                            pr.redeemdollars = 0.00;
                            pr.awardbonus = 0.00;
                            pr.awarddollars = 0.00;
                            pr.serviceallowance = 0.00;
                            pr.servicecommission = 0.00;
                            db.productprices.Add(pr);
                            db.SaveChanges();
                           saveToLog(rid, p.id, "CREATE", "Create New Price - Product Code -" + p.productcode);
                        //}
                    
                       saveToLog(rid, p.id, "CREATE", "Create New Product - Code -" + p.productcode);
                   // }

                   

                    if (p.Images != null)
                    {
                        for (int i = 0; i < (p.Images.Count()); i++)
                        {
                            p.Images.ElementAt(i).productid = p.id;
                            p.Images.ElementAt(i).createdate = DateTime.Now;
                            p.Images.ElementAt(i).lastmodifieddate = DateTime.Now;
                            db.productImages.Add(p.Images.ElementAt(i));
                        }
                        db.SaveChanges();
                    }


                }

                pb.createdate = DateTime.Now;
                pb.lastmodifieddate = DateTime.Now;
                pb.itemid = p.id;
                db.productBundle.Add(pb);
                db.SaveChanges();

                saveToLog(rid, pb.id, "CREATE", "Add New Bundle item to " + p.productcode, pb.itemid.ToString());


                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProductSave(Product_m_Product p, string priceItemids="",string bundleItemids="",string altcodes ="",string rcode ="Product")
        {

            //ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
            //ViewBag.StatusOptions = db.productStatus.AsEnumerable();
            //ViewBag.UOMOptions = db.UOM.AsEnumerable();
            //ICollection<Configuration_m_Company> companies = db.Companies.ToList();
            //for (int i = 0; i < companies.Count; i++)
            //{
            //    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
            //}
            //ViewBag.CompanyOptions = companies;
            //return View(p);
            //var returnStr= "FAIL";
            var returnStr = "0";
            
            try
            {
                string curr = "";
                double exrate = 0;
                ICollection<Configuration_m_Default> config = db.ConfigDefault.Where(x => x.key == "CURR").ToList();

                if (config.Count > 0)
                    curr = config.ElementAt(0).value;
                //ViewBag.Currency = config.ElementAt(0).value;
                else
                    curr = "";
                    //ViewBag.Currency = "";

                config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
                if (config.Count > 0)
                    exrate = Convert.ToDouble(config.ElementAt(0).value);
                    //ViewBag.ExchangeRate = config.ElementAt(0).value;
                else
                    exrate = 0;
                    //ViewBag.ExchangeRate = "";

                var ridtmp = db.Resources.Where(x=>x.resource == rcode).FirstOrDefault().id;
                int rid =0;
                if (ridtmp != null)  rid = Convert.ToInt32(ridtmp);

                if (p.id == 0)
                {
                    p.createdate = DateTime.Now;
                    p.lastmodifieddate = DateTime.Now;
                    p.cocode = Convert.ToString(Session["cocode"]);
                    p.productcode = getGeneratedNewID(p.category, p.categorysub, p.brand,p.RangesNSeries); 
                    db.products.Add(p);
                    db.SaveChanges();
                    using (var context = new BigMacEntities())
                    {
                        var blogNames = context.Database.ExecuteSqlCommand("Update Product_z_CategorySub set lastnumber = IFNULL(lastnumber,0) + 1 where Category ='" + p.categorysub + "'");
                    }                    
                    //int pcount = p.prices.Count();
                    if (p.prices != null)
                    {
                        for (int i = 0; i < (p.prices.Count()); i++)
                        {
                            p.prices.ElementAt(i).currency = curr;
                            p.prices.ElementAt(i).exchangerate = exrate;
                            p.prices.ElementAt(i).createdate = DateTime.Now;
                            p.prices.ElementAt(i).lastmodifieddate = DateTime.Now;
                            p.prices.ElementAt(i).productid = p.id;
                            p.prices.ElementAt(i).pricetype = "Selling Price";
                            p.prices.ElementAt(i).stockreceivedref = 0;
                            db.productprices.Add(p.prices.ElementAt(i));
                            saveToLog(rid, p.prices.ElementAt(i).id, "CREATE", "Create New Price - Product Code -" + p.productcode);
                        }
                        db.SaveChanges();
                        saveToLog(rid, p.id, "CREATE", "Create New Product - Code -" + p.productcode );
                    }

                    //if (p.bundleItems != null)
                    //{                        
                    //    for (int i = 0; i < (p.bundleItems.Count()); i++)
                    //    {
                    //        p.bundleItems.ElementAt(i).productid = p.id;
                    //        p.bundleItems.ElementAt(i).createdate = DateTime.Now;
                    //        p.bundleItems.ElementAt(i).lastmodifieddate = DateTime.Now;
                    //        db.productBundle.Add(p.bundleItems.ElementAt(i));
                    //    }
                    //    db.SaveChanges();
                    //}


                    //if (altcodes.Length > 0)
                    //{

                    //    altcodes = altcodes.Substring(0, altcodes.Length - 1);
                    //}

                    //using (var context = new BigMacEntities())
                    //{
                    //    var result = context.Database.ExecuteSqlCommand("call saveProductAltCode (" + p.id + ",'" + altcodes  + "')");
                    //}

                    if (p.Images != null)
                    {
                        for (int i = 0; i < (p.Images.Count()); i++)
                        {
                            p.Images.ElementAt(i).productid = p.id;
                            p.Images.ElementAt(i).createdate = DateTime.Now;
                            p.Images.ElementAt(i).lastmodifieddate = DateTime.Now;
                            db.productImages.Add(p.Images.ElementAt(i));
                        }
                        db.SaveChanges();
                    }


                }
                else
                {
                    Product_m_Product ptmp = db.products.Find(p.id);
                    String from = ""; String to = "";

                    if (ptmp != null)
                    {
                        from = "Type -" + ptmp.category + ", Category -" + ptmp.categorysub + ", Brand - " + ptmp.brand + ", R&S -" + ptmp.RangesNSeries + ", Desc - " + ptmp.desc + ", Status -" + ptmp.status + ", uom -" + ptmp.uom + ", uom2 -" + ptmp.uom2 + ", uom3 - " + ptmp.uom3 + ", uomF2 - " + ptmp.uomfactor2.ToString() + ", uomF3 - " + ptmp.uomfactor3.ToString();
                        to = "Type -" + p.category + ", Category -" + p.categorysub + ", Brand - " + p.brand + ", R&S -" + p.RangesNSeries + ", Desc - " + p.desc + ", Status -" + p.status + ", uom -" + p.uom + ", uom2 -" + p.uom2 + ", uom3 - " + p.uom3 + ", uomF2 - " + p.uomfactor2.ToString() + ", uomF3 - " + p.uomfactor3.ToString();
                        ptmp.categorysub = p.categorysub;
                        ptmp.brand = p.brand;
                        ptmp.desc = p.desc;
                        ptmp.productcode = p.productcode;
                        ptmp.remark = p.remark;
                        ptmp.status = p.status;
                        ptmp.uom = p.uom;
                        ptmp.uom2 = p.uom2;
                        ptmp.uom3 = p.uom3;
                        ptmp.uomfactor2 = p.uomfactor2;
                        ptmp.uomfactor3 = p.uomfactor3;
                        ptmp.RangesNSeries = p.RangesNSeries;
                        //ptmp.qty = p.qty;
                        db.SaveChanges();
                        saveToLog(rid, p.id, "UPDATE", "Update Product - Code -" + ptmp.productcode, from, to);
                        priceItemids = priceItemids + "0";
                        using (var context = new BigMacEntities())
                        {
                            var result = context.Database.ExecuteSqlCommand("Delete from Product_m_ProductPrice where productid ='" + p.id + "' and id not in (" + priceItemids + ")");
                        }

                        if (p.prices != null)
                        {
                            foreach(Product_m_ProductPrice prc in p.prices) 
                            {
                                if (prc.id == 0)
                                {
                                    prc.currency = curr;
                                    prc.exchangerate = exrate;
                                    prc.createdate = DateTime.Now;
                                    prc.lastmodifieddate = DateTime.Now;
                                    prc.productid = p.id;
                                    prc.pricetype = "Selling Price";
                                    prc.stockreceivedref =0;
                                    db.productprices.Add(prc);
                                    db.SaveChanges();
                                    saveToLog(rid, prc.id, "CREATE", "Adding Price - Price ID -" + prc.id.ToString());
                                }
                                else
                                {
                                    Product_m_ProductPrice prctmp = db.productprices.Find(prc.id);
                                    //string from =""; string to ="";
                                    if (prctmp != null)
                                    {
                                        from = "UOM-" + prctmp.uom + ",Price - " + prctmp.sellprice + " ,Sales Commission" + prctmp.servicecommission + ",Service Commission" + prc.serviceallowance + " RD - " + prctmp.redeemdollars.ToString() + ", RB-" + prctmp.redeembonus + ", AD -" + prctmp.awarddollars + ", AB - " + prctmp.awardbonus;
                                        to = "UOM-" + prc.uom + ",Price - " + prc.sellprice + " ,Sales Commission" + prc.servicecommission + ",Service Commission" + prc.serviceallowance + " RD - " + prc.redeemdollars.ToString() + ", RB-" + prc.redeembonus + ", AD -" + prc.awarddollars + ", AB - " + prc.awardbonus;
                                        prctmp.uom = prc.uom;
                                        prctmp.redeemdollars = prc.redeemdollars;
                                        prctmp.redeembonus = prc.redeembonus;
                                        prctmp.awarddollars = prc.awarddollars;
                                        prctmp.awardbonus = prc.awardbonus;
                                        prctmp.servicecommission = prc.servicecommission;
                                        prctmp.serviceallowance = prc.serviceallowance;
                                        prctmp.sellprice = prc.sellprice;
                                        prctmp.pricetype = "Selling Price";
                                        //itemids += prc.id + ",";
                                        db.SaveChanges();
                                        saveToLog(rid, prc.id, "UPDATE", "Update- Product Code - " + ptmp.productcode + ", Price ID -" + prc.id.ToString(),from,to);
                                    }
                                }

                            }

                        }

                        //if (altcodes.Length > 0)
                        //{
                        //    altcodes = altcodes.Substring(0, altcodes.Length - 1);
                        //}

                        //using (var context = new BigMacEntities())
                        //{
                        //    var result = context.Database.ExecuteSqlCommand("call saveProductAltCode (" + p.id + ",'" + altcodes + "')");
                        //}

                        if (p.Images != null)
                        {
                            try
                            {
                                string images="";
                                string dimage = "";

                                for (int i = 0; i < p.Images.Count(); i++)
                                {
                                    if (p.Images.ElementAt(i).defaultflag == 1)
                                        dimage = p.Images.ElementAt(i).image;
                                    else
                                        images += p.Images.ElementAt(i).image + ",";
                                }

                                if (images.Length > 0)
                                {
                                    images = images.Substring(0, images.Length - 1);
                                }

                                using (var context = new BigMacEntities())
                                {
                                    var blogNames = context.Database.ExecuteSqlCommand("call saveProductImages (" + p.id + ",'" + images + "','" + dimage + "')");
                                }

                            }catch(Exception e)
                            {}
                        }
                        //db.SaveChanges(); 
                        //if (p.category.ToUpper() == "PACKAGE" || p.category.ToUpper() == "SALESKIT")
                        //{   
                        //    bundleItemids = bundleItemids + "0";
                        //    using (var context = new BigMacEntities())
                        //    {
                        //        var result = context.Database.ExecuteSqlCommand("Delete from Product_m_ProductBundles where productid ='" + p.id + "' and id not in (" + bundleItemids + ")");
                        //    }

                        //    if (p.bundleItems != null)
                        //    {
                        //        for (int i = 0; i < p.bundleItems.Count(); i++)
                        //        {
                        //            if (p.bundleItems.ElementAt(i).id == 0)
                        //            {
                        //                p.bundleItems.ElementAt(i).productid = p.id;
                        //                p.bundleItems.ElementAt(i).createdate = DateTime.Now;
                        //                p.bundleItems.ElementAt(i).lastmodifieddate = DateTime.Now;
                        //                db.productBundle.Add(p.bundleItems.ElementAt(i));
                        //                db.SaveChanges();
                        //            }
                        //            else
                        //            {
                        //                Product_m_ProductBundles bdltmp = db.productBundle.Find(p.bundleItems.ElementAt(i).id);
                        //                bdltmp.productid = p.id;
                        //                bdltmp.lineno = p.bundleItems.ElementAt(i).lineno;
                        //                bdltmp.itemid = p.bundleItems.ElementAt(i).itemid;
                        //                bdltmp.qty = p.bundleItems.ElementAt(i).qty;
                        //                //bdltmp.lastmodifieddate
                        //                db.SaveChanges();
                        //            }

                        //        }
                        //    }
                        //}
                    }
                }
                //returnStr = "SUCCESS";
                returnStr = p.id.ToString();
                //Response.Redirect("ProductSave?id=" + p.id);
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PromotionIndex()
        {
            return View(db.productpromotions.OrderBy(x => new {x.product.desc}).ToList());
        }

        public ActionResult PromotionListing(string rcode = "Promotion")
        {
            try
            {
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                ViewBag.RCode = rcode;
            }
            catch (Exception e)
            { }
            return View();
            //return View(db.productpromotions.OrderByDescending(x => new { x.startdate}).OrderBy(x=> x.product.desc).ToList());
        }



        public ActionResult PromotionSave(int id = 0,string rcode="PROMOTION")
        {
            try
            {
                ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                ViewBag.RangesNSeriesOptions = db.productRNS.AsEnumerable();
                ViewBag.StatusOptions = db.productStatus.AsEnumerable();
                ViewBag.UOMOptions = db.UOM.AsEnumerable();
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                ViewBag.RCode = rcode;
                //ViewBag.ProductOptions = db.products.Where(x=>x.status == "Active").OrderBy(x=>x.desc).AsEnumerable();
                if (id == 0)
                {
                    Product_m_ProductPromotion p = new Product_m_ProductPromotion();
                    ViewBag.ptype = "";
                    ViewBag.pcat = "";
                    ViewBag.pbrand ="";
                    ViewBag.prns = "";
                    return View(p);
                }
                else
                {
                    Product_m_ProductPromotion p = db.productpromotions.Find(id);
                    Product_m_Product ptmp = db.products.Find(p.productid);
                    ViewBag.ptype = ptmp.category;
                    ViewBag.pcat = ptmp.categorysub;
                    ViewBag.pbrand = ptmp.brand;
                    ViewBag.prns = ptmp.RangesNSeries;
                    return View(p);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult PromotionSave(Product_m_ProductPromotion p,string rcode ="Promotion")
        {
            try { 
            //ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).AsEnumerable();
            //rcode = Request["rcode"].ToString();

            var ridtmp = db.Resources.Where(x => x.resource == rcode).FirstOrDefault().id;
            int rid = 0;
            if (ridtmp != null) rid = Convert.ToInt32(ridtmp);
            string from = ""; string to = "";

            ViewBag.BrandOptions = db.productBrand.AsEnumerable();
            ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
            ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
            ViewBag.RangesNSeriesOptions = db.productRNS.AsEnumerable();
            ViewBag.StatusOptions = db.productStatus.AsEnumerable();
            ViewBag.UOMOptions = db.UOM.AsEnumerable();
            var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
            var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
            ViewBag.CitiDesc = c;
            ViewBag.BonusDesc = b;
            if (p.id == 0)
            {
                p.createdate = DateTime.Now;
                p.lastmodifieddate = DateTime.Now;
                p.cocode = Session["cocode"].ToString();
                db.productpromotions.Add(p);
                db.SaveChanges();
                
                saveToLog(rid, p.id, "CREATE", "Add new promotion"," ProudctID - " + p.productid.ToString() + ", Product -"  + p.product);
            }
            else
            {
                Product_m_ProductPromotion ptmp = db.productpromotions.Find(p.id);
                from = "product id-" + ptmp.productid.ToString() + "(" + ptmp.product + ") , from date -" + string.Format("{0:dd/MM/yyyy}", ptmp.startdate.ToString()) + ",ToDate -" + string.Format("{0:dd/MM/yyyy}", ptmp.enddate.ToString()) + ", Price -" + ptmp.price + ", commission -" + ptmp.servicecommission + ", RD-" + ptmp.redeemdollars + ", RB-" + ptmp.redeembonus + ", AD -" + ptmp.awarddollars + ", AB -" + ptmp.awardbonus;
                to = "product id-" + p.productid.ToString() + "(" + p.product + ") , from date -" + string.Format("{0:dd/MM/yyyy}", p.startdate.ToString()) + ",ToDate -" + string.Format("{0:dd/MM/yyyy}", p.enddate.ToString()) + ", Price -" + p.price + ", commission -" + p.servicecommission + ", RD-" + p.redeemdollars + ", RB-" + p.redeembonus + ", AD -" + p.awarddollars + ", AB -" + p.awardbonus;
                ptmp.productid = p.productid;
                ptmp.uom = p.uom;
                ptmp.startdate = p.startdate;
                ptmp.enddate = p.enddate;
                ptmp.price = p.price;
                ptmp.servicecommission = p.servicecommission;
                ptmp.redeemdollars = p.redeemdollars;
                ptmp.redeembonus = p.redeembonus;
                ptmp.awarddollars = p.awarddollars;
                ptmp.awardbonus = p.awardbonus;
                ptmp.cocode = Session["cocode"].ToString();
                db.SaveChanges();
                saveToLog(rid, p.id, "UPDATE", "Update promotion id =" + p.id.ToString(), from, to);
            }

            //Product_m_Product prd = db.products.Find(p.productid);
            //ViewBag.ptype = prd.category;
            //ViewBag.pcat = prd.categorysub;
            //ViewBag.pbrand = prd.brand;
            //ViewBag.prns = prd.RangesNSeries;
            }
            catch (Exception e) { }
            return RedirectToAction("PromotionSave", "Product", new { id = p.id,rcode = rcode });
            //return View(p);
        }

        public ActionResult ProductCitiBellaList(string filter,string rcode="")
        {
            try
            {
                ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                ViewBag.RangesNSeriesOptions = db.productRNS.AsEnumerable();
                ViewBag.UOMOptions = db.UOM.AsEnumerable();
                ViewBag.Rcode = rcode;
                ViewBag.Filter = filter;
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }

        public ActionResult PostedProductCitiBellaList(string filter)
        {
            try
            {
                //ViewBag.BrandOptions = db.productBrand.AsEnumerable();
                //ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                //ViewBag.SubCategoryOptions = db.productSubCategory.AsEnumerable();
                //ViewBag.RangesNSeriesOptions = db.productRNS.AsEnumerable();
                //ViewBag.UOMOptions = db.UOM.AsEnumerable();
                ViewBag.Filter = filter;
                var c = db.ConfigDefault.Where(x => x.key == "C$").FirstOrDefault().value;
                var b = db.ConfigDefault.Where(x => x.key == "B$").FirstOrDefault().value;
                ViewBag.CitiDesc = c;
                ViewBag.BonusDesc = b;
                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }

        public JsonResult getPostedProductCitiBella(string filter = "%", int pageno = 1)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                    ICollection<Product_m_Citibella> pList = context.Database.SqlQuery<Product_m_Citibella>("call getPostedProductCitibellaListing('" + filter + "')").ToList();
                    var paginatedQPList = new PaginatedList<Product_m_Citibella>(pList.AsQueryable(), pageno, pagesize);
                    var paginatedPList = paginatedQPList.ToList();

                    if (paginatedPList != null)
                    {
                        if (paginatedPList.Count > 0)
                            paginatedPList.ElementAt(0).TotalPages = paginatedQPList.TotalPages;
                    }
                    return Json(paginatedPList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public JsonResult getProductCitiBella(string filter = "%",int pageno =1)
        {
            try
            {
                using (var context = new BigMacEntities())
                {
                    int pagesize = Convert.ToInt32(WebConfigurationManager.AppSettings["pagesize"]);
                    ICollection<Product_m_Citibella> pList = context.Database.SqlQuery<Product_m_Citibella>("call getProductCitibellaListing('" + filter + "')").ToList();
                    var paginatedQPList = new PaginatedList<Product_m_Citibella>(pList.AsQueryable(), pageno, pagesize);
                    var paginatedPList = paginatedQPList.ToList();

                    if (paginatedPList != null)
                    {
                        if (paginatedPList.Count > 0)
                            paginatedPList.ElementAt(0).TotalPages = paginatedQPList.TotalPages;
                    }
                    return Json(paginatedPList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
            //return View(db.products.ToList());
        }

        public ActionResult ProductCitiBellaSave(ICollection<Product_m_Citibella> Plist, string filter="%",string rcode="")
        {
            string returnStr = "";
            //string postItemNos = "";

            try
            {
                string curr = "";
                double exrate = 0;
                Int32 uid = Convert.ToInt32(Session["userid"].ToString());
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
    
                var ridtmp = db.Resources.Where(x=>x.resource == rcode).FirstOrDefault().id;
                int rid = 0;
                if (ridtmp != null)  rid = Convert.ToInt32(ridtmp);

                foreach (Product_m_Citibella citip in Plist)
                {                 
                    
                    Product_m_Product p = new Product_m_Product();
                    
                    p.category = citip.Class;
                    p.categorysub = citip.Div;
                    p.brand = citip.Brand;                    
                    p.RangesNSeries = citip.Dept;
                    p.desc = citip.Item_Desc;
                    p.uom = citip.uom;
                    p.uomfactor2 = 1;
                    p.createdate = DateTime.Now;
                    p.lastmodifieddate = DateTime.Now;
                    p.cocode = Convert.ToString(Session["cocode"]);
                    p.productcode = getGeneratedNewID(p.category, p.categorysub, p.brand,p.RangesNSeries); 
                    p.status = "Active";
                    db.products.Add(p);
                    db.SaveChanges();

                    using (var context = new BigMacEntities())
                    {
                        var blogNames = context.Database.ExecuteSqlCommand("Update Product_z_CategorySub set lastnumber = lastnumber + 1 where Category ='" + p.categorysub + "'");
                    }                    

     
                    Product_m_Citibella cp = db.productCiti.Where(x=> x.Item_no == citip.Item_no).FirstOrDefault();
                    cp.post = 1;
                    cp.postby = uid;
                    cp.postdatetime = DateTime.Now;
                    cp.newItemid = p.id;
                    cp.newitemcode = p.productcode;
                    cp.type = p.category;
                    cp.category = p.categorysub;
                    cp.brandnew = p.brand;
                    cp.ranges = p.RangesNSeries;
                    cp.uom = p.uom;
                    cp.redeemciti = citip.redeemciti;
                    cp.redeembonus =citip.redeembonus;
                    cp.awardciti = citip.awardciti;
                    cp.awardbonus = citip.awardbonus;
                    cp.sellingprice = citip.sellingprice;
                    cp.servicecomm = citip.servicecomm;                
                    db.SaveChanges();

                    Product_m_ProductAltCode pa = new Product_m_ProductAltCode();
                    pa.productid = p.id;
                    pa.altcode = citip.Rpt_Code;
                    pa.createdate = DateTime.Now;
                    pa.lastmodifieddate = DateTime.Now;
                    db.productALTCode.Add(pa);
                    db.SaveChanges();

                    Product_m_ProductPrice pp = new Product_m_ProductPrice();
                    pp.productid = p.id;
                    pp.pricetype = "Selling Price";
                    pp.currency = curr;
                    pp.exchangerate = exrate;
                    pp.uom = citip.uom;
                    pp.sellprice = citip.sellingprice;
                    pp.servicecommission = citip.servicecomm;
                    pp.redeemdollars = citip.redeemciti;
                    pp.redeembonus = citip.redeembonus;
                    pp.awarddollars = citip.awardciti;
                    pp.awardbonus = citip.awardbonus;
                    pp.stockreceivedref = 0;
                    pp.createdate = DateTime.Now;
                    pp.lastmodifieddate = DateTime.Now;
                    db.productprices.Add(pp);
                    db.SaveChanges();

                    saveToLog(rid, p.id, "Transfer", "TransferProduct Citi ID -" + citip.Item_no + ", Product ID -" + p.id.ToString() + "Code -" + p.productcode ,citip.Item_no,p.productcode);
                }
  

                returnStr = "SUCCESS";
            }
            catch (Exception e)
            {
                 returnStr=e.Message;
            }
            return Json(returnStr, JsonRequestBehavior.AllowGet); 
        }

        public Boolean saveToLog(int resourceid, int resourcecode, string logtype, string description, string from = "", string to = "", string ip = "")
        {
            GeneralController gc = new GeneralController();
            int uid= Convert.ToInt32(Session["userid"].ToString());
            string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            return gc.SaveToLog(uid, Session["cocode"].ToString(), Session.SessionID, resourceid, resourcecode, logtype, description, from, to, visitorIPAddress);
        }
        //public ActionResult ProductPrice(int productid = 0, int prc = 0)
        //{
        //    try
        //    {                
        //        ViewBag.PriceTypeOptions = db.pricetype.AsEnumerable();
        //        ViewBag.ProductName = db.products.Where(x => x.id == productid).FirstOrDefault().desc;
        //        ViewBag.prc = prc;
        //        //ViewBag.ProductPrices = db.productprices.Where(x => x.productid == productid).OrderBy(x => x.pricetype).AsEnumerable();
        //        ICollection<Configuration_m_Default> config= db.ConfigDefault.Where(x=>x.key == "CURR").ToList();
        //        if (config.Count > 0 )
        //            ViewBag.Currency = config.ElementAt(0).value;
        //        else
        //            ViewBag.Currency = "";

        //        config = db.ConfigDefault.Where(x => x.key == "EXCH").ToList();
        //        if (config.Count > 0)
        //            ViewBag.ExchangeRate = config.ElementAt(0).value;
        //        else
        //            ViewBag.ExchangeRate = "";

        //        Product_m_ProductPrice p = new Product_m_ProductPrice();
        //        p.productid = productid;
        //        p.currency = ViewBag.Currency;
        //        p.exchangerate = Convert.ToDouble(ViewBag.ExchangeRate);
        //        return View(p);
        //    }
        //    catch (Exception e)
        //    {
        //        return View();
        //    }
            
        //}

        //[HttpPost]
        //public JsonResult ProductPriceList(int productid = 0)
        //{
        //    //ICollection<Campaign_m_Attendance_Staff> staffs = db.CampaignAttendanceStaffs.Where(x => x.campaignid == campaignid).Include("Group").Include("Branch").Include("Staff").OrderBy(x => new { x.Group.group,x.branchcode,x.Staff.name}).ToList();
        //    try
        //    {
        //        ICollection<Product_m_ProductPrice> productPrice = db.productprices.Where(x => x.productid == productid).OrderBy(x => x.pricetype).ToList();

        //        return Json(productPrice, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //public ActionResult ProductPrice(Product_m_ProductPrice p)
        //{
        //    //ViewBag.PriceTypeOptions = db.pricetype.AsEnumerable();
        //    //ViewBag.ProductPrices = db.productprices.Where(x => x.productid == id).OrderBy(x => x.pricetype).AsEnumerable();
        //    return View(p);
        //}
    }
}
