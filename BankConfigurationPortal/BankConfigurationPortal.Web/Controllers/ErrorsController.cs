using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [AllowAnonymous]
    public class ErrorsController : BaseController {
        public ActionResult NotFound() {
            try {
                return View("NotFound");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
