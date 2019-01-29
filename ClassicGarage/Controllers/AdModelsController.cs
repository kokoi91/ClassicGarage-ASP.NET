using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassicGarage.DAL;
using ClassicGarage.Models;
using Microsoft.AspNet.Identity;

namespace ClassicGarage.Controllers
{
    public class AdModelsController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: AdModels
        public ActionResult Index()
        {
            var ads = db.Ads.Include(a => a.Car);


            return View(ads.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdModels adModels = db.Ads.Find(id);
            if (adModels == null)
            {
                return HttpNotFound();
            }
            return View(adModels);
        }

        public ActionResult Create()
        {
            var OwnerId = (int)Session["UserId"];

            ViewBag.CarID =

           new SelectList(from Car in db.Car
                           join Ad in db.Ads on Car.Id equals Ad.CarID into gj
                           from x in gj.DefaultIfEmpty()
                           where (Car.OwnerID == OwnerId && x.CarID == null)
                           select new
                           {
                               id = Car.Id,
                               Make = Car.Make
                           }, "id", "Make");

            return View();
        }

        // POST: AdModels/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CarID,IfActive,des")] AdModels adModels)
        {
            if (ModelState.IsValid)
            {
               
                adModels.IfActive = true;
                db.Ads.Add(adModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Car, "Id", "Make", adModels.ID);
            return View(adModels);
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAd()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdModels adModels = db.Ads.Find(id);
            if (adModels == null)
            {
                return HttpNotFound();
            }
            return View(adModels);
        }

        // POST: AdModels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdModels adModels = db.Ads.Find(id);
            db.Ads.Remove(adModels);
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
