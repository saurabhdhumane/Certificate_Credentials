using Certificate_Credentials.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Certificate_Credentials.Controllers
{
    public class AccountController : Controller
    {
        private credentials_DBEntities dbContext = new credentials_DBEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            // Validate user credentials
            if (IsValidUser(user))
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return RedirectToAction("Index", "credentials"); // Redirect to the home page or another secure page
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home"); // Redirect to the home page or another page
        }


        private bool IsValidUser(User user)
        {
            // Replace this with actual database validation logic
            var dbUser = dbContext.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            return dbUser != null;
        }

    }
}