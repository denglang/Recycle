using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;

namespace Recycle.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            //return RedirectToAction("Index", "MyController"); //redirect to view in another controler 
            // return View("MapView");
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Iowa DNR Recycles.";

            return View();
        }
        //[Authorize]  this will force user to log in first before being able to see the contact view
        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult GoToMap()
        {

            //StreamReader sr = new StreamReader(@"C:\MyRepo\Recycle\Recycle\Data\Material4Search.txt");

            //DataTable dt = new DataTable();

            //dt.Columns.Add(new DataColumn("StatesTextField", typeof(String)));
            //dt.Columns.Add(new DataColumn("StatesValueField", typeof(String)));

            //string strInput = sr.ReadLine();

            //while(strInput != "EOF")
            //{
                
            //}

            return Redirect("../Home/MapView");
        }
        [Authorize]
        public ActionResult MapView()
        {
            return View();
        }

    }
}