using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Security;
using System.Web.Mvc;
using CourseWork.Models;
using System.IO;
using System.Web.WebPages;
using System.Data.Entity;

namespace CourseWork.Controllers
{
    public class AdminController : Controller
    {

        BetterHelpDbEntities db = new BetterHelpDbEntities();
        
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View(db.Funds);
        }

        /*[HttpPost]
        public ActionResult Index(string title, string goalAmount, string collectedAmount, string info, HttpPostedFileBase logo)
        {


            string fileName = System.IO.Path.GetFileName(logo.FileName);
            logo.SaveAs(Server.MapPath("~/images/" + fileName));

            using (BetterHelpDbEntities db = new BetterHelpDbEntities())
            {
                Fund fund = new Fund
                {
                    FundId = db.Funds.Select(x => x.FundId).Max() + 1,
                    Title = title,
                    GoalAmount = Convert.ToInt32(goalAmount),
                    CollectedAmount = String.IsNullOrEmpty(collectedAmount) ? 0 : Convert.ToInt32(collectedAmount),
                    Info = info,
                    LogoName = fileName
                };
                db.Funds.Add(fund);
                db.SaveChanges();
            }
            ViewData["isSuccess"] = "Фонд успешно добавлен.";
            return View();
        }*/

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string title, string goalAmount, string collectedAmount, string info, HttpPostedFileBase logo)
        {


            string fileName = System.IO.Path.GetFileName(logo.FileName);
            logo.SaveAs(Server.MapPath("~/images/" + fileName));

            using (BetterHelpDbEntities db = new BetterHelpDbEntities())
            {
                Fund fund = new Fund
                {
                    FundId = db.Funds.Select(x => x.FundId).Max() + 1,
                    Title = title,
                    GoalAmount = Convert.ToInt32(goalAmount),
                    CollectedAmount = String.IsNullOrEmpty(collectedAmount) ? 0 : Convert.ToInt32(collectedAmount),
                    Info = info,
                    LogoName = fileName
                };
                db.Funds.Add(fund);
                db.SaveChanges();
            }
            ViewData["isSuccess"] = true;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = db.Funds.Find(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            return View(fund);
        }


        [HttpPost]
        public ActionResult Edit(int id, string title, string goalAmount, string collectedAmount, string info, HttpPostedFileBase logo)
        {


            string fileName = logo != null ? System.IO.Path.GetFileName(logo.FileName) : null;

            if (!String.IsNullOrEmpty(fileName))
            {
                logo.SaveAs(Server.MapPath("~/images/" + fileName));
            }

            Fund fund = db.Funds.Find(id);
            fund.Title = title;
            fund.GoalAmount = Convert.ToInt32(goalAmount);
            fund.CollectedAmount = String.IsNullOrEmpty(collectedAmount) ? 0 : Convert.ToInt32(collectedAmount);
            fund.Info = info;

            if (!String.IsNullOrEmpty(fileName))
            {
                logo.SaveAs(Server.MapPath("~/images/" + fileName));
                fund.LogoName = fileName;
            }           

            db.Entry(fund).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = db.Funds.Find(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            return View(fund);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Fund fund = db.Funds.Find(id);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fund.LogoName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.Funds.Remove(fund);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            Admin admin = null;
            if (ModelState.IsValid)
            {
                using(BetterHelpDbEntities db = new BetterHelpDbEntities())
                {
                    admin = db.Admins.FirstOrDefault(a => a.Name == model.Name && a.Password == model.Password);
                }

                if (admin != null)
                {
                    try
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    ViewBag.adminName = model.Name;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

    }
}