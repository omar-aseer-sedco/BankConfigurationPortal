using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Attributes {
    public class CookieAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationContext filterContext) {
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            filterContext.HttpContext.Response.Cache.SetNoStore();
            filterContext.HttpContext.Response.AppendHeader("Pragma", "no-cache");

            if (!IsUserAuthenticated(filterContext.HttpContext)) {
                var returnUrl = new StringBuilder($"returnUrl={filterContext.HttpContext.Request.Path}");
                var queryString = filterContext.HttpContext.Request.QueryString;
                if (queryString.Count > 0) {
                    returnUrl.Append('?').Append(queryString);
                }

                filterContext.Result = new RedirectResult($"/Account/Login?{returnUrl}");
            }
        }

        private bool IsUserAuthenticated(HttpContextBase context) {
            return context.Request.Cookies[FormsAuthentication.FormsCookieName] != null;
        }
    }
}
