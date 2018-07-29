using AppToScrape.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace AppToScrape.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() { 
            SampleData Sd = new SampleData();
            return View(Sd);
        }

        
        public ActionResult FormData()
        {
            var FD = Request.Form;
            ViewBag.Name = FD.GetValues("UserName").First();
            ViewBag.Gender = FD.GetValues("Gender").First();
            return View("~/Views/Home/PostSuccess.cshtml");
        }

        public ActionResult ViewDetail (int id)
        {
            SampleData SD = new SampleData();
            SD.SetSelected(id);
            return View(SD);
        }
    }
}