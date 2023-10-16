using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class BankServiceController : Controller {
        private readonly IBankServiceData db;

        public BankServiceController() {
            try {
                db = new SqlBankServiceData(); // TODO: use dependency injection
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index() {
            try {
                var model = db.GetAllBankServices(CookieUtils.GetBankName(Request));
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

                var model = db.GetBankService(CookieUtils.GetBankName(Request), bankServiceId);
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
                bankService.BankName = CookieUtils.GetBankName(Request);

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
                var model = db.GetBankService(CookieUtils.GetBankName(Request), bankServiceId);
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
                bankService.BankName = CookieUtils.GetBankName(Request);
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
                var model = db.GetBankService(CookieUtils.GetBankName(Request), bankServiceId);
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
                db.Delete(CookieUtils.GetBankName(Request), bankServiceId);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
