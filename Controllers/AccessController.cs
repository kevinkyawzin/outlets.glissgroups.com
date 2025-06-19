using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigMac.Models;

namespace BigMac.Controllers
{
    public class AccessController : Controller
    {
        private BigMacEntities db = new BigMacEntities();

        //
        // GET: /Access/

        public ActionResult BranchStaffIndex(int sid=0)
        {
 
            string bcode = "";
            try
            {
                //if (sid == 0)
                //{
                    if (string.IsNullOrEmpty(Session["branchcode"].ToString()) == false)
                        bcode = Session["branchcode"].ToString();
                //}
                if (sid ==0)
                    Session["staffid"] = "";
                return View(db.BranchStaff.Where(x => x.branchcode == bcode).ToList());

            }
            catch (Exception e)
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult BranchStaffIndex(int sid=0,string sname="")
        {
            try
            {
      
                if (sid==0)
                {
                    sid = Convert.ToInt32(Request.Params["sid"]);
                }
                Common_m_Staff s = db.Staffs.Find(sid);
                if (s != null)
                {
                    int uid = s.userid;
                    Session["staffid"] = sid;
                    Session["staffname"] = sname;
                    Session["staffuserid"] = "";
                    //Changed by ZawZO on 10 Sep 2015
                    /*Access_m_Users u = db.Users.Include("Role").Where(x => x.id == uid && x.Role.Role.ToUpper() != "STAFF").FirstOrDefault();
                    if (u != null)
                    {
                        Session["staffuserid"] = u.id;
                        return RedirectToAction("Login", "Access", new { uFlag = 2 });
                    }
                    else
                    {
                        return RedirectToAction("Index", "HOME");
                    }*/
                    Session["isBranchHomePage"] = false;
                    return RedirectToAction("Index", "HOME");                            
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public ActionResult QR()
        {
            string bcode = "";
            try
            {
                if (string.IsNullOrEmpty(Session["branchcode"].ToString()) == false)
                    bcode = Session["branchcode"].ToString();
               
                return View();

            }
            catch (Exception e)
            {
                return RedirectToAction("Login");
            }
        }

        //Added by ZawZO on 14 Jan 2016
        public ActionResult BranchAssetIndex(int sid = 0)
        {
            string bcode = "";
            try
            {
                if (string.IsNullOrEmpty(Session["branchcode"].ToString()) == false)
                    bcode = Session["branchcode"].ToString();
                if (sid == 0)
                    Session["staffid"] = "";
                return View(db.BranchAsset.Where(x => x.branchcode == bcode).ToList());

            }
            catch (Exception e)
            {
                return RedirectToAction("Login");
            }
        }

        //Added by ZawZO on 14 Jan 2016
        [HttpPost]
        public ActionResult BranchAssetIndex(int aid = 0, string aname = "")
        {
            try
            {
                if (aid == 0)
                {
                    aid = Convert.ToInt32(Request.Params["aid"]);
                }
                Common_m_BranchAsset a = db.BranchAsset.Find(aid);
                if (a != null)
                {
                    Session["assetid"] = a.id;
                    Session["assetname"] = a.name;
                    return RedirectToAction("SalesOrder", "POS");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public ActionResult RoleIndex()
        {
            return View(db.Roles.OrderBy(x => x.Role).ToList());
        }

        public ActionResult RoleSave(int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    Access_z_Roles r = new Access_z_Roles();
                    return View(r);
                }
                else
                {
                    Access_z_Roles r = db.Roles.Find(id);
                    return View(r);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult RoleSave(Access_z_Roles r)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(user).State = EntityState.Modified;
                    //sid = Convert.Toint(Request.Params["staffid"].ToString());
                    if (r.id == 0)
                    {
                        r.createdate = DateTime.Now;
                        db.Roles.Add(r);
                        db.SaveChanges();
                        return RedirectToAction("RoleIndex");
                    }
                    else
                    {
                        var role = db.Roles.Find(r.id);
                        if (role != null)
                        {
                            role.Role  = r.Role;
                            role.lastmodifieddate = DateTime.Now;
                            db.SaveChanges();
                            return RedirectToAction("RoleIndex");
                        }
                    }
                }

            }
            catch (Exception e) { }
            return View(r);
        }

        [HttpPost]
        public ActionResult RoleDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Access_z_Roles r = db.Roles.Find(id);
                db.Roles.Remove(r);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { 
                returnStr = e.Message.ToString();
            }
            return Content(returnStr);
        }

        public ActionResult AccessRightIndex()
        {
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";
            //var urole =  db.UserRoles.AsEnumerable();
            var listOfUser = db.Users.ToList();
            foreach (var user in listOfUser)
            {
                cAESEncryption.AESDecryption(user.name, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.name = dstr;
                cAESEncryption.AESDecryption(user.username, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.username = dstr;
                cAESEncryption.AESDecryption(user.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.email = dstr;
                //var r =urole.Select(u => u.id == user.roldid).Single();
                //user.role = r.ToString(); 
                //ViewBag.Role = "Administrator";//db.UserRoles.Find(user.roldid).Role;
                //user.role = db.UserRoles.Find(user.roldid);
                //user.role = db.UserRoles.Find(user.roldid).Role;
                //user.role = z.Role;
            }
            var model = listOfUser; //db.Users.ToList();
           
            return View(model);
        }


        public ActionResult Index()
        {
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";
  
            //var urole =  db.UserRoles.AsEnumerable();
            foreach (var user in db.Users)
            {
                cAESEncryption.AESDecryption(user.name, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.name = dstr;
                cAESEncryption.AESDecryption(user.username, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.username = dstr;
                cAESEncryption.AESDecryption(user.email, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
                user.email = dstr;
                //var r =urole.Select(u => u.id == user.roldid).Single();
                //user.role = r.ToString(); 
                //ViewBag.Role = "Administrator";//db.UserRoles.Find(user.roldid).Role;
                //user.role = db.UserRoles.Find(user.roldid);
                //user.role = db.UserRoles.Find(user.roldid).Role;
                //user.role = z.Role;
            }
            var model = db.Users.Include("Role").ToList();   
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public JsonResult ChangePasswordSave(string oldpwd="",string newpwd="")
        {
            var returnStr = "FAIL";

            try
            {
                string uname = User.Identity.Name;
                int uid = 0;
                string urole = "";
                int staffid = 0;
                if (isValidUser(uname, oldpwd, "", ref uid, ref urole, ref staffid))
                {
                    Access_m_Users u = db.Users.Find(uid);
                    if (u != null)
                    {
                        u.password = cAESEncryption.getEncryptedString(newpwd);
                        db.SaveChanges();
                        returnStr = "SUCCESS";
                    }
                }
                else
                { returnStr = "Invalid Password!"; }
            }

            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            //return Content(returnStr);
            return Json(returnStr, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Login(int uFlag=1)
        {
            int i;
            callDefaultAppFields();
            ICollection<Configuration_m_Branches> branches = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
            for (i = 0; i < branches.Count; i++)
            {
                branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
            }
            //Configuration_m_Branches b = new Configuration_m_Branches();
            //b.id = 0;
            //b.branchcode = "";
            //b.branchname = "-- Select Branch --";
            //branches.Add(b);
            ViewBag.BranchOptions = branches;
            ViewBag.uFlag = uFlag;
            if (uFlag == 2)
                ViewBag.Branch = Session["branchcode"].ToString();
            else
            {
                Configuration_m_Default b= db.ConfigDefault.Where(x => x.key == "BRID").FirstOrDefault();
                if (b != null)
                { 
                    ViewBag.Branch = b.value;  
                }
                else
                    ViewBag.Branch = "";

            }
               
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Access_m_Users user,string rememberme ="off" )
        {

            int i; string uFlag = "1";
            callDefaultAppFields();
            try
            {
                var context = new BigMacEntities();
                var value = context.Database.ExecuteSqlCommand("Delete from rptencryptvalues where createddate < current_date or uname='" + user.username.Trim() + "'");               
            }
            catch (Exception e)
            { }
            try
            {
                ICollection<Configuration_m_Branches> branches = db.Branches.Where(x=>x.branchcode!="MILEAGE").ToList();
                for (i = 0; i < branches.Count; i++)
                {
                    branches.ElementAt(i).branchname = cAESEncryption.getDecryptedString(branches.ElementAt(i).branchname);
                }
                ViewBag.BranchOptions = branches;
                int uid = 0;
                string urole = "";
                user.username = user.username.ToUpper();
             
                string b = Request.Params["branchcode"].ToString();
                string c = db.Branches.Where(x => x.branchcode == b).FirstOrDefault().cocode;
                string branchid = db.Branches.Where(x => x.branchcode == b).FirstOrDefault().id.ToString();
                int staffid = 0;
                uFlag = Request.Params["uFlag"].ToString();

                ViewBag.uFlag = uFlag;
                //Added by ZawZO on 21 Jan 2016
                string ismob = Request.Params["ismobile"].ToString();
                Session.Add("ismobile", ismob);

                if (ModelState.IsValid)
                {
                    if (isValidUser(user.username, user.password, c, ref uid, ref urole,ref staffid))
                    {

                        if (uFlag == "1")
                        {
                            Session.Add("userid", uid);
                            Session.Add("staffid", "");
                            Session.Add("staffname", "");
                            Session.Add("staffuserid", "");
                            //Changed by ZawZO on 21 Dec 2015
                            Session.Add("cocode", c);
                            //Changed by ZawZO on 2 Sep 2015
                            //Session.Add("branchcode", "");
                            Session.Add("branchcode", b);
                            Session.Add("branchid", branchid);

                            Session.Add("cardno", "");
                            Session.Add("company", "");
                            Session.Add("possale", "0");
                            Session.Add("isBranchHomePage", false);
                        }
                        else 
                            Session["staffuserid"] = uid;
                        if (urole == "IPADMEMBER" || urole == "FINANCE" || urole == "BRANCH" || urole == "MANAGER" || urole == "PARTNERS OUTLET" || urole == "ADMINISTRATOR" || urole == "SYSTEMADMINISTRATOR" || urole == "SYSTEM ADMINISTRATOR" || urole == "HQBRANCH" || urole == "DIRECTOR" || urole == "OPERATION MANAGER" || urole == "OPERATIONMANAGER")
                        {
                            Boolean rFlag = false;

                            if (rememberme.ToUpper() == "ON" || rememberme.ToUpper() == "TRUE" || rememberme.ToUpper() == "1")
                                rFlag = true;
                            else
                                rFlag = false;

                            FormsAuthentication.SetAuthCookie(user.username, rFlag);
                            Session["userid"] = uid;
                            Session["staffid"] = "";
                            Session["branchcode"] = b;
                            Session.Add("branchid", branchid);
                            Session["cardno"] = 0;
                            var company = db.Companies.Where(x => x.cocode == c).FirstOrDefault().name;
                            Session["userrole"] = urole;
                            Session["company"] = cAESEncryption.getDecryptedString(company);
                            Session["cocode"] = c;
                            if (staffid > 0)
                            {
                                Session["staffid"] = staffid.ToString();

                                if (urole == "1MMCLUB MANAGER" || urole == "MANAGER" || urole == "BRANCH")
                                {
                                    Session["isBranchHomePage"] = true;
                                    return RedirectToAction("BranchStaffIndex", "Access");
                                   
                                }
                                else
                                    return RedirectToAction("Index", "HOME");
                            
                   
                            
                            }
                        }
                        //Added by ZawZO on 18 Feb 2016
                        else if (urole == "PARTNERSHIP MAIN ACCOUNT" || urole == "PARTNERSHIP USER")
                        {
                            FormsAuthentication.SetAuthCookie(user.username,true);
                            Session["userid"] = uid;

                            //return RedirectToAction("MileageReward", "POS");
                            return RedirectToAction("Login", "Access");
                   
                        }
                        //else if (uFlag == "1" && urole == "MANAGER")
                        //{
                        //    Common_m_Staff s = db.Staffs.Where(x => x.userid == uid).FirstOrDefault();
                        //    if (s != null)
                        //        Session["staffid"] = s.id;
                        //    else
                        //        Session["staffid"] = "";
                        //}
                        //AuthorizeAttribute.GetCustomAttributes
                        //Access_m_Users u= db.Users.fin                    
                        Session["cocode"] = c;
                        Session["branchcode"] = b;
                        Session["cardno"] = 0;                        
                        var com = db.Companies.Where(x => x.cocode == c).FirstOrDefault().name;
                        var bsale = db.Companies.Where(x => x.cocode == c).FirstOrDefault().branchsale;
                        var pstmp = db.ConfigDefault.Where(x => x.key == "POSSALE").FirstOrDefault();
                        var possale = "0";
                        if (pstmp != null)
                        { possale = pstmp.value; }
                        Session["possale"] = possale;
                        Session["company"] = cAESEncryption.getDecryptedString(com);
                        Session["branchsale"] = bsale;


                        //Changed by ZawZO on 21 Jan 2016
                        if (ismob == "1")
                        {
                           // return RedirectToAction("BranchAssetIndex", "Access");
                            if (uFlag == "1" && urole == "BRANCH")
                            {
                                return RedirectToAction("BranchStaffIndex", "Access");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            if (uFlag == "1" && urole == "BRANCH" )
                            {
                                return RedirectToAction("BranchStaffIndex", "Access");
                              //  return RedirectToAction("CusSupIndex", "CusSup");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                       
                        }
                      

                    }
                    else
                    {
                        var euname = "";
                        var epwd = "";
                        var EncryptHex = "";

                        cAESEncryption.AESEncryption(user.username, out EncryptHex, out euname);
                        cAESEncryption.AESEncryption(user.password, out EncryptHex, out epwd);

                        Access_m_Users u = db.Users.Where(x => x.username == euname && x.password == epwd).FirstOrDefault();
                     
                        if (u != null)
                        {
                            if (u.Role.Role.ToUpper() != "PARTNERSHIP MAIN ACCOUNT" && u.Role.Role.ToUpper() != "PARTNERSHIP USER")
                            {
                                if (u.Role.Role.ToUpper() != "PARTNERS OUTLET")
                                {

                                    if (u.status == "Discontinued" || u.status == "Banned")
                                    {
                                        AppSettings settings = new AppSettings();
                                        if (settings.appName == "Citiwarrior")
                                            ModelState.AddModelError("", "Pls approach Tiffany, Fanny or Hideaki to activate your ID for Citiwarrior.");
                                        else
                                            ModelState.AddModelError("", "Pls approach the administrator to activate your account.");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "Login Failed!");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "You can't login to other company.");
                                }
                            }
                            else
                            {

                                ModelState.AddModelError("", "Partners account can't use to login to outlets.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid username or password!");
                        }
                    
                    }
                }
                //FormsAuthentication.SetAuthCookie(user.username, false);
                //Session.Add("userid", uid);
                //return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            { ModelState.AddModelError("", e.Message.ToString());  }
            return View(user);
        }
        public ActionResult Logout()
        {
            //int i; string uFlag = "1";
            try
            {
                var context = new BigMacEntities();
                var value = context.Database.ExecuteSqlCommand("Delete from rptencryptvalues where createddate < current_date or uname='" + User.Identity.Name.Trim() + "'");
            }
            catch (Exception e)
            { }
            Session.RemoveAll();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Access");
            //Membership.GetUser( 
        }

        [HttpPost]
        public ActionResult ForgotPassword(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Access_m_Users user = db.Users.Find(id);
                var epwd = "";
                var EncryptHex = "";
                user.password = Membership.GeneratePassword(5, 1);
                cAESEncryption.AESEncryption(user.password, out EncryptHex, out epwd);
                user.password = epwd;
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }
            return Content(returnStr);
        }


        public Boolean isValidUser(string uname, string pwd, string cocode, ref int uid, ref string urole, ref int staffid ,string returnUrl = "")
        {
            Boolean flag = false;
            try
            {
                
                var euname = "";
                var epwd = "";
                var EncryptHex = "";

                cAESEncryption.AESEncryption(uname, out EncryptHex, out euname);
                cAESEncryption.AESEncryption(pwd, out EncryptHex, out epwd);

                ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x=> x.status.ToUpper() == "ACTIVE").ToList();
                Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();

               if (u != null)
               {
                   if (u.Role.Role.ToUpper() != "PARTNERSHIP MAIN ACCOUNT" && u.Role.Role.ToUpper() != "PARTNERSHIP USER") {

                       if (u.Role.Role.ToUpper() == "PARTNERS OUTLET")
                       {
                           if (cocode == u.cocode)
                           {
                               uid = u.id;
                               urole = u.Role.Role.ToUpper();
                               flag = true;
                               Common_m_Staff stmp = db.Staffs.Where(x => x.userid == u.id).FirstOrDefault();
                               if (stmp != null)
                                   staffid = stmp.id;
                           }

                       }
                       else
                       {
                           uid = u.id;
                           urole = u.Role.Role.ToUpper();
                           flag = true;
                           Common_m_Staff stmp = db.Staffs.Where(x => x.userid == u.id).FirstOrDefault();
                           if (stmp != null)
                               staffid = stmp.id;
                       }
                   }
                   
                }
                //using (var context = new BigMacEntities())
                //{
                //    ICollection<Access_m_Users> tmpuser = context.Database.SqlQuery<Access_m_Users>("call chkValidUser ('" + euname + "','" + epwd + "')").ToList();

                //    if (tmpuser.Count() > 0)
                //    {
                //        //uid = tmpuser.ElementAt(0).id;
                //        //flag = true;

                //    }

                //}
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("",e.Message.ToString());                
            }
            finally
            {
                //return true;
            }
            //return true;
            return flag;
            
        }

        public Boolean isValidUser(string uname, string pwd)
        {
            Boolean flag = false;
            try
            {

                //var euname = "";
                //var epwd = "";
                //var EncryptHex = "";

                //cAESEncryption.AESEncryption(uname, out EncryptHex, out euname);
                //cAESEncryption.AESEncryption(pwd, out EncryptHex, out epwd);

                ICollection<Access_m_Users> ulist = db.Users.Include("Role").Where(x => x.status.ToUpper() == "ACTIVE").ToList();
                Access_m_Users u = ulist.Where(x => cAESEncryption.getDecryptedString(x.username) == uname && cAESEncryption.getDecryptedString(x.password) == pwd).FirstOrDefault();
                if (u != null)
                {
                    flag = true;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message.ToString());
            }
            finally
            {                
            }            
            return flag;

        }

        //public Boolean isValidUser1(string uname, string pwd, string returnUrl,ref int uid)
        //{
        //    int i = 0;
        //    var euname = "";
        //    var epwd = "";
        //    var MKey = "";
        //    var IVKey = "";
        //    var MKeyHex = "";
        //    var IVKeyHex = "";
        //    var EncryptHex = "";
        //    //int c = Convert.Toint(db.Access_m_Users.Count().ToString());
        //    uname = Request.Params["username"].ToString();
        //    pwd = Request.Params["password"].ToString();
        //    Boolean flag = false;
        //    foreach (var item in db.Users.AsEnumerable())
        //    {
        //        cAESEncryption.AESDecryption(item.username, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out euname);
        //        cAESEncryption.AESDecryption(item.password, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out epwd);
        //        if (euname == uname && epwd == pwd)
        //        {
        //            uid = item.id;
        //            flag = true;
        //        }
        //    }
        //    return flag;
        //}

        [HttpPost]
        [AllowAnonymous]        
        public ActionResult Logon(string uname,string pwd, string returnUrl)
        {
            int i = 0;
            //int c = Convert.Toint(db.Access_m_Users.Count().ToString());
            uname = Request.Params["username"].ToString();
            pwd = Request.Params["password"].ToString(); 

            Boolean flag =false ;
            //var users = db.Access_m_Users.OfType<Access_m_Users_View>();
            foreach (var item in db.Users.AsEnumerable())
            {
                if (item.name == uname && item.password == pwd)
                {
                    flag = true;
                }
            }
            
            if (flag) return RedirectToAction("Index", "Access"); //return RedirectToLocal(returnUrl);
            else return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }
        //
        // GET: /Access/Details/5

        public ActionResult Details(int id = 0)
        {
            Access_m_Users access_m_users = db.Users.Find(id);
            if (access_m_users == null)
            {
                return HttpNotFound();
            }
            return View(access_m_users);
        }

        //
        // GET: /Access/Create

        public ActionResult UserCreate()
        {
            ViewBag.StatusOptions = db.UserStauts.AsEnumerable();
            ViewBag.RoleOptions = db.Roles.AsEnumerable();
            var user = new Access_m_Users();
            //user.accessRights = new List<Access_m_AccessRights>();
            return View();
        }

        //
        // POST: /Access/Create

        [HttpPost]
        public ActionResult UserCreate(Access_m_Users user)
        {
            string dstr = "";
            var EncryptHex = "";
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                cAESEncryption.AESEncryption(user.name, out EncryptHex, out dstr);
                user.name = dstr;
                cAESEncryption.AESEncryption(user.username, out EncryptHex, out dstr);
                user.username = dstr;
                cAESEncryption.AESEncryption(user.password, out EncryptHex, out dstr);
                user.password = dstr;
                cAESEncryption.AESEncryption(user.email, out EncryptHex, out dstr);
                user.email = dstr;
                user.createdate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult UserSave(int id = 0)
        {
            try
            {
                ViewBag.StatusOptions = db.UserStauts.AsEnumerable();
                ViewBag.RoleOptions = db.Roles.AsEnumerable();
                ICollection<Common_m_Staff> staffs = db.Staffs.ToList();
                Common_m_Staff s = new Common_m_Staff();
                s.id = 0;
                s.name = "-select-";
                staffs.Add(s);
                ViewBag.StaffOptions =staffs;

                if (id == 0)
                {
                    Access_m_Users user = new Access_m_Users();
                    user.accessRights = db.AccessRights.Include("Resources").Include("accessTypes").Where(x => x.userid == 0).AsEnumerable();
                    ViewBag.StaffID = 0;
                    return View(user);
                }
                else
                {
                    Access_m_Users user = db.Users.Find(id);                    
                    user.accessRights = db.AccessRights.Include("Resources").Include("accessTypes").Where(x => x.userid == user.id).AsEnumerable();
                    user.name = cAESEncryption.getDecryptedString(user.name);
                    user.username = cAESEncryption.getDecryptedString(user.username);
                    //user.password = cAESEncryption.getDecryptedString(user.password);
                    user.email = cAESEncryption.getDecryptedString(user.email);
                    ICollection<Common_m_Staff> staff=  db.Staffs.Where(x => x.userid == user.id).ToList();
                    if (staff.Count > 0)
                    {
                        ViewBag.StaffID = staff.ElementAt(0).id;
                        user.staffid = staff.ElementAt(0).id;
                    }
                    else
                    {
                        ViewBag.StaffID = 0;                        
                    }

                    return View(user);
                }
            }catch (Exception e)
            {
                return View();
            }

        }

        //
        // POST: /Access/Edit/5

        [HttpPost]
        public ActionResult UserSave(Access_m_Users user)
        {
            string dstr = "";
            var EncryptHex = "";
            int sid = 0;

            //var opwd = db.Users.Find(user.id).password;
            user.username = user.username.ToUpper(); 
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(user).State = EntityState.Modified;
                    sid = Convert.ToInt32(Request.Params["staffid"].ToString());
                    if (user.id == 0)
                    {
                        db.Users.Add(user);
                        cAESEncryption.AESEncryption(user.name, out EncryptHex, out dstr);
                        user.name = dstr;
                        cAESEncryption.AESEncryption(user.username, out EncryptHex, out dstr);
                        user.username = dstr;
                        cAESEncryption.AESEncryption(user.password, out EncryptHex, out dstr);
                        user.password = dstr;
                        cAESEncryption.AESEncryption(user.email, out EncryptHex, out dstr);
                        user.email = dstr;
                        user.createdate = DateTime.Now;
                        db.SaveChanges();
                        //if (user.staffid > 0)
                        if (sid > 0)
                        {
                            Common_m_Staff stmp = db.Staffs.Find(user.staffid);
                            //stmp.userid = user.id;
                            stmp.userid = sid;
                            db.SaveChanges();
                            //using (var context = new BigMacEntities())
                            //{
                            //    var c = context.Database.ExecuteSqlCommand("call UpdateCommonStaffUserID(" + sid + "," + user.id + ")");
                            //}

                        }
                        return RedirectToAction("UserSave", new { id = user.id });
                    }
                    else
                    {
                        var duser = db.Users.Find(user.id);
                        if (duser != null)
                        {
                            duser.roldid = user.roldid;
                            cAESEncryption.AESEncryption(user.name, out EncryptHex, out dstr);
                            //user.name = dstr;
                            duser.name = dstr;
                            cAESEncryption.AESEncryption(user.username, out EncryptHex, out dstr);
                            //user.username = dstr;
                            duser.username = dstr;
                            //cAESEncryption.AESEncryption(user.password, out EncryptHex, out dstr);
                            //user.password = dstr;
                            //user.password = opwd;
                            cAESEncryption.AESEncryption(user.email, out EncryptHex, out dstr);
                            //user.email = dstr;
                            duser.email = dstr;
                            duser.status = user.status;
                            db.SaveChanges();
                            //if (user.staffid > 0)
                            if (sid > 0)
                            {
                                Common_m_Staff stmp = db.Staffs.Find(sid);
                                //stmp.userid = user.id;
                                stmp.userid = user.id;
                                db.SaveChanges();
                                //using (var context = new BigMacEntities())
                                //{
                                //    var c = context.Database.ExecuteSqlCommand("call UpdateCommonStaffUserID(" + sid + "," + duser.id + ")");
                                //}

                            }
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            catch (Exception e) { }
            return View(user);
        }

        //
        // GET: /Access/Delete/5

        //public ActionResult UserDelete(int id = 0)
        //{
        //    Access_m_Users access_m_users = db.Users.Find(id);
        //    if (access_m_users == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(access_m_users);
        //}

        ////
        //// POST: /Access/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult UserDeleteConfirmed(int id)
        //{
        //    Access_m_Users access_m_users = db.Users.Find(id);
        //    db.Users.Remove(access_m_users);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult RemoveUser(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Access_m_Users access_m_users = db.Users.Find(id);
                db.Users.Remove(access_m_users);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch (Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult getUserAccessRights(int userid = 0)
        {
            try
            {
                if (userid == 0)
                {
                    if (Session["staffuserid"] != null)
                    {
                        if (Session["staffuserid"].ToString() != "" && Session["staffuserid"].ToString() != "0")
                        { userid = Convert.ToInt32(Session["staffuserid"]); }
                        else if (Session["userid"] != null)
                        { userid = Convert.ToInt32(Session["userid"]); }
                    }
                    else if (Session["userid"] != null)
                    { userid = Convert.ToInt32(Session["userid"]); }
                }

                //using (var context = new BigMacEntities())
                //{
                //    var blogNames = context.Database.ExecuteSqlCommand("call getUserAccessRights(" + userid + ")");
                //}

                //ICollection<Access_m_AccessRights> 
                //var userar = db.AccessRights.Include("Resources").Join(db.Users, a => a.userid, u => u.id, (a, u) => new { AccessRights = a, Users = u }).Where(x => x.Users.username == username).Select(x => new { x.AccessRights.userid, x.Users.username, x.AccessRights.Resources.resource}).Distinct(); ;
                //var userar;
                int rid = db.Users.Where(x => x.id == userid).FirstOrDefault().roldid;
                if (rid == 1)
                {
                    var userar = db.Resources.Include("Resources").Select(x => new { x.id ,x.resource }).Distinct().ToList();
                    return Json(userar, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //var rolear = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == rid).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().ToList();
                    var userar = db.RoleAccessRights.Include("Resources").Where(x => x.roleid == rid).Select(x => new { x.Resources.id, userid, x.Resources.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == userid).Select(x => new { x.Resources.id, x.userid, x.Resources.resource }).Distinct());
                    //var userar = db.AccessRights.Include("Resources").Where(x => x.userid == userid).Select(x => new { x.Resources.id , x.userid, x.Resources.resource }).Distinct().ToList();
                    return Json(userar, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }


        //
        public ActionResult AccessRightCreate(int id = 0, int ar = 0)
        {
            Access_m_Users user = db.Users.Find(id);
            //user.accessRights  = new List<Access_m_AccessRights>();
            Access_m_AccessRights accessright = new Access_m_AccessRights();
            accessright.userid = id;
            accessright.username = accessright.username = cAESEncryption.getDecryptedString(user.username); ; 

            ViewBag.ResourceOptions = db.Resources.AsEnumerable();
            ViewBag.AccessTypeOptions = db.AccessTypes.AsEnumerable();
            ViewBag.AccessRights = db.AccessRights.Where(x => x.userid == user.id).AsEnumerable();
            ViewBag.ar = ar;
            //user.accessRights.Add(  
            return View(accessright);
        }
        // GET: /Access/Edit/5

        [HttpPost]
        public ActionResult AccessRightCreate(Access_m_AccessRights accessright)
        {
            //Database tmpdb;

            if (ModelState.IsValid)
            {
                //Access_m_AccessRights accessright = new Access_m_AccessRights();                
                db.AccessRights.Add(accessright);                
                db.SaveChanges();
                //return View(accessright);
                //return RedirectToAction("Edit", "Access", new { id = accessright.userid });
            }
            ViewBag.ResourceOptions = db.Resources.AsEnumerable();
            ViewBag.AccessTypeOptions = db.AccessTypes.AsEnumerable();
            ViewBag.AccessRights = db.AccessRights.Where(x => x.userid == accessright.userid).AsEnumerable();
            Access_m_Users user = db.Users.Find(accessright.userid);
            accessright.username = cAESEncryption.getDecryptedString(user.username);
            return View(accessright);
        }

        [HttpPost]
        public ActionResult AccessRightDelete(int id)
        {
            var returnStr = "FAIL";
            try
            {
                Access_m_AccessRights accessRight = db.AccessRights.Find(id);
                db.AccessRights.Remove(accessRight);
                db.SaveChanges();
                returnStr = "SUCCESS";
            }
            catch(Exception e)
            { returnStr = e.Message.ToString(); }

            return Content(returnStr);
        }

        public ActionResult getAppointmentListWithPaging(jQueryDataTableParamModel param)
        {
            try
            {
                string bcode = Session["branchcode"].ToString();

                ICollection<Schedule_m_Appointment> staffAppointment = db.appointment.Where(x => x.branchcode == bcode && x.status == "Booking" || x.status == "Wait List").OrderBy(x => x.appointmentdate).ThenBy(x => x.status).ToList();

                var appointmentList = (from app in staffAppointment
                                       join staff in db.Staffs on app.staffid equals staff.id
                                       join asset in db.BranchAsset on app.branchassetid equals asset.id
                                       select new
                                       {
                                           id = app.id,
                                           appdate = app.appointmentdate.ToString("MM/dd/yyyy"),
                                           time = app.starttime + " - " + app.endtime,
                                           customername = app.customername,
                                           mobile = app.mobile,
                                           nric = app.nric,
                                           staffname = staff.name,
                                           room = asset.name,
                                           description = app.description ?? "",
                                           status = app.status
                                       }).ToList();


                var searchValue = "";

                if (param.sSearch == null) searchValue = "";
                else searchValue = param.sSearch;

                searchValue = searchValue.ToUpper();
                var filterTList = appointmentList.Where(x => x.customername.ToUpper().Contains(searchValue) || x.description.ToString().Contains(searchValue) || x.staffname.Contains(searchValue) || x.description.Contains(searchValue) || x.status.ToUpper().Contains(searchValue)).ToList();

                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

                var sortDirection = Request["sSortDir_0"]; // asc or desc

                if (sortDirection == "asc")
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderBy(x => x.appdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderBy(x => x.time).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderBy(x => x.customername).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderBy(x => x.mobile).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderBy(x => x.nric).ToList();
                    else if (sortColumnIndex == 5)
                        filterTList = filterTList.OrderBy(x => x.staffname).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderBy(x => x.room).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderBy(x => x.description).ToList();
                    else if (sortColumnIndex == 7)
                        filterTList = filterTList.OrderBy(x => x.status).ToList();
                }
                else
                {
                    if (sortColumnIndex == 0)
                        filterTList = filterTList.OrderByDescending(x => x.appdate).ToList();
                    else if (sortColumnIndex == 1)
                        filterTList = filterTList.OrderByDescending(x => x.time).ToList();
                    else if (sortColumnIndex == 2)
                        filterTList = filterTList.OrderByDescending(x => x.customername).ToList();
                    else if (sortColumnIndex == 3)
                        filterTList = filterTList.OrderByDescending(x => x.mobile).ToList();
                    else if (sortColumnIndex == 4)
                        filterTList = filterTList.OrderByDescending(x => x.nric).ToList();
                    else if (sortColumnIndex == 5)
                        filterTList = filterTList.OrderByDescending(x => x.staffname).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderByDescending(x => x.room).ToList();
                    else if (sortColumnIndex == 6)
                        filterTList = filterTList.OrderByDescending(x => x.description).ToList();
                    else if (sortColumnIndex == 7)
                        filterTList = filterTList.OrderByDescending(x => x.status).ToList();
                }

                var paginatedQPList = filterTList.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = appointmentList.Count, //paginatedQPList.TotalCount,
                    iTotalDisplayRecords = filterTList.Count, //paginatedQPList.TotalCount,
                    aaData = paginatedQPList
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }

        

        [HttpPost]
        [Authorize]
        public JsonResult UpdateAppoinmentNoShow(int id = 0, string uname = "", string pwd = "")
        {
        
            try
            {
                if (chkUserAccessRightForAppointmentUpdate(uname.ToUpper(), pwd) == true)
                {
                    Schedule_m_Appointment app = db.appointment.FirstOrDefault(x => x.id == id);
                    app.status = "No Show";
                    db.SaveChanges();
                
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Wrong username and password or user doesn't have access to update.", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }
        [HttpPost]
        [Authorize]
        public JsonResult CancelAppointment(int id = 0, string uname = "", string pwd = "")
        {
        
            try
            {
                if (chkUserAccessRightForAppointmentVoid(uname.ToUpper(), pwd) == true)
                {
                    Schedule_m_Appointment app = db.appointment.FirstOrDefault(x => x.id == id);
                    app.status = "Cancelled";
                    db.SaveChanges();
                
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Wrong username and password or user doesn't have access to cancel.", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            { return Json("Failed", JsonRequestBehavior.AllowGet); }
        }





        [Authorize]
        public Boolean chkUserAccessRightForAppointmentVoid(string uname, string pwd)
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
                        var aList = db.RoleAccessRights.Where(x => x.roleid == u.roldid && x.resource == "APPTMENT" && x.voidField == true).Select(x => new { x.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == u.id && x.resource == "APPTMENT" && x.voidField == true).Select(x => new { x.Resources.resource }).Distinct()).ToList();
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
        public Boolean chkUserAccessRightForAppointmentUpdate(string uname, string pwd)
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
                        var aList = db.RoleAccessRights.Where(x => x.roleid == u.roldid && x.resource == "APPTMENT" && x.update == true).Select(x => new { x.resource }).Distinct().Union(db.AccessRights.Include("Resources").Where(x => x.userid == u.id && x.resource == "APPTMENT" && x.update == true).Select(x => new { x.Resources.resource }).Distinct()).ToList();
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
        

        public void callDefaultAppFields()
        {
            AppSettings settings = new AppSettings();
            ViewBag.favicon = settings.favicon;
            ViewBag.logo = settings.logo;
            ViewBag.home = settings.home;
            ViewBag.appName = settings.appName;
            ViewBag.contact = settings.contact;

        }
    }
}
