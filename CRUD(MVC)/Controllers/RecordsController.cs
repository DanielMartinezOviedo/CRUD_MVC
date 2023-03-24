using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
		public ActionResult Index(string search)
		{
			/*List<ListRecordsViewModels> list;
			using (mydbEntities db = new mydbEntities())
			{
				if (!string.IsNullOrEmpty(search))
				{
					list = db.Database.SqlQuery<ListRecordsViewModels>("EXEC sp_SearchRecords @Name", new SqlParameter("@Name", search)).ToList();
				}
				else
				{
					list = db.Database.SqlQuery<ListRecordsViewModels>(
						"EXEC sp_GetRecords"
					).ToList();
				}
			}
			return View(list);*/
			List<ListRecordsViewModels> list;
			using (mydbEntities db = new mydbEntities())
			{
				var records = from d in db.Records
							  select new ListRecordsViewModels
							  {
								  Id = d.Personid.ToString(),
								  Name = d.FirstName,
								  LastName = d.LastName,
								  Email = d.Email,
							  };

				if (!string.IsNullOrEmpty(Request.QueryString["search"]))
				{
					string searching = Request.QueryString["search"];
					records = records.Where(r => r.Name.Contains(searching) || r.LastName.Contains(searching));
				}

				list = records.ToList();
			}

			ViewBag.Title = "Index";

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
				if (ModelState.IsValid)
				{
					using (mydbEntities db = new mydbEntities())
					{
						var firstNameParam = new SqlParameter("@firstName", model.Name);
						var lastNameParam = new SqlParameter("@lastName", model.LastName);
						var emailParam = new SqlParameter("@email", model.Email);
						var dateOfBirthParam = new SqlParameter("@dateOfBirth", model.DateOfBirth);

						db.Database.ExecuteSqlCommand("exec sp_InsertRecord @firstName, @lastName, @email, @dateOfBirth",
							firstNameParam, lastNameParam, emailParam, dateOfBirthParam);
					}
					return RedirectToAction("Index");
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
			using (mydbEntities db = new mydbEntities())
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
						var idParam = new SqlParameter("@id", model.Id);
						var firstNameParam = new SqlParameter("@firstName", model.Name);
						var lastNameParam = new SqlParameter("@lastName", model.LastName);
						var emailParam = new SqlParameter("@email", model.Email);
						var dateOfBirthParam = new SqlParameter("@dateOfBirth", model.DateOfBirth);

						db.Database.ExecuteSqlCommand("exec sp_UpdateRecord @id, @firstName, @lastName, @email, @dateOfBirth",
							idParam, firstNameParam, lastNameParam, emailParam, dateOfBirthParam);
					}
					return RedirectToAction("Index");
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
				var idParam = new SqlParameter("@id", id);
				db.Database.ExecuteSqlCommand("exec sp_DeleteRecord @id", idParam);
			}
			return RedirectToAction("Index");
		}
	}
}