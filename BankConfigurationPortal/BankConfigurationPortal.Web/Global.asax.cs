using BankConfigurationPortal.Web.App_Start;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BankConfigurationPortal.Web {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            //System.Diagnostics.Debugger.Launch();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerConfig.RegisterContainer();
        }
    }
}
