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
using System.IO;
using Flex_SGM.emaildata;
using Flex_SGM.Scripts;

namespace Flex_SGM.Controllers
{
    public class OILsController : Controller
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
        // GET: OILs
        public async Task<ActionResult> Index(string searchString, string tipo, string Dep,  string maquina, bool 
            c1=false,bool c2 = false, bool c3 = false, bool c4 = false, bool c5 = false, bool c6 = false,string area= "",string btn="")
        {
            var oILs = db.OILs.Include(o => o.Maquinas);
            var dw = dayofweek.GetIso8601WeekOfYear(DateTime.Now);


            var objet = "'";
            var dat = "";
            var dat2 = "";
            for (int i=1;i<= dw;i++)
            {
                objet = objet +"Semana "+i.ToString() + "','";
            }
            int[] semanas = new int[55];
            int[] tsemanas = new int[55];
            int Serrealizado = 0, Serpendiente = 0, Sertotal=0;
            int Estrealizado = 0, Estpendiente = 0, Esttotal = 0;
            int Soldrealizado = 0,Soldpendiente = 0, Soldtotal = 0;
            int MFrealizado = 0, MFpendiente = 0, MFtotal = 0;
            int ENrealizado = 0, ENpendiente = 0, ENtotal = 0;

            foreach (var item in oILs.Where(s => s.Tipo == "TPM"))
            {
                semanas[dayofweek.GetIso8601WeekOfYear(item.DiaHora)]++;

                tsemanas[dayofweek.GetIso8601WeekOfYear(item.DiaHora_Cierre)]++;
                if (item.Maquinas.Area.Contains("Servicios"))
                {
                    Sertotal++;
                    if (item.Estatus == 1)
                        Serrealizado++;
                    else
                        Serpendiente++;
                }
                if (item.Maquinas.Area.Contains("Estampado"))
                {
                    Esttotal++;
                    if (item.Estatus == 1)
                        Estrealizado++;
                    else
                        Estpendiente++;
                }

                if (item.Maquinas.Area.Contains("Soldadura"))
                {
                    Soldtotal++;
                    if (item.Estatus == 1)
                        Soldrealizado++;
                    else
                        Soldpendiente++;
                }
                if (item.Maquinas.Area.Contains("Cromo")|| item.Maquinas.Area.Contains("Cromo1") || item.Maquinas.Area.Contains("Cromo2") || item.Maquinas.Area.Contains("AutoPulido1") || item.Maquinas.Area.Contains("AutoPulido2") || item.Maquinas.Area.Contains("Ecoat") || item.Maquinas.Area.Contains("Topcoat") || item.Maquinas.Area.Contains("MetalFinish"))
                {
                    MFtotal++;
                    if (item.Estatus == 1)
                        MFrealizado++;
                    else
                        MFpendiente++;
                }
                if (item.Maquinas.Area.Contains("Ensamble"))
                {
                    ENtotal++;
                    if (item.Estatus == 1)
                        ENrealizado++;
                    else
                        ENpendiente++;
                }

            }


            ViewBag.sr = Serrealizado;
            ViewBag.sp = Serpendiente;
            ViewBag.st = Sertotal;
            ViewBag.er = Estrealizado;
            ViewBag.ep = Estpendiente;
            ViewBag.et = Esttotal;
            ViewBag.sor = Soldrealizado;
            ViewBag.sop = Soldpendiente;
            ViewBag.sot = Soldtotal;
            ViewBag.mfr = MFrealizado;
            ViewBag.mfp = MFpendiente;
            ViewBag.mft = MFtotal;
            ViewBag.enr = ENrealizado;
            ViewBag.enp = ENpendiente;
            ViewBag.ent = ENtotal;


            bool bypass = true;
            foreach (var item in semanas)
            {
                if(!bypass)
                dat = dat + item.ToString() + ",";
                bypass = false;
            }
            bypass = true;
            foreach (var item in tsemanas)
            {
                if (!bypass)
                    dat2 = dat2 + item.ToString() + ",";
                bypass = false;
            }

            objet = objet.TrimEnd(',', (char)39);
            objet = objet + "'";
            dat = dat.TrimEnd(',', (char)39);
            dat2 = dat2.TrimEnd(',', (char)39);

            ViewBag.Objectsem = objet;
            ViewBag.Datas1 = dat;
            ViewBag.Datas2 = dat2;



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

            foreach (OILs oil in oILs.Where(s=>s.Tipo=="TPM"))
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
            ViewBag.tpmrealizada = realizada;
            ViewBag.tpmActivos = ConFecha;
            ViewBag.tpmpendientes = sinfecha + Urgente + Fechaprox;
            ViewBag.tpmNoReali = NoReali;
            ViewBag.tpmtotal = NoReali+ sinfecha + Urgente + Fechaprox+ ConFecha+realizada;

            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion").ToList())
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
            ViewBag.arealizada = realizada;
            ViewBag.aActivos = ConFecha;
            ViewBag.apendientes = sinfecha + Urgente + Fechaprox;
            ViewBag.aNoReali = NoReali;
            ViewBag.atotal = NoReali + sinfecha + Urgente + Fechaprox + ConFecha + realizada;

            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion" &&( s.User_res == "Pedro Luis Ramirez Lopez" || s.User_asig == "Pedro Luis Ramirez Lopez" || s.User_res == "José Mario Castilla González" || s.User_asig == "José Mario Castilla González")).ToList())
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
            ViewBag.pmrealizada = realizada;
            ViewBag.pmActivos =  NoReali + sinfecha + Urgente + Fechaprox + ConFecha ;

            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion" && (s.User_res == "José Mario Castilla González" || s.User_asig == "José Mario Castilla González" || s.User_res == "Juan Ramon Arredondo Pacheco" || s.User_asig == "Juan Ramon Arredondo Pacheco")).ToList())
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
            ViewBag.gprealizada = realizada;
            ViewBag.gpActivos = NoReali + sinfecha + Urgente + Fechaprox + ConFecha+ realizada;

            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion" && (s.User_res == "Pedro Dorantes Sanchez" || s.User_asig == "Pedro Dorantes Sanchez" || s.User_res == "Jaime Martinez Prado" || s.User_asig == "Jaime Martinez Prado")).ToList())
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
            ViewBag.ajrealizada = realizada;
            ViewBag.ajActivos = NoReali + sinfecha + Urgente + Fechaprox + ConFecha + realizada;
            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion" && (s.User_res == "Marco Arvizu" || s.User_asig == "Marco Arvizu" || s.User_res == "Francisco Armenta Martínez" || s.User_asig == "Francisco Armenta Martínez")).ToList())
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
            ViewBag.mfrealizada = realizada;
            ViewBag.mfActivos = NoReali + sinfecha + Urgente + Fechaprox + ConFecha + realizada;
            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            foreach (OILs oil in oILs.Where(s => s.Tipo == "Automatizacion" && ((s.User_res == "Jose Luis Olivarez Dominguez" && s.User_asig == "-") || s.User_asig == "Jose Luis Olivarez Dominguez" )).ToList())
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
            ViewBag.jlrealizada = realizada;
            ViewBag.jlmfActivos = NoReali + sinfecha + Urgente + Fechaprox + ConFecha + realizada;

            Urgente = 0; NoReali = 0; Fechaprox = 0; ConFecha = 0; sinfecha = 0; realizada = 0;

            if (string.IsNullOrEmpty(""))
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
                foreach (OILs oil in oILs.Where(p => p.User_res == cuser).ToList())
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

            ViewBag.area = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            ViewBag.puesto = new SelectList(Enum.GetValues(typeof(flex_Puesto)).Cast<flex_Puesto>().ToList());
            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            var maquinas = db.Maquinas.Where(m => m.ID > 0);

            ViewBag.maquina = new SelectList(maquinas, "ID", "SubMaquina");

            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;



            ViewBag.Dep = new SelectList(Enum.GetValues(typeof(flex_Departamento)).Cast<flex_Departamento>().ToList());
            ViewBag.Tipo = new SelectList(Enum.GetValues(typeof(flex_Oils)).Cast<flex_Oils>().ToList());


            if(btn.Contains("Buscar OILs")) { 
            if (!String.IsNullOrEmpty(searchString))
            {
                oILs = oILs.Where(s => s.Tipo.Contains(searchString)
                   || s.Maquinas.Area.Contains(searchString)
                       || s.Maquinas.SubMaquina.Contains(searchString)
                        || s.Maquinas.Maquina.Contains(searchString)
                         || s.User_asig.Contains(searchString)
                          || s.User_gen.Contains(searchString)
                          || s.User_res.Contains(searchString)
                           || s.Actividad.Contains(searchString)
                            || s.Comentarios.Contains(searchString)
                                 );
            }

            if (!String.IsNullOrEmpty(tipo) && tipo != "--Todas--")
            {
                oILs = oILs.Where(s => s.Tipo == tipo);
            }

            if (area.Contains("Soldadura"))
                area = "Soldadura";
            if (!String.IsNullOrEmpty(area) && area != "--Todas--")
            {
                oILs = oILs.Where(s => s.Maquinas.Area == area);
            }

            if (!String.IsNullOrEmpty(Dep) && Dep != "--Todas--")
            {
                oILs = oILs.Where(s => s.User_res == Dep);
            }

            if (!String.IsNullOrEmpty(maquina) && maquina != "--Todas--")
            {
                oILs = oILs.Where(s => s.Maquinas.SubMaquina == maquina);

            }

            int a1=0, a2 = 0, a3 = 0, a4 = 0, a5 = 0, a6 = 0;

            if (c1)
                a1 = 1;
            if (c2)
                a2 = 2;
            if (c3)
                a3 = 3;
            if (c4)
                a4 = 4;
            if (c5)
                a5 = 5;
            if (c6)
                a6 = 6;

            if (c1 == false&& c2 == false && c3 == false && c4 == false && c5 == false && c6 == false)
            {

                    a1 = 1;

                    a2 = 2;

                    a3 = 3;

                    a4 = 4;

                    a5 = 5;

                    a6 = 6;

            }

         oILs = oILs.Where(s => s.Estatus == a1
            || s.Estatus == a2
            || s.Estatus == a3
            || s.Estatus == a4
            || s.Estatus == a5
            || s.Estatus == a6
            );
            }
            else
            {
                if (!String.IsNullOrEmpty(cuser)&&cuser!= "xxx")
                {
                    oILs = oILs.Where(s => s.User_asig.Contains(cuser)
                              || s.User_gen.Contains(cuser)
                              || s.User_res.Contains(cuser)
                                     );
                }

            }


            return View(await oILs.ToListAsync());
        }

        // GET: OILs
        public async Task<ActionResult> Ress( string Area, string dep, string tipo, string usuario1, string usuario2, string usuario3)
        {
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
            oILs = db.OILs.Include(o => o.Maquinas);

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
                foreach (OILs oil in oILs.Where(p => p.User_res == cuser).ToList())
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

            return PartialView("Ress", await oILs.Where(s => s.User_gen == usuario1|| s.User_res == usuario2 || s.User_asig==usuario3|| s.Maquinas.Area== Area||s.Tipo==tipo).ToListAsync());           
        }

        // GET: OILs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OILs oILs = await db.OILs.FindAsync(id);
            if (oILs == null)
            {
                return HttpNotFound();
            }
            return View(oILs);
        }
        [Authorize]
        // GET: OILs/Create
        public ActionResult Create()
        {
            OILs oils = new OILs();
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            oils.User_gen = currentUser.UserFullName;
            oils.DiaHora=DateTime.Now;

            ViewBag.User_res = new SelectList(db.Users, "UserFullName", "UserFullName");
            ViewBag.Tipo = new SelectList(Enum.GetValues(typeof(flex_Oils)).Cast<flex_Oils>().ToList());
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina");
            return View(oils);
        }

        // POST: OILs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Tipo,Folio,MaquinasID,Actividad,User_Gen,User_res,DiaHora,Comentarios,Comentarios2,Material_necesario,urgente")] OILs oILs)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
                
                if (string.IsNullOrEmpty(oILs.User_asig))
                    oILs.User_asig = "-";
                if (string.IsNullOrEmpty(oILs.User_res))
                    oILs.User_res = "Jose Luis Olivarez Dominguez";

                db.OILs.Add(oILs);
                await db.SaveChangesAsync();
                ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(oILs.User_res)).FirstOrDefault();
                ApplicationUser cUser2 = UserManager.Users.Where(u => u.UserFullName.Contains(oILs.User_asig)).FirstOrDefault();
                //ApplicationUser cUser3 = UserManager.Users.Where(u => u.UserFullName.Contains(oILs.User_gen)).FirstOrDefault();
                var scorreo = new EmailController();
                if (cUser1 != null)
                {
                    if (cUser1.Email != null && cUser1.UserFullName != cuser)
                    if (cUser1.Email.Contains("@flexngate.com"))
                    {

                        scorreo.newoil(cUser1.Email, cuser, oILs.Actividad);
                    }
                }
                if (cUser2 != null)
                {
                    if (cUser2.Email != null && cUser2.UserFullName != cuser)
                        if (cUser2.Email.Contains("@flexngate.com"))
                    {

                        scorreo.newoil(cUser2.Email, cuser, oILs.Actividad);
                    }
                }
                /*
                if (cUser3 != null )
                {
                    if (cUser3.Email != null && cUser3.UserFullName != cuser)
                        if (cUser3.Email.Contains("@flexngate.com"))
                    {

                        scorreo.newoil(cUser3.Email, cuser, oILs.Actividad);
                    }
                }
                */
                return RedirectToAction("Index");
            }
           
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "Area", oILs.MaquinasID);
            return View(oILs);
        }
       
        public PartialViewResult _Comments(int id)
        {

            var comme = db.Comments.Where(i=>i.OILs.ID==id).AsQueryable();

            return PartialView("~/Views/Shared/_Comments.cshtml", comme);
        }

        [HttpPost]
       
        public ActionResult AddComment(string commentMsg, int cid)
        {
            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id); 

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            Comments Commente = new Comments
            {
                OILs = db.OILs.Where(o=>o.ID==cid).FirstOrDefault(),
                fecha = DateTime.Now,
                message = commentMsg,
                Usuario = cuser

            };
            db.Comments.Add(Commente);
            db.SaveChanges();
            ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(Commente.OILs.User_res)).FirstOrDefault();
            ApplicationUser cUser2 = UserManager.Users.Where(u => u.UserFullName.Contains(Commente.OILs.User_asig)).FirstOrDefault();
            ApplicationUser cUser3 = UserManager.Users.Where(u => u.UserFullName.Contains(Commente.OILs.User_gen)).FirstOrDefault();
            var scorreo = new EmailController();
            if (cUser1 != null)
            {
                if (cUser1.Email != null && cUser1.UserFullName != cuser)
                if (cUser1.Email.Contains("@flexngate.com")) {
                   
                      scorreo.updateoil(cUser1.Email, cuser, cid.ToString(), commentMsg);
                }
            }
            if (cUser2 != null )
            {
                if(cUser2.Email != null && cUser2.UserFullName != cuser)
                if (cUser2.Email.Contains("@flexngate.com"))
                {

                    scorreo.updateoil(cUser2.Email, cuser, cid.ToString(), commentMsg);
                }
            }
            if (cUser3 != null )
            {
                if( cUser3.Email != null && cUser3.UserFullName != cuser)
                if (cUser3.Email.Contains("@flexngate.com"))
                {

                    scorreo.updateoil(cUser3.Email, cuser, cid.ToString(), commentMsg);
                }
            }
            return RedirectToAction("_Comments", "OILs", new { id = cid });

        }


        public ActionResult Update(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            OILs OILs = db.OILs.Find(id);
            if (OILs == null)
            {
                return HttpNotFound();
            }
            SelectList selectList1 = new SelectList(db.Users, "UserFullName", "UserFullName", OILs.User_asig);


            ViewBag.User_asig = selectList1;

            return View(OILs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,Tipo,Folio,MaquinasID ,Actividad ,User_gen,DiaHora ,User_res ,DiaHora_Compromiso ,User_asig ,DiaHora_Cierre ,DiaHora_Verificado ,Usuario_Verifico ,Comentarios,Comentarios2,Material_necesario,urgente,Estatus")] OILs OILs, bool Actividad_Finalizada)
        {
            SelectList selectList1 = new SelectList(db.Users, "UserFullName", "UserFullName", OILs.User_asig);
            ViewBag.User_asig = selectList1;

            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);

            string cuser = "Anonimo";
            if (currentUser != null)
                cuser = currentUser.UserFullName;

            if (ModelState.IsValid)
            {
                if (Actividad_Finalizada) { 
                string path = Path.Combine(Server.MapPath("~/Evidence"), "before", OILs.ID.ToString());
                string path2 = Path.Combine(Server.MapPath("~/Evidence"), "after", OILs.ID.ToString());
                if (!Directory.Exists(path2))//!Directory.Exists(path)||
                    {
                    ViewBag.Danger = "No existe Carpeta de Evidencia( Para poder cerrar un OIL hay que subir imagenes de evidencia)";
                    return View(OILs);

                }

                DirectoryInfo Folder;
                FileInfo[] Images;
                 /*   Folder = new DirectoryInfo(path);
                    Images = Folder.GetFiles();
                if (Images.Count()==0)
                {
                    ViewBag.Danger = "No existe imagenes de Evidencia( Para poder cerrar un OIL hay que subir imagenes de evidencia)";
                    return View(OILs);

                }*/
                Folder = new DirectoryInfo(path2);
                Images = Folder.GetFiles();
                if (Images.Count() == 0)
                {
                    ViewBag.Warning = "No existe imagenes de Evidencia( Para poder cerrar un OIL hay que subir imagenes de evidencia)";
                    return View(OILs);

                }

                }
                if (string.IsNullOrEmpty(OILs.User_asig))
                    OILs.User_asig = "-";

                if (Actividad_Finalizada)
                {
                    OILs.DiaHora_Cierre = DateTime.Now;
                    if (cuser != "Anonimo")
                    OILs.User_asig= cuser;
                }
                  

                db.Entry(OILs).State = EntityState.Modified;
                db.SaveChanges();
                ApplicationUser cUser1 = UserManager.Users.Where(u => u.UserFullName.Contains(OILs.User_res)).FirstOrDefault();
                ApplicationUser cUser2 = UserManager.Users.Where(u => u.UserFullName.Contains(OILs.User_asig)).FirstOrDefault();
                ApplicationUser cUser3 = UserManager.Users.Where(u => u.UserFullName.Contains(OILs.User_gen)).FirstOrDefault();
                var scorreo = new EmailController();
                if (cUser1 != null )
                {
                    if(cUser1.Email != null && cUser1.UserFullName != cuser)
                    if (cUser1.Email.Contains("@flexngate.com"))
                    {

                        scorreo.updateoil(cUser1.Email, cuser, OILs.ID.ToString(), "Actualización de Actividades");
                    }
                }
                if (cUser2 != null)
                {
                    if (cUser2.Email != null && cUser2.UserFullName != cuser)
                    if (cUser2.Email.Contains("@flexngate.com"))
                    {

                        scorreo.updateoil(cUser2.Email, cuser, OILs.ID.ToString(), "Actualización de Actividades");
                    }
                }
                if (cUser3 != null)
                {
                    if(cUser3.Email != null && cUser3.UserFullName != cuser)
                    if (cUser3.Email.Contains("@flexngate.com"))
                    {
                        scorreo.updateoil(cUser3.Email, cuser, OILs.ID.ToString(), "Actualización de Actividades");                      
                    }
                }
                return RedirectToAction("Index");
            }
            return View(OILs);
        }


        [HttpPost]
        public ActionResult fileud(OILs model, HttpPostedFileBase someFile)
        {
            if (model == null || !someFile.ContentType.Contains("image"))
            {
                ViewBag.Message = "Solo Archivos de Imagen";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          //  OILs OILs = db.OILs.Find(model.ID);
            if (someFile != null && someFile.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Evidence"), "after", model.ID.ToString());
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    path = Path.Combine(path, Path.GetFileName(someFile.FileName));
                    someFile.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Update", new { id = model.ID });
        }

        [HttpPost]
        public ActionResult fileua(OILs model, HttpPostedFileBase someFile)
        {
            if (model == null || !someFile.ContentType.Contains("image"))
            {
                ViewBag.Message = "Solo Archivos de Imagen";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // OILs OILs = db.OILs.Find(model.ID);
            if (someFile != null && someFile.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Evidence"), "before", model.ID.ToString());
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    path = Path.Combine(path, Path.GetFileName(someFile.FileName));
                    someFile.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Update", new { id = model.ID });
        }

        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = Guid.NewGuid().ToString(); //file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}Images\\uploaded", Server.MapPath(@"\")));
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");
                        var fileName1 = System.IO.Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, fName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex!=null)
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
                return Json(new { Message = fName });
            else
                return Json(new { Message = "Error in saving file" });
        }

        [HttpPost]
        public ActionResult SaveFile(int id,string t)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var path = "cool";
                    if (t == "a")
                        path = Server.MapPath($"~/Evidence/before/{id}");

                    if (t == "d")
                        path = Server.MapPath($"~/Evidence/after/{id}");

                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    path = Path.Combine(path, file.FileName);
                    file.SaveAs(path);
                    return Json(true);
                }
                else
                    return Json(false);
            }
            else
                return Json(false);

        }
        public FileResult deleteFile(string link)
        {

            string fileName2 = Server.MapPath("~/Evidence/Delete");
            string fileName = "";
            link = link.Replace(@"../../", "");
            fileName2 = fileName2 + link;
            string dirName2=Path.GetDirectoryName(fileName2);
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            if(!System.IO.Directory.Exists(dirName2))
            System.IO.Directory.CreateDirectory(dirName2);

            System.IO.File.Copy(link, fileName2);
            System.IO.File.Delete(link);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }
        public FileResult Download(string link)
        {
            string fileName = "";
            link = link.Replace(@"../../", "");
            link = Server.MapPath("~/" + link);
            fileName = Path.GetFileName(link);
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // GET: OILs/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OILs oILs = await db.OILs.FindAsync(id);
            if (oILs == null)
            {
                return HttpNotFound();
            }
            var stringA = Enum.Parse(typeof(flex_Oils), oILs.Tipo);
            ViewBag.User_res = new SelectList(db.Users, "UserFullName", "UserFullName", oILs.User_res);
            ViewBag.Tipo = new SelectList(Enum.GetValues(typeof(flex_Oils)).Cast<flex_Oils>().ToList(), stringA);
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", oILs.MaquinasID);
            return View(oILs);
        }

        // POST: OILs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Tipo,Folio,MaquinasID ,Actividad ,User_gen,DiaHora ,User_res ,DiaHora_Compromiso ,User_asig ,DiaHora_Cierre ,DiaHora_Verificado ,Usuario_Verifico ,Comentarios,Comentarios2,Material_necesario,urgente,Estatus")] OILs oILs)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(oILs.User_asig))
                    oILs.User_asig = "-";
                db.Entry(oILs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var stringA = Enum.Parse(typeof(flex_Oils), oILs.Tipo);
            ViewBag.Tipo = new SelectList(Enum.GetValues(typeof(flex_Oils)).Cast<flex_Oils>().ToList(), stringA);
            ViewBag.MaquinasID = new SelectList(db.Maquinas, "ID", "SubMaquina", oILs.MaquinasID);
            return View(oILs);
        }

        // GET: OILs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OILs oILs = await db.OILs.FindAsync(id);
            if (oILs == null)
            {
                return HttpNotFound();
            }
            return View(oILs);
        }

        // POST: OILs/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            OILs oILs = await db.OILs.FindAsync(id);
            var allcoment = db.Comments.Where(c => c.OILs.ID == id);
            db.Comments.RemoveRange(allcoment);
            db.OILs.Remove(oILs);

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
