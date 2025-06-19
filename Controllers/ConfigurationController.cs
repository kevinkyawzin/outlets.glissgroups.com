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
    public class ConfigurationController : Controller
    {
        //
        // GET: /Configuration/
        private BigMacEntities db = new BigMacEntities();

        public ActionResult DefaultIndex()
        {
            return View(db.ConfigDefault.OrderBy(x => x.description).ToList());
        }

        public ActionResult DefaultSave(int id = 0)
        {
            try
            {
                //int i = 0;
                //ViewBag.CategoryOptions = db.productCategory.AsEnumerable();
                //ViewBag.StatusOptions = db.productStatus.AsEnumerable();
                //ViewBag.UOMOptions = db.UOM.AsEnumerable();
                //ICollection<Configuration_m_Company> companies = db.Companies.ToList();
                //for (i = 0; i < companies.Count; i++)
                //{
                //    companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                //}
                //ViewBag.CompanyOptions = companies;
                if (id == 0)
                {
                    Configuration_m_Default d = new Configuration_m_Default();
                    return View(d);
                }
                else
                {
                    Configuration_m_Default d = db.ConfigDefault.Find(id);
                    return View(d);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult DefaultSave(Configuration_m_Default d)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(user).State = EntityState.Modified;
                    //sid = Convert.Toint(Request.Params["staffid"].ToString());
                    if (d.id == 0)
                    {
                        db.ConfigDefault.Add(d);
                        d.createdate = DateTime.Now; 
                        db.SaveChanges();
                        return RedirectToAction("DefaultIndex");
                    }
                    else
                    {
                        var de = db.ConfigDefault.Find(d.id);
                        if (de != null)
                        {
                            de.description = d.description;
                            de.value = d.value;
                            de.key = d.key; 
                            db.SaveChanges();
                            return RedirectToAction("DefaultIndex");
                        }
                    }
                }

            }
            catch (Exception e) { }
            return View(d);
        }

        [HttpPost]
        public ActionResult DefaultDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Configuration_m_Default d = db.ConfigDefault.Find(id);
                db.ConfigDefault.Remove(d);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult BranchIndex()
        {
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";

            foreach (var b in db.Branches)
            {
                cAESEncryption.AESDecryption(b.branchname, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                b.branchname = dstr;
                cAESEncryption.AESDecryption(b.tel, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                b.tel = dstr;
                cAESEncryption.AESDecryption(b.fax, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                b.fax = dstr;
                cAESEncryption.AESDecryption(b.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                b.email = dstr;
            }

            return View(db.Branches.OrderBy(x => x.branchname).ToList());

        }

        public ActionResult BranchSave(string branchcode= "")
        {
            try
            {
                ICollection<Configuration_m_Company> cmp = db.Companies.ToList();
                for (int i = 0; i < cmp.Count; i++)
                {
                    cmp.ElementAt(i).name = cAESEncryption.getDecryptedString(cmp.ElementAt(i).name);
                }
                ViewBag.CompanyOptions = cmp.ToList();

                if (branchcode == "")
                {
                    Configuration_m_Branches b= new Configuration_m_Branches();
                    return View(b);
                }
                else
                {
                    Configuration_m_Branches b = db.Branches.Find(branchcode);                   
                    b.branchname = cAESEncryption.getDecryptedString(b.branchname);
                    b.tel = cAESEncryption.getDecryptedString(b.tel);
                    b.fax = cAESEncryption.getDecryptedString(b.fax);
                    b.email = cAESEncryption.getDecryptedString(b.email);
                    return View(b);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult BranchSave(Configuration_m_Branches branch)
        {
            try
            {
                var EncryptHex = "";
                string dstr = "";

                if (ModelState.IsValid)
                {
                    //db.Entry(user).State = EntityState.Modified;
                    //sid = Convert.Toint(Request.Params["staffid"].ToString());
                    if (branch.branchcode == "")
                    {
                        cAESEncryption.AESEncryption(branch.branchname, out EncryptHex, out dstr);
                        branch.branchname = dstr;
                        cAESEncryption.AESEncryption(branch.tel, out EncryptHex, out dstr);
                        branch.tel = dstr;
                        cAESEncryption.AESEncryption(branch.fax, out EncryptHex, out dstr);
                        branch.fax = dstr;
                        cAESEncryption.AESEncryption(branch.email, out EncryptHex, out dstr);
                        branch.email = dstr;
                        db.Branches.Add(branch);
                        db.SaveChanges();
                        return RedirectToAction("BranchIndex");
                    }
                    else
                    {
                            db.Entry(branch).State = EntityState.Modified;
                            cAESEncryption.AESEncryption(branch.branchname, out EncryptHex, out dstr);
                            branch.branchname = dstr;
                            cAESEncryption.AESEncryption(branch.tel, out EncryptHex, out dstr);
                            branch.tel = dstr;
                            cAESEncryption.AESEncryption(branch.fax, out EncryptHex, out dstr);
                            branch.fax = dstr;
                            cAESEncryption.AESEncryption(branch.email, out EncryptHex, out dstr);
                            branch.email = dstr;
                            db.SaveChanges();
                            return RedirectToAction("BranchIndex");    
                        
                    }
                }

            }
            catch (Exception e) { }
            return View(branch);
        }

        public ActionResult BranchCreate()
        {
            //ViewBag.StatusOptions = db.CampaignStatus.AsEnumerable();
            //var campaign = new Campaign_m_Campaign();
            //user.accessRights = new List<Access_m_AccessRights>();    
             ICollection<Configuration_m_Company> cmp= db.Companies.ToList();
            for(int i=0; i < cmp.Count ; i++)
            {
                cmp.ElementAt(i).name = cAESEncryption.getDecryptedString(cmp.ElementAt(i).name);  
            }
            ViewBag.CompanyOptions = cmp.ToList(); 
            return View();
        }


        [HttpPost]
        public ActionResult BranchCreate(Configuration_m_Branches branch)
        {

            //dbtmp.ope
            //var userid = db.Users.Where(x =>x.username == User.Identity.Name).AsEnumerable().First().id;
            var EncryptHex = "";
            string dstr = "";
            try
            {
                if (ModelState.IsValid)
                {
                    //branch.branchname = cAESEncryption.getDecryptedString(branch.branchname);
                    //branch.tel = cAESEncryption.getDecryptedString(branch.tel);
                    //branch.fax = cAESEncryption.getDecryptedString(branch.fax);
                    //branch.email = cAESEncryption.getDecryptedString(branch.email);
                    cAESEncryption.AESEncryption(branch.branchname, out EncryptHex, out dstr);
                    branch.branchname = dstr;
                    cAESEncryption.AESEncryption(branch.tel, out EncryptHex, out dstr);
                    branch.tel = dstr;
                    cAESEncryption.AESEncryption(branch.fax, out EncryptHex, out dstr);
                    branch.fax = dstr;
                    cAESEncryption.AESEncryption(branch.email, out EncryptHex, out dstr);
                    branch.email = dstr;
                    db.Branches.Add(branch);
                    db.SaveChanges();
                    return RedirectToAction("BranchIndex");
                }
            }
            catch (Exception e)
            {

            }
            return View(branch);
        }

        public ActionResult BranchEdit(int id = 0)
        {

            //ViewBag.CompanyOptions = db.Companies.AsEnumerable();
             ICollection<Configuration_m_Company> cmp= db.Companies.ToList();
            for(int i=0; i < cmp.Count ; i++)
            {
                cmp.ElementAt(i).name = cAESEncryption.getDecryptedString(cmp.ElementAt(i).name);  
            }
            ViewBag.CompanyOptions = cmp.ToList(); 
            Configuration_m_Branches branch = db.Branches.Find(id);            
            //user.accessRights = db.AccessRights.Include("Resources").Include("accessTypes").Where(x => x.userid == user.id).AsEnumerable();
            //string dstr = "";
            //var MKey = "";
            //var IVKey = "";
            //var MKeyHex = "";
            //var IVKeyHex = "";
            //var EncryptHex = "";

            if (branch == null)
            {
                return HttpNotFound();
            }

            //cAESEncryption.AESDecryption(branch.branchname, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            //branch.branchname = dstr;
            //cAESEncryption.AESDecryption(branch.tel, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            //branch.tel = dstr;
            //cAESEncryption.AESDecryption(branch.fax, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            //branch.fax = dstr;
            //cAESEncryption.AESDecryption(branch.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            //branch.email = dstr;
            branch.branchname = cAESEncryption.getDecryptedString(branch.branchname);
            branch.tel = cAESEncryption.getDecryptedString(branch.tel);
            branch.fax = cAESEncryption.getDecryptedString(branch.fax);
            branch.email = cAESEncryption.getDecryptedString(branch.email);
            return View(branch);
        }

        //
        // POST: /Access/Edit/5
        [HttpPost]
        public ActionResult BranchEdit(Configuration_m_Branches branch)
        {
            string dstr = "";
            var EncryptHex = "";
            //var opwd = db.Users.Find(user.id).password;
            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                cAESEncryption.AESEncryption(branch.branchname, out EncryptHex, out dstr);
                branch.branchname = dstr;
                cAESEncryption.AESEncryption(branch.tel, out EncryptHex, out dstr);
                branch.tel = dstr;
                cAESEncryption.AESEncryption(branch.fax, out EncryptHex, out dstr);
                branch.fax = dstr;
                cAESEncryption.AESEncryption(branch.email, out EncryptHex, out dstr);
                branch.email = dstr;
                db.SaveChanges();
                return RedirectToAction("BranchIndex");                
            }
            return View(branch);
        }

        [HttpPost]
        public ActionResult BranchDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Configuration_m_Branches branch = db.Branches.Find(id);
                db.Branches.Remove(branch);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult CompanyIndex()
        {
            int i = 0;
            ICollection<Configuration_m_Company> companies = db.Companies.OrderBy(x => x.id).ToList();
            for (i = 0; i < (companies.Count); i++)
            {
                companies.ElementAt(i).name = cAESEncryption.getDecryptedString(companies.ElementAt(i).name);
                companies.ElementAt(i).address = cAESEncryption.getDecryptedString(companies.ElementAt(i).address);
                companies.ElementAt(i).postalcode = cAESEncryption.getDecryptedString(companies.ElementAt(i).postalcode);
                companies.ElementAt(i).tel = cAESEncryption.getDecryptedString(companies.ElementAt(i).tel);
                companies.ElementAt(i).fax = cAESEncryption.getDecryptedString(companies.ElementAt(i).fax);
                companies.ElementAt(i).email = cAESEncryption.getDecryptedString(companies.ElementAt(i).email);
            }

            return View(companies.OrderBy(x=>x.name).ToList());
        }

        public ActionResult CompanySave(int id = 0)
        {
            try
            {

                if (id == 0)
                {
                    Configuration_m_Company company = new Configuration_m_Company();
                    return View(company);
                }
                else
                {
                    Configuration_m_Company company = db.Companies.Find(id);
                    company.name = cAESEncryption.getDecryptedString(company.name);
                    company.address = cAESEncryption.getDecryptedString(company.address);
                    company.postalcode = cAESEncryption.getDecryptedString(company.postalcode);
                    company.tel = cAESEncryption.getDecryptedString(company.tel);
                    company.fax = cAESEncryption.getDecryptedString(company.fax);
                    company.email = cAESEncryption.getDecryptedString(company.email);
                    company.website = cAESEncryption.getDecryptedString(company.website);
                    company.coregno = cAESEncryption.getDecryptedString(company.coregno);
                    company.gstregno = cAESEncryption.getDecryptedString(company.gstregno);
                    company.mailusername = cAESEncryption.getDecryptedString(company.mailusername);
                    company.mailpassword = cAESEncryption.getDecryptedString(company.mailpassword);
                    company.mailserver = cAESEncryption.getDecryptedString(company.mailserver);
                    company.mailserverport = cAESEncryption.getDecryptedString(company.mailserverport);
                    company.maildisplayname = cAESEncryption.getDecryptedString(company.maildisplayname);
                    return View(company);
                }
            }
            catch (Exception e)
            {
                return View();
            }

        }

        //
        // POST: /Access/Edit/5

        [HttpPost]
        public ActionResult CompanySave(Configuration_m_Company company)
        {
            string dstr = "";
            var EncryptHex = "";          
            
            try
            {
                if (ModelState.IsValid)
                {                    
                    
                    if (company.id == 0)
                    {
                        
                        cAESEncryption.AESEncryption(company.name, out EncryptHex, out dstr);
                        company.name = dstr;
                        cAESEncryption.AESEncryption(company.address, out EncryptHex, out dstr);
                        company.address = dstr;
                        cAESEncryption.AESEncryption(company.postalcode, out EncryptHex, out dstr);
                        company.postalcode = dstr;
                        cAESEncryption.AESEncryption(company.tel, out EncryptHex, out dstr);
                        company.tel = dstr;
                        cAESEncryption.AESEncryption(company.fax, out EncryptHex, out dstr);
                        company.fax = dstr;
                        cAESEncryption.AESEncryption(company.email, out EncryptHex, out dstr);
                        company.email = dstr;
                        cAESEncryption.AESEncryption(company.website, out EncryptHex, out dstr);
                        company.website = dstr;
                        cAESEncryption.AESEncryption(company.coregno, out EncryptHex, out dstr);
                        company.coregno = dstr;
                        cAESEncryption.AESEncryption(company.gstregno, out EncryptHex, out dstr);
                        company.gstregno = dstr;
                        cAESEncryption.AESEncryption(company.mailusername, out EncryptHex, out dstr);
                        company.mailusername = dstr;
                        cAESEncryption.AESEncryption(company.mailpassword, out EncryptHex, out dstr);
                        company.mailpassword = dstr;
                        cAESEncryption.AESEncryption(company.mailserver, out EncryptHex, out dstr);
                        company.mailserver = dstr;
                        cAESEncryption.AESEncryption(company.mailserverport, out EncryptHex, out dstr);
                        company.mailserverport = dstr;
                        cAESEncryption.AESEncryption(company.maildisplayname, out EncryptHex, out dstr);
                        company.maildisplayname = dstr;
                        db.Companies.Add(company);
                        db.SaveChanges();

                    }
                    else
                    {
                        var Ucompany = db.Companies.Find(company.id);
                        if (Ucompany != null)
                        {
                            cAESEncryption.AESEncryption(company.name, out EncryptHex, out dstr);
                            Ucompany.name = dstr;
                            cAESEncryption.AESEncryption(company.address, out EncryptHex, out dstr);
                            Ucompany.address = dstr;
                            cAESEncryption.AESEncryption(company.postalcode, out EncryptHex, out dstr);
                            Ucompany.postalcode = dstr;
                            cAESEncryption.AESEncryption(company.tel, out EncryptHex, out dstr);
                            Ucompany.tel = dstr;
                            cAESEncryption.AESEncryption(company.fax, out EncryptHex, out dstr);
                            Ucompany.fax = dstr;
                            cAESEncryption.AESEncryption(company.email, out EncryptHex, out dstr);
                            Ucompany.email = dstr;
                            cAESEncryption.AESEncryption(company.website, out EncryptHex, out dstr);
                            Ucompany.website = dstr;
                            cAESEncryption.AESEncryption(company.coregno, out EncryptHex, out dstr);
                            Ucompany.coregno = dstr;
                            cAESEncryption.AESEncryption(company.gstregno, out EncryptHex, out dstr);
                            Ucompany.gstregno = dstr;
                            cAESEncryption.AESEncryption(company.mailusername, out EncryptHex, out dstr);
                            Ucompany.mailusername = dstr;
                            cAESEncryption.AESEncryption(company.mailpassword, out EncryptHex, out dstr);
                            Ucompany.mailpassword = dstr;
                            cAESEncryption.AESEncryption(company.mailserver, out EncryptHex, out dstr);
                            Ucompany.mailserver = dstr;
                            cAESEncryption.AESEncryption(company.mailserverport, out EncryptHex, out dstr);
                            Ucompany.mailserverport = dstr;
                            cAESEncryption.AESEncryption(company.maildisplayname, out EncryptHex, out dstr);
                            Ucompany.maildisplayname = dstr;
                            Ucompany.mailsmtpssl = company.mailsmtpssl; 
                            db.SaveChanges();
                            //return RedirectToAction("Index");
                        }
                    }
                }

            }
            catch (Exception e) { }
            return RedirectToAction("CompanyIndex");
        }

        [HttpPost]
        public ActionResult RemoveCompany(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Configuration_m_Company company = db.Companies.Find(id);
                db.Companies.Remove(company);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }
    }
}
