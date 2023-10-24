using BankConfigurationPortal.Utils.Helpers;
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

        private bool IsAuthenticated() {
            try {
                if (Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName) && Session["UserSessionId"] != null) {
                    var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    int cookieId = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(authCookie.Value).UserData, JsonSerializerOptions.Default).UserSessionId;

                    return cookieId == (int) Session["UserSessionId"];
                }
                else {
                    return false;
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return default;
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = "") {
            try {
                if (IsAuthenticated()) {
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

                        byte[] rngBytes = new byte[4];
                        RandomNumberGenerator.Create().GetBytes(rngBytes);
                        int userSessionId = BitConverter.ToInt32(rngBytes, 0);

                        string userData = JsonSerializer.Serialize(new SerializableUserData() { Username = user.Username, BankName = user.BankName, UserSessionId = userSessionId });
                        FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddDays(1), true, userData);
                        string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));
                        Session["UserSessionId"] = userSessionId;

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

        public ActionResult Logout() {
            try {
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
