using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Constants;
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
                        if (Request.Cookies.AllKeys.Contains(AuthenticationConstants.AUTHENTICATION_COOKIE_NAME)) {
                            Response.Cookies.Remove(AuthenticationConstants.AUTHENTICATION_COOKIE_NAME);
                        }
                        
                        byte[] rngBytes = new byte[4];
                        RandomNumberGenerator.Create().GetBytes(rngBytes);
                        int userSessionId = BitConverter.ToInt32(rngBytes, 0);

                        Session[AuthenticationConstants.USER_SESSION_ID] = userSessionId;

                        Session session = new Session() { 
                            Username = user.Username,
                            SessionId = userSessionId,
                            Expires = DateTime.Now.AddMinutes(30),
                            UserAgent = Request.UserAgent,
                            IpAddress = Request.UserHostAddress
                        };

                        if (db.SetSession(session) != 1) { // failed to save the session
                            Session[AuthenticationConstants.USER_SESSION_ID] = null;
                            return View("Error");
                        }

                        var claims = new[] {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(AuthenticationConstants.BANK_NAME, user.BankName),
                            new Claim(AuthenticationConstants.USER_SESSION_ID, userSessionId.ToString()),
                            new Claim(AuthenticationConstants.USER_AGENT, Request.UserAgent),
                            new Claim(AuthenticationConstants.IP_ADDRESS, Request.UserHostAddress),
                        };
                        var identity = new ClaimsIdentity(claims, AuthenticationConstants.AUTHENTICATION_TYPE);
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
                int userSessionId = int.Parse(claimsIdentity.FindFirst(AuthenticationConstants.USER_SESSION_ID).Value);
                db.DeleteSession(userSessionId);

                Request.GetOwinContext().Authentication.SignOut(AuthenticationConstants.AUTHENTICATION_TYPE);
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
