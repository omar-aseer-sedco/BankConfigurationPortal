using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            try {
                filters.Add(new HandleErrorAttribute());
                filters.Add(new InternationalizationAttribute());
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }
    }
}
