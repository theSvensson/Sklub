using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Sklub.Models;

namespace Sklub.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD Operations
        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Account).Include(m => m.Sections).OrderBy(o => o.LastName).ThenBy(o => o.FirstName);

            ViewData["SectionList"] = db.Sections.ToList();
            return View(members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Members/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Users, "Id", "Email", member.AccountID);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Users, "Id", "Email", member.AccountID);
            return View(member);
        }

        // POST: Members/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Users, "Id", "Email", member.AccountID);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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

        #endregion

        public ActionResult Import()
        {
            string fileName = Server.MapPath("~/App_Data/Import_Jugend.xlsx");
            using (var excelWorkbook = new XLWorkbook(fileName))
            {
                var nonEmptyDataRows = excelWorkbook.Worksheet(1).RangeUsed().RowsUsed();

                foreach (var dataRow in nonEmptyDataRows)
                {
                    var gender = dataRow.Cell(1).Value;                    
                    var lastName = dataRow.Cell(2).Value;
                    var firstName = dataRow.Cell(3).Value;
                    var street = dataRow.Cell(4).Value;
                    var postalCode = dataRow.Cell(5).Value;
                    var city = dataRow.Cell(6).Value;
                    var phone = dataRow.Cell(7).Value;
                    var mobile = dataRow.Cell(8).Value;
                    var email = dataRow.Cell(9).Value;
                    var stv = dataRow.Cell(10).Value;
                    var comment = dataRow.Cell(11).Value;
                    var birthdate = dataRow.Cell(12).Value;


                    Member newMember = new Member();

                    // Gender
                    newMember.IsMale = (gender.Equals("Weiblich")) ? false : true;

                    // Name
                    newMember.LastName = lastName.ToString();
                    newMember.FirstName = firstName.ToString();

                    var nuStart = street.ToString().IndexOfAny("0123456789".ToCharArray());
                    newMember.Street = (nuStart == -1)?street.ToString():street.ToString().Substring(0, nuStart);
                    newMember.StreetNo = (nuStart != -1) ? street.ToString().Substring(nuStart, street.ToString().Length - nuStart) : null;

                    int plz;
                    newMember.PostalCode = (Int32.TryParse(postalCode.ToString(), out plz))?plz:0;

                    newMember.City = (city.ToString() != "")?city.ToString():null;
                    newMember.Phone = (phone.ToString() != "")? phone.ToString():null;
                    newMember.Mobile = (mobile.ToString() != "")? mobile.ToString():null;

                    newMember.Email = (email.ToString() != "") ? email.ToString() : null;
                    newMember.ClubNo = (stv.ToString() != "") ? stv.ToString() : null;

                    DateTime birthday;
                    newMember.Birthdate = (DateTime.TryParse(birthdate.ToString(), out birthday)) ? birthday : DateTime.Now;

                    var m = db.Members.Where(me => me.FirstName == newMember.FirstName && me.LastName == newMember.LastName).FirstOrDefault();

                    if (m == null)
                    {
                        try
                        {
                            db.Members.Add(newMember);
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                        }
                    }
                }
            }

            return View();
        }
    }
}
