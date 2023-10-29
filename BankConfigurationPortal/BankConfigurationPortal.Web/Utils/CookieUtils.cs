using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace BankConfigurationPortal.Web.Utils {
    public class CookieUtils {
        public static string GetBankName(IPrincipal user) {
            try {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                return claimsIdentity.FindFirst(AuthenticationConstants.BANK_NAME).Value;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return string.Empty;
            }
        }
    }
}
