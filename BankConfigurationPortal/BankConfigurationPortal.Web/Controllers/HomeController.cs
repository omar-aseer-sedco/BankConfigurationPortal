using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankConfigurationPortal.Data.Services;

namespace BankConfigurationPortal.Web.Controllers {
    public class HomeController : Controller {
        private readonly IBranchData db;

        public HomeController() {
            db = new SqlBranchData(); // TODO: use dependency injection
        }

        public ActionResult Index() {
            int branchCount = db.GetAllBranches("bank1").Count();
            ViewBag.BranchCount = branchCount;
            return View();
        }
    }
}
