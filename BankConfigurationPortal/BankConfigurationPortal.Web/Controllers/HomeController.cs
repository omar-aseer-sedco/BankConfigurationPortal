using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [AllowAnonymous]
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult ToggleLanguage() {
            string currentLanguage = "en";
            var languageCookie = HttpContext.Request.Cookies["language"];
            if (languageCookie != null) {
                currentLanguage = languageCookie.Value;
            }

            string newLanguage = currentLanguage == "en" ? "ar" : "en";

            Response.Cookies.Remove("language");
            Response.Cookies.Add(new HttpCookie("language") {
                Value = newLanguage,
                Expires = DateTime.Now.AddYears(1),
                HttpOnly = false,
            });

            var cultureInfo = new CultureInfo(newLanguage);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
