using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;

namespace Flex_SGM.Controllers
{
    public class AndonSupervisoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AndonSupervisores
        public ActionResult Index()
        {
            var andonSupervisores = db.AndonSupervisores.Include(a => a.Areas);
            return View(andonSupervisores.ToList());
        }

        // GET: AndonSupervisores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonSupervisores andonSupervisores = db.AndonSupervisores.Find(id);
            if (andonSupervisores == null)
            {
                return HttpNotFound();
            }
            return View(andonSupervisores);
        }

        // GET: AndonSupervisores/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            return View();
        }

        // POST: AndonSupervisores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AreasID,Supervisor")] AndonSupervisores andonSupervisores)
        {
            if (ModelState.IsValid)
            {
                db.AndonSupervisores.Add(andonSupervisores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonSupervisores.AreasID);
            return View(andonSupervisores);
        }

        // GET: AndonSupervisores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonSupervisores andonSupervisores = db.AndonSupervisores.Find(id);
            if (andonSupervisores == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonSupervisores.AreasID);
            return View(andonSupervisores);
        }

        // POST: AndonSupervisores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AreasID,Supervisor")] AndonSupervisores andonSupervisores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(andonSupervisores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonSupervisores.AreasID);
            return View(andonSupervisores);
        }

        // GET: AndonSupervisores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonSupervisores andonSupervisores = db.AndonSupervisores.Find(id);
            if (andonSupervisores == null)
            {
                return HttpNotFound();
            }
            return View(andonSupervisores);
        }

        // POST: AndonSupervisores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AndonSupervisores andonSupervisores = db.AndonSupervisores.Find(id);
            db.AndonSupervisores.Remove(andonSupervisores);
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
