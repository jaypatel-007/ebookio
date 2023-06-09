﻿using Ebookio.Models;
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
        //----------------------------------------------Category--------------------------------------
        //--------------------------------------------------------------------------------------------

        public ActionResult Category()
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
                    var categorylist = ctx.tbl_category.SqlQuery("Select * from tbl_category").ToList<tbl_category>();
                    return View(categorylist);

                }
            }
        }
        public ActionResult AddCategory()
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
        public ActionResult AddCategory(tbl_category cmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_category c = new tbl_category();
                    if (eobj.tbl_category.Any(model => model.category_name.Equals(cmodel.category_name)))
                    {
                        TempData["categoryuse"] = "Category Name Already Use..!!";
                    }
                    else
                    {
                        c.category_name = cmodel.category_name;
                        eobj.tbl_category.Add(c);
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Inserted !!!";
                        return RedirectToAction("Category");
                    }
                }
                return View();
            }

        }

        public ActionResult DeleteCategory(int deleteid)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var deleterecord = eobj.tbl_category.Where(x => x.category_id == deleteid).First();
                eobj.tbl_category.Remove(deleterecord);
                eobj.SaveChanges();
                TempData["msg"] = "Successfuly Deleted !!!";
                return RedirectToAction("Category");
            }

        }
        public ActionResult EditCategory(int category_id)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var row = eobj.tbl_category.Where(x => x.category_id == category_id).FirstOrDefault();
                ViewBag.category_name = row.category_name;
                return View(row);
            }

        }

        [HttpPost]
        public ActionResult EditCategory(tbl_category cmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    if (eobj.tbl_category.Any(model => model.category_name.Equals(cmodel.category_name)))
                    {
                        TempData["categoryuse"] = "Category Name Already Use..!!";
                    }
                    else
                    {
                        tbl_category c = new tbl_category();
                        c.category_id = cmodel.category_id;
                        c.category_name = cmodel.category_name;
                        eobj.Entry(c).State = EntityState.Modified;
                        eobj.SaveChanges();
                        TempData["msg"] = "Successfuly Updated !!!";
                        return RedirectToAction("Category");
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

            var catlist = eobj.tbl_category.ToList();
            ViewBag.catlist = new SelectList(catlist, "category_id", "category_name");

            return View();

        }

        [HttpPost]
        public ActionResult AddBook(HttpPostedFileBase file, HttpPostedFileBase Pdffile, tbl_book bmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    tbl_book b = new tbl_book();

                    if (bmodel.is_free=="paid")
                    {
                        if(bmodel.real_price==null)
                        {
                            ViewBag.realprice = "please Enter Real Price !! ";
                        }
                        if(bmodel.sell_price==null)
                        {
                            ViewBag.sellprice =  "please Enter Sell Price !! ";
                        }
                        else
                        {
                            
                            if (eobj.tbl_book.Any(model => model.book_title.Equals(bmodel.book_title)))
                            {
                                TempData["bookuse"] = "Book Name Already Use..!!";
                            }
                            if (eobj.tbl_book.Any(model => model.isbn.Equals(bmodel.isbn)))
                            {
                                TempData["isbnuse"] = "ISBN number Already Use..!!";
                            }
                            else
                            {
                                b.is_free = bmodel.is_free;
                                b.book_title = bmodel.book_title;
                                b.isbn = bmodel.isbn;
                                b.publish_date = bmodel.publish_date;
                                b.no_of_pages = bmodel.no_of_pages;
                                b.publisher_id = bmodel.publisher_id;
                                b.author_id = bmodel.author_id;
                                b.language_id = bmodel.language_id;
                                b.category_id = bmodel.category_id;
                                b.real_price = bmodel.real_price;
                                b.sell_price = bmodel.sell_price;

                                string filename = Path.GetFileName(file.FileName);
                                string _filename = DateTime.Now.ToString("yymmssff") + filename;
                                string extension = Path.GetExtension(file.FileName);
                                string path = Path.Combine(Server.MapPath("~/Document/Image/"), _filename);
                                bmodel.cover_image = "~/Document/Image/" + _filename;

                                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                                {
                                    if (file.ContentLength <= 1000000)
                                    {
                                        b.cover_image = bmodel.cover_image;
                                        file.SaveAs(path);
                                        eobj.tbl_book.Add(bmodel);
                                    }
                                    else
                                    {
                                        ViewBag.size = "Select image Size big";
                                        ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                        ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                        ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                        ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.format = "Select Only PNG,JPEG,JPG Format";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                                if (eobj.SaveChanges() > 0)
                                {
                                    TempData["msg"] = "Successfuly Inserted !!!";
                                    return RedirectToAction("Book");
                                }
                            }
                        }
                    }
                    else
                    {

                            if (eobj.tbl_book.Any(model => model.book_title.Equals(bmodel.book_title)))
                            {
                                TempData["bookuse"] = "Book Name Already Use..!!";
                            }
                            if (eobj.tbl_book.Any(model => model.isbn.Equals(bmodel.isbn)))
                            {
                                TempData["isbnuse"] = "ISBN number Already Use..!!";
                            }
                            else
                            {
                                b.is_free = bmodel.is_free;
                                b.book_title = bmodel.book_title;
                                b.isbn = bmodel.isbn;
                                b.publish_date = bmodel.publish_date;
                                b.no_of_pages = bmodel.no_of_pages;
                                b.publisher_id = bmodel.publisher_id;
                                b.author_id = bmodel.author_id;
                                b.language_id = bmodel.language_id;
                                b.category_id = bmodel.category_id;

                                string filename = Path.GetFileName(file.FileName);
                                string _filename = DateTime.Now.ToString("yymmssff") + filename;
                                string extension = Path.GetExtension(file.FileName);
                                string path = Path.Combine(Server.MapPath("~/Document/Image/"), _filename);
                                bmodel.cover_image = "~/Document/Image/" + _filename;

                                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                                {
                                    if (file.ContentLength <= 1000000)
                                    {
                                        b.cover_image = bmodel.cover_image;
                                        file.SaveAs(path);
                                       // eobj.tbl_book.Add(bmodel);
                                    }
                                    else
                                    {
                                        ViewBag.size = "Select image Size big";
                                        ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                        ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                        ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                        ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.format = "Select Only PNG,JPEG,JPG Format";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                                //pdf
                                string pdffilename = Path.GetFileName(Pdffile.FileName);
                                string _pdffilename = DateTime.Now.ToString("yymmssff") + pdffilename;
                                string pdfextension = Path.GetExtension(Pdffile.FileName);
                                string pdfpath = Path.Combine(Server.MapPath("~/Document/Pdf/"), _pdffilename);
                                bmodel.upload_pdf = "~/Document/Pdf/" + _pdffilename;

                                if (pdfextension.ToLower() == ".pdf")
                                {
                                    if (Pdffile.ContentLength <= 1000000)
                                    {
                                        b.upload_pdf = bmodel.upload_pdf;
                                        Pdffile.SaveAs(pdfpath);
                                       // eobj.tbl_book.Add(bmodel);
                                    }
                                    else
                                    {
                                        ViewBag.pdfsize = "Select Pdf Size big";
                                        ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                        ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                        ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                        ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                        return View();
                                    }
                                }
                                else
                                {
                                    ViewBag.pdfformat = "Select Only PNG,JPEG,JPG Format";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                                eobj.tbl_book.Add(bmodel);
                                if (eobj.SaveChanges() > 0)
                                {
                                    TempData["msg"] = "Successfuly Inserted !!!";
                                    return RedirectToAction("Book");
                                }
                            }

                    }
                    
                }
                var publisherlist = eobj.tbl_publisher.ToList();
                ViewBag.Publisherlist = new SelectList(publisherlist, "publisher_id", "publisher_name");

                var authorlist = eobj.tbl_author.ToList();
                ViewBag.Authorlist = new SelectList(authorlist, "author_id", "author_name");

                var lanlist = eobj.tbl_language.ToList();
                ViewBag.Lanlist = new SelectList(lanlist, "language_id", "language_name");

                var catlist = eobj.tbl_category.ToList();
                ViewBag.catlist = new SelectList(catlist, "category_id", "category_name");

                return View();
            }

        }
        public ActionResult EditBook(int book_id)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var row = eobj.tbl_book.Where(x => x.book_id == book_id).FirstOrDefault();
                Session["isfree"] = row.is_free;
                Session["pagno"] = row.no_of_pages;
                Session["cimage"] = row.cover_image;
                Session["pdfu"] = row.upload_pdf;


                var publisherlist = eobj.tbl_publisher.ToList();
                ViewBag.Publisherlist = new SelectList(publisherlist, "publisher_id", "publisher_name");

                var authorlist = eobj.tbl_author.ToList();
                ViewBag.Authorlist = new SelectList(authorlist, "author_id", "author_name");

                var lanlist = eobj.tbl_language.ToList();
                ViewBag.Lanlist = new SelectList(lanlist, "language_id", "language_name");

                var catlist = eobj.tbl_category.ToList();
                ViewBag.catlist = new SelectList(catlist, "category_id", "category_name");

                return View(row);
            }
        }
        [HttpPost]
        public ActionResult EditBook(HttpPostedFileBase file, HttpPostedFileBase Pdffile, tbl_book bmodel)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    if (Session["isfree"].ToString() == "paid")
                    {
                        if (file != null)
                        {
                            string filename = Path.GetFileName(file.FileName);
                            string _filename = DateTime.Now.ToString("yymmssff") + filename;
                            string extension = Path.GetExtension(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Document/Image/"), _filename);
                            bmodel.cover_image = "~/Document/Image/" + _filename;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                            {
                                if (file.ContentLength <= 1000000)
                                {
                                    bmodel.is_free = Session["isfree"].ToString();
                                    eobj.Entry(bmodel).State = EntityState.Modified;

                                    string oldimagepath = Request.MapPath(Session["cimage"].ToString());

                                    if (eobj.SaveChanges() > 0)
                                    {
                                        file.SaveAs(path);
                                        if (System.IO.File.Exists(oldimagepath))
                                        {
                                            System.IO.File.Delete(oldimagepath);
                                        }
                                        TempData["msg"] = "Successfuly Updated !!!";
                                        return RedirectToAction("Book");
                                    }
                                }
                                else
                                {
                                    ViewBag.size = "Select image Size big";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.format = "Select Only PNG,JPEG,JPG Format";
                                ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                return View();
                            }
                        }

                        bmodel.cover_image = Session["cimage"].ToString();
                        bmodel.is_free = Session["isfree"].ToString();
                        eobj.Entry(bmodel).State = EntityState.Modified;
                        if (eobj.SaveChanges() > 0)
                        {
                            TempData["msg"] = "Successfuly Updated !!!";
                            return RedirectToAction("Book");
                        }
                    }
                    if (Session["isfree"].ToString() == "free")
                    {
                        if (file != null)
                        {
                            string filename = Path.GetFileName(file.FileName);
                            string _filename = DateTime.Now.ToString("yymmssff") + filename;
                            string extension = Path.GetExtension(file.FileName);
                            string path = Path.Combine(Server.MapPath("~/Document/Image/"), _filename);
                            bmodel.cover_image = "~/Document/Image/" + _filename;

                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                            {
                                if (file.ContentLength <= 1000000)
                                {
                                    bmodel.is_free = Session["isfree"].ToString();
                                    bmodel.upload_pdf = Session["pdfu"].ToString();
                                    eobj.Entry(bmodel).State = EntityState.Modified;

                                    string oldimagepath = Request.MapPath(Session["cimage"].ToString());

                                    if (eobj.SaveChanges() > 0)
                                    {
                                        file.SaveAs(path);
                                        if (System.IO.File.Exists(oldimagepath))
                                        {
                                            System.IO.File.Delete(oldimagepath);
                                        }
                                        TempData["msg"] = "Successfuly Updated !!!";
                                        return RedirectToAction("Book");
                                    }
                                }
                                else
                                {
                                    ViewBag.size = "Select image Size big";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.format = "Select Only PNG,JPEG,JPG Format";
                                ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                return View();
                            }
                        }
                        if (Pdffile != null)
                        {
                            string pdffilename = Path.GetFileName(Pdffile.FileName);
                            string _pdffilename = DateTime.Now.ToString("yymmssff") + pdffilename;
                            string pdfextension = Path.GetExtension(Pdffile.FileName);
                            string pdfpath = Path.Combine(Server.MapPath("~/Document/Pdf/"), _pdffilename);
                            bmodel.upload_pdf = "~/Document/Pdf/" + _pdffilename;

                            if (pdfextension.ToLower() == ".pdf")
                            {
                                if (Pdffile.ContentLength <= 1000000)
                                {
                                    bmodel.is_free = Session["isfree"].ToString();
                                    bmodel.cover_image = Session["cimage"].ToString();
                                    eobj.Entry(bmodel).State = EntityState.Modified;

                                    string oldpdfpath = Request.MapPath(Session["pdfu"].ToString());

                                    if (eobj.SaveChanges() > 0)
                                    {
                                        Pdffile.SaveAs(pdfpath);
                                        if (System.IO.File.Exists(oldpdfpath))
                                        {
                                            System.IO.File.Delete(oldpdfpath);
                                        }
                                        TempData["msg"] = "Successfuly Updated !!!";
                                        return RedirectToAction("Book");
                                    }
                                }
                                else
                                {
                                    ViewBag.size = "Select Pdf Size big";
                                    ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                    ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                    ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                    ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.format = "Select Only Pdf Format";
                                ViewBag.Publisherlist = new SelectList(eobj.tbl_publisher.ToList(), "publisher_id", "publisher_name");
                                ViewBag.Authorlist = new SelectList(eobj.tbl_author.ToList(), "author_id", "author_name");
                                ViewBag.Lanlist = new SelectList(eobj.tbl_language.ToList(), "language_id", "language_name");
                                ViewBag.catlist = new SelectList(eobj.tbl_category.ToList(), "category_id", "category_name");
                                return View();
                            }
                        }

                        bmodel.cover_image = Session["cimage"].ToString();
                        bmodel.upload_pdf = Session["pdfu"].ToString();
                        bmodel.is_free = Session["isfree"].ToString();
                        eobj.Entry(bmodel).State = EntityState.Modified;
                        if (eobj.SaveChanges() > 0)
                        {
                            TempData["msg"] = "Successfuly Updated !!!";
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

                var catlist = eobj.tbl_category.ToList();
                ViewBag.catlist = new SelectList(catlist, "category_id", "category_name");

                return View();
            }

        }
        public ActionResult DeleteBook(int deleteid)
        {
            if (Session["admin_username"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            else
            {
                var deleterecord = eobj.tbl_book.Where(x => x.book_id == deleteid).First();
                string currentImg = Request.MapPath(deleterecord.cover_image);
                string currentPdf = Request.MapPath(deleterecord.upload_pdf);

                //remove image
                if (System.IO.File.Exists(currentImg))
                {
                    System.IO.File.Delete(currentImg);
                }

                //remove pdf
                if (System.IO.File.Exists(currentPdf))
                {
                    System.IO.File.Delete(currentPdf);
                }

                eobj.tbl_book.Remove(deleterecord);
                eobj.SaveChanges();
                TempData["msg"] = "Successfuly Deleted !!!";
                return RedirectToAction("Book");
            }

        }
    }
}