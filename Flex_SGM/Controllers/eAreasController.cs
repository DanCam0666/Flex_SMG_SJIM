using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Flex_SGM.Controllers
{
    public class eAreasController : Controller
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

        // GET: eAreas
        public ActionResult Index()
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

            return View(db.eAreas.ToList());
        }

        // GET: eAreas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eAreas eAreas = db.eAreas.Find(id);
            if (eAreas == null)
            {
                return HttpNotFound();
            }
            return View(eAreas);
        }

        // GET: eAreas/Create
        public ActionResult Create()
        {
            ViewBag.Area = new SelectList(db.eAreas, "ID", "Area");
            return View();
        }

        // POST: eAreas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Area")] eAreas eAreas)
        {
            if (ModelState.IsValid)
            {
                db.eAreas.Add(eAreas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eAreas);
        }

        // GET: eAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eAreas eAreas = db.eAreas.Find(id);
            if (eAreas == null)
            {
                return HttpNotFound();
            }
            return View(eAreas);
        }

        // POST: eAreas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Area")] eAreas eAreas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eAreas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eAreas);
        }

        // GET: eAreas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eAreas eAreas = db.eAreas.Find(id);
            if (eAreas == null)
            {
                return HttpNotFound();
            }
            return View(eAreas);
        }

        // POST: eAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eAreas eAreas = db.eAreas.Find(id);
            db.eAreas.Remove(eAreas);
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
