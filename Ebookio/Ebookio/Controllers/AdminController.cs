using Ebookio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ebookio.Controllers
{
    public class AdminController : Controller
    {
        ebookioEntities eobj = new ebookioEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            HttpCookie cookie = Request.Cookies["AdminLoginDetails"];
            if(cookie != null)
            {
                ViewBag.admin_emailid= cookie["admin_emailid"].ToString();
                ViewBag.admin_password = cookie["admin_password"].ToString();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(tbl_admin a)
        {
            HttpCookie cookie = new HttpCookie("AdminLoginDetails");
            if(ModelState.IsValid==true)
            {

                if(a.RememberMe == true)
                {
                    cookie["admin_emailid"] = a.admin_emailid;
                    cookie["admin_password"] = a.admin_password;
                    cookie.Expires = DateTime.Now.AddDays(2);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);
                }

                var adminauth = eobj.tbl_admin.Where(model=>model.admin_emailid == a.admin_emailid && model.admin_password == a.admin_password).FirstOrDefault();
                if(adminauth == null)
                {
                    ViewBag.msg = "Please Enter Correct EmailId Or Password";
                    return View();
                }
                else
                {
                    Session["admin_username"] = adminauth.admin_username;
                    return RedirectToAction("AdminDashboard");

                }
            }
            return View();
        }
        public ActionResult AdminDashboard()
        {
            if(Session["admin_username"]==null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return View();
            }
            
        }
        public ActionResult AdminLogOut()
        {
            Session.Abandon();
            return RedirectToAction("AdminLogin");
        }

        //--------------------------------------------------------------------------------------------
        //----------------------------------------------publisher--------------------------------------
        //--------------------------------------------------------------------------------------------

        public ActionResult Publisher()
        {
            ViewBag.msg = TempData["msg"] as string;
            //ViewBag.message1 = TempData["message1"] as string; 
            // ViewBag.insertmsg = TempData["insertmsg"] as string;
            // ViewBag.deletemsg = TempData["deletemsg"] as string;
            //ViewBag.updatemsg = TempData["updatemsg"] as string;
            using (var ctx = new ebookioEntities())
            {

                var publisherlist = ctx.tbl_publisher.SqlQuery("Select * from tbl_publisher").ToList<tbl_publisher>();
                return View(publisherlist);

            }
          
        }
        public ActionResult AddPublisher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPublisher(tbl_publisher pmodel)
        {
            if(ModelState.IsValid==true)
            {
                tbl_publisher p = new tbl_publisher();
                if (eobj.tbl_publisher.Any(model=>model.publisher_name.Equals(pmodel.publisher_name)))
                {
                    ViewBag.errormsg = "Pulisher Name Already Use..!!";
                }
                else
                {
                    p.publisher_name = pmodel.publisher_name;
                    eobj.tbl_publisher.Add(p);
                    eobj.SaveChanges();
                    // TempData["insertmsg"] = "Successfuly Inserted !!!";
                    TempData["msg"] = "Successfuly Inserted !!!";
                    return RedirectToAction("Publisher");
                }
                
              
            }
            return View();
        }
        public ActionResult DeletePublisher(int deleteid)
        {
            var deleterecord = eobj.tbl_publisher.Where(x => x.publisher_id == deleteid).First();
            eobj.tbl_publisher.Remove(deleterecord);
            eobj.SaveChanges();
            // TempData["msg"] = "Successfuly Deleted !!!";
            //TempData["message1"] = "Delete Record Successfully";
            var list = eobj.tbl_publisher.ToList();
            return View("Publisher",list);
        }
        public ActionResult EditPublisher(int publisher_id)
        {
            var row = eobj.tbl_publisher.Where(x=>x.publisher_id == publisher_id).FirstOrDefault();
            return View(row);
        }
        [HttpPost]
        public ActionResult EditPublisher(tbl_publisher pmodel)
        {
            if(ModelState.IsValid==true)
            {
                if (eobj.tbl_publisher.Any(model => model.publisher_name.Equals(pmodel.publisher_name)))
                {
                    ViewBag.errormsg = "Pulisher Name Already Use..!!";
                }
                else
                {
                    tbl_publisher p = new tbl_publisher();
                    p.publisher_id = pmodel.publisher_id;
                    p.publisher_name = pmodel.publisher_name;
                    eobj.Entry(p).State = EntityState.Modified;
                    eobj.SaveChanges();
                    //  TempData["updatemsg"] = "Successfuly Updated !!!";
                    TempData["msg"] = "Successfuly Updated !!!";
                    return RedirectToAction("Publisher");
                }
              
            }
            return View();
        }

    }
}