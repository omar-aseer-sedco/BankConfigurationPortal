using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Models;
using System;
using System.Text.Json;
using System.Web;
using System.Web.Security;

namespace BankConfigurationPortal.Web.Utils {
    public class CookieUtils {
        public static string GetBankName(HttpRequestBase request) {
            try {
                var authCookie = request.Cookies[FormsAuthentication.FormsCookieName];
                SerializableUserData userData = JsonSerializer.Deserialize<SerializableUserData>(FormsAuthentication.Decrypt(authCookie.Value).UserData, JsonSerializerOptions.Default);
                return userData.BankName;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return string.Empty;
            }
        }
    }
}
