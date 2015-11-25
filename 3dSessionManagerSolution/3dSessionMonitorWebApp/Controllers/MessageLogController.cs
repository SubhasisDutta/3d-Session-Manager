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
    public class MessageLogController : BaseController
    {
        private MYSQL3DSessionEntities db = new MYSQL3DSessionEntities();

        // GET: /MessageLog/
        public ActionResult Index()
        {
            return View(db.messagelogs.OrderByDescending(r=>r.timestamp).Take(50).ToList());
        }

        // GET: /MessageLog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messagelog messagelog = db.messagelogs.Find(id);
            if (messagelog == null)
            {
                return HttpNotFound();
            }
            return View(messagelog);
        }

        // GET: /MessageLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MessageLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,externalID,timestamp,dataType,data")] messagelog messagelog)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    messagelog.timestamp = DateTime.Now;
                    db.messagelogs.Add(messagelog);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return View("Error", ex);
                }  
                return RedirectToAction("Index");
            }

            return View(messagelog);
        }

        // GET: /MessageLog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messagelog messagelog = db.messagelogs.Find(id);
            if (messagelog == null)
            {
                return HttpNotFound();
            }
            return View(messagelog);
        }

        // POST: /MessageLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,externalID,timestamp,dataType,data")] messagelog messagelog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messagelog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messagelog);
        }

        // GET: /MessageLog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            messagelog messagelog = db.messagelogs.Find(id);
            if (messagelog == null)
            {
                return HttpNotFound();
            }
            return View(messagelog);
        }

        // POST: /MessageLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            messagelog messagelog = db.messagelogs.Find(id);            
            try
            {
                db.messagelogs.Remove(messagelog);
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
