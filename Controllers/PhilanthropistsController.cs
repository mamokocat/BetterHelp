using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseWork.Models;

namespace CourseWork.Controllers
{
    public class PhilanthropistsController : Controller
    {
        private BetterHelpDbEntities db = new BetterHelpDbEntities();

        // GET: Philanthropists
        public ActionResult Index()
        {
            var philanthropists = db.Philanthropists.Include(p => p.Fund);
            return View(philanthropists.ToList());
        }

        // GET: Philanthropists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Philanthropist philanthropist = db.Philanthropists.Find(id);
            if (philanthropist == null)
            {
                return HttpNotFound();
            }
            return View(philanthropist);
        }

        // GET: Philanthropists/Create
        public ActionResult Create()
        {
            ViewBag.FundId = new SelectList(db.Funds, "FundId", "Title");
            return View();
        }

        // POST: Philanthropists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FundId,Name,Amount,Message")] Philanthropist philanthropist)
        {
            if (ModelState.IsValid)
            {
                db.Philanthropists.Add(philanthropist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FundId = new SelectList(db.Funds, "FundId", "Title", philanthropist.FundId);
            return View(philanthropist);
        }

        // GET: Philanthropists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Philanthropist philanthropist = db.Philanthropists.Find(id);
            if (philanthropist == null)
            {
                return HttpNotFound();
            }
            ViewBag.FundId = new SelectList(db.Funds, "FundId", "Title", philanthropist.FundId);
            return View(philanthropist);
        }

        // POST: Philanthropists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FundId,Name,Amount,Message")] Philanthropist philanthropist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(philanthropist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FundId = new SelectList(db.Funds, "FundId", "Title", philanthropist.FundId);
            return View(philanthropist);
        }

        // GET: Philanthropists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Philanthropist philanthropist = db.Philanthropists.Find(id);
            if (philanthropist == null)
            {
                return HttpNotFound();
            }
            return View(philanthropist);
        }

        // POST: Philanthropists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Philanthropist philanthropist = db.Philanthropists.Find(id);
            db.Philanthropists.Remove(philanthropist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
