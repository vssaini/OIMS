using System;
using System.Web.Optimization;

namespace OIMS.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // For enabling bundling during Debug mode
            //BundleTable.EnableOptimizations = true;

            // So as to fix for .min extension files were not loading
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            // NOTE: Bundles are cached in browser. So it is one time load and good ;-).
            // NOTE: Sequence of scripts matter lot.

            #region HOME PAGE

            bundles.Add(new ScriptBundle("~/bundles/oims-login").Include
               (
               "~/Scripts/jquery.unobtrusive-ajax.min.js",
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/jquery.validate.unobtrusive.min.js",
               "~/Scripts/jquery.toastmessage-min.js",
               "~/Scripts/oims.login.js"
               ));

            #endregion

            #region COMMON SCRIPTS - FOR ZONES

            // Script for header as DevExpress need them first
            bundles.Add(new ScriptBundle("~/bundles/jqueryMix").Include
              (
              "~/Scripts/jquery-{version}.min.js",
              "~/Scripts/modernizr-*"
              ));

            // Base scripts for all other zones (as for optimization)
            bundles.Add(new ScriptBundle("~/bundles/oims-base").Include
               (
               "~/Scripts/jquery.unobtrusive-ajax.min.js",
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/jquery.validate.unobtrusive.min.js",

               "~/Scripts/oims.common.js", // For zone's tabs
               "~/Scripts/select2.min.js", // For styling select elements
               "~/Content/bootstrap/js/bootstrap-tooltip.js", // For showing tooltip over elements

                "~/Scripts/jquery.pnotify.min.js", // For showing notification

               "~/Scripts/typeahead.jquery.min.js", // Base for autofilling
                "~/Scripts/bloodhound.min.js" // Optimized autofilling from remote
               ));

            #endregion

            #region MANAGER ZONE

            bundles.Add(new StyleBundle("~/Content/m-css").Include
                (
                "~/Content/Layout.css",
                "~/Content/Manager.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/oims-base-mgrExt").Include
              (
              "~/Content/bootstrap/js/bootstrap-alert.js",
              "~/Scripts/jquery.numeric.js",

             "~/Scripts/underscore.min.js", // Supporter for event broker
             "~/Scripts/oims.common.eventbroker.js", // For handling events 
             "~/Scripts/oims.common.throbber.js", // For text (Please wait! Loading...)
             "~/Scripts/oims.common.notice.js" // For showing default notification
              ));

            bundles.Add(new ScriptBundle("~/bundles/oims-m-req").Include
                (
                    "~/Scripts/oims.common.requests.js", // For showing detail box
                    "~/Scripts/oims.manager.requests.js"
                ));

            #endregion

            #region SUPERVISOR ZONE

            bundles.Add(new StyleBundle("~/Content/s-css").Include
                (
                "~/Content/Layout.css",
                "~/Content/Supervisor.css"
                ));

            // Base extension
            bundles.Add(new ScriptBundle("~/bundles/oims-base-supExt").Include
               (
               "~/Content/bootstrap/js/bootstrap-alert.js",
               "~/Scripts/jquery.numeric.js",

               "~/Scripts/underscore.min.js",
                "~/Scripts/oims.common.eventbroker.js",
                "~/Scripts/oims.common.throbber.js",
                "~/Scripts/oims.common.notice.js"
               ));

            // Requests tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-req").Include
                (
                "~/Scripts/oims.supervisor.requests.js"
                ));

            // Users tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-users").Include
               (
                "~/Scripts/oims.supervisor.users.js"
               ));

            // Vendors tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-vendors").Include
               (
                "~/Scripts/oims.supervisor.vendors.js"
               ));

            // Items tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-items").Include
               (
                "~/Scripts/oims.supervisor.items.js"
               ));

            // Items Log tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-itemsLog").Include
              (
               "~/Scripts/oims.supervisor.itemslog.js"
              ));

            // Products tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-shelters").Include
               (
                "~/Scripts/oims.supervisor.shelters.js"
               ));

            // Reports tab
            bundles.Add(new ScriptBundle("~/bundles/oims-s-reports").Include
              (
               "~/Scripts/oims.supervisor.reports.js"
              ));

            #endregion

            #region REQUESTOR ZONE

            bundles.Add(new StyleBundle("~/Content/r-css").Include
                (
                "~/Content/Layout.css",
                "~/Content/Requestor.css"
                ));

            // Base extension
            bundles.Add(new ScriptBundle("~/bundles/oims-base-reqExt").Include
               (
               "~/Scripts/jquery-ui/jquery.effects.core.min.js",
               "~/Scripts/jquery-ui/jquery.effects.transfer.min.js",

               "~/Scripts/underscore.min.js", // Supporter for event broker
                "~/Scripts/oims.common.eventbroker.js", // For handling events 
                "~/Scripts/oims.common.throbber.js", // For text (Please wait! Loading...)

               "~/Scripts/oims.requestor.shopbar.js", // For shopcart
               "~/Scripts/oims.requestor.ajaxcart.js", // For shopcart ajax calls
               "~/Scripts/oims.common.notice.js" // For notification and data binding
               ));

            bundles.Add(new ScriptBundle("~/bundles/oims-r-new").Include
               (
                "~/Scripts/jquery.numeric.js",
                "~/Scripts/oims.requestor.new.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/oims-r-req").Include
               (
               "~/Scripts/oims.common.requests.js" // For showing request detail
               ));

            #endregion

            // General style for other pages
            bundles.Add(new StyleBundle("~/Content/gen-css").Include
               (
               "~/Content/Layout.css"
               ));
            
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null) throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
        }
    }
}