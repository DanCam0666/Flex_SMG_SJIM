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
    public class eoriginatorsController : Controller
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

        // GET: eoriginators
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

            var eoriginators = db.eoriginators.Include(e => e.Areas);
            return View(eoriginators.ToList());
        }

        // GET: eoriginators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = db.eoriginators.Find(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            return View(eoriginator);
        }

        // GET: eoriginators/Create
        public ActionResult Create()
        {
            ViewBag.AreasID = new SelectList(db.eAreas, "ID", "Area");
            return View();
        }

        // POST: eoriginators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AreasID,Supervisor")] eoriginator eoriginator)
        {
            if (ModelState.IsValid)
            {
                db.eoriginators.Add(eoriginator);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreasID = new SelectList(db.eAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // GET: eoriginators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = db.eoriginators.Find(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreasID = new SelectList(db.eAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // POST: eoriginators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AreasID,Supervisor")] eoriginator eoriginator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eoriginator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreasID = new SelectList(db.eAreas, "ID", "Area", eoriginator.AreasID);
            return View(eoriginator);
        }

        // GET: eoriginators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            eoriginator eoriginator = db.eoriginators.Find(id);
            if (eoriginator == null)
            {
                return HttpNotFound();
            }
            return View(eoriginator);
        }

        // POST: eoriginators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            eoriginator eoriginator = db.eoriginators.Find(id);
            db.eoriginators.Remove(eoriginator);
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
