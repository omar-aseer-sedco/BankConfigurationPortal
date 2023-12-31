﻿using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Attributes {
    public class OwinCookieAuthorizationAttribute : AuthorizeAttribute {
        private readonly IUserData db;

        public OwinCookieAuthorizationAttribute() : base() {
            try {
                db = new SqlUserData();
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext) {
            try {
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                filterContext.HttpContext.Response.Cache.SetNoStore();
                filterContext.HttpContext.Response.AppendHeader("Pragma", "no-cache");

                if (!IsUserAuthenticated(filterContext.HttpContext, db)) {
                    var returnUrl = new StringBuilder(filterContext.HttpContext.Request.Path);
                    var queryString = filterContext.HttpContext.Request.QueryString;
                    if (queryString.Count > 0) {
                        returnUrl.Append('?').Append(queryString);
                    }

                    filterContext.Result = new RedirectResult($"/Account/Login?returnUrl={HttpUtility.UrlEncode(returnUrl.ToString())}");
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public static bool IsUserAuthenticated(HttpContextBase context, IUserData db) {
            try {
                if (!context.User.Identity.IsAuthenticated || context.Session[AuthenticationConstants.USER_SESSION_ID] == null) {
                    return false;
                }

                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                string username = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
                int userSessionId = int.Parse(claimsIdentity.FindFirst(AuthenticationConstants.USER_SESSION_ID).Value);
                string userAgent = claimsIdentity.FindFirst(AuthenticationConstants.USER_AGENT).Value;
                string ipAddress = claimsIdentity.FindFirst(AuthenticationConstants.IP_ADDRESS).Value;

                Session session = db.GetSession(userSessionId);

                if (session == null) {
                    return false;
                }

                if (session.Expires <= DateTime.Now) {
                    db.DeleteSession(userSessionId);
                    return false;
                }

                return userSessionId == (int) context.Session[AuthenticationConstants.USER_SESSION_ID] && username == session.Username && context.Request.UserAgent == session.UserAgent && context.Request.UserHostAddress == session.IpAddress;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return default;
            }
        }
    }
}