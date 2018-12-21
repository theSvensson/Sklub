using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sklub.Models;

namespace Sklub.Controllers
{
    public class SectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sections
        public ActionResult Index()
        {
            return View(db.Sections.ToList());
        }

        public ActionResult ManageSection(int id)
        {
            var section = db.Sections.Find(id);
            var membersIn = section.Members.ToList();
            var membersAll = db.Members.ToList();
            var membersOut = membersAll.Except(membersIn).ToList(); 

            ViewData["MembersIn"] = membersIn;
            ViewData["MembersOut"] = membersOut;

            return View(section);
        }

        #region public voids
        [HttpPost]
        public ActionResult AddUsersToSection(FormCollection form, int sectionID)
        {
            string[] ids = form["ID"].Split(new char[] { ',' });

            var section = db.Sections.Where(s => s.ID == sectionID).FirstOrDefault();
            var members = db.Members.Where(m => ids.Contains(m.ID.ToString())).ToList();

            if (section != null && members != null)
            {
                foreach(var m in members)
                {
                    m.Sections.Add(section);
                    db.Entry(m).State = EntityState.Modified;
                    db.SaveChanges();
                }                
            }

            return RedirectToAction("ManageSection",new { id = sectionID });
        }

        public void RemoveUsersToSection(FormCollection form, int sectionID)
        {
            string[] ids = form["ID"].Split(new char[] { ',' });

            var section = db.Sections.Where(s => s.ID == sectionID).FirstOrDefault();
            var members = db.Members.Where(m => ids.Contains(m.ID.ToString())).ToList();

            if (section != null && members != null)
            {
                foreach (var m in members)
                {
                    m.Sections.Remove(section);
                    db.Entry(m).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(section);
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(section);
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = db.Sections.Find(id);
            db.Sections.Remove(section);
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
