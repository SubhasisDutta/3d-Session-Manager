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
    public class SessionController : BaseController
    {
        private MYSQL3DSessionEntities db = new MYSQL3DSessionEntities();

        // GET: /Session/
        public ActionResult Index()
        {
            var sessions = db.sessions.Include(s => s.setup);
            return View(sessions.ToList());
        }

        // GET: /Session/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            session session = db.sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: /Session/Create
        public ActionResult Create()
        {
            ViewBag.setupId = new SelectList(db.setups, "id", "name");
            return View();
        }

        // POST: /Session/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,setupId,name,description,startTime,endTime,isActive")] session session)
        {
            if (ModelState.IsValid)
            {                
                try
                {
                    if (session.isActive == true)
                    {
                        session.startTime = DateTime.Now;
                        session.endTime = null;
                    }                    
                    else
                    {
                        session.startTime = null;
                        session.endTime = null;
                    }

                    db.sessions.Add(session);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return View("Error", ex);
                }
                return RedirectToAction("Index");
            }

            ViewBag.setupId = new SelectList(db.setups, "id", "name", session.setupId);
            return View(session);
        }

        // GET: /Session/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            session session = db.sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            ViewBag.setupId = new SelectList(db.setups, "id", "name", session.setupId);
            return View(session);
        }

        // POST: /Session/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,setupId,name,description,startTime,endTime,isActive")] session session)
        {
            if (ModelState.IsValid)
            {

                
                try
                {
                    if (session.isActive == true)
                    {
                        session.startTime = DateTime.Now;
                    }
                    else if (session.isActive == false)
                    {
                        session.endTime = DateTime.Now;
                    }
                    else
                    {
                        session.startTime = null;
                        session.endTime = null;
                    }

                    db.Entry(session).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return View("Error", ex);
                }
                return RedirectToAction("Index");
            }
            ViewBag.setupId = new SelectList(db.setups, "id", "name", session.setupId);
            return View(session);
        }

        // GET: /Session/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            session session = db.sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: /Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            session session = db.sessions.Find(id);           
            try
            {
                db.sessions.Remove(session);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
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
