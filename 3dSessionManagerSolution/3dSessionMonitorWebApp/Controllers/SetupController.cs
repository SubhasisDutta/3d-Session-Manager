using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _3dSessionMonitorWebApp;

namespace _3dSessionMonitorWebApp.Controllers
{
    public class SetupController : BaseController
    {
        private MYSQL3DSessionEntities db = new MYSQL3DSessionEntities();

        // GET: /Setup/
        public ActionResult Index()
        {
            return View(db.setups.ToList());
        }

        // GET: /Setup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setup setup = db.setups.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        // GET: /Setup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Setup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,name,description,creationTimestamp")] setup setup)
        {
            if (ModelState.IsValid)
            {
                db.setups.Add(setup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setup);
        }

        // GET: /Setup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setup setup = db.setups.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        // POST: /Setup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,name,description,creationTimestamp")] setup setup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setup);
        }

        // GET: /Setup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            setup setup = db.setups.Find(id);
            if (setup == null)
            {
                return HttpNotFound();
            }
            return View(setup);
        }

        // POST: /Setup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            setup setup = db.setups.Find(id);
            db.setups.Remove(setup);
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
