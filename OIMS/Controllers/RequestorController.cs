using System;
using System.Text;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using OIMS.Global;
using OIMS.Models.Requestor;
using OIMS.Models.Supervisor;
using OIMS.Repository;
using OIMS.Repository.Requestor;
using OIMS.Repository.Supervisor;
using Resources;
using RequestDetailModel = OIMS.Models.Requestor.RequestDetailModel;
using RequestsRepository = OIMS.Repository.Requestor.RequestsRepository;

namespace OIMS.Controllers
{
    public class RequestorController : Controller
    {
        #region GET REQUEST'S ACTIONS

        public ActionResult Index()
        {
            if (Application.LoggedRequestor == null)
                return RedirectToAction("Index", "Home");

            var model = new CartModel();
            return View(model);
        }

        public ActionResult NewRequest()
        {
            var model = new CartModel();
            return PartialView("_NewRequest", model);
        }

        public ActionResult Requests()
        {
            return PartialView("_Requests");
        }

        #endregion

        #region CART ACTIONS (... in order)

        #region 1. Add to Cart (using Ajax)

        // 1.A. Add shelter to cart
        [HttpPost]
        public ActionResult AddShelterToCart(int? product, int? prodQuantity)
        {
            var msgType = false;
            var msg = Requestor.Req_Cart_ErrorNoShelter;

            try
            {
                if (Application.LoggedRequestorId != null)
                {
                    var userId = (int)Application.LoggedRequestorId;

                    if (product != null && prodQuantity != null)
                    {
                        var prodId = (int)product;
                        var prodQuan = (float)prodQuantity;

                        CartRepository.AddProductToCart(userId, prodId, prodQuan);

                        msgType = true;
                        msg = Requestor.Req_Cart_ShelterAdded;
                    }
                }
                else
                {
                    msg = Common.ErrorMsgForSession;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                Logger.LogError(e, "Error while adding shelter to cart from Requestor zone");
            }

            return Json(new
            {
                success = msgType,
                message = msg
            });
        }

        // 1.B. Add item to cart
        [HttpPost]
        public ActionResult AddItemToCart(int? item, float? itemQuantity)
        {
            var msgType = false;
            var msg = Requestor.Req_Cart_ErrorNoItem;

            try
            {
                if (Application.LoggedRequestorId != null)
                {
                    var userId = (int)Application.LoggedRequestorId;

                    if (item != null && itemQuantity != null)
                    {
                        var itemId = (int)item;
                        var itemQuan = (float)itemQuantity;

                        CartRepository.AddItemToCart(userId, itemId, itemQuan);

                        msgType = true;
                        msg = Requestor.Req_Cart_ItemAdded;
                    }
                }
                else
                {
                    msg = Common.ErrorMsgForSession;
                }
            }
            catch (Exception exc)
            {
                msg = exc.Message;
                Logger.LogError(exc, "Error while adding item to cart from Requestor zone");
            }

            return Json(new
            {
                success = msgType,
                message = msg
            });
        }

        #endregion

        // 2. Show total shelter count
        public ActionResult TotalShelter()
        {
            return Json(new
            {
                TotalProducts = CartRepository.GetTotalStuffCount(Application.LoggedRequestorId)
            }, JsonRequestBehavior.AllowGet);
        }

        // 3. Set flyout cart for showing shelters list
        public ActionResult FlyoutCart()
        {
            var model = new CartModel();
            return PartialView("SubView/_FlyoutCart", model);
        }

        // 4. Save cart items
        [HttpPost]
        public ActionResult SaveCartStuffs()
        {
            var msgType = false;
            string msg;

            try
            {
                if (Application.LoggedRequestorId != null)
                {
                    var userId = (int)Application.LoggedRequestorId;
                    CartRepository.SaveRequest(userId);

                    msgType = true;
                    msg = Requestor.Req_Cart_MaterialSaved;
                }
                else
                {
                    msg = Common.ErrorMsgForSession;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                BaseRepository.OimsDataContext.ClearChanges(); // Roll back all changes
                Logger.LogError(e, "Error while saving cart stuff from Requestor zone");
            }

            return Json(new
            {
                success = msgType,
                message = msg
            });
        }

        // 5. Delete cart item
        [HttpPost]
        public ActionResult DeleteCartItem(int cartItemId)
        {
            var msgType = false;
            string msg;

            try
            {
                if (Application.LoggedRequestorId != null)
                {
                    var userId = (int)Application.LoggedRequestorId;
                    CartRepository.DeleteCartStuff(userId, cartItemId);

                    msgType = true;
                    msg = Requestor.Req_Cart_MaterialDelete;
                }
                else
                {
                    msg = Common.ErrorMsgForSession;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                Logger.LogError(e, "Error while deleting cart item from Requestor zone");
            }

            return Json(new
            {
                success = msgType,
                message = msg
            });
        }

        #endregion

        #region REQUESTS

        // Request details
        [HttpPost]
        public ActionResult RequestDetails(int? rowId)
        {
            string msg;

            try
            {
                if (Application.LoggedRequestorId != null)
                {
                    if (rowId != null)
                    {
                        var reqId = (int)rowId;

                        var model = new RequestDetailModel();
                        model.SetReqDetails(reqId);

                        return PartialView("SubView/_RequestDetails", model);
                    }
                    msg = Common.ErrorMsgReqNotFound;
                }
                else
                {
                    msg = Common.ErrorMsgForSession;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
                Logger.LogError(e, "Error while getting request details from Requestor zone");
            }

            return Json(new
            {
                success = false,
                message = msg
            });
        }

        /// <summary>
        /// Gridview for Requests.
        /// </summary>
        public ActionResult GridForRequests()
        {
            return PartialView("Grid/_GridForRequests", RequestsRepository.Requests);
        }

        /// <summary>
        /// Update Gridview for Requests.
        /// </summary>
        public ActionResult UpdateGridForRequests()
        {
            return PartialView("Grid/_GridForRequests", RequestsRepository.Requests);
        }

        /// <summary>
        /// Delete Gridview for Requests.
        /// </summary>
        public ActionResult DeleteGridForRequests([ModelBinder(typeof(DevExpressEditorsBinder))] int requestId)
        {
            try
            {
                if (requestId > 0)
                {
                    RequestsRepository.DeleteRequest(requestId);
                }
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while deleting request from Requestor zone");
            }

            return PartialView("Grid/_GridForRequests", RequestsRepository.Requests);
        }

        /// <summary>
        /// Save user entered job and company for further new requests.
        /// </summary>
        /// <param name="setDefault">True or false to create default job</param>
        /// <param name="existingJob">Name of the existing job</param>
        /// <param name="newJob">Name of the new job</param>
        /// <param name="company">Name of company</param>
        public ActionResult SaveJob(bool setDefault, string existingJob, string newJob, string company)
        {
            string errorMsg;
            Session["Job"] = null;
            var builder = new StringBuilder("Job-");

            if (!string.IsNullOrEmpty(company) && !company.Contains(Requestor.Req_SelectCompanyOption))
            {
                Session["Company"] = company;

                // Create default job and set it
                if (setDefault)
                {
                    // Default job
                    builder.Append(DateTime.Now.ToString("dd MMM hh:mm:ss"));
                    Session["Job"] = builder.ToString();

                    return Json(new
                                {
                                    success = true,
                                    job = builder.ToString(),
                                    message = string.Format(Requestor.Nr_JobNotice, builder)
                                });
                }

                // If existing job value provided
                if (!string.IsNullOrEmpty(existingJob) && !existingJob.Contains(Requestor.Req_SelectJobOption))
                {
                    // Assign to Session for saving later
                    Session["Job"] = existingJob;

                    return Json(new
                                {
                                    success = true,
                                    job = existingJob,
                                    message = string.Format(Requestor.Nr_JobNotice, existingJob)
                                });
                }

                // If new job value is provided
                if (!string.IsNullOrEmpty(newJob))
                {
                    // Set job name
                    builder.Append(DateTime.Now.ToString("dd MMM")).Append("-").Append(newJob);
                    var jobName = builder.ToString();

                    // Assign to Session for saving later
                    Session["Job"] = jobName;

                    // Show response to user
                    return Json(new
                                {
                                    success = true,
                                    job = jobName,
                                    message = string.Format(Requestor.Nr_JobNotice, jobName)
                                });
                }

                errorMsg = string.Format(Common.ErrorDiv, Requestor.Nr_ErrorMsgJobEmpty);

                if (Session["Job"] == null)
                    errorMsg = string.Format(Common.ErrorDiv, Requestor.Nr_ErrorMsgNoJob);

            }
            else
            {
                errorMsg = string.Format(Common.ErrorDiv, Requestor.Nr_ErrorMsgNoCompany);
            }

            TempData["ErrorMessage"] = errorMsg;
            return PartialView("_Result");
        }

        /// <summary>
        /// Load content for Job Popup.
        /// </summary>
        public ActionResult LoadJobContent()
        {
            var model = new JobModel();
            return PartialView("SubView/_JobContent", model);
        }

        #endregion

        #region SHELTER ITEMS

        /// <summary>
        /// Gridview for shelter items.
        /// </summary>
        public ActionResult GridForShelterItems(string shelterNqty)
        {
            var paramValues = shelterNqty.Split(',');

            var shelterId = Int32.Parse(paramValues[0]);
            var qtyMultiple = Int32.Parse(paramValues[1]);

            if (qtyMultiple.Equals(0))
                qtyMultiple = 1;

            return PartialView("Grid/_GridForShelterItems", GetShelterItems(shelterId, qtyMultiple));
        }

        /// <summary>
        /// Get model for ShelterItems.
        /// </summary>
        /// <param name="shelterId">shelterId for which items to be shown.</param>
        /// <param name="qtyMultiple">Number of shelter user is requesting.</param>
        /// <returns></returns>
        private static ShelterItemsModel GetShelterItems(int shelterId, int qtyMultiple)
        {
            var items = ShelterRepository.GetShelterItemsByKey(shelterId, qtyMultiple);
            return new ShelterItemsModel { ShelterKey = shelterId, ShelterItems = items };
        }

        #endregion
    }
}
