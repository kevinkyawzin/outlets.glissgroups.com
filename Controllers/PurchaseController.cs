using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;

namespace BigMac.Controllers
{
    public class PurchaseController : Controller
    {
        //
        // GET: /Purchase/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult Index()
        {
            return View(db.purchase.Include("Create").OrderBy(x => new { x.status, x.id }).ToList());
        }

        public ActionResult PurchaseOrder(int id = 0)
        {
            try
            {
                int i = 0;
                ViewBag.CusSupOptions = db.CusSup.Where(x=>x.status == "Active").ToList();
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
                    Purchase_m_Purchase p = new Purchase_m_Purchase();
                    p.items = db.purchaseItems.Where(x => x.purchaseid == 0).OrderBy(x => x.lineno).AsEnumerable();
                    p.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;                    
                    var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;                
                    if ( ex.Length > 0 )
                        p.exchangerate = Convert.ToDouble(ex);
                    else
                        p.exchangerate = 0;                  
                        return View(p);
                }
                else
                {
                    Purchase_m_Purchase p = db.purchase.Find(id);
                    p.items = db.purchaseItems.Where(x => x.purchaseid == id).OrderBy(x => x.lineno).AsEnumerable();   
                    return View(p);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult PurchaseOrder(Purchase_m_Purchase p)
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

            return View(p);
        }

        //public ActionResult PurchaseOrderDetail(int purchaseid = 0)
        //{
        //    try
        //    {                
        //        ViewBag.ProductOptions = db.products.Where(x=>x.status == "Active").OrderBy(x=>x.desc).ToList();
        //        ViewBag.UOMOptions = db.UOM.ToList();
        //        ViewBag.POItemOptions = db.purchaseItems.Where(x => x.purchaseid == purchaseid).OrderBy(x => x.lineno).ToList();
        //        Purchase_m_Purchase_Items p = new Purchase_m_Purchase_Items();
        //        p.purchaseid = purchaseid;
        //        p.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;                
        //        p.gstcode = db.ConfigDefault.Where(x => x.key == "PGST").FirstOrDefault().value;
        //        var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;
                
        //        if ( ex.Length > 0 )
        //            p.exchangerate = Convert.ToDouble(ex);
        //        else
        //            p.exchangerate = 0;
        //        return View(p);

        //    }
        //    catch (Exception e)
        //    {
        //        return View();
        //    }

        //}

        public ActionResult PurchaseOrderDetail(int itemid=0,int purchaseid = 0)
        {
            try
            {
                ViewBag.ProductOptions = db.products.Where(x => x.status == "Active").OrderBy(x => x.desc).ToList();
                ViewBag.UOMOptions = db.UOM.ToList();
                //ViewBag.POItemOptions = db.purchaseItems.Where(x => x.purchaseid == purchaseid).OrderBy(x => x.lineno).ToList();
                Purchase_m_Purchase_Items p;
                if (itemid == 0)
                {
                    p = new Purchase_m_Purchase_Items();
                    p.purchaseid = purchaseid;
                    p.currency = db.ConfigDefault.Where(x => x.key == "CURR").FirstOrDefault().value;
                    p.gstcode = db.ConfigDefault.Where(x => x.key == "PGST").FirstOrDefault().value;
                    var ex = db.ConfigDefault.Where(x => x.key == "EXCH").FirstOrDefault().value;

                    if (ex.Length > 0)
                        p.exchangerate = Convert.ToDouble(ex);
                    else
                        p.exchangerate = 0;
                }
                else
                {
                    p = db.purchaseItems.Find(itemid); 
                }
                return View(p);

            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult SavePurchaseOrderDetail(Purchase_m_Purchase_Items p)
        {
            return View(p);
        }

        public ActionResult PurchasePayment(int purchaseid = 0)
        {
            try
            {
                int i = 0;
                ViewBag.PaymentStatusOptions = db.PaymentStatus.ToList();
                ViewBag.ResourceOptions = db.Resources.ToList();
                ViewBag.PaymentItemOptions = db.purchasePayment.Where(x => x.purchaseid == purchaseid).OrderBy(x => x.id).ToList();
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
                Purchase_m_Payment p = new Purchase_m_Payment();
                p.purchaseid = purchaseid;
                return View(p);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SavePurchasePayment(Purchase_m_Payment p)
        {
            return View(p);
        }
    }
}
