using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Utils;
using BankConfigurationPortal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BankConfigurationPortal.Web.Controllers {
    [CookieAuthorization]
    public class CounterServiceController : Controller {
        private readonly IBankServiceData db;

        public CounterServiceController() {
            try {
                db = new SqlBankServiceData(); // TODO: use dependency injection
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index(int branchId, int counterId) {
            try {
                ViewBag.BranchId = branchId;
                ViewBag.CounterId = counterId;

                var list = new List<CounterServiceViewModel>();
                var allServices = db.GetAllBankServices(CookieUtils.GetBankName(Request));
                foreach (var service in allServices) {
                    bool isAvailableOnCounter = db.IsAvailableOnCounter(CookieUtils.GetBankName(Request), branchId, counterId, service.BankServiceId);
                    list.Add(new CounterServiceViewModel() {
                        Service = service,
                        BranchId = branchId,
                        CounterId = counterId,
                        IsAvailableOnCounter = isAvailableOnCounter,
                    });
                }
                var model = new SelectServicesViewModel() {
                    CounterServices = list,
                };
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

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

        [HttpPost]
        public ActionResult ModifyServices(int branchId, int counterId, ServiceOperation operation, SelectServicesViewModel postData) {
            try {
                List<int> selectedServiceIds = new List<int>();
                foreach (var counterService in postData.CounterServices) {
                    if (counterService.Selected) {
                        selectedServiceIds.Add(counterService.Service.BankServiceId);
                    }
                }

                if (operation == ServiceOperation.addServices) {
                    db.AddServices(CookieUtils.GetBankName(Request), branchId, counterId, selectedServiceIds);
                }
                else if (operation == ServiceOperation.removeServices) {
                    db.RemoveServices(CookieUtils.GetBankName(Request), branchId, counterId, selectedServiceIds);
                }

                return RedirectToAction("Index", new { branchId, counterId });
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
