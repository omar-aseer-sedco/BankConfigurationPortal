using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

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
                if (OwinCookieAuthorizationAttribute.IsUserAuthenticated(HttpContext, db)) {
                    if (Url.IsLocalUrl(HttpUtility.UrlDecode(returnUrl))) {
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
                        if (Request.Cookies.AllKeys.Contains(".AspNet.ApplicationCookie")) {
                            Response.Cookies.Remove(".AspNet.ApplicationCookie");
                        }
                        
                        byte[] rngBytes = new byte[4];
                        RandomNumberGenerator.Create().GetBytes(rngBytes);
                        int userSessionId = BitConverter.ToInt32(rngBytes, 0);

                        Session["UserSessionId"] = userSessionId;

                        Session session = new Session() { 
                            Username = user.Username,
                            SessionId = userSessionId,
                            Expires = DateTime.Now.AddMinutes(30),
                            UserAgent = Request.UserAgent,
                            IpAddress = Request.UserHostAddress
                        };

                        if (db.SetSession(session) != 1) { // failed to save the session
                            Session["UserSessionId"] = null;
                            return View("Error");
                        }

                        var claims = new[] {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim("BankName", user.BankName),
                            new Claim("UserSessionId", userSessionId.ToString()),
                            new Claim("UserAgent", Request.UserAgent),
                            new Claim("IpAddress", Request.UserHostAddress),
                        };
                        var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                        Request.GetOwinContext().Authentication.SignIn(identity);

                        if (Url.IsLocalUrl(HttpUtility.UrlDecode(returnUrl))) {
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
                if (!OwinCookieAuthorizationAttribute.IsUserAuthenticated(HttpContext, db)) {
                    return RedirectToAction("Index", "Home");
                }

                var claimsIdentity = User.Identity as ClaimsIdentity;
                int userSessionId = int.Parse(claimsIdentity.FindFirst("UserSessionId").Value);
                db.DeleteSession(userSessionId);

                Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
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
