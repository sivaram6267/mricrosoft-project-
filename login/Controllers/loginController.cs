using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using login.Models;
using System.Security;
using System.Web.Security;

namespace login.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Userlogin login)
        {
            Employee_ProfileEntities1 db = new Employee_ProfileEntities1();
            if (ModelState.IsValid)
            {


                var user = (from userlist in db.Userlogins
                            where userlist.UserName == login.UserName && userlist.Password == login.Password
                            select new
                            {
                                userlist.UserId,
                                userlist.UserName
                            }).ToList();


                bool IsValidUser = db.Userlogins.Any(x => x.UserName.ToLower() ==
                            login.UserName.ToLower() && x.Password == login.Password);
                if (IsValidUser)
                {
                    FormsAuthentication.SetAuthCookie(login.UserName, false);
                    return RedirectToAction("WelcomePage", "Welcome");
                }
                else
                {
                    ModelState.AddModelError("", "invalid Username or Password");
                }
               
            }
            return View(login);
        }

        
    }
}
    