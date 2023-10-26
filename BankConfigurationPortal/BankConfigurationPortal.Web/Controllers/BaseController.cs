using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using System.Security.Claims;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class BaseController : Controller {
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            bool isRtl = false;
            var languageCookie = filterContext.HttpContext.Request.Cookies["language"];
            if (languageCookie != null && languageCookie.Value == Languages.ARABIC) {
                isRtl = true;
            }
            ViewBag.IsRtl = isRtl;

            string username = null, bankName = null;
            var sessionId = filterContext.HttpContext.Session["UserSessionId"];
            if (User != null && sessionId != null) {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                int userSessionId = int.Parse(claimsIdentity.FindFirst("UserSessionId").Value);

                if (userSessionId == (int) sessionId) {
                    username = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
                    bankName = claimsIdentity.FindFirst("BankName").Value;
                }
            }
            ViewBag.Username = username;
            ViewBag.BankName = bankName;

            if (!WindowsLogsHelper.IsLogSourceInitialized()) {
                filterContext.Result = View("LogsNotInitializedError");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
