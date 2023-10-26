using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;
using BankConfigurationPortal.Web.ViewModels;
using System.Collections.Generic;
using BankConfigurationPortal.Web.Constants;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class CounterController : Controller {
        private readonly ICounterData db;
        private readonly IBankServiceData serviceData;

        public CounterController(ICounterData db, IBankServiceData serviceData) {
            try {
                this.db = db;
                this.serviceData = serviceData;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index(int branchId) {
            try {
                if (!db.CheckIfBranchExists(CookieUtils.GetBankName(Request), branchId)) {
                    return View("NotFound");
                }

                ViewBag.Title = WebResources.Counters;

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

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
                ViewBag.Title = WebResources.Details;

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

                var counter = db.GetCounter(CookieUtils.GetBankName(Request), branchId, counterId);
                if (counter == null) {
                    return View("NotFound");
                }

                ViewBag.BranchId = branchId;
                ViewBag.CounterId = counterId;

                var allServices = serviceData.GetAllBankServices(CookieUtils.GetBankName(Request));
                List<CounterServiceViewModel> counterServices = new List<CounterServiceViewModel>();
                foreach (var service in allServices) {
                    counterServices.Add(new CounterServiceViewModel() {
                        Service = service,
                        IsAvailableOnCounter = serviceData.IsAvailableOnCounter(CookieUtils.GetBankName(Request), branchId, counterId, service.BankServiceId),
                    });
                }

                var model = new CounterDetailsViewModel() {
                    BranchId = counter.BranchId,
                    CounterId = counter.CounterId,
                    NameEn = counter.NameEn,
                    NameAr = counter.NameAr,
                    Active = counter.Active,
                    Type = counter.Type,
                    Services = counterServices,
                };
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
                ViewBag.Title = WebResources.Create;

                ViewBag.BranchId = branchId;
                var model = new Counter() { BranchId = branchId, Type = CounterType.Teller };
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
                    JsonLogsHelper.Log("Invalid model state - create counter", EventSeverity.Warning);
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
                ViewBag.Title = WebResources.Edit;

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
                    JsonLogsHelper.Log("Invalid model state - edit counter", EventSeverity.Warning);
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
                ViewBag.Title = WebResources.Delete;

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
