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
    public class SessionDataController : BaseController
    {
        private MYSQL3DSessionEntities db = new MYSQL3DSessionEntities();

        // GET: /SessionData/
        public ActionResult Index()
        {
            var sessiondatas = db.sessiondatas.Include(s => s.instance).Include(s => s.session);
            return View(sessiondatas.ToList());
        }

        // GET: /SessionData/AvailableSessionData/5
        public ActionResult AvailableSessionData(int? id)
        {
            var sessiondatas = db.sessiondatas.Where(a=>a.sessionId == id).ToList();
            return View(sessiondatas);
        }

        // GET: /SessionData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sessiondata sessiondata = db.sessiondatas.Find(id);
            if (sessiondata == null)
            {
                return HttpNotFound();
            }
            return View(sessiondata);
        }

        // GET: /SessionData/Create
        public ActionResult Create()
        {
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId");
            ViewBag.sessionId = new SelectList(db.sessions, "id", "name");
            return View();
        }

        // POST: /SessionData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,sessionId,instanceId,dataType,data,dataBlob,timeStamp,processedData")] sessiondata sessiondata)
        {
            if (ModelState.IsValid)
            {
                db.sessiondatas.Add(sessiondata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", sessiondata.instanceId);
            ViewBag.sessionId = new SelectList(db.sessions, "id", "name", sessiondata.sessionId);
            return View(sessiondata);
        }

        // GET: /SessionData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sessiondata sessiondata = db.sessiondatas.Find(id);
            if (sessiondata == null)
            {
                return HttpNotFound();
            }
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", sessiondata.instanceId);
            ViewBag.sessionId = new SelectList(db.sessions, "id", "name", sessiondata.sessionId);
            return View(sessiondata);
        }

        // POST: /SessionData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,sessionId,instanceId,dataType,data,dataBlob,timeStamp,processedData")] sessiondata sessiondata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sessiondata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.instanceId = new SelectList(db.instances, "id", "externalId", sessiondata.instanceId);
            ViewBag.sessionId = new SelectList(db.sessions, "id", "name", sessiondata.sessionId);
            return View(sessiondata);
        }

        // GET: /SessionData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sessiondata sessiondata = db.sessiondatas.Find(id);
            if (sessiondata == null)
            {
                return HttpNotFound();
            }
            return View(sessiondata);
        }

        // POST: /SessionData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sessiondata sessiondata = db.sessiondatas.Find(id);
            db.sessiondatas.Remove(sessiondata);
            db.SaveChanges();
            return RedirectToAction("Index", "Session");
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
