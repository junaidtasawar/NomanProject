using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedSystem.Models;
using WebBasedSystem.ViewModels;

namespace WebBasedSystem.Controllers
{
    public class StandardUserController : Controller
    {
        // GET: StandardUser
        /// <summary>
        //
        /// </summary>
        WebBaseSystemEntities db = new WebBaseSystemEntities();
        public ActionResult Dashboard()
        {
            //    List<SpecialistModel> ObjList = new List<SpecialistModel>()
            //    {

            //        new SpecialistModel {TitleId=1,Title="Mr" },
            //        new SpecialistModel {TitleId=2,Title="Mrs" },
            //        new SpecialistModel {TitleId=3,Title="Dr" },
            //        new SpecialistModel {TitleId=4,Title="Others" }


            //};
            //    ViewBag.Title = new SelectList(ObjList, "TitleId", "Title");

            //    List<SpecialistType> CountryList = db.SpecialistTypes.ToList();
            //    ViewBag.CountryList = new SelectList(CountryList, "SpecialistTypeId", "SpecialistTypeName");
            //    List<SpecialistModel> noteslist = new List<SpecialistModel>()
            //    {

            //        new SpecialistModel {Waiting_Time_Id=1,Waiting_Time="0-3 month" },
            //        new SpecialistModel {Waiting_Time_Id=2,Waiting_Time="3-6 month" },
            //        new SpecialistModel {Waiting_Time_Id=3,Waiting_Time="upto 1 year" },


            //};
            //    ViewBag.WaitingTime = new SelectList(noteslist, "Waiting_Time_Id", "Waiting_Time");

            
            List<SpecialistType> CountryList = db.SpecialistTypes.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SpecialistTypeId", "SpecialistTypeName");
            //IEnumerable<SelectListItem> state = db.WayPoints.Select(s => new SelectListItem {Value=(s.Id.ToString()), Text = (s.State) .ToString() }).Distinct().FirstOrDefault();

            var state = db.WayPoints.GroupBy(t => t.State).Select(x => x.FirstOrDefault()).ToList();

            var suburbs = db.WayPoints.GroupBy(t => t.Suburbs).Select(x => x.FirstOrDefault()).ToList();
            var postcodes = db.WayPoints.GroupBy(t => t.PostCode).Select(x => x.FirstOrDefault()).ToList();

            ViewBag.State = new SelectList(state, "Id", "State");
            ViewBag.Suburbs = new SelectList(state, "Id", "Suburbs");
            ViewBag.PostCode = new SelectList(state, "Id", "PostCode");

            return View();
        }
        public JsonResult GetPostCode(string SuburbsId)
        {
            WayPoint model = new WayPoint();
            try
            {
                model = db.WayPoints.Where(x => x.Suburbs == SuburbsId).Distinct().ToList().Select(x => new WayPoint
                {
                    PostCode = x.PostCode,
                    Id = x.Id

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetSubUrbs(string stateid)
        {
            List<WayPoint> model = new List<WayPoint>();
            try
            {
       model = db.WayPoints.Where(x => x.State == stateid).Distinct().ToList().Select(x => new WayPoint
                {
                    Suburbs = x.Suburbs,
                    Id = x.Id

                }).ToList();

            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllSpecialist(StandardUserModel model)
        {
            //
            var contacts = db.Specialists.Where(x=>x.SpecialistTypeId==x.SpecialistTypeId && x.State==model.waypoints.State && x.Suburbs==model.waypoints.Suburbs).Select(x => new
            {
                Id = x.SpecialistId,
                Name = x.SpecialistName,
                //model.specialistType. = x.MobileNumber
            }).ToList(); // <--- cast to list if GetUserContacts returns an IEnumerable
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getJSONMemberList()
        {
            return Json(new
            {
                aaData = new[]
                    {
                    //Hard coded data here that I want to replace with query results
                    new MemberList { ID = "000001", FIRST_NAME = "James", LAST_NAME = "Smith" },
                    new MemberList { ID = "000003", FIRST_NAME = "David", LAST_NAME = "Aaronson" },
                    new MemberList { ID = "000005", FIRST_NAME = "Jim", LAST_NAME = "Thompson" }
                }
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetJsonData(StandardUserModel model)
        {
            var submenufind = db.SpecialistTypeSubMenus.Where(x=>x.SubMenuId==model.specialistTypesubmenu.SubMenuId).FirstOrDefault();
            var waypointsfind = db.WayPoints.Where(x => x.Id == model.waypoints.Id).FirstOrDefault();
            List<SpecialistModel> data = new List<SpecialistModel>();

            
            List<SpecialistModel> specialistfirstlist = db.Specialists.Where(x => x.SubMenu == submenufind.SubMenuName && x.PostCode == waypointsfind.PostCode && x.State == waypointsfind.State && x.Suburbs == waypointsfind.Suburbs).ToList().Select(x => new SpecialistModel() { Latitude = waypointsfind.Latitude, Longitude = waypointsfind.Longitude, Title = x.Title, SpecialistName = x.SpecialistName, Surname = x.Surname, Address1 = x.Address1 + x.Address2, MobileNo = x.MobileNo, PhoneNo = x.PhoneNo, EmailAddress = x.EmailAddress, State = x.State, TakingNewPatients = x.TakingNewPatients, IsBooking = x.IsBooking, Waiting_Time = x.Waiting_Time, price = x.price, Suburbs = x.Suburbs }).ToList();
         
            if (specialistfirstlist!=null)
            {
                data=specialistfirstlist;
       
            }
            else
            {

                DbHandleWayPoints dvp = new DbHandleWayPoints();
                WayPoint wp = dvp.GetNearestWayPoint(waypointsfind.Latitude, waypointsfind.Longitude);
                List<SpecialistModel> specialistsecondtlist = db.Specialists.Where(x => x.SubMenu == submenufind.SubMenuName && x.PostCode == wp.PostCode && x.State == wp.State && x.Suburbs == wp.Suburbs).ToList() .Select(x => new SpecialistModel() {Latitude=wp.Latitude,Longitude=wp.Longitude,Title=x.Title, SpecialistName=x.SpecialistName,Surname=x.Surname,Address1=x.Address1+x.Address2,MobileNo=x.MobileNo,PhoneNo=x.PhoneNo, EmailAddress = x.EmailAddress,State=x.State, TakingNewPatients=x.TakingNewPatients,IsBooking=x.IsBooking,Waiting_Time=x.Waiting_Time,price=x.price,Suburbs=x.Suburbs }).ToList();

                data = specialistsecondtlist;
          
            }
            //
//
            //  var submenu=model.waypoints.Id
            //var specialistmodel=db.Specialists.Where(x=>x.)
            //
          
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
    
}