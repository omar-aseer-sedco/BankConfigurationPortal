using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Controllers {
    [AllowAnonymous]
    public class AccountController : BaseController {
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
                if (CookieAuthorizationAttribute.IsUserAuthenticated(HttpContext, db)) {
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
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string returnUrl = "") {
            try {
                if (ModelState.IsValid) {
                    if (db.ValidateUser(user)) {
                        if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName)) {
                            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                        }
                        
                        byte[] rngBytes = new byte[4];
                        RandomNumberGenerator.Create().GetBytes(rngBytes);
                        int userSessionId = BitConverter.ToInt32(rngBytes, 0);

                        string userData = JsonSerializer.Serialize(new SerializableUserData() { Username = user.Username, BankName = user.BankName, UserSessionId = userSessionId });
                        FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddDays(1), true, userData);
                        string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                        Session["UserSessionId"] = userSessionId;

                        Session session = new Session() { 
                            Username = user.Username,
                            SessionId = userSessionId,
                            Expires = DateTime.Now.AddMinutes(30),
                            UserAgent = Request.UserAgent,
                            IpAddress = Request.UserHostAddress
                        };

                        if (db.SetSession(session) != 1) { // failed to save the session
                            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                            Session["UserSessionId"] = null;
                            return View("Error");
                        }

                        if (Url.IsLocalUrl(returnUrl)) {
                            return Redirect(returnUrl);
                        }
                        else {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    ModelState.AddModelError("", WebResources.IncorrectCredentials);
                }

                return View(user);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout() {
            try {
                if (!CookieAuthorizationAttribute.IsUserAuthenticated(HttpContext, db)) {
                    return RedirectToAction("Index", "Home");
                }

                int cookieId = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData, JsonSerializerOptions.Default).UserSessionId;
                db.DeleteSession(cookieId);

                Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName) {
                    Value = "",
                    Expires = DateTime.Now.AddDays(-1),
                });

                FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
