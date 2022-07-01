using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Flex_SGM.Controllers
{
    public class IngenieriaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Sugerencias
        public async Task<ActionResult> Index()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Super") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;

            return View();
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
        public ActionResult Sugerencias()
        {
            return View();
        }

        // POST: Sugerencias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Sugerencias([Bind(Include = "ID,DiaHora,Usuario,Muy_Bien,Bien,Mediocre,Mal,Muy_Mal,Comentarios")] Sugerencias sugerencias)
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