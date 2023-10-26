using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Security.Claims;
using System.Web.Helpers;

[assembly: OwinStartup(typeof(BankConfigurationPortal.Web.App_Start.OwinStartup))]

namespace BankConfigurationPortal.Web.App_Start {
    public class OwinStartup {
        public void Configuration(IAppBuilder app) {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}
