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
    public class ereasonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ereasons
        public async Task<ActionResult> Index()
        {
            return View(await db.ereasons.ToListAsync());
        }

        // GET: ereasons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ereason ereason = await db.ereasons.FindAsync(id);
            if (ereason == null)
            {
                return HttpNotFound();
            }
            return View(ereason);
        }

        // GET: ereasons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ereasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Reason")] ereason ereason)
        {
            if (ModelState.IsValid)
            {
                db.ereasons.Add(ereason);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ereason);
        }

        // GET: ereasons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ereason ereason = await db.ereasons.FindAsync(id);
            if (ereason == null)
            {
                return HttpNotFound();
            }
            return View(ereason);
        }

        // POST: ereasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Reason")] ereason ereason)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ereason).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ereason);
        }

        // GET: ereasons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ereason ereason = await db.ereasons.FindAsync(id);
            if (ereason == null)
            {
                return HttpNotFound();
            }
            return View(ereason);
        }

        // POST: ereasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ereason ereason = await db.ereasons.FindAsync(id);
            db.ereasons.Remove(ereason);
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
