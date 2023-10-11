using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    public class CounterController : Controller {
        private readonly ICounterData db;
        private readonly string bankName;

        public CounterController() {
            try {
                db = new SqlCounterData(); // TODO: use dependency injection
                bankName = "bank1"; // TODO: get actual bank name
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index(int branchId) {
            try {
                var model = db.GetAllCountersWithoutServices(bankName, branchId);
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
                var model = db.GetCounter(bankName, branchId, counterId);
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
                counter.BankName = bankName;

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
                var model = db.GetCounter(bankName, branchId, counterId);
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
                counter.BankName = bankName;
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
                var model = db.GetCounter(bankName, branchId, counterId);
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
                db.Delete(bankName, branchId, counterId);
                return RedirectToAction("Index", new { branchId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
