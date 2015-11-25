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
    public class LocationController : BaseController
    {
        private MYSQL3DSessionEntities db = new MYSQL3DSessionEntities();

        // GET: /Location/
        public ActionResult Index()
        {
            var locations = db.locations.Include(l => l.instance).Include(l => l.setup);
            return View(locations.ToList());
        }

        // GET: /Location/AvailableInstance/5
        public ActionResult AvailableInstance(int? id)
        {
            var locations = db.locations.Where(a=>a.setupId == id).ToList();
            return View(locations);
        }

        // GET: /Location/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: /Location/Create
        public ActionResult Create()
        {
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId");
            ViewBag.setupId = new SelectList(db.setups, "id", "name");
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,setupId,instanceId,name,description,creationTimestamp")] location location)
        {
            if (ModelState.IsValid)
            {
                db.locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", location.instanceId);
            ViewBag.setupId = new SelectList(db.setups, "id", "name", location.setupId);
            return View(location);
        }

        // GET: /Location/AddInstance/5
        public ActionResult AddInstance(int? id)
        {
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId");
            //ViewBag.setupId = new SelectList(db.setups, "id", "name");
            ViewBag.setup = db.setups.Find(id);
            return View();
        }

        // POST: /Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInstance([Bind(Include = "id,setupId,instanceId,name,description,creationTimestamp")] location location,int? id)
        {
            if (ModelState.IsValid)
            {
                location.setupId = (int)id;
                location.creationTimestamp = DateTime.Now;
                db.locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("AvailableInstance", new { id=id});
            }
                        
            return RedirectToAction("Index","Setup");
        }


        // GET: /Location/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", location.instanceId);
            ViewBag.setupId = new SelectList(db.setups, "id", "name", location.setupId);
            return View(location);
        }

        // POST: /Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,setupId,instanceId,name,description,creationTimestamp")] location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", location.instanceId);
            ViewBag.setupId = new SelectList(db.setups, "id", "name", location.setupId);
            return View(location);
        }

        // GET: /Location/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: /Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            location location = db.locations.Find(id);
            db.locations.Remove(location);
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
