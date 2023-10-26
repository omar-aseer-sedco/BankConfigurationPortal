using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class ErrorsController : BaseController {
        public ActionResult NotFound() {
            return View("NotFound");
        }
    }
}
