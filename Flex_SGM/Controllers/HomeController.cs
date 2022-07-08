using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data;
using System.Data.Entity;

namespace Flex_SGM.Controllers
{
    public class HomeController : Controller
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
        public ActionResult Index()
        {
            var user = this;
            var Suggestions = db.Sugerencias.Include(s => s.ID);
            var oILs = db.OILs.Include(o => o.Maquinas);

            int Urgente = 0, NoReali = 0, Fechaprox = 0, ConFecha = 0, sinfecha = 0, realizada = 0;
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id); 

            string cuser = "";
            if (currentUser != null)
                cuser = currentUser.UserFullName;


            foreach (OILs oil in oILs.ToList())
            {
                oil.Estatus = 0;
                if (oil.DiaHora_Compromiso == null)
                    oil.Estatus = 2;
                if (oil.DiaHora_Compromiso != null)
                    oil.Estatus = 3;
                if (oil.DiaHora_Compromiso < DateTime.Now.AddDays(4))
                    oil.Estatus = 4;
                if (oil.DiaHora_Compromiso < DateTime.Now)
                    oil.Estatus = 5;
                if (oil.urgente)
                    oil.Estatus = 6;
                if (oil.DiaHora_Cierre != null)
                    oil.Estatus = 1;

                db.Entry(oil).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (string.IsNullOrEmpty(cuser))
            {
                foreach (OILs oil in oILs.ToList())
                {

                    if (oil.Estatus == 1)
                        realizada++;
                    if (oil.Estatus == 2)
                        sinfecha++;
                    if (oil.Estatus == 3)
                        ConFecha++;
                    if (oil.Estatus == 4)
                        Fechaprox++;
                    if (oil.Estatus == 5)
                        NoReali++;
                    if (oil.Estatus == 6)
                        Urgente++;
                }
            }
            else
            {
                foreach (OILs oil in oILs.Where(p => p.User_res == cuser || p.User_asig == cuser).ToList())
                {

                    if (oil.Estatus == 1)
                        realizada++;
                    if (oil.Estatus == 2)
                        sinfecha++;
                    if (oil.Estatus == 3)
                        ConFecha++;
                    if (oil.Estatus == 4)
                        Fechaprox++;
                    if (oil.Estatus == 5)
                        NoReali++;
                    if (oil.Estatus == 6)
                        Urgente++;
                }
            }

            ViewBag.realizada = realizada;
            ViewBag.sinfecha = sinfecha;
            ViewBag.ConFecha = ConFecha;
            ViewBag.Fechaprox = Fechaprox;
            ViewBag.NoReali = NoReali;
            ViewBag.Urgente = Urgente;
            ViewBag.cuser = cuser;
            return View();
        }
        public ActionResult Working()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}