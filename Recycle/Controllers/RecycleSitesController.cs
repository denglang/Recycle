using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recycle.Models;

namespace Recycle.Controllers
{
    public class RecycleSitesController : Controller
    {
        [Authorize]
        public ViewResult Index()
        {
            //ViewBag.Countries = new List<string>()
            //{
            //    "India","US","Canada","UK"
            //};
            RecycleContext rContext = new RecycleContext();

            List<RecycleSites> rSites = rContext.RecycleSites.ToList();
           return View(rSites);
        }
        
        // GET: RecycleSites
        public ActionResult Details(int Id)
        {
            
            RecycleContext rContext = new RecycleContext();

           RecycleSites rSite = rContext.RecycleSites.Single(site => site.ObjectID == Id);

            return View(rSite);
        }

        [HttpGet]
       
        public ActionResult Edit(int id)
        {
            RecycleContext rContext = new RecycleContext();

            RecycleSites rSite = rContext.RecycleSites.Single(site => site.ObjectID == id);

            return View(rSite);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_Post(RecycleSites rSite)
            //use bind Include or Exclude to determine which data can be updated to database 
         //public ActionResult Edit_Post([Bind(Exclude="Member_Name")]RecycleSites rSite)
        {
            //the following Update will control which field can be posted to the server, in case, tools like Fiddler will 
            //use post to change data by dragging the post request from the web sessiosn into Composer, edit the Request body and execute it
            //UpdateModel(rSite, new string[] { "Member_Name", "Contact_Email", "Phone_Number" });

            if (ModelState.IsValid) { 
                RecycleContext rContext = new RecycleContext();
                rContext.SaveRecycleSite(rSite);
                return RedirectToAction("Index"); //when update successful, return the list of all sites
            }
            return View(rSite); //if update err,  return the same edit view for correction
        }

        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [Authorize]
        //public ActionResult Create_Post(FormCollection formCollection)
        public ActionResult Create_Post()
        //we can use RecycleSites directly, but the recycleContext model will return error if any parameters is empty.
        // to avoid that, we have to set every paramater to null in SQL Stored Procedure like: @website nvarchar(100)=null
        //FormCollection will not give any error if we left any param empty 
        //public ActionResult Create(RecycleSites site)
        {
            //foreach (string key in formCollection.AllKeys)
            //{
            //    Response.Write("Key = " + key + " ");
            //    Response.Write(formCollection[key]);
            //    Response.Write("<br>");
            //}

            //RecycleSites site = new RecycleSites();
            //site.Member_Name = formCollection["Member_Name"];
            //site.Contact_Email = formCollection["Contact_Email"];
            //site.Phone_Number = formCollection["Phone_Number"];
            //site.Website = formCollection["Website"];
            //site.Street = formCollection["Street"];
            //site.City = formCollection["City"];
            //site.Zip_Code = Convert.ToInt32(formCollection["Zip_Code"]);
            //site.County = formCollection["County"];
            //site.State = formCollection["State"];
            //site.Status_1 = formCollection["Status_1"];
            //site.Profile_Hours = formCollection["Profile_Hours"];
            //site.Construction_Demolition = formCollection["Construction_Demolition"];
            //site.Hazardous_Waste = formCollection["Hazardous_Waste"];
            //site.Organics = formCollection["Organics"];
            //site.Solid_Waste = formCollection["Solid_Waste"];
            //site.Electronics = formCollection["Electronics"];
            //site.Address_Website = formCollection["Address_Website"];

            if (ModelState.IsValid) {

                RecycleSites site = new RecycleSites();
                TryUpdateModel(site); //take all input from create form to put into site
                RecycleContext recycleCon = new RecycleContext();
                recycleCon.AddRecycleSite(site);

                return RedirectToAction("Index");
            }
            return View(); //return the same create view for user to correct if there is problem saving the new site

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            RecycleContext rSite = new RecycleContext();
            rSite.DeleteRecycleSite(id);
            return RedirectToAction("Index");
        }
    }
}