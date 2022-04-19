﻿using System;
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
using System.IO;
using Flex_SGM.emaildata;
using Flex_SGM.Scripts;

namespace Flex_SGM.Controllers
{
    public class CI_MPController : Controller
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

        // GET: CI_MP
        public async Task<ActionResult> Index()
        {
            //  var id = User.Identity.GetUserId();
            //  ApplicationUser currentUser = UserManager.FindById(id);
            var metricos = db.Metricos.ToList<Metricos>();

            var chartData1 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 1)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData2 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 2)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData3 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 3)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData4 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 4)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData5 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 5)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData6 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 6)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData7 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 7)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData8 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 8)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData9 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 9)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData10 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 10)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData11 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 11)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartData12 = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .Where(m => m.DiaHora.Month == 12)
                .GroupBy(w => w.Usuario_responsable)
                .Select(w => w.Sum(t => t.Proyectos))
                .ToList();
            var chartLabel = metricos
                .OrderByDescending(w => w.DiaHora)
                .Where(m => m.Usuario_area is "Cont_Imprv_ManP")
                .Where(m => m.DiaHora.Year == DateTime.Now.Year)
                .GroupBy(m => m.Usuario_responsable)
                .Select(m => m.Key)
                .ToList();

            ViewBag.ChartData1 = chartData1;
            ViewBag.ChartData2 = chartData2;
            ViewBag.ChartData3 = chartData3;
            ViewBag.ChartData4 = chartData4;
            ViewBag.ChartData5 = chartData5;
            ViewBag.ChartData6 = chartData6;
            ViewBag.ChartData7 = chartData7;
            ViewBag.ChartData8 = chartData8;
            ViewBag.ChartData9 = chartData9;
            ViewBag.ChartData10 = chartData10;
            ViewBag.ChartData11 = chartData11;
            ViewBag.ChartData12 = chartData12;
            ViewBag.ChartLabel = chartLabel;

            ViewBag.Usuario_responsable = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Usuario_area = new SelectList(Enum.GetValues(typeof(flex_Areasv1)).Cast<flex_Areasv1>().ToList());
            ViewBag.Usuario_puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());

            return View(await db.Metricos.ToListAsync());
        }

        // GET: CI_MP/Details/5
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

        [Authorize]
        // GET: CI_MP/Create
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

        // POST: CI_MP/Create
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

        [Authorize]
        // GET: CI_MP/Edit/5
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

        // POST: CI_MP/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
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

                db.Entry(metricos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "UserFullName");

            return View(metricos);
        }
        
        [Authorize]
        // GET: CI_MP/Delete/5
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

        // POST: CI_MP/Delete/5
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