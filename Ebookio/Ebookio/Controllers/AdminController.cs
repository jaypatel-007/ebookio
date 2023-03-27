using Ebookio.Models;
using System;
using System.Collections.Generic;
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
    }
}