using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebBasedSystem.Models;

using WebBasedSystem.ViewModels;
using LinqToExcel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace WebBasedSystem.Controllers
{
    public class AdminController : Controller
    {
        //changes
        WebBaseSystemEntities db = new WebBaseSystemEntities();
        // GET: AdminDashboard
        public ActionResult Dashboard()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AutoCompleteState(string prefix)
        {
           
            var waypoints = (from waypoint in db.WayPoints
                             where waypoint.State.StartsWith(prefix)
                             select new
                             {
                                 label = waypoint.State,
                                 val = waypoint.Id
                             }).ToList();

            return Json(waypoints);
        }

        [HttpPost]
        public JsonResult AutoCompletePostCode(string prefix)
        {

            var waypoints = (from waypoint in db.WayPoints
                             where waypoint.PostCode.StartsWith(prefix)
                             select new
                             {
                                 label = waypoint.PostCode,
                                 val = waypoint.Id
                             }).ToList();

            return Json(waypoints);
        }

        [HttpPost]
        public JsonResult AutoCompleteSuburbs(string prefix)
        {

            var waypoints = (from waypoint in db.WayPoints
                             where waypoint.Suburbs.StartsWith(prefix)
                             select new
                             {
                                 label = waypoint.Suburbs,
                                 val = waypoint.Id
                             }).ToList();

            return Json(waypoints);
        }
        public ActionResult AddSpecialistType()
        {
            List<SpecialistTypeModel> listEmp = db.SpecialistTypes.Select(x => new SpecialistTypeModel
            {
                SpecialistTypeId = x.SpecialistTypeId,
                SpecialistTypeName = x.SpecialistTypeName,

            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }
   


        [HttpPost]
        public ActionResult AddSpecialistType(SpecialistTypeModel model)
        {
            try
            {


                SpecialistType emp = new SpecialistType();
                emp.SpecialistTypeName = model.SpecialistTypeName;
     

                db.SpecialistTypes.Add(emp);
                db.SaveChanges();

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult GetWayPoints(int Id)
        {
            //Specialist model = db.Specialists.Where(x => x.SpecialistId == SpecialistId).SingleOrDefault();

            //string value = string.Empty;
            //value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});

            //return Json(value, JsonRequestBehavior.AllowGet);

            //UrlHelper(Request.RequestContext).Action("ProjectExport", "Project");

            //?SpecialistId = "
            return Json(new { redirecturl = "http://localhost:50311/Admin/UpdateWayPoints?id=" + Id }, JsonRequestBehavior.AllowGet);

            //return Json(Url = redirectUrl, response = message,
            //      responseData = JsonConvert.SerializeObject(vmOneSheet) };
        }
        public JsonResult GetSpecialist(int SpecialistId)
        {
            //Specialist model = db.Specialists.Where(x => x.SpecialistId == SpecialistId).SingleOrDefault();

            //string value = string.Empty;
            //value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});

            //return Json(value, JsonRequestBehavior.AllowGet);

            //UrlHelper(Request.RequestContext).Action("ProjectExport", "Project");
            //?SpecialistId = "
            return Json(new { redirecturl = "http://localhost:50311/Admin/UpdateSpecialist?id="+SpecialistId }, JsonRequestBehavior.AllowGet);

            //return Json(Url = redirectUrl, response = message,
            //      responseData = JsonConvert.SerializeObject(vmOneSheet) };
    }


        public JsonResult Get(int SpecialistTypeId)
        {
            SpecialistType model = db.SpecialistTypes.Where(x => x.SpecialistTypeId == SpecialistTypeId).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(SpecialistType employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteEmployee(int SpecialistTypeId)
        {
            bool result = false;
            var emp = db.SpecialistTypes.SingleOrDefault(x => x.SpecialistTypeId == SpecialistTypeId);
            if (emp != null)
            {
                //emp.IsDeleted = true;
                db.SpecialistTypes.Remove(emp);

                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSepecialist(int SpecialistId)
        {

            //   SpecialistType model = db.SpecialistTypes.Where(x => x.SpecialistTypeId == SpecialistTypeId).SingleOrDefault();
            Specialist model = db.Specialists.Where(x => x.SpecialistId == SpecialistId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddSpecialistTypeSubMenu()
        {

            List<SpecialistType> list = db.SpecialistTypes.ToList();
            ViewBag.DepartmentList = new SelectList(list, "SpecialistTypeId", "SpecialistTypeName");

            List<SpecialistTypeSubMenuModel> listEmp = db.SpecialistTypeSubMenus.Select(x => new SpecialistTypeSubMenuModel
            {
                SubMenuId = x.SubMenuId,
                SubMenuName = x.SubMenuName,
                SpecialistTypeName = x.SpecialistType.SpecialistTypeName
            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }
        [HttpPost]
        public ActionResult AddSpecialistTypeSubMenu(SpecialistTypeSubMenuModel model)
        {
            try
            {

                List<SpecialistType> list = db.SpecialistTypes.ToList();
                ViewBag.DepartmentList = new SelectList(list, "SpecialistTypeId", "SpecialistTypeName");

                SpecialistTypeSubMenu emp = new SpecialistTypeSubMenu();
                emp.SubMenuName = model.SubMenuName;
                emp.SpecialistTypeId = model.SpecialistTypeId;



                db.SpecialistTypeSubMenus.Add(emp);
                db.SaveChanges();

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult GetSubMenu(int SubMenuId)
        {
            SpecialistTypeSubMenu model = db.SpecialistTypeSubMenus.Where(x => x.SubMenuId == SubMenuId).SingleOrDefault();

            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSubMenu(SpecialistTypeSubMenu employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteSubMenu(int SubMenuId)
        {
            bool result = false;
            var emp = db.SpecialistTypeSubMenus.SingleOrDefault(x => x.SubMenuId == SubMenuId);
            if (emp != null)
            {
                //emp.IsDeleted = true;
                db.SpecialistTypeSubMenus.Remove(emp);

                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMenuList(int SpecialistTypeId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<SpecialistTypeSubMenu> StateList = db.SpecialistTypeSubMenus.Where(x => x.SpecialistTypeId == SpecialistTypeId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Specialist()
        {
            List<SpecialistModel> ObjList = new List<SpecialistModel>()
            {

                new SpecialistModel {TitleId=1,Title="Mr" },
                new SpecialistModel {TitleId=2,Title="Mrs" },
                new SpecialistModel {TitleId=3,Title="Dr" },
                new SpecialistModel {TitleId=4,Title="Others" }
             

        };
            ViewBag.Title = new SelectList(ObjList, "TitleId", "Title");

            List<SpecialistType> CountryList = db.SpecialistTypes.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SpecialistTypeId", "SpecialistTypeName");
            List<SpecialistModel> noteslist = new List<SpecialistModel>()
            {

                new SpecialistModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new SpecialistModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new SpecialistModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
            ViewBag.WaitingTime = new SelectList(noteslist, "Waiting_Time_Id", "Waiting_Time");
            return View();
        }

        [HttpPost]
        public ActionResult Specialist(SpecialistModel model)
        {
            try
            {


                List<SpecialistModel> ObjList = new List<SpecialistModel>()
            {

                new SpecialistModel {TitleId=1,Title="Mr" },
                new SpecialistModel {TitleId=2,Title="Mrs" },
                new SpecialistModel {TitleId=3,Title="Dr" },
                new SpecialistModel {TitleId=4,Title="Others" }


        };
                {
                    List<SpecialistModel> noteslist = new List<SpecialistModel>()
            {

                new SpecialistModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new SpecialistModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new SpecialistModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
                    ViewBag.WaitingTime = new SelectList(noteslist, "Waiting_Time_Id", "Waiting_Time");
                    var findtitle = ObjList.Where(x => x.TitleId == model.TitleId).FirstOrDefault();


                    var submenu = db.SpecialistTypeSubMenus.FirstOrDefault(x => x.SubMenuId == model.SubMenuId);


                    var getwaitingtime = noteslist.Where(x => x.Waiting_Time_Id == model.Waiting_Time_Id).FirstOrDefault();

                    Specialist spec = new Specialist();

                    spec.Title = findtitle.Title;
                    spec.SpecialistName = model.SpecialistName;
                    spec.MobileNo = model.MobileNo;
                    spec.PhoneNo = model.PhoneNo;
                    spec.Address1 = model.Address1;
                    spec.Age = model.Age;
                    spec.Suburbs = model.Suburbs;
                    spec.State = model.State;
                    spec.PostCode = model.PostCode;
                    spec.SpecialistTypeId = model.SpecialistTypeId;
                    spec.SubMenu = submenu.SubMenuName;
                    spec.Address2 = model.Address2;
                    spec.EmailAddress = model.EmailAddress;
                    spec.BusinessName = model.BusinessName;
                    spec.Surname = model.Surname;
                    spec.OfficeNo = model.OfficeNo;
                    spec.IsDeleted = false;
                    spec.NotesDescription = model.NotesDescription;
                    spec.TakingNewPatients = model.TakingNewPatients;
                    spec.price = model.price;
                    spec.Waiting_Time = getwaitingtime.Waiting_Time;
                    spec.IsBooking = model.IsBooking;

                    db.Specialists.Add(spec);
                    db.SaveChanges();


                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult AllSpecialist()
        {

            List<SpecialistModel> listEmp = db.Specialists.Where(x => x.IsDeleted == false).Select(x => new SpecialistModel
            {
                SpecialistId = x.SpecialistId,
                Title = x.Title,
                SpecialistName = x.SpecialistName,
                MobileNo = x.MobileNo,
                PhoneNo = x.PhoneNo,
                PostCode = x.PostCode,
                State = x.State,
                Suburbs = x.Suburbs,
                Address1 = x.Address1,
                Address2 = x.Address2,
                EmailAddress = x.EmailAddress,
                BusinessName = x.BusinessName,
                Surname = x.Surname,
                OfficeNo = x.OfficeNo,
                SubMenu = x.SubMenu,
                Age = x.Age

            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }


        public JsonResult DeleteSpecialist(int SpecialistId)
        {
            bool result = false;

            var emp = db.Specialists.SingleOrDefault(x => x.IsDeleted == false && x.SpecialistId == SpecialistId);
            if (emp != null)
            {
                emp.IsDeleted = true;
                //db.Specialists.Remove(emp);

                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSpecialist(int id)
        {

            List<SpecialistModel> noteslist = new List<SpecialistModel>()
            {

                new SpecialistModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new SpecialistModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new SpecialistModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
            ViewBag.WaitingTime = new SelectList(noteslist, "Waiting_Time_Id", "Waiting_Time");



   
            List<SpecialistModel> ObjList = new List<SpecialistModel>()
            {

                new SpecialistModel {TitleId=1,Title="Mr" },
                new SpecialistModel {TitleId=2,Title="Mrs" },
                new SpecialistModel {TitleId=3,Title="Dr" },
                new SpecialistModel {TitleId=4,Title="Others" }


        };
            ViewBag.Title = new SelectList(ObjList, "TitleId", "Title");

            List<SpecialistType> CountryList = db.SpecialistTypes.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SpecialistTypeId", "SpecialistTypeName");

            List<SpecialistTypeSubMenu> submenu = db.SpecialistTypeSubMenus.ToList();
            ViewBag.SubMenu = new SelectList(submenu, "SubMenuId", "SubMenuName");
            SpecialistModel row = db.Specialists.Where(model => model.SpecialistId == id)
            .Select(x => new SpecialistModel
            {

                NotesDescription = x.NotesDescription,
                TakingNewPatients = x.TakingNewPatients,
                price = x.price,
                SpecialistId = x.SpecialistId,
                //Waiting_Time = x.Waiting_Time,
                //SpecialistId = x.SpecialistId,
                IsBooking = x.IsBooking,
                SpecialistTypeId=x.SpecialistTypeId,
                IsDeleted = x.IsDeleted,
            Title = x.Title,
                SpecialistName = x.SpecialistName,
                MobileNo = x.MobileNo,
                PhoneNo = x.PhoneNo,
                Address1 = x.Address1,
                Address2 = x.Address2,
                PostCode=x.PostCode,
                Suburbs=x.Suburbs,
                State=x.State,
                Age = x.Age,
                EmailAddress = x.EmailAddress,
                BusinessName = x.BusinessName,
                Surname = x.Surname,
                OfficeNo = x.OfficeNo,
                SubMenu = x.SubMenu,
            }).FirstOrDefault();
            var record = new SpecialistModel();
            foreach (var e in ObjList)
            {
                record = db.Specialists.Where(x => x.Title == e.Title).Select(x => new SpecialistModel
                {
                    TitleId = e.TitleId

                }).FirstOrDefault();
                //row.StateId=record.state.
                if (record != null)
                {
                    break;
                }
            }
            row.TitleId = record.TitleId;


            var record2 = db.SpecialistTypeSubMenus.FirstOrDefault(x => x.SubMenuName ==row.SubMenu );
            row.SubMenuId = record2.SubMenuId;



            foreach (var e in noteslist)
            {
                record = db.Specialists.Where(x => x.Waiting_Time == e.Waiting_Time).Select(x => new SpecialistModel
                {
                    Waiting_Time_Id = e.Waiting_Time_Id

                }).FirstOrDefault();
                //row.StateId=record.state.
                if (record != null)
                {
                    break;
                }
            }
            row.Waiting_Time_Id = record.Waiting_Time_Id;
            return View(row);
        }

        [HttpPost]
        public ActionResult UpdateSpecialist(SpecialistModel employee)
        {
            try
            {
                List<SpecialistModel> notelist = new List<SpecialistModel>()
            {

                new SpecialistModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new SpecialistModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new SpecialistModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
                var getwaitingtime = notelist.FirstOrDefault(x => x.Waiting_Time_Id == employee.Waiting_Time_Id);
                List<SpecialistModel> ObjList = new List<SpecialistModel>()
            {

                new SpecialistModel {TitleId=1,Title="Mr" },
                new SpecialistModel {TitleId=2,Title="Mrs" },
                new SpecialistModel {TitleId=3,Title="Dr" },
                new SpecialistModel {TitleId=4,Title="Others" }


        };
                var gettitle = ObjList.FirstOrDefault(x => x.TitleId == employee.TitleId);
                var submenu = db.SpecialistTypeSubMenus.FirstOrDefault(x => x.SubMenuId == employee.SubMenuId);

                Models.Specialist sp = new Models.Specialist();
                if (ModelState.IsValid)
                {
                    sp.SpecialistId = employee.SpecialistId;
                    sp.SpecialistTypeId = employee.SpecialistTypeId;
                    sp.Title = gettitle.Title;
                    sp.SpecialistName = employee.SpecialistName;
                    sp.MobileNo = employee.MobileNo;
                    sp.PhoneNo = employee.PhoneNo;
                    sp.Address1 = employee.Address1;
                    sp.Address2 = employee.Address2;
                    sp.PostCode = employee.PostCode;
                    sp.Suburbs = employee.Suburbs;
                    sp.State = employee.State;
                    sp.Age = employee.Age;
                    sp.EmailAddress = employee.EmailAddress;
                    sp.IsDeleted = employee.IsDeleted;

                    sp.BusinessName = employee.BusinessName;
                    sp.Surname = employee.Surname;
                    sp.OfficeNo = employee.OfficeNo;
                    sp.SubMenu = submenu.SubMenuName;
                    sp.NotesDescription = employee.NotesDescription;
                    sp.TakingNewPatients = employee.TakingNewPatients;
                    sp.price = employee.price;
                    sp.SpecialistId = employee.SpecialistId;
                    sp.Waiting_Time = getwaitingtime.Waiting_Time;
                    sp.IsBooking = employee.IsBooking;
                    db.Entry(sp).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {

            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewWayPoints()
        {

            return View();
        }


        [HttpPost]
        public ActionResult UploadExcelWayPoints(HttpPostedFileBase postedFile)
        {

            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Doc/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.WayPoints";

                        //[OPTIONAL]: Map the Excel columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("PostCode", "PostCode");
                        sqlBulkCopy.ColumnMappings.Add("Latitude", "Latitude");
                        sqlBulkCopy.ColumnMappings.Add("Longitude", "Longitude");
                        sqlBulkCopy.ColumnMappings.Add("Suburbs", "Suburbs");
                        sqlBulkCopy.ColumnMappings.Add("State", "State");


                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return RedirectToAction("AllWaysPoints");
        
        }
        public ActionResult AddWayPoints()
        {


            List<WayPointsModel> ObjList = new List<WayPointsModel>()
            {

                new WayPointsModel {Id=1,State="ACT" },
                new WayPointsModel {Id=2,State="Mumbai" },
                new WayPointsModel {Id=3,State="Pune" },
                new WayPointsModel {Id=4,State="Delhi" },
                new WayPointsModel {Id=5,State="Dehradun" },
                new WayPointsModel {Id=6,State="Noida" },
                new WayPointsModel {Id=7,State="New Delhi" }

           };
          
            ViewBag.objlist = new SelectList(ObjList,"Id","State");
            List<WayPointsModel> listEmp = db.WayPoints.Select(x => new WayPointsModel
            {
                Id = x.Id,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Suburbs = x.Suburbs,
                PostCode = x.PostCode,
                
                //State = x.State,

            }).ToList();
            ViewBag.EmployeeList = listEmp;
            return View();
        }



        [HttpPost]
        public ActionResult AddWayPoints(WayPointsModel model)
        {
            try
            {
                List<WayPointsModel> ObjList = new List<WayPointsModel>()
            {

                new WayPointsModel {Id=1,State="ACT" },
                new WayPointsModel {Id=2,State="Mumbai" },
                new WayPointsModel {Id=3,State="Pune" },
                new WayPointsModel {Id=4,State="Delhi" },
                new WayPointsModel {Id=5,State="Dehradun" },
                new WayPointsModel {Id=6,State="Noida" },
                new WayPointsModel {Id=7,State="New Delhi" }

           };
                var statename = ObjList.Where(x => x.Id == model.Id).FirstOrDefault();

                WayPoint emp = new WayPoint();
                emp.Latitude = model.Latitude;
                emp.Longitude = model.Longitude;
                emp.Suburbs = model.Suburbs;
                emp.PostCode = model.PostCode;
                emp.State = statename.State;


                db.WayPoints.Add(emp);
                db.SaveChanges();

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult AllWaysPoints()
        {
            List<WayPointsModel> listEmp = db.WayPoints.Select(x => new WayPointsModel
            {
                Id = x.Id,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Suburbs = x.Suburbs,
                PostCode = x.PostCode,
                State = x.State,

            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }
       

        public JsonResult DeleteWayPoints(int Id)
        {
            bool result = false;
            var emp = db.WayPoints.SingleOrDefault(x => x.Id == Id);
            if (emp != null)
            {
                //emp.IsDeleted = true;
                db.WayPoints.Remove(emp);

                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetWayPoints(int NoteId)
        //{
        //    //Specialist model = db.Specialists.Where(x => x.SpecialistId == SpecialistId).SingleOrDefault();

        //    //string value = string.Empty;
        //    //value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
        //    //{
        //    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    //});

        //    //return Json(value, JsonRequestBehavior.AllowGet);

        //    //UrlHelper(Request.RequestContext).Action("ProjectExport", "Project");
        //    //?SpecialistId = "
        //    return Json(new { redirecturl = "http://localhost:50311/Admin/UpdateWayPoints?id=" + NoteId }, JsonRequestBehavior.AllowGet);

        //    //return Json(Url = redirectUrl, response = message,
        //    //      responseData = JsonConvert.SerializeObject(vmOneSheet) };
        //}


        public ActionResult UpdateWayPoints(int id)
        {
            List<WayPointsModel> ObjList = new List<WayPointsModel>()
            {

                new WayPointsModel {StateId=1,State="ACT" },
                new WayPointsModel {StateId=2,State="Mumbai" },
                new WayPointsModel {StateId=3,State="Pune" },
                new WayPointsModel {StateId=4,State="Delhi" },
                new WayPointsModel {StateId=5,State="Dehradun" },
                new WayPointsModel {StateId=6,State="Noida" },
                new WayPointsModel {StateId=7,State="New Delhi" }

           };

            //List<WayPoint> CountryList = db.WayPoints.ToList();
            //ViewBag.CountryList = new SelectList(CountryList, "Id", "Latitude");
            ViewBag.objlist = new SelectList(ObjList, "StateId", "State");
            WayPointsModel row = db.WayPoints.Where(model => model.Id == id)
            .Select(x => new WayPointsModel
            {
                Id = x.Id,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Suburbs = x.Suburbs,
                PostCode = x.PostCode,
                //State = x.State,
            }).FirstOrDefault();
            var record = new WayPointsModel();
            foreach (var e in ObjList)
            {
                record = db.WayPoints.Where(x => x.State == e.State).Select(x => new WayPointsModel
                {
                    StateId = e.StateId

                }).FirstOrDefault();
                //row.StateId=record.state.
                if (record != null)
                {
                    break;
                }
            }
            row.StateId = record.StateId;
            //ViewBag.objlist = new SelectList(ObjList, "StateId", "State");

            return View(row);
        }

        [HttpPost]
        public ActionResult UpdateWayPoints(WayPointsModel employee)
        {
            try
            {
                List<WayPointsModel> ObjList = new List<WayPointsModel>()
            {

                new WayPointsModel {StateId=1,State="ACT" },
                new WayPointsModel {StateId=2,State="Mumbai" },
                new WayPointsModel {StateId=3,State="Pune" },
                new WayPointsModel {StateId=4,State="Delhi" },
                new WayPointsModel {StateId=5,State="Dehradun" },
                new WayPointsModel {StateId=6,State="Noida" },
                new WayPointsModel {StateId=7,State="New Delhi" }

           };
                var getstate = ObjList.FirstOrDefault(x => x.StateId == employee.StateId);

                WayPoint w = new WayPoint();
                if (ModelState.IsValid)
                {
                    w.Id = employee.Id;
                    //w.Id = employee.Id;
                    w.Latitude = employee.Latitude;
                    w.Longitude = employee.Longitude;
                   w.Suburbs = employee.Suburbs;
                    w.State = getstate.State;
                w.PostCode = employee.PostCode;


                    db.Entry(w).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }


        public List<WayPointsModel> City()
        {
            List<WayPointsModel> ObjList = new List<WayPointsModel>()
            {

                new WayPointsModel {Id=1,State="Latur" },
                new WayPointsModel {Id=2,State="Mumbai" },
                new WayPointsModel {Id=3,State="Pune" },
                new WayPointsModel {Id=4,State="Delhi" },
                new WayPointsModel {Id=5,State="Dehradun" },
                new WayPointsModel {Id=6,State="Noida" },
                new WayPointsModel {Id=7,State="New Delhi" }

           };

            ViewBag.objlist = ObjList;
            return ObjList;

        }


        public ActionResult AddNotes()
        {
            List<NotesModel> ObjList = new List<NotesModel>()
            {

                new NotesModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new NotesModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new NotesModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
            ViewBag.WaitingTime = new SelectList(ObjList, "Waiting_Time_Id", "Waiting_Time");


            List<Specialist> specialisit = db.Specialists.ToList();
            ViewBag.Specialist = new SelectList(specialisit, "SpecialistId", "SpecialistName");



            List<NotesModel> listEmp = db.Notes.Select(x => new NotesModel
            {



                NoteId = x.NoteId,
                NotesDescription = x.NotesDescription,
                TakingNewPatients = x.TakingNewPatients,
                price = x.price,
                //Waiting_Time = x.Waiting_Time,
                //SpecialistId = x.SpecialistId,
                IsBooking = x.IsBooking




            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }



        [HttpPost]
        public ActionResult AddNotes(NotesModel model)
        {
            try
            {


                List<NotesModel> ObjList = new List<NotesModel>()
            {

                new NotesModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new NotesModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new NotesModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
                var getwaitingtime = ObjList.Where(x => x.Waiting_Time_Id == model.Waiting_Time_Id).FirstOrDefault();
                Note emp = new Note();
                emp.NoteId = model.NoteId;
                emp.NotesDescription = model.NotesDescription;
                emp.TakingNewPatients = model.TakingNewPatients;
                emp.price = model.price;
                emp.Waiting_Time = getwaitingtime.Waiting_Time;
                emp.SpecialistId = model.SpecialistId;
                emp.IsBooking = model.IsBooking;
                db.Notes.Add(emp);
                db.SaveChanges();

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult AllNotes()
        {
            List<NotesModel> listEmp = db.Notes.Select(x => new NotesModel
            {
                NoteId=x.NoteId,
                NotesDescription = x.NotesDescription,
                TakingNewPatients = x.TakingNewPatients,
                price = x.price,
                Waiting_Time = x.Waiting_Time,
                IsBooking = x.IsBooking,
      
            }).ToList();
            ViewBag.EmployeeList = listEmp;

            return View();
        }

        public JsonResult DeleteNotes(int NoteId)
        {
            bool result = false;
            var emp = db.Notes.SingleOrDefault(x => x.NoteId == NoteId);
            if (emp != null)
            {
                //emp.IsDeleted = true;
                db.Notes.Remove(emp);

                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNotes(int NoteId)
        {
            //Specialist model = db.Specialists.Where(x => x.SpecialistId == SpecialistId).SingleOrDefault();

            //string value = string.Empty;
            //value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});

            //return Json(value, JsonRequestBehavior.AllowGet);

            //UrlHelper(Request.RequestContext).Action("ProjectExport", "Project");
            //?SpecialistId = "
            return Json(new { redirecturl = "http://localhost:50311/Admin/UpdateNotes?id=" + NoteId }, JsonRequestBehavior.AllowGet);

            //return Json(Url = redirectUrl, response = message,
            //      responseData = JsonConvert.SerializeObject(vmOneSheet) };
        }


        public ActionResult UpdateNotes(int id)
        {
            List<NotesModel> ObjList = new List<NotesModel>()
            {

                new NotesModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new NotesModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new NotesModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
            ViewBag.WaitingTime = new SelectList(ObjList, "Waiting_Time_Id", "Waiting_Time");


            List<Specialist> specialisit = db.Specialists.ToList();
            ViewBag.Specialist = new SelectList(specialisit, "SpecialistId", "SpecialistName");

     

            NotesModel row = db.Notes.Where(model => model.NoteId == id)
            .Select(x => new NotesModel
            {
                NoteId = x.NoteId,
                NotesDescription = x.NotesDescription,
                TakingNewPatients = x.TakingNewPatients,
                price = x.price,
                SpecialistId=x.SpecialistId,
                //Waiting_Time = x.Waiting_Time,
                //SpecialistId = x.SpecialistId,
                IsBooking = x.IsBooking
            }).FirstOrDefault();
            var record = new NotesModel();
            foreach (var e in ObjList)
            {
                record = db.Notes.Where(x => x.Waiting_Time == e.Waiting_Time).Select(x => new NotesModel
                {
                    Waiting_Time_Id = e.Waiting_Time_Id

                }).FirstOrDefault();
                //row.StateId=record.state.
                if (record != null)
                {
                    break;
                }
            }
            row.Waiting_Time_Id = record.Waiting_Time_Id;


            //var record = db.Notes.FirstOrDefault(x => x.NotesDescription == row.NotesDescription);
            //row.NoteId = record.NoteId;
            return View(row);
        }

        [HttpPost]
        public ActionResult UpdateNotes(NotesModel employee)
        {
            try
            {
                List<NotesModel> ObjList = new List<NotesModel>()
            {

                new NotesModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
                new NotesModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
                new NotesModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


        };
                var getwaitingtime = ObjList.FirstOrDefault(x => x.Waiting_Time_Id == employee.Waiting_Time_Id);

                Note note = new Note();
                if (ModelState.IsValid)
                {

                    note.NoteId = employee.NoteId;
                    note.NotesDescription = employee.NotesDescription;
                    note.TakingNewPatients = employee.TakingNewPatients;
                    note.price = employee.price;
                    note.SpecialistId = employee.SpecialistId;
                    note.Waiting_Time = getwaitingtime.Waiting_Time;
                    note.IsBooking = employee.IsBooking;
                    db.Entry(note).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }





        //public ActionResult AllWaysPoints()
        //{
        //    List<SpecialistModel> listEmp = db.Specialists.Select(x => new SpecialistModel
        //    {
        //        SpecialistId = x.SpecialistId,
        //        Title = x.Title,
        //        SpecialistName = x.SpecialistName,
        //        MobileNo = x.MobileNo,
        //        PhoneNo = x.PhoneNo,
        //        Address1 = x.Address1,
        //        Address2 = x.Address2,
        //        EmailAddress = x.EmailAddress,
        //        BusinessName = x.BusinessName,
        //        Surname = x.Surname,
        //        OfficeNo = x.OfficeNo,
        //        SubMenu = x.SubMenu,
        //        // Age = Convert.ToInt32(x.Age)

        //    }).ToList();
        //    ViewBag.EmployeeList = listEmp;

        //    return View();
        //}

        //[HttpPost]
        //public JsonResult UploadExcelWayPoints(HttpPostedFileBase FileUpload)
        //{

        //    List<string> data = new List<string>();
        //    if (FileUpload != null)
        //    {
        //        // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
        //        if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //        {


        //            string filename = FileUpload.FileName;
        //            string targetpath = Server.MapPath("~/Doc/");
        //            FileUpload.SaveAs(targetpath + filename);
        //            string pathToExcelFile = targetpath + filename;
        //            var connectionString = "";
        //            if (filename.EndsWith(".xls"))
        //            {
        //                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
        //            }
        //            else if (filename.EndsWith(".xlsx"))
        //            {
        //                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
        //            }

        //            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
        //            var ds = new DataSet();

        //            adapter.Fill(ds, "ExcelTable");

        //            DataTable dtable = ds.Tables["ExcelTable"];

        //            string sheetName = "Sheet1";

        //            var excelFile = new ExcelQueryFactory(pathToExcelFile);
        //            var listCourses = from c in excelFile.Worksheet<WayPoint>(sheetName) select c;

        //            //  BEGIN - Clearing up the existing data in the table before Inserting the records.
        //            WebBaseSystemEntities cleanTableEntities = new WebBaseSystemEntities();
        //            var objContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)cleanTableEntities).ObjectContext;
        //            objContext.ExecuteStoreCommand("Truncate table WayPoints");
        //            //  END
        //            var list = db.WayPoints.ToList();
        //            db.WayPoints.RemoveRange(list);
        //            foreach (var c in listCourses)
        //            {
        //                try
        //                {


        //                    //if (a.Name != "" && a.Address != "" && a.ContactNo != "")
        //                    if (c.PostCode != "")
        //                    {

        //                        //User TU = new User();
        //                        WayPoint TC = new WayPoint();
        //                        TC.Latitude = c.Latitude;
        //                        TC.Longitude = c.Longitude;
        //                        TC.PostCode = c.PostCode;
        //                        TC.Suburbs = c.Suburbs;
        //                        TC.State = c.State;

        //                        db.SaveChanges();
        //                        db.WayPoints.Add(TC);
        //                        db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        return Json(data, JsonRequestBehavior.AllowGet);
        //                    }
        //                }

        //                catch (DbEntityValidationException ex)
        //                {

        //                }
        //            }
        //            //deleting excel file from folder  
        //            if ((System.IO.File.Exists(pathToExcelFile)))
        //            {
        //                System.IO.File.Delete(pathToExcelFile);
        //            }
        //            return Json("success", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            //alert message for invalid file format  
        //            data.Add("<ul>");
        //            data.Add("<li>Only Excel file format is allowed</li>");
        //            data.Add("</ul>");
        //            data.ToArray();
        //            return Json(data, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        data.Add("<ul>");
        //        if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
        //        data.Add("</ul>");
        //        data.ToArray();
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public ActionResult ShowWayPoints()
        //{
        //    List<WayPoint> listEmp = db.WayPoints.Select(x => new WayPointsModel
        //    {
        //        SpecialistId = x.SpecialistId,
        //        Title = x.Title,
        //        SpecialistName = x.SpecialistName,
        //        MobileNo = x.MobileNo,
        //        PhoneNo = x.PhoneNo,
        //        Address1 = x.Address1,
        //        Address2 = x.Address2,
        //        EmailAddress = x.EmailAddress,
        //        BusinessName = x.BusinessName,
        //        Surname = x.Surname,
        //        OfficeNo = x.OfficeNo,
        //        SubMenu = x.SubMenu,
        //        // Age = Convert.ToInt32(x.Age)

        //    }).ToList();
        //    ViewBag.EmployeeList = listEmp;

        //    return View();
        //}
    }
}