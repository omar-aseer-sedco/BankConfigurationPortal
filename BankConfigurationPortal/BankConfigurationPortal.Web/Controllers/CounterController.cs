using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class CounterController : Controller {
        private readonly ICounterData db;

        public CounterController() {
            try {
                db = new SqlCounterData(); // TODO: use dependency injection
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index(int branchId) {
            try {
                var model = db.GetAllCountersWithoutServices(CookieUtils.GetBankName(Request), branchId);
                ViewBag.BranchId = branchId;
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int branchId, int counterId) {
            try {
                var model = db.GetCounter(CookieUtils.GetBankName(Request), branchId, counterId);
                if (model == null) {
                    return View("NotFound", branchId);
                }

                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Create(int branchId) {
            try {
                var model = new Counter() { BranchId = branchId };
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int branchId, Counter counter) {
            try {
                counter.BankName = CookieUtils.GetBankName(Request);

                if (ModelState.IsValid) {
                    int counterId = db.Add(counter);
                    return RedirectToAction("Details", new { branchId, counterId });
                }
                else {
                    LogsHelper.Log("Invalid model state - create counter", EventSeverity.Warning);
                    return View();
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int branchId, int counterId) {
            try {
                var model = db.GetCounter(CookieUtils.GetBankName(Request), branchId, counterId);
                if (model == null) {
                    return View("NotFound", branchId);
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
        public ActionResult Edit(int branchId, Counter counter) {
            try {
                counter.BankName = CookieUtils.GetBankName(Request);
                counter.BranchId = branchId;

                if (ModelState.IsValid) {
                    db.Update(counter);
                    return RedirectToAction("Details", new { branchId, counterId = counter.CounterId });
                }
                else {
                    LogsHelper.Log("Invalid model state - edit counter", EventSeverity.Warning);
                    return View(counter);
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int branchId, int counterId) {
            try {
                var model = db.GetCounter(CookieUtils.GetBankName(Request), branchId, counterId);
                if (model == null) {
                    return View("NotFound", branchId);
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
        public ActionResult Delete(int branchId, int counterId, FormCollection form) {
            try {
                db.Delete(CookieUtils.GetBankName(Request), branchId, counterId);
                return RedirectToAction("Index", new { branchId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
