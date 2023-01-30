using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_MVC_.Models;
using CRUD_MVC_.Models.ViewModels;

namespace CRUD_MVC_.Controllers
{
    public class RecordsController : Controller
    {
        // GET: Records
        public ActionResult Index()
        {
            List<ListRecordsViewModels> list;
            using (mydbEntities db = new mydbEntities())
            {
                 list = (from d in db.Records
                            select new ListRecordsViewModels
                            {
                                Id = d.Personid,
                                Name = d.FirstName,
                                LastName = d.LastName,
                                Email = d.Email,
                            }).ToList();

            }
            return View(list);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(RecordsViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    using( mydbEntities db = new mydbEntities())
                    {
                        var oElement = new Records();
                        oElement.FirstName = model.Name;
                        oElement.LastName = model.LastName;
                        oElement.Email = model.Email;
                        oElement.Date_of_birth = model.DateOfBirth;

                        db.Records.Add(oElement);
                        db.SaveChanges();
                    }
                    return Redirect("~/Records/");
                }
                return View(model);

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message); 
            }
        }

        public ActionResult Update(int id)
        {
            RecordsViewModel model = new RecordsViewModel();
            using( mydbEntities db = new mydbEntities())
            {
                var oRecord = db.Records.Find(id);
                model.Name = oRecord.FirstName;
                model.LastName = oRecord.LastName;
                model.Email = oRecord.Email;
                model.DateOfBirth = (DateTime)oRecord.Date_of_birth;
                model.Id = oRecord.Personid;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(RecordsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (mydbEntities db = new mydbEntities())
                    {
                        var oRecord = db.Records.Find(model.Id);
                        oRecord.FirstName = model.Name;
                        oRecord.LastName = model.LastName;
                        oRecord.Email = model.Email;
                        oRecord.Date_of_birth = model.DateOfBirth;

                        db.Entry(oRecord).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Redirect("~/Records/");
                }
                return View(model);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Delete(int id)
        {
            using (mydbEntities db = new mydbEntities())
            {
                var oRecord = db.Records.Find(id);
                db.Records.Remove(oRecord);
                db.SaveChanges();
            }
            return Redirect("~/Records/");
        }


    }
}