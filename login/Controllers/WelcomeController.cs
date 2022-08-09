using login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace login.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        public Employee_ProfileEntities1 db = new Employee_ProfileEntities1();
        
        [HttpGet]
        public ActionResult WelcomePage()
        {
            return View(db.Employee_details.ToList());

        }
    }
}

