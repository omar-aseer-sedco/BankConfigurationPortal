using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using System.Text.Json;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Controllers {
    public class BaseController : Controller {
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            bool isRtl = false;
            var languageCookie = filterContext.HttpContext.Request.Cookies["language"];
            if (languageCookie != null && languageCookie.Value == Languages.ARABIC) {
                isRtl = true;
            }
            ViewBag.IsRtl = isRtl;

            var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            var sessionId = filterContext.HttpContext.Session["UserSessionId"];
            string username = null, bankName = null;
            if (authCookie != null && sessionId != null) {
                SerializableUserData userData = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(authCookie.Value).UserData, JsonSerializerOptions.Default);
                if (userData.UserSessionId == (int) sessionId) {
                    username = userData.Username;
                    bankName = userData.BankName;
                }
            }
            ViewBag.Username = username;
            ViewBag.BankName = bankName;

            base.OnActionExecuting(filterContext);
        }
    }
}
