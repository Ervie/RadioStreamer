using RadioStreamer.Domain;
using RadioStreamer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RadioStreamer.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            using (UserService db = new UserService())
            {


                var validUser = db.LogInUser(user);

                if (validUser != null)
                {
                    FormsAuthentication.SetAuthCookie(validUser.Login, true);

                    Session["UserId"] = validUser.Id.ToString();
                    Session["Username"] = validUser.Login.ToString();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login or Password is wrong.");
                }
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }
    }
}