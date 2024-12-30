using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eTaxAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            ViewBag.Title = "eTaxAPI Management API";

            return View();
        }
    }
}
