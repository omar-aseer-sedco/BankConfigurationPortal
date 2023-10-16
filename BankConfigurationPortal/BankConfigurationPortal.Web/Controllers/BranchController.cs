using BankConfigurationPortal.Data.Models;
using BankConfigurationPortal.Data.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class BranchController : Controller {
        private readonly IBranchData db;

        public BranchController() {
            try {
                db = new SqlBranchData(); // TODO: use dependency injection
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index() {
            try {
                var model = db.GetAllBranches(CookieUtils.GetBankName(Request));
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int branchId) {
            try {
                var model = db.GetBranch(CookieUtils.GetBankName(Request), branchId);
                if (model == null) {
                    return View("NotFound");
                }

                ViewBag.NumberOfCounters = db.GetNumberOfCounters(CookieUtils.GetBankName(Request), branchId);
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
        public ActionResult Create(Branch branch) {
            try {
                branch.BankName = CookieUtils.GetBankName(Request);

                if (ModelState.IsValid) {
                    int branchId = db.Add(branch);
                    return RedirectToAction("Details", new { branchId });
                }
                else {
                    LogsHelper.Log("Invalid model state - create branch", EventSeverity.Warning);
                    return View();
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Edit(int branchId) {
            try {
                var model = db.GetBranch(CookieUtils.GetBankName(Request), branchId);
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
        public ActionResult Edit(Branch branch) {
            try {
                branch.BankName = CookieUtils.GetBankName(Request);
                if (ModelState.IsValid) {
                    db.Update(branch);
                    return RedirectToAction("Details", new { branchId = branch.BranchId });
                }
                else {
                    LogsHelper.Log("Invalid model state - edit branch", EventSeverity.Warning);
                    return View(branch);
                }
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Delete(int branchId) {
            try {
                var model = db.GetBranch(CookieUtils.GetBankName(Request), branchId);
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
        public ActionResult Delete(int branchId, FormCollection form) {
            try {
                db.Delete(CookieUtils.GetBankName(Request), branchId);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
