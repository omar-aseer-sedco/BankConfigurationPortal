using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Attributes {
    public class InternationalizationAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            try {
                string language = Languages.ENGLISH;
                var languageCookie = filterContext.HttpContext.Request.Cookies["language"];
                if (languageCookie != null) {
                    language = languageCookie.Value;
                }

                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}", language));
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(string.Format("{0}", language));
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }
    }
}
