using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class CounterServiceController : BaseController {
        private readonly IBankServiceData db;

        public CounterServiceController(IBankServiceData db) {
            try {
                this.db = db;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        [HttpPost]
        public ActionResult AddService(int branchId, int counterId, int bankServiceId) {
            try {
                db.AddService(CookieUtils.GetBankName(Request), branchId, counterId, bankServiceId);
                return RedirectToAction("Index", new { branchId, counterId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult RemoveService(int branchId, int counterId, int bankServiceId) {
            try {
                db.RemoveService(CookieUtils.GetBankName(Request), branchId, counterId, bankServiceId);
                return RedirectToAction("Index", new { branchId, counterId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
