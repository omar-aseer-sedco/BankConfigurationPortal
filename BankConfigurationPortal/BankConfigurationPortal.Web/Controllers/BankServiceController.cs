using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [OwinCookieAuthorization]
    public class BankServiceController : BaseController {
        private readonly IBankServiceData db;

        public BankServiceController(IBankServiceData db) {
            try {
                this.db = db;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index() {
            try {
                ViewBag.Title = WebResources.BankServices;

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

                var model = db.GetAllBankServices(CookieUtils.GetBankName(User));
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int bankServiceId = 0) {
            try {
                ViewBag.Title = WebResources.Details;

                if (bankServiceId == 0) {
                    return RedirectToAction("Index");
                }

                var model = db.GetBankService(CookieUtils.GetBankName(User), bankServiceId);
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
                ViewBag.Title = WebResources.Create;

                return View(new BankService());
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
                bankService.BankName = CookieUtils.GetBankName(User);

                if (ModelState.IsValid) {
                    int bankServiceId = db.Add(bankService);
                    return RedirectToAction("Details", new { bankServiceId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - create bank service", EventLogEntryType.Warning);
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
                ViewBag.Title = WebResources.Edit;

                var model = db.GetBankService(CookieUtils.GetBankName(User), bankServiceId);
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
                bankService.BankName = CookieUtils.GetBankName(User);
                if (ModelState.IsValid) {
                    db.Update(bankService);
                    return RedirectToAction("Details", new { bankServiceId = bankService.BankServiceId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - update bank service", EventLogEntryType.Warning);
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
                ViewBag.Title = WebResources.Delete;

                var model = db.GetBankService(CookieUtils.GetBankName(User), bankServiceId);
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
                db.Delete(CookieUtils.GetBankName(User), bankServiceId);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
