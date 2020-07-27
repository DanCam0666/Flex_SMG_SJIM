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
using Microsoft.Owin.Security;

namespace Flex_SGM.Controllers
{
    public class troubleshootingsController : Controller
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

        // GET: troubleshootings
        public ActionResult Menu()
        {

            return View();
        }
        // GET: troubleshootings
        public ActionResult MenuFiltro(string amaquina="", string maquina = "")
        {
            var troubleshootings = db.troubleshootings.Include(t => t.Maquinas);
            if(!string.IsNullOrEmpty(amaquina))
            troubleshootings = troubleshootings.Where(w => w.Maquinas.Area.Contains(amaquina));

            if (!string.IsNullOrEmpty(maquina)) { 
                int idmaquina = Convert.ToInt32(Request.Form["maquinas"]);
            troubleshootings = troubleshootings.Where(w => w.Maquinas.ID== idmaquina);
            }
            var maquinas =db.Maquinas.Where(w => w.Area.Contains(amaquina));

            var user = User.Identity;
            if (string.IsNullOrEmpty(user.Name))          
                ViewBag.Danger = "Inicia Sesion para poder generar un registro";


            ViewBag.Maquinas = new SelectList(maquinas, "ID", "SubMaquina");
            return View(troubleshootings);
        }

        // GET: troubleshootings
        public ActionResult Index()
        {
            var troubleshootings = db.troubleshootings.Include(t => t.Maquinas);
            return View(troubleshootings.ToList());
        }

        // GET: troubleshootings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            troubleshooting troubleshooting = db.troubleshootings.Find(id);
            if (troubleshooting == null)
            {
                return HttpNotFound();
            }
            return View(troubleshooting);
        }

        // GET: troubleshootings/Create
        public ActionResult Create()
        {
            var user = User.Identity;
            if(string.IsNullOrEmpty(user.Name))
            {

                return RedirectToAction("MenuFiltro");
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina");
            return View();
        }

        // POST: troubleshootings/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DiaHora,usuario,MaquinasID,Problema,Posiblerazon,Solucion,Comentarios")] troubleshooting troubleshooting)
        {
            troubleshooting.DiaHora = DateTime.Now;
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            troubleshooting.usuario = currentUser.UserFullName;

            if (!string.IsNullOrEmpty(troubleshooting.usuario))
            {
                db.troubleshootings.Add(troubleshooting);
                db.SaveChanges();
                return RedirectToAction("Menu");
            }

            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", troubleshooting.MaquinasID);
            return View(troubleshooting);
        }

        // GET: troubleshootings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            troubleshooting troubleshooting = db.troubleshootings.Find(id);
            if (troubleshooting == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", troubleshooting.MaquinasID);
            return View(troubleshooting);
        }

        // POST: troubleshootings/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DiaHora,usuario,MaquinasID,Problema,Posiblerazon,Solucion,Comentarios")] troubleshooting troubleshooting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(troubleshooting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Menu");
            }
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", troubleshooting.MaquinasID);
            return View(troubleshooting);
        }

        // GET: troubleshootings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            troubleshooting troubleshooting = db.troubleshootings.Find(id);
            if (troubleshooting == null)
            {
                return HttpNotFound();
            }
            return View(troubleshooting);
        }

        // POST: troubleshootings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            troubleshooting troubleshooting = db.troubleshootings.Find(id);
            db.troubleshootings.Remove(troubleshooting);
            db.SaveChanges();
            return RedirectToAction("Menu");
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
