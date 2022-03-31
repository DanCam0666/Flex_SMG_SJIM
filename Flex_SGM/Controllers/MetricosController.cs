using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flex_SGM.Controllers
{
    public class MetricosController : Controller
    {
        // GET: Metricos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Metricos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Metricos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Metricos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Metricos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Metricos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Metricos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Metricos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
