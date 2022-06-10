using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;

namespace Flex_SGM.Controllers
{
    public class eoriginatorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: eoriginators
        public async Task<ActionResult> Index()
        {
            var eoriginators = db.eoriginators.Include(e => e.Areas);
            return View(await eoriginators.ToListAsync());
        }

        // GET: eoriginators/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = await db.eoriginators.FindAsync(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            return View(eoriginator);
        }

        // GET: eoriginators/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area");
            return View();
        }

        // POST: eoriginators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,AreasID,Supervisor")] eoriginator eoriginator)
        {
            if (ModelState.IsValid)
            {
                db.eoriginators.Add(eoriginator);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // GET: eoriginators/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = await db.eoriginators.FindAsync(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // POST: eoriginators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,AreasID,Supervisor")] eoriginator eoriginator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eoriginator).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.cAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // GET: eoriginators/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = await db.eoriginators.FindAsync(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            return View(eoriginator);
        }

        // POST: eoriginators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            eoriginator eoriginator = await db.eoriginators.FindAsync(id);
            db.eoriginators.Remove(eoriginator);
            await db.SaveChangesAsync();
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
