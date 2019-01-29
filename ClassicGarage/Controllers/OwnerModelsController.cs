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
    public class OwnerModelsController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: OwnerModels
        public ActionResult Index()
        {
            return View(db.Owner.ToList());
        }

        // GET: OwnerModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerModels ownerModels = db.Owner.Find(id);
            if (ownerModels == null)
            {
                return HttpNotFound();
            }
            return View(ownerModels);
        }

        // GET: OwnerModels/Create
        public ActionResult Create()
        {
            var OwnerId = (int)Session["UserId"];

            var e_mail = User.Identity.GetUserName();
            var query = db.Owner.Where(s => s.Email == e_mail).Select(s => s.ID).FirstOrDefault();
            if(query!=0) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: OwnerModels/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,PhoneNo,Email")] OwnerModels ownerModels)
        {
            if (ModelState.IsValid)
            {

                db.Owner.Add(ownerModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ownerModels);
        }

        // GET: OwnerModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerModels ownerModels = db.Owner.Find(id);
            if (ownerModels == null)
            {
                return HttpNotFound();
            }
            return View(ownerModels);
        }

        // POST: OwnerModels/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,PhoneNo,Email")] OwnerModels ownerModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ownerModels);
        }

        // GET: OwnerModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerModels ownerModels = db.Owner.Find(id);
            if (ownerModels == null)
            {
                return HttpNotFound();
            }
            return View(ownerModels);
        }

        // POST: OwnerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerModels ownerModels = db.Owner.Find(id);
            db.Owner.Remove(ownerModels);
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
