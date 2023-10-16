using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Attributes {
    public class CookieAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationContext filterContext) {
            if (!IsUserAuthenticated(filterContext.HttpContext)) {
                filterContext.Result = new RedirectResult($"/Account/Login?returnUrl={filterContext.HttpContext.Request.Path}");
            }
        }

        private bool IsUserAuthenticated(HttpContextBase context) {
            return context.Request.Cookies[FormsAuthentication.FormsCookieName] != null;
        }
    }
}
