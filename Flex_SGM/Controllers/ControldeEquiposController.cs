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
    public class ControldeEquiposController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ControldeEquipos
        public ActionResult Index()
        {
            var controldeEquipos = db.ControldeEquipos.Include(c => c.Maquinas);
            return View(controldeEquipos.ToList());
        }

        // GET: ControldeEquipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControldeEquipos controldeEquipos = db.ControldeEquipos.Find(id);
            if (controldeEquipos == null)
            {
                return HttpNotFound();
            }
            return View(controldeEquipos);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: ControldeEquipos/Create
        public ActionResult Create()
        {
            ViewBag.Danger = TempData["ErrorMessage"] as string;
            ControldeEquipos elnew = new ControldeEquipos();
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina");
            elnew.DiaHora = DateTime.Now;

            return View(elnew);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // POST: ControldeEquipos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaquinasID,DiaHora,equipo,descripcion,iplocal,ipintra,vlan")] ControldeEquipos controldeEquipos)
        {

            var all = db.ControldeEquipos.ToList();

            if (ModelState.IsValid)
            {
                foreach(var item in all)
                {
                    if (item.ipintra == controldeEquipos.ipintra)
                    {
                        TempData["ErrorMessage"] = "Direccion IP de Planta Duplicada";
                        return RedirectToAction("Create");
                    }

                }


                db.ControldeEquipos.Add(controldeEquipos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina", controldeEquipos.MaquinasID);
            return View(controldeEquipos);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: ControldeEquipos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControldeEquipos controldeEquipos = db.ControldeEquipos.Find(id);
            if (controldeEquipos == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina", controldeEquipos.MaquinasID);
            return View(controldeEquipos);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // POST: ControldeEquipos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaquinasID,DiaHora,equipo,descripcion,iplocal,ipintra,vlan")] ControldeEquipos controldeEquipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controldeEquipos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina", controldeEquipos.MaquinasID);
            return View(controldeEquipos);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // GET: ControldeEquipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControldeEquipos controldeEquipos = db.ControldeEquipos.Find(id);
            if (controldeEquipos == null)
            {
                return HttpNotFound();
            }
            return View(controldeEquipos);
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Supervisor")]
        [Authorize(Roles = "Admin,Mantenimiento")]
        // POST: ControldeEquipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ControldeEquipos controldeEquipos = db.ControldeEquipos.Find(id);
            db.ControldeEquipos.Remove(controldeEquipos);
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
