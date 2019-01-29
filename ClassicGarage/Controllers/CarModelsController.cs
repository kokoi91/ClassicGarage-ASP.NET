using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassicGarage.DAL;
using ClassicGarage.Models;
using Microsoft.AspNet.Identity;

namespace ClassicGarage.Controllers
{
    public class CarModelsController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: CarModels
        public ActionResult Index()
        {
            var OwnerId = (int)Session["UserId"];
            var car = db.Car.Include(c => c.Owner).Where(c => c.OwnerID == OwnerId);
            
            return View(car.ToList());
        }

        // GET: CarModels/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CarModels carModels = db.Car.Find(id);
            if (carModels == null)
            {
                return HttpNotFound();

            }
          
            var e_mail = User.Identity.GetUserName();
            //var query = db.Reapirs.Where(s => s. == e_mail).Select(s => s.ID).FirstOrDefault();

            ViewBag.ReapirList = db.Reapirs.Where(s => s.CarID == carModels.Id).ToList();

            return View(carModels);
        }

        // GET: CarModels/Create
        public ActionResult Create()
        {
             var query = (int)Session["UserId"];
            if (query == 0) return RedirectToAction("Create", "OwnerModels");

            ViewBag.Id = new SelectList(db.Ads, "ID", "ID");
            ViewBag.OwnerID = new SelectList(db.Owner, "ID", "FirstName");
            ViewBag.Id = new SelectList(db.Reapirs, "Id", "Name");
            return View();
        }

        // POST: CarModels/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Make,Model,Year,VIN,Name,Picture,PurchaseDate,PurchaseAmount,SalesDate,SalesAmount,OwnerID")] CarModels carModels)
        {
            var path = Server.MapPath("~\\Zdjecia\\" + User.Identity.GetUserName()+"\\");
            var path2 = "~\\Zdjecia\\" + User.Identity.GetUserName() + "\\";
            bool folderExists = Directory.Exists(path);

            if (!folderExists)
                Directory.CreateDirectory(path);



            if (ModelState.IsValid)
            {

                HttpPostedFileBase postedFile = Request.Files["Picture"];

                if (postedFile != null)
                {
                    var fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_")+ postedFile.FileName;

                    postedFile.SaveAs(path + fileName);
                    carModels.Picture = path2+ fileName;
                }

                    db.Car.Add(carModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Ads, "ID", "ID", carModels.Id);
            ViewBag.OwnerID = new SelectList(db.Owner, "ID", "FirstName", carModels.OwnerID);
            ViewBag.Id = new SelectList(db.Reapirs, "Id", "Name", carModels.Id);
            return View(carModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCar()
        {

            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModels carModels = db.Car.Find(id);
            if (carModels == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Ads, "ID", "ID", carModels.Id);
            ViewBag.OwnerID = new SelectList(db.Owner, "ID", "FirstName", carModels.OwnerID);
            ViewBag.Id = new SelectList(db.Reapirs, "Id", "Name", carModels.Id);
            return View(carModels);
        }

        // POST: CarModels/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Make,Model,Year,VIN,Name,Picture,PurchaseDate,PurchaseAmount,SalesDate,SalesAmount,OwnerID")] CarModels carModels)
        {
            if (ModelState.IsValid)
            {
                carModels.OwnerID = (int)Session["UserId"];
                carModels.Picture = Request.Form.Get("picture");
                db.Entry(carModels).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Ads, "ID", "ID", carModels.Id);
            ViewBag.OwnerID = new SelectList(db.Owner, "ID", "FirstName", carModels.OwnerID);
            ViewBag.Id = new SelectList(db.Reapirs, "Id", "Name", carModels.Id);
            return View(carModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCar()
        {

            int? id = Int32.Parse(Request.Form.Get("id"));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModels carModels = db.Car.Find(id);
            if (carModels == null)
            {
                return HttpNotFound();
            }
            return View(carModels);
        }

        // POST: CarModels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CarModels carModels = db.Car.Find(id);

            db.Reapirs.Where(i => i.CarID == carModels.Id).ToList().ForEach(
                d =>
                {
                db.Parts.Where(i => i.RepairID == d.Id).ToList().ForEach(p => db.Parts.Remove(p));
                    db.SaveChanges();
                  //  Debug.WriteLine("Repair " + d.Id);
                   db.Reapirs.Remove(d);
                   db.SaveChanges();
                }

            );
            Debug.WriteLine("Car  " + carModels.Id);
            db.Ads.Where(i => i.CarID == carModels.Id).ToList().ForEach(p => db.Ads.Remove(p));
            db.SaveChanges();
            db.Car.Remove(carModels);
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
