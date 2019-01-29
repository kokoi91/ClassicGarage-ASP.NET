using System;

using Microsoft.AspNet.Identity;
using ClassicGarage.DAL;
using ClassicGarage.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace ClassicGarage.Controllers
{
    public class HomeController : Controller
    {
        private GarageContext db = new GarageContext();


        public ActionResult Index()
        {
            var e_mail = User.Identity.GetUserName();
            var query = db.Owner.Where(s => s.Email == e_mail).Select(s => s.ID).FirstOrDefault();
            Session["UserId"] = query;
          //  var car = db.Car.Include(p => p.Owner);
            // var car = db.Car.Include(p => p.Owner).Where(p => p.OwnerID == query);
            //Console.WriteLine(string.Join(", ", car));
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}