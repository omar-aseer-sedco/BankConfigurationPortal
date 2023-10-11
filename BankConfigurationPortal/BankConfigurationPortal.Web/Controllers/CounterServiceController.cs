using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class CounterServiceController : Controller {
        private readonly IBankServiceData db;
        private readonly string bankName;

        public CounterServiceController() {
            try {
                db = new SqlBankServiceData(); // TODO: use dependency injection
                bankName = "bank1"; // TODO: get actual bank name
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index(int branchId, int counterId) {
            try {
                ViewBag.BranchId = branchId;
                ViewBag.CounterId = counterId;

                var model = new List<CounterServiceViewModel>();
                var allServices = db.GetAllBankServices(bankName);
                foreach (var service in allServices) {
                    bool isAvailableOnCounter = db.IsAvailableOnCounter(bankName, branchId, counterId, service.BankServiceId);
                    model.Add(new CounterServiceViewModel() {
                        Service = service,
                        BranchId = branchId,
                        CounterId = counterId,
                        IsAvailableOnCounter = isAvailableOnCounter,
                    });
                }
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult AddService(int branchId, int counterId, int bankServiceId) {
            try {
                db.AddService(bankName, branchId, counterId, bankServiceId);
                return RedirectToAction("Index", new { branchId, counterId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult RemoveService(int branchId, int counterId, int bankServiceId) {
            try {
                db.RemoveService(bankName, branchId, counterId, bankServiceId);
                return RedirectToAction("Index", new { branchId, counterId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
