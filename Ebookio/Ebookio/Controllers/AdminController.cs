using Ebookio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
            if (cookie != null)
            {
                ViewBag.admin_emailid = cookie["admin_emailid"].ToString();
                ViewBag.admin_password = cookie["admin_password"].ToString();
            }
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(tbl_admin a)
        {
            HttpCookie cookie = new HttpCookie("AdminLoginDetails");
            if (ModelState.IsValid == true)
            {

                if (a.RememberMe == true)
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

                var adminauth = eobj.tbl_admin.Where(model => model.admin_emailid == a.admin_emailid && model.admin_password == a.admin_password).FirstOrDefault();
                if (adminauth == null)
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
            if (Session["admin_username"] == null)
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
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                ViewBag.msg = TempData["msg"] as string;
                using (var ctx = new ebookioEntities())
                {

                    var publisherlist = ctx.tbl_publisher.SqlQuery("Select * from tbl_publisher").ToList<tbl_publisher>();
                    return View(publisherlist);

                }
            }


        }
        public ActionResult AddPublisher()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult AddPublisher(tbl_publisher pmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_publisher p = new tbl_publisher();
                    if (eobj.tbl_publisher.Any(model => model.publisher_name.Equals(pmodel.publisher_name)))
                    {
                        TempData["pubuse"] = "Publisher Name Already Use..!!";
                    }
                    else
                    {
                        p.publisher_name = pmodel.publisher_name;
                        eobj.tbl_publisher.Add(p);
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Inserted !!!";
                        return RedirectToAction("Publisher");
                    }


                }
                return View();
            }

        }
        public ActionResult DeletePublisher(int deleteid)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var deleterecord = eobj.tbl_publisher.Where(x => x.publisher_id == deleteid).First();
                eobj.tbl_publisher.Remove(deleterecord);
                eobj.SaveChanges();
                TempData["msg"] = "Successfuly Deleted !!!";
                return RedirectToAction("Publisher");
            }

        }
        public ActionResult EditPublisher(int publisher_id)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var row = eobj.tbl_publisher.Where(x => x.publisher_id == publisher_id).FirstOrDefault();
                return View(row);
            }

        }
        [HttpPost]
        public ActionResult EditPublisher(tbl_publisher pmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    if (eobj.tbl_publisher.Any(model => model.publisher_name.Equals(pmodel.publisher_name)))
                    {
                        TempData["pubuse"] = "Publisher Name Already Use..!!";
                    }
                    else
                    {
                        tbl_publisher p = new tbl_publisher();
                        p.publisher_id = pmodel.publisher_id;
                        p.publisher_name = pmodel.publisher_name;
                        eobj.Entry(p).State = EntityState.Modified;
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Updated !!!";
                        return RedirectToAction("Publisher");
                    }

                }
                return View();
            }

        }

        //--------------------------------------------------------------------------------------------
        //----------------------------------------------Language--------------------------------------
        //--------------------------------------------------------------------------------------------

        public ActionResult Language()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                ViewBag.msg = TempData["msg"] as string;
                using (var ctx = new ebookioEntities())
                {

                    var languagelist = ctx.tbl_language.SqlQuery("Select * from tbl_language").ToList<tbl_language>();
                    return View(languagelist);

                }
            }


        }
        public ActionResult AddLanguage()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult AddLanguage(tbl_language lmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_language l = new tbl_language();
                    if (eobj.tbl_language.Any(model => model.language_name.Equals(lmodel.language_name)))
                    {
                        TempData["lanuse"] = "Language Name Already Use..!!";
                    }
                    else
                    {
                        l.language_name = lmodel.language_name;
                        eobj.tbl_language.Add(l);
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Inserted !!!";
                        return RedirectToAction("Language");
                    }
                }
                return View();
            }

        }
        public ActionResult DeleteLanguage(int deleteid)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var deleterecord = eobj.tbl_language.Where(x => x.language_id == deleteid).First();
                eobj.tbl_language.Remove(deleterecord);
                eobj.SaveChanges();
                TempData["msg"] = "Successfuly Deleted !!!";
                return RedirectToAction("Language");
            }

        }
        public ActionResult EditLanguage(int language_id)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var row = eobj.tbl_language.Where(x => x.language_id == language_id).FirstOrDefault();
                return View(row);
            }

        }
        [HttpPost]
        public ActionResult EditLanguage(tbl_language lmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    if (eobj.tbl_language.Any(model => model.language_name.Equals(lmodel.language_name)))
                    {
                        TempData["lanuse"] = "Language Name Already Use..!!";
                    }
                    else
                    {
                        tbl_language l = new tbl_language();
                        l.language_id = lmodel.language_id;
                        l.language_name = lmodel.language_name;
                        eobj.Entry(l).State = EntityState.Modified;
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Updated !!!";
                        return RedirectToAction("Language");
                    }
                }
                return View();
            }

        }

        //--------------------------------------------------------------------------------------------
        //----------------------------------------------Author--------------------------------------
        //--------------------------------------------------------------------------------------------

        public ActionResult Author()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                ViewBag.msg = TempData["msg"] as string;
                using (var ctx = new ebookioEntities())
                {
                    var authorlist = ctx.tbl_author.SqlQuery("Select * from tbl_author").ToList<tbl_author>();
                    return View(authorlist);

                }
            }
        }
        public ActionResult AddAuthor()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult AddAuthor(tbl_author amodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_author a = new tbl_author();
                    if (eobj.tbl_author.Any(model => model.author_name.Equals(amodel.author_name)))
                    {
                        TempData["authoruse"] = "Author Name Already Use..!!";
                    }
                    else
                    {
                        a.author_name = amodel.author_name;
                        eobj.tbl_author.Add(a);
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Inserted !!!";
                        return RedirectToAction("Author");
                    }
                }
                return View();
            }
        }
        public ActionResult DeleteAuthor(int deleteid)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var deleterecord = eobj.tbl_author.Where(x => x.author_id == deleteid).First();
                eobj.tbl_author.Remove(deleterecord);
                eobj.SaveChanges();
                TempData["msg"] = "Successfuly Deleted !!!";
                return RedirectToAction("Author");
            }

        }
        public ActionResult EditAuthor(int author_id)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var row = eobj.tbl_author.Where(x => x.author_id == author_id).FirstOrDefault();
                return View(row);
            }

        }
        [HttpPost]
        public ActionResult EditAuthor(tbl_author amodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    if (eobj.tbl_author.Any(model => model.author_name.Equals(amodel.author_name)))
                    {
                        TempData["authoruse"] = "Author Name Already Use..!!";
                    }
                    else
                    {
                        tbl_author a = new tbl_author();
                        a.author_id = amodel.author_id;
                        a.author_name = amodel.author_name;
                        eobj.Entry(a).State = EntityState.Modified;
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Updated !!!";
                        return RedirectToAction("Author");
                    }
                }
                return View();
            }

        }


        //--------------------------------------------------------------------------------------------
        //----------------------------------------------Book--------------------------------------
        //--------------------------------------------------------------------------------------------

        public ActionResult Book()
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                ViewBag.msg = TempData["msg"] as string;
                var booklist = eobj.tbl_book.ToList();
                return View(booklist);
            }
        }

        public ActionResult AddBook()
        {
            var publisherlist = eobj.tbl_publisher.ToList();
            ViewBag.Publisherlist = new SelectList(publisherlist, "publisher_id", "publisher_name");

            var authorlist = eobj.tbl_author.ToList();
            ViewBag.Authorlist = new SelectList(authorlist, "author_id", "author_name");

            var lanlist = eobj.tbl_language.ToList();
            ViewBag.Lanlist = new SelectList(lanlist, "language_id", "language_name");
            return View();

        }

        [HttpPost]
        public ActionResult AddBook(tbl_book bmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {

                if (ModelState.IsValid == true)
                {
                    if (eobj.tbl_book.Any(model => model.book_title.Equals(bmodel.book_title)))
                    {
                        TempData["bookuse"] = "Book Name Already Use..!!";
                    }
                    if (eobj.tbl_book.Any(model => model.isbn.Equals(bmodel.isbn)))
                    {
                        TempData["isbnuse"] = "ISBN - 13 Already Use..!!";
                    }
                    else
                    {
                        tbl_book b = new tbl_book();
                        b.is_free = bmodel.is_free;
                        b.book_title = bmodel.book_title;
                        b.isbn = bmodel.isbn;
                        b.publish_date = bmodel.publish_date;
                        b.no_of_pages = bmodel.no_of_pages;
                        b.publisher_id = bmodel.publisher_id;
                        b.author_id = bmodel.author_id;
                        b.language_id = bmodel.language_id;

                        //-------start Image Upload-------

                        string imgname = Path.GetFileNameWithoutExtension(bmodel.ImageUpload.FileName);
                        string ext = Path.GetExtension(bmodel.ImageUpload.FileName);
                        HttpPostedFileBase image_new = bmodel.ImageUpload;

                        if (ext.ToLower() == ".jpg" || ext.ToLower() == ".png" || ext.ToLower() == ".jpeg")
                        {
                            imgname += ext;
                            bmodel.cover_image = "~/Document/Image/" + imgname;
                            imgname = Path.Combine(Server.MapPath("~/Document/Image/") + imgname);
                            bmodel.ImageUpload.SaveAs(imgname);
                        }
                        else
                        {
                            ViewBag.format = "Select Only PNG,JPEG,JPG Format";

                            ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");

                            ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");

                            ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");

                            return View();

                        }

                       

                        //---------End Image Upload----

                        b.real_price = bmodel.real_price;
                        b.sell_price = bmodel.sell_price;

                        //-------start Pdf Upload-------

                        string filename = Path.GetFileNameWithoutExtension(bmodel.PdfUpload.FileName);
                        string extension = Path.GetExtension(bmodel.PdfUpload.FileName);

                        HttpPostedFileBase pdf_new = bmodel.PdfUpload;

                        if (extension.ToLower() == ".pdf")
                        {
                            filename += extension;
                            bmodel.upload_pdf = "~/Document/Pdf/" + filename;
                            filename = Path.Combine(Server.MapPath("~/Document/Pdf/") + filename);
                            bmodel.PdfUpload.SaveAs(filename);
                        }
                        else
                        {
                            ViewBag.pdfformat = "Select Only PDF Format";

                            ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");

                            ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");

                            ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");

                            return View();
                        }
                        //------End pdf Upload------

                        b.cover_image = bmodel.cover_image;
                        b.upload_pdf = bmodel.upload_pdf;

                        eobj.tbl_book.Add(b);
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfully Inserted !!!";

                        return RedirectToAction("Book");
                    }
                }
            }
            var publisherlist = eobj.tbl_publisher.ToList();
            ViewBag.Publisherlist = new SelectList(publisherlist, "publisher_id", "publisher_name");

            var authorlist = eobj.tbl_author.ToList();
            ViewBag.Authorlist = new SelectList(authorlist, "author_id", "author_name");

            var lanlist = eobj.tbl_language.ToList();
            ViewBag.Lanlist = new SelectList(lanlist, "language_id", "language_name");

            //return RedirectToAction("AddBook");
            return View();
        }
    }
}