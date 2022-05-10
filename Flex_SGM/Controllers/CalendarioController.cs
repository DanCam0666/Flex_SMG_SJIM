﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Flex_SGM.Scripts;


namespace Flex_SGM.Controllers
{
    [Authorize(Roles = "Admin,Supervisor")]
    [Authorize(Roles = "Admin,Mantenimiento")]
    public class CalendarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Proyectos
        public ActionResult Index()
        {
            var proyectos = db.Proyectos.Include(p => p.Maquinas);
            return View(proyectos.ToList());
        }


        /// <summary>
        /// Metodo que devuelve un Array de eventos en formato Json
        /// summary>
        /// <param name="start">Star Dateparam>
        /// <param name="end">End Dateparam>
        /// <returns>returns>
          [AllowAnonymous]
        public JsonResult GetEvents()
        {
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var events = dc.CalendarioProye.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SaveEvent(CalendarioProye e)
        {
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                e.Start = e.Start;
                e.End = e.End;
                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.CalendarioProye.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    dc.CalendarioProye.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }
            return new JsonResult { Data = new { status = status } };
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult DeleteEvent(string eventID)
        {
            var ideve = Convert.ToInt64(eventID);
            var status = false;
            using (ApplicationDbContext dc = new ApplicationDbContext())
            {
                var v = dc.CalendarioProye.Where(a => a.EventID == ideve).FirstOrDefault();
                if (v != null)
                {
                    dc.CalendarioProye.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: Proyectos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            return View(proyectos);
        }

        // GET: Proyectos/Create
        public ActionResult Create()
        {
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area");
            return View();
        }

        // POST: Proyectos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaquinasID,User_gen,DiaHora,Tipo,Que,Porque,Paraque,Paraquien,Donde,Cuando,Conquien,conque,Como,DiaHora_Cierre,DiaHora_Verificado,Usuario_Verifico,Comentarios,urgente,Estatus")] Proyectos proyectos)
        {
            if (ModelState.IsValid)
            {
                db.Proyectos.Add(proyectos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            proyectos.DiaHora = DateTime.Now;
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", proyectos.MaquinasID);
            return View(proyectos);
        }

        // GET: Proyectos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", proyectos.MaquinasID);
            return View(proyectos);
        }

        // POST: Proyectos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaquinasID,User_gen,DiaHora,Tipo,Que,Porque,Paraque,Paraquien,Donde,Cuando,Conquien,conque,Como,DiaHora_Cierre,DiaHora_Verificado,Usuario_Verifico,Comentarios,urgente,Estatus")] Proyectos proyectos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", proyectos.MaquinasID);
            return View(proyectos);
        }

        // GET: Proyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyectos proyectos = db.Proyectos.Find(id);
            if (proyectos == null)
            {
                return HttpNotFound();
            }
            return View(proyectos);
        }

        // POST: Proyectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyectos proyectos = db.Proyectos.Find(id);
            db.Proyectos.Remove(proyectos);
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
