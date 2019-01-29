using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClassicGarage.DAL;
using ClassicGarage.Models;
using Microsoft.AspNet.Identity;

namespace ClassicGarage.Controllers
{
    public class RepairModelsController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: RepairModels
        public ActionResult Index()
        {
            int OwnerId = (int)Session["UserId"];

            var reapirs = db.Reapirs.Include(r => r.Car).Where(r => r.Car.OwnerID == OwnerId);
            return View(reapirs.ToList());
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
            RepairModels repairModels = db.Reapirs.Find(id);
            ViewBag.PartList = db.Parts.Where(i => i.RepairID == id);
            if (repairModels == null)
            {
                return HttpNotFound();
            }
            return View(repairModels);
            
        }

        // GET: RepairModels/Create
        public ActionResult Create()
        {
            var ownerId = (int)Session["UserId"];
            ViewBag.CarID = new SelectList(db.Car.Where(c => c.OwnerID == ownerId), "Id", "Make");
            return View();
        }

        // POST: RepairModels/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CarID,Name,Description,RepairAmount")] RepairModels repairModels)
        {
            if (ModelState.IsValid)
            {
                repairModels.RepairAmount = 0;
                db.Reapirs.Add(repairModels);


                db.SaveChanges();
                return RedirectToAction("Index");
            }
          

            ViewBag.Id = new SelectList(db.Car, "Id", "Make", repairModels.Id);
            return View(repairModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateForCar()
        {
            int carId = Int32.Parse( Request.Form.Get("id"));
            int ownerId = (int)Session["UserId"];

            ViewBag.Car = db.Car.Find(carId);

            ViewBag.CarID = new SelectList(db.Car.Where(c => c.OwnerID == ownerId && c.Id == carId ), "Id", "Make");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRepair()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairModels repairModels = db.Reapirs.Find(id);
            int ownerId = (int)Session["UserId"];
            if (repairModels == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id = new SelectList(db.Car.Where(c => c.OwnerID == ownerId && c.Id == repairModels.CarID), "Id", "Make");
            ViewBag.CarId = new SelectList(db.Car.Where(c => c.OwnerID == ownerId && c.Id == repairModels.CarID), "Id", "Make",repairModels.Id);
            return View(repairModels);
        }

        // POST: RepairModels/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CarID,Name,Description,RepairAmount")] RepairModels repairModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repairModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Car, "Id", "Make", repairModels.Id);
            return View(repairModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRepair()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepairModels repairModels = db.Reapirs.Find(id);
            if (repairModels == null)
            {
                return HttpNotFound();
               
            }
            return View(repairModels);
        }

        // POST: RepairModels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {

            int? id = Int32.Parse(Request.Form.Get("id"));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RepairModels repairModels = db.Reapirs.Find(id);

            db.Parts.Where(i => i.RepairID == id).ToList().ForEach( p => db.Parts.Remove(p));
            db.SaveChanges();
            db.Reapirs.Remove(repairModels);

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
