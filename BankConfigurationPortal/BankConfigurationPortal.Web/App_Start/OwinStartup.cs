using BankConfigurationPortal.Web.Constants;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Security.Claims;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(BankConfigurationPortal.Web.App_Start.OwinStartup))]

namespace BankConfigurationPortal.Web.App_Start {
    public class OwinStartup {
        public void Configuration(IAppBuilder app) {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                AuthenticationType = AuthenticationConstants.AUTHENTICATION_TYPE,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration = true,
            });
        }
    }
}
