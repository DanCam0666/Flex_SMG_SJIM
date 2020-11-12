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
    public class QuaCostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: QuaCosts
        public ActionResult Index()
        {
            var quaCosts = db.QuaCosts.Include(q => q.AndonSupervisores).Include(q => q.Billto).Include(q => q.Primary).Include(q => q.QuaCodes);
            return View(quaCosts.ToList());
        }

        // GET: QuaCosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCost quaCost = db.QuaCosts.Find(id);
            if (quaCost == null)
            {
                return HttpNotFound();
            }
            return View(quaCost);
        }

        // GET: QuaCosts/Create
        public ActionResult Create()
        {
            ViewBag.AndonSupervisoresID = new SelectList(db.AndonSupervisores, "ID", "Supervisor");
            ViewBag.BilltoID = new SelectList(db.Billtoes, "BilltoID", "bill");
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            ViewBag.QuaCodesID = new SelectList(db.QuaCodes, "QuaCodesID", "Code");
            return View();
        }

        // POST: QuaCosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Fecha,BilltoID,Location,CPO,ioc,aod,QuaCodesID,partnum,price,AreasID,AndonSupervisoresID,issueDescription,rootcause,Countermeasure,Comments,AVFSR")] QuaCost quaCost)
        {
            if (ModelState.IsValid)
            {
                db.QuaCosts.Add(quaCost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AndonSupervisoresID = new SelectList(db.AndonSupervisores, "ID", "Supervisor", quaCost.AndonSupervisoresID);
            ViewBag.BilltoID = new SelectList(db.Billtoes, "BilltoID", "bill", quaCost.BilltoID);
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", quaCost.AreasID);
            ViewBag.QuaCodesID = new SelectList(db.QuaCodes, "QuaCodesID", "Code", quaCost.QuaCodesID);
            return View(quaCost);
        }

        // GET: QuaCosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCost quaCost = db.QuaCosts.Find(id);
            if (quaCost == null)
            {
                return HttpNotFound();
            }
            ViewBag.AndonSupervisoresID = new SelectList(db.AndonSupervisores, "ID", "Supervisor", quaCost.AndonSupervisoresID);
            ViewBag.BilltoID = new SelectList(db.Billtoes, "BilltoID", "bill", quaCost.BilltoID);
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", quaCost.AreasID);
            ViewBag.QuaCodesID = new SelectList(db.QuaCodes, "QuaCodesID", "Code", quaCost.QuaCodesID);
            return View(quaCost);
        }

        // POST: QuaCosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Fecha,BilltoID,Location,CPO,ioc,aod,QuaCodesID,partnum,price,AreasID,AndonSupervisoresID,issueDescription,rootcause,Countermeasure,Comments,AVFSR")] QuaCost quaCost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quaCost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AndonSupervisoresID = new SelectList(db.AndonSupervisores, "ID", "Supervisor", quaCost.AndonSupervisoresID);
            ViewBag.BilltoID = new SelectList(db.Billtoes, "BilltoID", "bill", quaCost.BilltoID);
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", quaCost.AreasID);
            ViewBag.QuaCodesID = new SelectList(db.QuaCodes, "QuaCodesID", "Code", quaCost.QuaCodesID);
            return View(quaCost);
        }

        // GET: QuaCosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuaCost quaCost = db.QuaCosts.Find(id);
            if (quaCost == null)
            {
                return HttpNotFound();
            }
            return View(quaCost);
        }

        // POST: QuaCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuaCost quaCost = db.QuaCosts.Find(id);
            db.QuaCosts.Remove(quaCost);
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
