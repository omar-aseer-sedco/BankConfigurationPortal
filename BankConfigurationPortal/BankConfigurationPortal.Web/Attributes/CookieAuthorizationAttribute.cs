using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Models;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Attributes {
    public class CookieAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter {
        public void OnAuthorization(AuthorizationContext filterContext) {
            try {
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
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }
        
        private bool IsUserAuthenticated(HttpContextBase context) {
            try {
                if (context.Session["UserSessionId"] == null) {
                    return false;
                }

                if (context.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName)) {
                    var authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                    int cookieId = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(authCookie.Value).UserData, JsonSerializerOptions.Default).UserSessionId;

                    return cookieId == (int) context.Session["UserSessionId"];
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
    }
}
