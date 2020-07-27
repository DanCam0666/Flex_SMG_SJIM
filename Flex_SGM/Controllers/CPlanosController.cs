using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using PagedList;

namespace Flex_SGM.Controllers
{
    public class CPlanosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: controlplanos
        public ActionResult Index(int? page)
        {
            List<controlplanosview> items = new List<controlplanosview>();
          

            var alldata= db.controlplanos.ToList();
            var groupdata = alldata.GroupBy(g => g.Program);
            var gdl = groupdata.ToList();
            foreach (var data in gdl)
            {
                controlplanosview item = new controlplanosview();
                item.href = data.Key;
                item.imgurl =@"..\img\Flex.png";

                string pathserver = Path.Combine(Server.MapPath("~/BluePrints"), data.Key, data.Key + ".jpg");
                string pathclient = @"../../BluePrints/"+ data.Key+"/"+ data.Key + ".jpg";
                if (System.IO.File.Exists(pathserver))
                item.imgurl = pathclient;

                item.imgname = "BluePrints-" + data.Key;
                item.Nombreproyecto = data.Key;
                item.fechaactualizacion2d = "10/10/20";
                item.fechaactualizacion3d = "10/10/20";
                item.Descripcion = data.OrderByDescending(o=>o.ID).FirstOrDefault().Description;
                item.actualizaciones = data.Count().ToString();
                item.fechaactualizacion = data.OrderByDescending(o => o.ID).FirstOrDefault().Date.ToString("dd/MM/yyyy");
                items.Add(item);


            }

            var pageNumber = page ?? 1;
            var onePageOfProducts = items.ToPagedList(pageNumber, 12);

            ViewBag.items = onePageOfProducts;
            return View(alldata);
        }

        public ActionResult program(string program)
        {
            if (string.IsNullOrEmpty(program))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var controlplanos = db.controlplanos.Where(w=>w.Program == program);
            if (controlplanos == null)
            {
                return HttpNotFound();
            }
            return View(controlplanos);
        }

        // POST: controlplanos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult program([Bind(Include = "ID,Program,Level,InternalPN,CustomerPN,Description,Eng_Level,Revision,Date,MathData_3D,DrawingPN_2D")] controlplanos controlplanos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlplanos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(controlplanos);
        }
        [HttpPost]
        public ActionResult SaveUploadedFile(IEnumerable<HttpPostedFileBase> files)
        {
            bool SavedSuccessfully = true;
            string fName = "";
            try
            {
                //loop through all the files
                foreach (var file in files)
                {

                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {

                        var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\", Server.MapPath(@"\")));

                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        var fileName1 = Path.GetFileName(file.FileName);

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(path);

                    }

                }

            }
            catch (Exception ex)
            {
                if(ex!=null)
                SavedSuccessfully = false;
            }


            if (SavedSuccessfully)
            {
                return RedirectToAction("Index","controldeplanos",null);
            }
            else
            {
                return RedirectToAction("Index", "controldeplanos", null);
            }
        }
    
    // GET: controlplanos/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            controlplanos controlplanos = db.controlplanos.Find(id);
            if (controlplanos == null)
            {
                return HttpNotFound();
            }
            return View(controlplanos);
        }

        // GET: controlplanos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: controlplanos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Program,Level,InternalPN,CustomerPN,Description,Eng_Level,Revision,Date,MathData_3D,DrawingPN_2D")] controlplanos controlplanos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/BluePrints"), controlplanos.Program);
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);

                    path = Path.Combine(Server.MapPath("~/BluePrints"), controlplanos.Program, controlplanos.Description);
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                        ViewBag.Message = "Program Folder created successfully";
                    }
                    else
                    {
                        ViewBag.Message = "Program Folder already exists";

                    }
                    if (System.IO.Directory.Exists(path))
                    {
                        controlplanos.MathData_3D = Path.Combine(path,"3d");
                        controlplanos.DrawingPN_2D = Path.Combine(path, "2d");
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }


                db.controlplanos.Add(controlplanos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(controlplanos);
        }

        // GET: controlplanos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            controlplanos controlplanos = db.controlplanos.Find(id);
            if (controlplanos == null)
            {
                return HttpNotFound();
            }
            return View(controlplanos);
        }

        // POST: controlplanos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Program,Level,InternalPN,CustomerPN,Description,Eng_Level,Revision,Date,MathData_3D,DrawingPN_2D")] controlplanos controlplanos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controlplanos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(controlplanos);
        }

        // GET: controlplanos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            controlplanos controlplanos = db.controlplanos.Find(id);
            if (controlplanos == null)
            {
                return HttpNotFound();
            }
            return View(controlplanos);
        }

        // POST: controlplanos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            controlplanos controlplanos = db.controlplanos.Find(id);
            db.controlplanos.Remove(controlplanos);
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
