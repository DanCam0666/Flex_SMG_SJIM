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
    [Authorize(Roles = "Admin,Supervisor")]
    [Authorize(Roles = "Admin,Mantenimiento")]
    public class FallasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult dropdown()
        {
            

            var model = new MyViewFallas
            {

                // TODO: Fetch areas from somewhere               
                Areas = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList()),
            // initially we set the ddl to empty
            Tipos = Enumerable.Empty<SelectListItem>(),
                        // initially we set the ddl to empty
            Dess = Enumerable.Empty<SelectListItem>()
            };
            return View(model);
        }

        public ActionResult drop2(string Area)
        {
            // TODO: based on the selected country return the cities:
            var fallas = db.Fallas.Where(f=>f.Area== Area);
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")
            var gfallast = fallas.GroupBy(g => g.Tipo);
            List<string> tipos=new List<string>();

            foreach(var item in gfallast)
            {
                tipos.Add(item.Key);


            }
           // var cities = fallas.ToList();
            return Json(tipos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult drop3(string Tipo)
        {
            // TODO: based on the selected country return the cities:
            var fallas = db.Fallas.Where(f => f.Tipo == Tipo);
            // new SelectList(fallas, "Codigo", "DescripcionCodigo")

            List<string> codigo = new List<string>();

            foreach (var item in fallas)
            {
                codigo.Add(item.Descripcion);


            }
            // var cities = fallas.ToList();
            return Json(codigo, JsonRequestBehavior.AllowGet);
        }


        // GET: Fallas
        public ActionResult Index()
        {
            var fallas = db.Fallas;
            return View(fallas.ToList());
        }

        // GET: Fallas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fallas fallas = db.Fallas.Find(id);
            if (fallas == null)
            {
                return HttpNotFound();
            }

            return View(fallas);
        }

        // GET: Fallas/Create
        public ActionResult Create()
        {
            //ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Maquina");
            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>(), "Areas");
            return View();
        }

        // POST: Fallas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Area,Tipo,Descripcion,Codigo,DescripcionCodigo")] Fallas fallas)
        {
            if (ModelState.IsValid)
            {
                db.Fallas.Add(fallas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(fallas);
        }

        // GET: Fallas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fallas fallas = db.Fallas.Find(id);
            if (fallas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>(), "Areas");
            return View(fallas);
        }

        // POST: Fallas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Area,Tipo,Descripcion,Codigo,DescripcionCodigo")] Fallas fallas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fallas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(fallas);
        }

        // GET: Fallas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fallas fallas = db.Fallas.Find(id);
            if (fallas == null)
            {
                return HttpNotFound();
            }
            return View(fallas);
        }

        // POST: Fallas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fallas fallas = db.Fallas.Find(id);
            db.Fallas.Remove(fallas);
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
