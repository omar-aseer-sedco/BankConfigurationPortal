﻿using BankConfigurationPortal.Web.Models;
using BankConfigurationPortal.Web.Services;
using BankConfigurationPortal.Utils.Helpers;
using BankConfigurationPortal.Web.Attributes;
using BankConfigurationPortal.Web.Utils;
using System;
using System.Web.Mvc;
using BankConfigurationPortal.Web.Constants;
using System.Diagnostics;

namespace BankConfigurationPortal.Web.Controllers {
    [OwinCookieAuthorization]
    public class BranchController : BaseController {
        private readonly IBranchData db;

        public BranchController(IBranchData db) {
            try {
                this.db = db;
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }

        public ActionResult Index() {
            try {
                ViewBag.Title = WebResources.Branches;

                if (Request.Cookies["language"] != null) {
                    ViewBag.Language = Request.Cookies["language"].Value;
                }
                else {
                    ViewBag.Language = Languages.ENGLISH;
                }

                var model = db.GetAllBranches(CookieUtils.GetBankName(User));
                return View(model);
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }

        public ActionResult Details(int branchId) {
            try {
                ViewBag.Title = WebResources.Details;

                var model = db.GetBranch(CookieUtils.GetBankName(User), branchId);
                if (model == null) {
                    return View("NotFound");
                }

                ViewBag.NumberOfCounters = db.GetNumberOfCounters(CookieUtils.GetBankName(User), branchId);
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
                branch.BankName = CookieUtils.GetBankName(User);

                if (ModelState.IsValid) {
                    int branchId = db.Add(branch);
                    return RedirectToAction("Details", new { branchId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - create branch", EventLogEntryType.Warning);
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
                ViewBag.Title = WebResources.Edit;

                var model = db.GetBranch(CookieUtils.GetBankName(User), branchId);
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
                branch.BankName = CookieUtils.GetBankName(User);
                if (ModelState.IsValid) {
                    db.Update(branch);
                    return RedirectToAction("Details", new { branchId = branch.BranchId });
                }
                else {
                    WindowsLogsHelper.Log("Invalid model state - edit branch", EventLogEntryType.Warning);
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
                ViewBag.Title = WebResources.Delete;

                var model = db.GetBranch(CookieUtils.GetBankName(User), branchId);
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
                db.Delete(CookieUtils.GetBankName(User), branchId);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
                return View("Error");
            }
        }
    }
}
