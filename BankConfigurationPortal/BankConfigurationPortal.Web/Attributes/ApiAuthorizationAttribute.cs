using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace BankConfigurationPortal.Web.Attributes {
    public class ApiAuthorizationAttribute : AuthorizeAttribute {
        private readonly IUserData db;

        public ApiAuthorizationAttribute() : base() {
            try {
                db = new SqlUserData();
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext) {
            try {
                string[] credentials;
                try {
                    string encodedString = actionContext.Request.Headers.FirstOrDefault(h => h.Key.Equals(AuthenticationConstants.AUTHORIZATION_HEADER_NAME)).Value.First();
                    credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedString)).Split(':');
                    if (credentials.Length != 3)
                        throw new FormatException();
                }
                catch (ArgumentNullException) {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }
                catch (FormatException) {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                        Content = new StringContent("Incorrectly formatted credentials.")
                    };
                    return;
                }

                User user = new User() {
                    BankName = credentials[0],
                    Username = credentials[1],
                    Password = credentials[2],
                };

                if (!db.ValidateUser(user)) {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    return;
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }
    }
}
