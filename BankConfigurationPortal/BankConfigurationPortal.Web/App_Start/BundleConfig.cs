﻿using BankConfigurationPortal.Utils.Helpers;
using System;
using System.Web.Optimization;

namespace BankConfigurationPortal.Web {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            try {
                bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                            "~/Scripts/jquery-{version}.js"));

                bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                            "~/Scripts/jquery.validate*",
                            "~/Scripts/custom-validation.js"));

                // Use the development version of Modernizr to develop with and learn from. Then, when you're
                // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
                bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

                bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js"));

                bundles.Add(new ScriptBundle("~/bundles/bootstrap-rtl").Include(
                          "~/Scripts/bootstrap.rtl.min.js"));

                bundles.Add(new StyleBundle("~/Content/css").Include(
                          "~/Content/bootstrap.css",
                          "~/Content/site.css"));

                bundles.Add(new StyleBundle("~/Content/css-rtl").Include(
                          "~/Content/bootstrap.rtl.min.css",
                          "~/Content/site.rtl.css"));

                bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                          "~/Scripts/chosen.jquery.js",
                          "~/Scripts/chosen.jquery.min.js"));
            }
            catch (Exception ex) {
                ExceptionHelper.HandleGeneralException(ex);
            }
        }
    }
}
