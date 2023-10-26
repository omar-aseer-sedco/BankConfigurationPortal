using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace BankConfigurationPortal.Web.Utils {
    public class CookieUtils {
        public static string GetBankName(IPrincipal user) {
            try {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                return claimsIdentity.FindFirst("BankName").Value;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return string.Empty;
            }
        }
    }
}
