using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Flex_SGM.emaildata;

namespace Flex_SGM.Controllers
{
    public class Cust_Score_CardsController : Controller
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

        // GET: Cust_Score_Cards
        public async Task<ActionResult> Index()
        {
            return View(await db.Metricos.ToListAsync());
        }

        // GET: Cust_Score_Cards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // GET: Cust_Score_Cards/Create
        public ActionResult Create()
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            Metricos metricos = new Metricos
            {
                Usuario = currentUser.UserFullName,
                DiaHora = DateTime.Now
            };
            Console.WriteLine("User: ", currentUser.UserFullName);

            ViewBag.Usuario = currentUser.UserFullName;
            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());


            return View(metricos);
        }

        // POST: Cust_Score_Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Atendio,Proyectos,listarea")] Metricos metricos)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(metricos.Usuario_responsable))
                    metricos.Usuario_responsable = "-";

                ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(metricos.Usuario_responsable)).FirstOrDefault();
                var scorreo = new EmailController();
                if (cUser1 != null)
                {
                    if (cUser1.Email != null && cUser1.UserFullName != cuser)
                        if (cUser1.Email.Contains("@flexngate.com"))
                        {
                            scorreo.newoil(cUser1.Email, cuser, metricos.Descripcion);
                        }
                }

                db.Metricos.Add(metricos);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");
            ViewBag.Usuario = currentUser.UserFullName;
            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());

            return View(metricos);
        }

        // GET: Cust_Score_Cards/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Cust_Score_Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metricos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(metricos);
        }

        // GET: Cust_Score_Cards/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Cust_Score_Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Metricos metricos = await db.Metricos.FindAsync(id);
            db.Metricos.Remove(metricos);
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
