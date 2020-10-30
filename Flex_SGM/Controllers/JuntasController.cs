using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using System.IO;

namespace Flex_SGM.Controllers
{
    public class JuntasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult fileud(HttpPostedFileBase someFile)
        {
            string path = Server.MapPath("~/vids");
            // OILs OILs = db.OILs.Find(model.ID);
            if (someFile != null && someFile.ContentLength > 0 && someFile.FileName.Contains(".mp4"))
            {
                path = Path.Combine(path, Path.GetFileName("flexm.mp4"));
                try
                {
                    someFile.SaveAs(path);
                    ViewBag.Message = "Archivo se guardo satisfactoriamente";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "No Seleccionaste Archivo o el mismo no es un archivo *.mp4 Valido";
            }
            return View("Index", db.Juntas.ToList());
        }

        // GET: Juntas
        public ActionResult Index()
        {
            return View(db.Juntas.OrderByDescending(o=>o.ID).ToList());
        }

        // GET: Juntas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Junta junta = db.Juntas.Find(id);
            if (junta == null)
            {
                return HttpNotFound();
            }
            return View(junta);
        }

        // GET: Juntas/Create
        public ActionResult Create()
        {
            var juntas = db.Juntas.OrderByDescending(o => o.ID).FirstOrDefault();

            return View(juntas);
        }

        // POST: Juntas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FechaHora,Accidente,Dia_Accidente,OILSSeguridad,OILSnuevosproyectos,Oshas,RelevanteSafety,ToC_Pendientes,Vacaciones,Preventivos,comentariosa,comentarios,comentarios2,comentarios3")] Junta junta)
        {
            if (ModelState.IsValid)
            {
                junta.FechaHora = DateTime.Now;
                db.Juntas.Add(junta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(junta);
        }

        // GET: Juntas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Junta junta = db.Juntas.Find(id);
            if (junta == null)
            {
                return HttpNotFound();
            }
            return View(junta);
        }

        // POST: Juntas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FechaHora,Accidente,Dia_Accidente,OILSSeguridad,OILSnuevosproyectos,Oshas,RelevanteSafety,ToC_Pendientes,Vacaciones,Preventivos,comentariosa,comentarios,comentarios2,comentarios3")] Junta junta)
        {
            if (ModelState.IsValid)
            {
                junta.FechaHora = DateTime.Now;
                db.Entry(junta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(junta);
        }

        // GET: Juntas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Junta junta = db.Juntas.Find(id);
            if (junta == null)
            {
                return HttpNotFound();
            }
            return View(junta);
        }

        // POST: Juntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Junta junta = db.Juntas.Find(id);
            db.Juntas.Remove(junta);
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
