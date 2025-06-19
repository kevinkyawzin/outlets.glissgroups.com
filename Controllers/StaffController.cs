using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMac.Models;

namespace BigMac.Controllers
{
    public class StaffController : Controller
    {
        private BigMacEntities db = new BigMacEntities();
        //
        // GET: /Staff/

        public ActionResult Index()
       {    string dstr = "" ;
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";
            ViewBag.accessid = Request.QueryString["accessid"].ToString(); 

            foreach (var s in db.Staffs)
            {
                cAESEncryption.AESDecryption(s.address, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.address = dstr;
                cAESEncryption.AESDecryption(s.postalcode, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.postalcode = dstr;
                cAESEncryption.AESDecryption(s.nric, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.nric = dstr;
                cAESEncryption.AESDecryption(s.tel, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.tel = dstr;
                cAESEncryption.AESDecryption(s.fax, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.fax = dstr;
                cAESEncryption.AESDecryption(s.mobile, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.mobile = dstr;
                cAESEncryption.AESDecryption(s.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                s.email = dstr;
            }
             
            return View(db.Staffs.OrderBy(x => x.name).ToList());
        }

        //private void PopulatePositionDropDownList(object selectedPosition = null)
        //{
        //    ViewBag.positions = new SelectList(db.Positions, "id", "position", selectedPosition);
        //} 
        [HttpGet]
        public ActionResult Create()
        {
            //PopulatePositionDropDownList();
            //this.ViewData["Positions"] = new SelectList(db.Positions, "id", "position");
            //IEnumerable<SelectListItem> tmp = db.Positions.Select(b => new SelectListItem { Value = Convert.ToString(b.id) , Text = b.position });
            //ViewBag.Positions1 = db.Positions.AsEnumerable();
            int i;
            ViewBag.StaffTypeOptions = db.Types.AsEnumerable();
            ViewBag.PositionOptions = db.Positions.AsEnumerable();
            ICollection<Configuration_m_Company> companies = db.Companies.ToList();
            for (i = 0; i < companies.Count; i++)
            {
                companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
            }
            ViewBag.CompanyOptions = companies;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Common_m_Staff staff)
        {
            var EncryptHex = "";
            string dstr = "";
            if (ModelState.IsValid)
            {
                staff.createdate = DateTime.Today;
                //cAESEncryption.
                cAESEncryption.AESEncryption(staff.address, out EncryptHex, out dstr);
                staff.address = dstr;
                cAESEncryption.AESEncryption(staff.postalcode, out EncryptHex, out dstr);
                staff.postalcode = dstr;
                cAESEncryption.AESEncryption(staff.nric, out EncryptHex, out dstr);
                staff.nric = dstr;
                cAESEncryption.AESEncryption(staff.tel, out EncryptHex, out dstr);
                staff.tel = dstr;
                cAESEncryption.AESEncryption(staff.fax, out EncryptHex, out dstr);
                staff.fax = dstr;
                cAESEncryption.AESEncryption(staff.mobile, out EncryptHex, out dstr);
                staff.mobile = dstr;
                cAESEncryption.AESEncryption(staff.email, out EncryptHex, out dstr);
                staff.email = dstr;
                db.Staffs.Add(staff);                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        public ActionResult Edit(int id = 0)
        {
            int i;
            ViewBag.StaffTypeOptions = db.Types.AsEnumerable();
            ViewBag.PositionOptions = db.Positions.AsEnumerable();
            ICollection<Configuration_m_Company> companies = db.Companies.ToList();
            for (i = 0; i < companies.Count; i++)
            {
                companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
            }
            ViewBag.CompanyOptions = companies;
            Common_m_Staff staff = db.Staffs.Find(id);
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";
            if (staff == null)
            {
                return HttpNotFound();
            }

            cAESEncryption.AESDecryption(staff.address, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.address = dstr;
            cAESEncryption.AESDecryption(staff.postalcode, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.postalcode = dstr;
            cAESEncryption.AESDecryption(staff.nric, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.nric = dstr;
            cAESEncryption.AESDecryption(staff.tel, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.tel = dstr;
            cAESEncryption.AESDecryption(staff.fax, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.fax = dstr;
            cAESEncryption.AESDecryption(staff.mobile, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.mobile = dstr;
            cAESEncryption.AESDecryption(staff.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.email = dstr;
            return View(staff);
        }

        //
        // POST: /Access/Edit/5

        [HttpPost]
        public ActionResult Edit(Common_m_Staff staff)
        {
            string dstr = "";
            var EncryptHex = "";

            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;                
                cAESEncryption.AESEncryption(staff.address, out EncryptHex, out dstr);
                staff.address = dstr;
                cAESEncryption.AESEncryption(staff.postalcode, out EncryptHex, out dstr);
                staff.postalcode = dstr;
                cAESEncryption.AESEncryption(staff.nric, out EncryptHex, out dstr);
                staff.nric = dstr;
                cAESEncryption.AESEncryption(staff.tel, out EncryptHex, out dstr);
                staff.tel = dstr;
                cAESEncryption.AESEncryption(staff.fax, out EncryptHex, out dstr);
                staff.fax = dstr;
                cAESEncryption.AESEncryption(staff.mobile, out EncryptHex, out dstr);
                staff.mobile = dstr;
                cAESEncryption.AESEncryption(staff.email, out EncryptHex, out dstr);
                staff.email = dstr;                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        //
        // GET: /Access/Delete/5

        public ActionResult Delete(int id = 0)
        {
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";

            ViewBag.StaffTypeOptions = db.Types.AsEnumerable();
            ViewBag.PositionOptions = db.Positions.AsEnumerable();
            Common_m_Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }

            cAESEncryption.AESDecryption(staff.address, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.address = dstr;
            cAESEncryption.AESDecryption(staff.postalcode, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.postalcode = dstr;
            cAESEncryption.AESDecryption(staff.nric, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.nric = dstr;
            cAESEncryption.AESDecryption(staff.tel, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.tel = dstr;
            cAESEncryption.AESDecryption(staff.fax, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.fax = dstr;
            cAESEncryption.AESDecryption(staff.mobile, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.mobile = dstr;
            cAESEncryption.AESDecryption(staff.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            staff.email = dstr;
            return View(staff);
        }

        //
        // POST: /Access/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Common_m_Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
