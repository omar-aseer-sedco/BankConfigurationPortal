using Autofac;
using Autofac.Integration.Mvc;
using BankConfigurationPortal.Web.Services;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.App_Start {
    public class ContainerConfig {
        public static void RegisterContainer() {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<SqlBankServiceData>()
                   .As<IBankServiceData>()
                   .InstancePerRequest();
            builder.RegisterType<SqlBranchData>()
                   .As<IBranchData>()
                   .InstancePerRequest();
            builder.RegisterType<SqlCounterData>()
                   .As<ICounterData>()
                   .InstancePerRequest();
            builder.RegisterType<SqlUserData>()
                   .As<IUserData>()
                   .InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}