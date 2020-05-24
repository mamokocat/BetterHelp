using CourseWork.Models;
using CourseWork.Controllers;
using System.Web.Mvc;
using NUnit.Framework;


namespace CourseWork.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestMethod1()
        {
            var h = new HomeController();
            var db = new BetterHelpDbEntities();
            var ph = new Philanthropist { Amount = 100, FundId = 1, Id = 10, Name = "Андрей", Message = "" };

            var result = h.Donate(ph);



            Assert.IsTrue(result is ViewResult);
           
            
        }

        [Test]
        public void Test()
        {
            var adminController = new AdminController();
            var adminModel = new LoginModel { Name = "admin1", Password = "password1" };

            var result = adminController.Login(adminModel);
            Assert.IsTrue(result is RedirectToRouteResult);
        }
    }
}