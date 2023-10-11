using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class BankServiceController : Controller {
        private readonly IBankServiceData db;
        private readonly string bankName;

        public BankServiceController() {
            try {
                db = new SqlBankServiceData(); // TODO: use dependency injection
                bankName = "bank1"; // TODO: get the actual bank name 
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index() {
            try {
                var model = db.GetAllBankServices(bankName);
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int bankServiceId = 0) {
            try {
                if (bankServiceId == 0) {
                    return RedirectToAction("Index");
                }

                var model = db.GetBankService(bankName, bankServiceId);
                if (model == null) {
                    return View("NotFound");
                }

                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create() {
            try {
                return View();
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankService bankService) {
            try {
                bankService.BankName = bankName;

                if (ModelState.IsValid) {
                    int bankServiceId = db.Add(bankService);
                    return RedirectToAction("Details", new { bankServiceId });
                }
                else {
                    LogsHelper.Log("Invalid model state - create bank service", EventSeverity.Warning);
                    return View();
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int bankServiceId) {
            try {
                var model = db.GetBankService(bankName, bankServiceId);
                if (model == null) {
                    return View("NotFound");
                }

                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BankService bankService) {
            try {
                bankService.BankName = bankName;
                if (ModelState.IsValid) {
                    db.Update(bankService);
                    return RedirectToAction("Details", new { bankServiceId = bankService.BankServiceId });
                }
                else {
                    LogsHelper.Log("Invalid model state - update bank service", EventSeverity.Warning);
                    return View();
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int bankServiceId) {
            try {
                var model = db.GetBankService(bankName, bankServiceId);
                if (model == null) {
                    return View("NotFound");
                }

                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int bankServiceId, FormCollection form) {
            try {
                db.Delete(bankName, bankServiceId);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
