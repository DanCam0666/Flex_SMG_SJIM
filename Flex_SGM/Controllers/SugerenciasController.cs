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
    public class SugerenciasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sugerencias
        public async Task<ActionResult> Index()
        {
            return View(await db.Sugerencias.ToListAsync());
        }

        // GET: Sugerencias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugerencias sugerencias = await db.Sugerencias.FindAsync(id);
            if (sugerencias == null)
            {
                return HttpNotFound();
            }
            return View(sugerencias);
        }

        // GET: Sugerencias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sugerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,Usuario,Muy_Bien,Bien,Mediocre,Mal,Muy_Mal,Comentarios")] Sugerencias sugerencias)
        {
            if (ModelState.IsValid)
            {
                db.Sugerencias.Add(sugerencias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sugerencias);
        }

        // GET: Sugerencias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugerencias sugerencias = await db.Sugerencias.FindAsync(id);
            if (sugerencias == null)
            {
                return HttpNotFound();
            }
            return View(sugerencias);
        }

        // POST: Sugerencias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Muy_Bien,Bien,Mediocre,Mal,Muy_Mal,Comentarios")] Sugerencias sugerencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sugerencias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sugerencias);
        }

        // GET: Sugerencias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sugerencias sugerencias = await db.Sugerencias.FindAsync(id);
            if (sugerencias == null)
            {
                return HttpNotFound();
            }
            return View(sugerencias);
        }

        // POST: Sugerencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sugerencias sugerencias = await db.Sugerencias.FindAsync(id);
            db.Sugerencias.Remove(sugerencias);
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
