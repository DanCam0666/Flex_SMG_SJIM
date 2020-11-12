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
    public class QuaCodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: QuaCodes
        public ActionResult Index()
        {
            return View(db.QuaCodes.ToList());
        }

        // GET: QuaCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCodes quaCodes = db.QuaCodes.Find(id);
            if (quaCodes == null)
            {
                return HttpNotFound();
            }
            return View(quaCodes);
        }

        // GET: QuaCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuaCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuaCodesID,Code,Code_Definition")] QuaCodes quaCodes)
        {
            if (ModelState.IsValid)
            {
                db.QuaCodes.Add(quaCodes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quaCodes);
        }

        // GET: QuaCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCodes quaCodes = db.QuaCodes.Find(id);
            if (quaCodes == null)
            {
                return HttpNotFound();
            }
            return View(quaCodes);
        }

        // POST: QuaCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuaCodesID,Code,Code_Definition")] QuaCodes quaCodes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quaCodes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quaCodes);
        }

        // GET: QuaCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCodes quaCodes = db.QuaCodes.Find(id);
            if (quaCodes == null)
            {
                return HttpNotFound();
            }
            return View(quaCodes);
        }

        // POST: QuaCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuaCodes quaCodes = db.QuaCodes.Find(id);
            db.QuaCodes.Remove(quaCodes);
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
