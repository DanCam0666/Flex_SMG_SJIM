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
    public class BilltoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Billtoes
        public ActionResult Index()
        {
            return View(db.Billtoes.ToList());
        }

        // GET: Billtoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billto billto = db.Billtoes.Find(id);
            if (billto == null)
            {
                return HttpNotFound();
            }
            return View(billto);
        }

        // GET: Billtoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Billtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BilltoID,bill,Costumername,Costumerlocation")] Billto billto)
        {
            if (ModelState.IsValid)
            {
                db.Billtoes.Add(billto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(billto);
        }

        // GET: Billtoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billto billto = db.Billtoes.Find(id);
            if (billto == null)
            {
                return HttpNotFound();
            }
            return View(billto);
        }

        // POST: Billtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BilltoID,bill,Costumername,Costumerlocation")] Billto billto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(billto);
        }

        // GET: Billtoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billto billto = db.Billtoes.Find(id);
            if (billto == null)
            {
                return HttpNotFound();
            }
            return View(billto);
        }

        // POST: Billtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Billto billto = db.Billtoes.Find(id);
            db.Billtoes.Remove(billto);
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
