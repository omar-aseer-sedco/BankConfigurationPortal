using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Controllers {
    [AllowAnonymous]
    public class AccountController : Controller {
        private readonly IUserData db;

        public AccountController(IUserData db) {
            try {
                this.db = db;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = "") {
            try {
                if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName)) {
                    if (Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewBag.Title = WebResources.Login;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Login(User user, string returnUrl = "") {
            try {
                if (ModelState.IsValid) {
                    if (db.ValidateUser(user)) {
                        if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName)) {
                            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                        }

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

                    ModelState.AddModelError("", "Bank name, username, or password is incorrect");
                }

                return View(user);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Logout() {
            try {
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName) {
                    Value = "",
                    Expires = DateTime.Now.AddDays(-1),
                });

                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
