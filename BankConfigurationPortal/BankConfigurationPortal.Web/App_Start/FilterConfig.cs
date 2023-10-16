﻿using BankConfigurationPortal.Web.Attributes;
using System.Web;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InternationalizationAttribute());
        }
    }
}
