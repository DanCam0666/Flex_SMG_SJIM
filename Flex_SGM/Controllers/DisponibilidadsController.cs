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
    public class DisponibilidadsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Disponibilidads
        public ActionResult Index()
        {
            var disponibilidads = db.Disponibilidads;

            var da = db.Maquinas;

            ViewBag.machines = new SelectList(da, "ID", "Maquina");
            var DISPO = disponibilidads.ToList();
            return View(DISPO);

        }

        [AllowAnonymous]
        public JsonResult GetEvents()
        {
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var events = db.Disponibilidads;
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveEvent(Disponibilidad e)
        {
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                e.Start = e.Start;
                e.End = e.End;
                if (e.ID > 0)
                {
                    //Update the event
                    var v = dc.Disponibilidads.Where(a => a.ID == e.ID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Maquinas = e.Maquinas;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.Disponibilidads.Add(e);
                }

                var zo=dc.SaveChanges();

                if(zo!=0)
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult DeleteEvent(string eventID)
        {
            var ID = Convert.ToInt64(eventID);
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var v = db.Disponibilidads.Find(ID);
                if (v != null)
                {
                    db.Disponibilidads.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: Disponibilidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilidad disponibilidad = db.Disponibilidads.Find(id);
            if (disponibilidad == null)
            {
                return HttpNotFound();
            }
            return View(disponibilidad);
        }

        // GET: Disponibilidads/Create
        public ActionResult Create()
        {
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area");
            return View();
        }

        // POST: Disponibilidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaquinasID,Subject,Description,Start,End,daysOfWeek,ThemeColor,IsFullDay")] Disponibilidad disponibilidad)
        {
            if (ModelState.IsValid)
            {
                db.Disponibilidads.Add(disponibilidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", disponibilidad.Maquinas);
            return View(disponibilidad);
        }

        // GET: Disponibilidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilidad disponibilidad = db.Disponibilidads.Find(id);
            if (disponibilidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", disponibilidad.Maquinas);
            return View(disponibilidad);
        }

        // POST: Disponibilidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaquinasID,Subject,Description,Start,End,daysOfWeek,ThemeColor,IsFullDay")] Disponibilidad disponibilidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(disponibilidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", disponibilidad.Maquinas);
            return View(disponibilidad);
        }

        // GET: Disponibilidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disponibilidad disponibilidad = db.Disponibilidads.Find(id);
            if (disponibilidad == null)
            {
                return HttpNotFound();
            }
            return View(disponibilidad);
        }

        // POST: Disponibilidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Disponibilidad disponibilidad = db.Disponibilidads.Find(id);
            db.Disponibilidads.Remove(disponibilidad);
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
