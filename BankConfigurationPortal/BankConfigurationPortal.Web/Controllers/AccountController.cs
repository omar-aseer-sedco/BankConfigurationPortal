using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Web.Models;
using System;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Controllers {
    [AllowAnonymous]
    public class AccountController : Controller {
        private readonly IUserData db;

        public AccountController() {
            db = new SqlUserData();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = "") {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, string returnUrl = "") {
            if (ModelState.IsValid) {
                if (db.ValidateUser(user)) {
                    string userData = JsonSerializer.Serialize(new SerializableUserData() { Username = user.Username, BankName = user.BankName });
                    FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddDays(1), true, userData);
                    string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));

                    if (Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Username or password is incorrect");
            return View(user);
        }

        public ActionResult Logout() {
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName) {
                Value = "",
                Expires = DateTime.Now.AddDays(-1),
            });

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
