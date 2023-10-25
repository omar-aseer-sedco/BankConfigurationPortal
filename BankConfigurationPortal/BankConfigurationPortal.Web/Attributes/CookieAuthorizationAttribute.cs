using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Attributes {
    public class CookieAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter {
        private readonly IUserData db;

        public CookieAuthorizationAttribute() : base() {
            db = new SqlUserData(); // TODO: maybe figure out dependency injection for this
        }

        public void OnAuthorization(AuthorizationContext filterContext) {
            try {
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                filterContext.HttpContext.Response.Cache.SetNoStore();
                filterContext.HttpContext.Response.AppendHeader("Pragma", "no-cache");

                if (!IsUserAuthenticated(filterContext.HttpContext, db)) {
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
        
        public static bool IsUserAuthenticated(HttpContextBase context, IUserData db) {
            try {
                if (!context.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName) || context.Session["UserSessionId"] == null) {
                    return false;
                }

                var userData = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(context.Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData, JsonSerializerOptions.Default);
                string username = userData.Username;
                int cookieId = userData.UserSessionId;

                Session session = db.GetSession(cookieId);

                if (session == null) {
                    return false;
                }

                if (session.Expires <= DateTime.Now) {
                    db.DeleteSession(cookieId);
                    return false;
                }

                return cookieId == (int) context.Session["UserSessionId"] && username == session.Username && context.Request.UserAgent == session.UserAgent && context.Request.UserHostAddress == session.IpAddress;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return default;
            }
        }
    }
}
