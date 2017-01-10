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

            Session["UserId"] = string.Empty;
            Session["Username"] = string.Empty;
				
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

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (UserService db = new UserService())
                {
                    if (!db.IsOccupied(user.Login, user.Email))
                    {
                        db.RegisterUser(user);
                        ViewBag.Message = user.Login + " succesfully registered.";
                    }
                    else
                    {
                        ModelState.AddModelError("Occupied", "Login or e-mail is already taken.");
                        ViewBag.Message = "Login or e-mail is already taken.";
                    }

                }
                ModelState.Clear();

                
            }

            return View();
        }
    }
}