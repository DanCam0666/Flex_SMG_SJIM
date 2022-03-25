using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;

namespace Flex_SGM.Views.Metricos
{
    public class CI_MPController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CI_MP
        public ActionResult Index()
        {
            var bitacoras = db.Bitacoras.Include(b => b.Fallas).Include(b => b.Maquinas);
            return View(bitacoras.ToList());
        }

        // GET: CI_MP/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.Bitacoras.Find(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // GET: CI_MP/Create
        public ActionResult Create()
        {
            ViewBag.FallasID = new SelectList(db.Fallas, "ID", "Area");
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area");
            return View();
        }

        // POST: CI_MP/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DiaHora,usuario,usuario_area,usuario_puesto,MaquinasID,Sintoma,Causa,AccionCorrectiva,Atendio,Fallaoperacion,Tiempo,Scrap,Folio,Verifico,FechaVerificacion,MttoPreventivo,MttoCorrectivo,MttoMejora,noterminado,findesemana,turno,Descripcion,FallasID")] Bitacora bitacora)
        {
            if (ModelState.IsValid)
            {
                db.Bitacoras.Add(bitacora);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FallasID = new SelectList(db.Fallas, "ID", "Area", bitacora.FallasID);
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", bitacora.MaquinasID);
            return View(bitacora);
        }

        // GET: CI_MP/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.Bitacoras.Find(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            ViewBag.FallasID = new SelectList(db.Fallas, "ID", "Area", bitacora.FallasID);
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", bitacora.MaquinasID);
            return View(bitacora);
        }

        // POST: CI_MP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DiaHora,usuario,usuario_area,usuario_puesto,MaquinasID,Sintoma,Causa,AccionCorrectiva,Atendio,Fallaoperacion,Tiempo,Scrap,Folio,Verifico,FechaVerificacion,MttoPreventivo,MttoCorrectivo,MttoMejora,noterminado,findesemana,turno,Descripcion,FallasID")] Bitacora bitacora)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bitacora).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FallasID = new SelectList(db.Fallas, "ID", "Area", bitacora.FallasID);
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", bitacora.MaquinasID);
            return View(bitacora);
        }

        // GET: CI_MP/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bitacora bitacora = db.Bitacoras.Find(id);
            if (bitacora == null)
            {
                return HttpNotFound();
            }
            return View(bitacora);
        }

        // POST: CI_MP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bitacora bitacora = db.Bitacoras.Find(id);
            db.Bitacoras.Remove(bitacora);
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
