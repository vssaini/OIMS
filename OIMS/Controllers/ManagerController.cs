using System;
using System.Web.Mvc;
using OIMS.Global;
using OIMS.Repository;
using OIMS.Repository.Manager;
using Resources;
using RequestDetailModel = OIMS.Models.Manager.RequestDetailModel;

namespace OIMS.Controllers
{
    public class ManagerController : Controller
    {
        #region GET REQUEST'S ACTIONS

        public ActionResult Index()
        {
            if (Application.LoggedManager != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Requests()
        {
            return PartialView("_Requests");
        }

        #endregion

        #region REQUESTS

        /// <summary>
        /// Gridview for Requests.
        /// </summary>
        public ActionResult GridForRequests()
        {
            return PartialView("Grid/_GridForRequests", RequestsRepository.Requests);
        }

        // Request details
        [HttpPost]
        public ActionResult RequestDetails(int? requestId)
        {
            if (Application.LoggedManagerId != null)
            {
                if (requestId != null)
                {
                    var reqId = (int)requestId;

                    var model = new RequestDetailModel();
                    model.SetReqDetails(reqId);

                    return PartialView("_RequestDetails", model);
                }
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgReqNotFound);
            }
            else
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgForSession);
            }

            return PartialView("_Result");
        }

        [HttpPost]
        public ActionResult SaveStatus(int statusId, int? requestId, int requestorId)
        {
            string msg;


            if (Application.LoggedManagerId != null)
            {
                if (requestId != null)
                {
                    var mgrId = (int)Application.LoggedManagerId;
                    var reqId = (int)requestId;
                    ViewData["Status"] = statusId;

                    try
                    {
                        RequestsRepository.SaveRequest(requestorId, mgrId, statusId, reqId);

                        return Json(new
                        {
                            success = true,
                            message = "Status saved for request Id " + reqId + " successfully!"
                        });
                    }
                    catch (Exception exc)
                    {
                        msg = exc.Message;
                        BaseRepository.OimsDataContext.ClearChanges(); // Roll back all changes
                        Logger.LogError(exc, "Error while saving status from Manager zone");
                    }
                }
                else
                {
                    msg = Common.ErrorMsgReqNotFound;
                }
            }
            else
            {
                msg = Common.ErrorMsgForSession;
            }

            return Json(new
            {
                success = false,
                message = msg
            });
        }

        #endregion

    }
}
