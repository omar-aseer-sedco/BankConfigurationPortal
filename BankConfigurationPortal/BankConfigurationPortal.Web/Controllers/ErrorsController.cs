using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class ErrorsController : Controller {
        public ActionResult NotFound() {
            return View("NotFound");
        }
    }
}
