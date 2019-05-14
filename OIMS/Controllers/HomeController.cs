using System;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using OIMS.Global;
using OIMS.Models;
using OIMS.Repository;
using Resources;

namespace OIMS.Controllers
{
    public class HomeController : Controller
    {
        // Global variables
        string _username, _userId;

        [DonutOutputCache(CacheProfile = "CacheForDay")]
        public ActionResult Index()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("Authenticate")]
        public ActionResult Index(LoginModel model, string command)
        {
            Application.ValidateLicenseFile();

            // If not in trial mode and not licensed
            if (!Application.IsTrialMode && !Application.IsLicensed)
                return RedirectToAction("TrialExpired", "Home");

            try
            {
                switch (command)
                {
                    case "Manager":
                        if (model.AuthenticateUser(Roles.Manager))
                        {
                            BaseRepository.GetUserInfo(1, model.ManagerEmail, out _username, out _userId);

                            Session["Manager"] = _username;
                            Session["ManagerId"] = _userId;

                            return RedirectToAction("Index", "Manager");
                        }
                        break;

                    case "Supervisor":
                        if (model.AuthenticateUser(Roles.Supervisor))
                        {
                            BaseRepository.GetUserInfo(2, model.SupervisorEmail, out _username, out _userId);

                            Session["Supervisor"] = _username;
                            Session["SupervisorId"] = _userId;

                            return RedirectToAction("Index", "Supervisor");
                        }
                        break;

                    case "Requestor":
                        if (model.AuthenticateUser(Roles.Requestor))
                        {
                            BaseRepository.GetUserInfo(3, model.RequestorEmail, out _username, out _userId);

                            Session["Requestor"] = _username;
                            Session["RequestorId"] = _userId;

                            return RedirectToAction("Index", "Requestor");
                        }
                        break;
                }
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Home.ErrorMsgForCredentials);
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc,"Error while authenticating user");
            }

            // If we got this far, something failed, redisplay form
            return PartialView("Index", model);
        }

        #region Helpers

        public ActionResult Result()
        {
            return PartialView("_Result");
        }

        public ActionResult LogOut()
        {
            // Reset all session(s) to null
            Application.ResetSession();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TrialExpired()
        {
            return View("TrialExpired");
        }

        #endregion
    }
}
