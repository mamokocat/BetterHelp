using CourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseWork.Controllers
{
    public class HomeController : Controller
    {
        private BetterHelpDbEntities db;
        public HomeController()
        {
            db = new BetterHelpDbEntities();
        }
        public ActionResult Index()
        {
            return View(db.Funds);
        }

        public ActionResult Funds()
        {
            return View(db.Funds);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Donate(int fundId = 0)
        {
            
            var fundsIds = new List<SelectListItem> { };
            foreach (var fund in db.Funds)
            {
                fundsIds.Add(new SelectListItem { Text = fund.Title, Value = $"{fund.FundId}", Selected = fund.FundId == fundId ? true : false });
            }
            var fundsList = new SelectList(fundsIds, fundId.ToString());
            
            ViewBag.fundList = fundsList;
            //ViewBag.FundName = db.Funds.ToList<Fund>().Find(fund => fund.FundId == fundId).Title;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Donate([Bind(Include = "Id,FundId,Name,Amount,Message")] Philanthropist philanthropist)
        {
            if (ModelState.IsValid)
            {
                var fundCollectedAmount = db.Funds.Find(philanthropist.FundId);
                fundCollectedAmount.CollectedAmount = fundCollectedAmount.CollectedAmount + philanthropist.Amount;
                try
                {
                    db.Philanthropists.Add(philanthropist);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                db.SaveChanges();
                return View("ThankYouPage");
            }

            return RedirectToAction("Donate", "Home", new{ fundId = philanthropist.FundId });
        }

        public ActionResult ThankYouPage()
        {
            return View();
        }
    }
}