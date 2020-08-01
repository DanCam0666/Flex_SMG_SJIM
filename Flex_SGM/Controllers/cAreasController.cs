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
    public class cAreasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: cAreas
        public ActionResult Index()
        {
            return View(db.cAreas.ToList());
        }

        // GET: cAreas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cAreas cAreas = db.cAreas.Find(id);
            if (cAreas == null)
            {
                return HttpNotFound();
            }
            return View(cAreas);
        }

        // GET: cAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: cAreas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Area")] cAreas cAreas)
        {
            if (ModelState.IsValid)
            {
                db.cAreas.Add(cAreas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cAreas);
        }

        // GET: cAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cAreas cAreas = db.cAreas.Find(id);
            if (cAreas == null)
            {
                return HttpNotFound();
            }
            return View(cAreas);
        }

        // POST: cAreas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Area")] cAreas cAreas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cAreas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cAreas);
        }

        // GET: cAreas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cAreas cAreas = db.cAreas.Find(id);
            if (cAreas == null)
            {
                return HttpNotFound();
            }
            return View(cAreas);
        }

        // POST: cAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cAreas cAreas = db.cAreas.Find(id);
            db.cAreas.Remove(cAreas);
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
