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
    public class AndonAuditorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AndonAuditors
        public ActionResult Index()
        {
            var andonAuditors = db.AndonAuditors.Include(a => a.Areas);
            return View(andonAuditors.ToList());
        }

        // GET: AndonAuditors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonAuditor andonAuditor = db.AndonAuditors.Find(id);
            if (andonAuditor == null)
            {
                return HttpNotFound();
            }
            return View(andonAuditor);
        }

        // GET: AndonAuditors/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            return View();
        }

        // POST: AndonAuditors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AreasID,Auditor")] AndonAuditor andonAuditor)
        {
            if (ModelState.IsValid)
            {
                db.AndonAuditors.Add(andonAuditor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonAuditor.AreasID);
            return View(andonAuditor);
        }

        // GET: AndonAuditors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonAuditor andonAuditor = db.AndonAuditors.Find(id);
            if (andonAuditor == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonAuditor.AreasID);
            return View(andonAuditor);
        }

        // POST: AndonAuditors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AreasID,Auditor")] AndonAuditor andonAuditor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(andonAuditor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", andonAuditor.AreasID);
            return View(andonAuditor);
        }

        // GET: AndonAuditors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AndonAuditor andonAuditor = db.AndonAuditors.Find(id);
            if (andonAuditor == null)
            {
                return HttpNotFound();
            }
            return View(andonAuditor);
        }

        // POST: AndonAuditors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AndonAuditor andonAuditor = db.AndonAuditors.Find(id);
            db.AndonAuditors.Remove(andonAuditor);
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
