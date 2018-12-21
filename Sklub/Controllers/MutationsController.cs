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
    public class MutationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD Operations
        // GET: Mutations
        public ActionResult Index()
        {
            var mutations = db.Mutations.Include(m => m.Group).Include(m => m.Member);
            return View(mutations.ToList());
        }

        // GET: Mutations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mutation mutation = db.Mutations.Find(id);
            if (mutation == null)
            {
                return HttpNotFound();
            }
            return View(mutation);
        }

        // GET: Mutations/Create
        public ActionResult Create()
        {
            ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name");
            ViewBag.MemberID = new SelectList(db.Members, "ID", "FirstName");
            return View();
        }

        // POST: Mutations/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Triggered,HasJoined,GroupID,MemberID")] Mutation mutation)
        {
            if (ModelState.IsValid)
            {
                db.Mutations.Add(mutation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name", mutation.GroupID);
            ViewBag.MemberID = new SelectList(db.Members, "ID", "FirstName", mutation.MemberID);
            return View(mutation);
        }

        // GET: Mutations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mutation mutation = db.Mutations.Find(id);
            if (mutation == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name", mutation.GroupID);
            ViewBag.MemberID = new SelectList(db.Members, "ID", "FirstName", mutation.MemberID);
            return View(mutation);
        }

        // POST: Mutations/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Triggered,HasJoined,GroupID,MemberID")] Mutation mutation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mutation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupID = new SelectList(db.Groups, "ID", "Name", mutation.GroupID);
            ViewBag.MemberID = new SelectList(db.Members, "ID", "FirstName", mutation.MemberID);
            return View(mutation);
        }

        // GET: Mutations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mutation mutation = db.Mutations.Find(id);
            if (mutation == null)
            {
                return HttpNotFound();
            }
            return View(mutation);
        }

        // POST: Mutations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mutation mutation = db.Mutations.Find(id);
            db.Mutations.Remove(mutation);
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
        #endregion
        #region PartialActions
        public ActionResult GetAllMutationsByUserId(int id)
        {
            var mutations = db.Mutations.Where(m => m.MemberID == id).ToList();
            return PartialView(mutations);
        }
        #endregion
    }
}
