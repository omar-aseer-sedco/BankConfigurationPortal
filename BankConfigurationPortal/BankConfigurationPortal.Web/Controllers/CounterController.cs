using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Constants;
using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Web.Utils;
using BankConfigurationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [OwinCookieAuthorization]
    public class CounterController : BaseController {
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
                if (!db.CheckIfBranchExists(CookieUtils.GetBankName(User), branchId)) {
                    return View("NotFound");
                }

                ViewBag.Title = WebResources.Counters;

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

                var model = db.GetAllCountersWithoutServices(CookieUtils.GetBankName(User), branchId);
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

                var model = db.GetCounter(CookieUtils.GetBankName(User), branchId, counterId);
                if (model == null) {
                    return View("NotFound");
                }

                ViewBag.BranchId = branchId;
                ViewBag.CounterId = counterId;

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
                counter.BankName = CookieUtils.GetBankName(User);

                if (ModelState.IsValid) {
                    int counterId = db.Add(counter);
                    return RedirectToAction("Details", new { branchId, counterId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - create counter", EventLogEntryType.Warning);
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

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

                var model = db.GetCounter(CookieUtils.GetBankName(User), branchId, counterId);
                if (model == null) {
                    return View("NotFound", branchId);
                }

                HashSet<int> counterServiceIds = new HashSet<int>();
                foreach (var service in model.Services) {
                    counterServiceIds.Add(service.BankServiceId);
                }
                model.Services = new List<ServiceViewModel>();

                var allServices = serviceData.GetAllBankServices(CookieUtils.GetBankName(User));
                foreach (var service in allServices) {
                    if (counterServiceIds.Contains(service.BankServiceId)) {
                        ((List<ServiceViewModel>) model.Services).Add(new ServiceViewModel(service) { IsAvailableOnCounter = true });
                    }
                    else {
                        ((List<ServiceViewModel>) model.Services).Add(new ServiceViewModel(service) { IsAvailableOnCounter = false });
                    }
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
        public ActionResult Edit(int branchId, Counter counter, [System.Web.Http.FromBody] string[] addedButtonIds, [System.Web.Http.FromBody] string[] removedButtonIds) {
            try {
                counter.BankName = CookieUtils.GetBankName(User);
                counter.BranchId = branchId;

                if (ModelState.IsValid) {
                    db.Update(counter);
                    var addedButtonIdsDeserialized = JsonSerializer.Deserialize<int[]>(addedButtonIds[0]);
                    var removedButtonIdsDeserialized = JsonSerializer.Deserialize<int[]>(removedButtonIds[0]);

                    serviceData.AddServices(CookieUtils.GetBankName(User), branchId, counter.CounterId, addedButtonIdsDeserialized);
                    serviceData.RemoveServices(CookieUtils.GetBankName(User), branchId, counter.CounterId, removedButtonIdsDeserialized);

                    return RedirectToAction("Details", new { branchId, counterId = counter.CounterId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - edit counter", EventLogEntryType.Warning);
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

                var model = db.GetCounter(CookieUtils.GetBankName(User), branchId, counterId);
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
                db.Delete(CookieUtils.GetBankName(User), branchId, counterId);
                return RedirectToAction("Index", new { branchId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
