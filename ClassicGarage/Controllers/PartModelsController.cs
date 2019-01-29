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

namespace ClassicGarage.Controllers
{
    public class PartModelsController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: PartModels_tmp
        public ActionResult Index()
        {
            int OwnerId = (int)Session["UserId"];

            var parts = db.Parts.Include(p => p.Repair).Where(o => o.Repair.Car.OwnerID == OwnerId);
            return View(parts.ToList());
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
            PartModels partModels = db.Parts.Find(id);
            if (partModels == null)
            {
                return HttpNotFound();
            }
            return View(partModels);
        }

        // GET: PartModels_tmp/Create
        public ActionResult Create()
        {
            int OwnerId = (int)Session["UserId"];
            ViewBag.RepairID = new SelectList(db.Reapirs.Where(p => p.Car.OwnerID == OwnerId), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePart()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Id = id;
            ViewBag.Repair = db.Reapirs.Find(id);
           
            return View();
        }

        // POST: PartModels_tmp/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RepairID,Name,CatalogNumber,PurchaseAmount,PurchaseDate,SalesAmount,SalesDate")] PartModels partModels)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(partModels);
                db.SaveChanges();


                var result = db.Reapirs.SingleOrDefault(b => b.Id == partModels.RepairID);
                if (result != null)
                {
                    
                        result.RepairAmount = result.RepairAmount + partModels.PurchaseAmount;
                        db.Reapirs.Attach(result);
                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                                        
                }

                return RedirectToAction("Index");
            }

            ViewBag.RepairID = new SelectList(db.Reapirs, "Id", "Name", partModels.RepairID);
            return View(partModels);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPart()
        {

            int? id = Int32.Parse(Request.Form.Get("id"));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartModels partModels = db.Parts.Find(id);

            if (partModels == null)
            {
                return HttpNotFound();
            }
            ViewBag.RepairID = new SelectList(db.Reapirs.Where(i => i.Id == partModels.RepairID), "Id", "Name", partModels.RepairID);
            return View(partModels);
        }

        // POST: PartModels_tmp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RepairID,Name,CatalogNumber,PurchaseAmount,PurchaseDate,SalesAmount,SalesDate")] PartModels partModels)
        {
            if (ModelState.IsValid)
            {

                var oldPart = db.Parts.Where(b => b.ID == partModels.ID).AsNoTracking().SingleOrDefault();
                db.Parts.Attach(partModels);
                db.Entry(partModels).State = EntityState.Modified;
                db.SaveChanges();



                var result = db.Reapirs.SingleOrDefault(b => b.Id == partModels.RepairID);
                if (result != null)
                {

                    result.RepairAmount = result.RepairAmount + (partModels.PurchaseAmount- oldPart.PurchaseAmount);
                    db.Reapirs.Attach(result);
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return RedirectToAction("Index");
            }
            ViewBag.RepairID = new SelectList(db.Reapirs, "Id", "Name", partModels.RepairID);
            return View(partModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePart()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartModels partModels = db.Parts.Find(id);
            if (partModels == null)
            {
                return HttpNotFound();
            }
            return View(partModels);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            int? id = Int32.Parse(Request.Form.Get("id"));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PartModels partModels = db.Parts.Find(id);

            var result = db.Reapirs.SingleOrDefault(b => b.Id == partModels.RepairID);
            if (result != null)
            {

                result.RepairAmount = result.RepairAmount - (partModels.PurchaseAmount);
                db.Reapirs.Attach(result);
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();

            }

            db.Parts.Remove(partModels);
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
