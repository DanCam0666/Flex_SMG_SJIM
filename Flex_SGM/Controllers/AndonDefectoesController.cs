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
    public class AndonDefectoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AndonDefectoes
        public ActionResult Index()
        {
            var andonDefectoes = db.AndonDefectoes.Include(a => a.Areas);
            return View(andonDefectoes.ToList());
        }

        // GET: AndonDefectoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonDefecto andonDefecto = db.AndonDefectoes.Find(id);
            if (andonDefecto == null)
            {
                return HttpNotFound();
            }
            return View(andonDefecto);
        }

        // GET: AndonDefectoes/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            return View();
        }

        // POST: AndonDefectoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AreasID,Defecto")] AndonDefecto andonDefecto)
        {
            if (ModelState.IsValid)
            {
                db.AndonDefectoes.Add(andonDefecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonDefecto.AreasID);
            return View(andonDefecto);
        }

        // GET: AndonDefectoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonDefecto andonDefecto = db.AndonDefectoes.Find(id);
            if (andonDefecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonDefecto.AreasID);
            return View(andonDefecto);
        }

        // POST: AndonDefectoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AreasID,Defecto")] AndonDefecto andonDefecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(andonDefecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonDefecto.AreasID);
            return View(andonDefecto);
        }

        // GET: AndonDefectoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonDefecto andonDefecto = db.AndonDefectoes.Find(id);
            if (andonDefecto == null)
            {
                return HttpNotFound();
            }
            return View(andonDefecto);
        }

        // POST: AndonDefectoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AndonDefecto andonDefecto = db.AndonDefectoes.Find(id);
            db.AndonDefectoes.Remove(andonDefecto);
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
