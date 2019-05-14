using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DevExpress.Web.Mvc;
using OIMS.Global;
using OIMS.Models;
using OIMS.Models.Supervisor;
using OIMS.Reports;
using OIMS.Repository;
using OIMS.Repository.Supervisor;
using OimsDataModel;
using Resources;
using Action = OIMS.Models.Supervisor.Action;

namespace OIMS.Controllers
{
    public class SupervisorController : Controller
    {
        #region GET ACTIONS

        public ActionResult Index()
        {
            if (Application.LoggedSupervisor != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Items()
        {
            return PartialView("_Items");
        }

        public ActionResult ItemsLog()
        {
            var model = new VendorsModel();
            return PartialView("_ItemsLog", model);
        }

        public ActionResult Users()
        {
            var model = new UsersModel();
            return PartialView("_Users", model);
        }

        public ActionResult Vendors()
        {
            return PartialView("_Vendors");
        }

        public ActionResult Shelters()
        {
            var model = new ShelterItemsModel();
            return PartialView("_Shelters", model);
        }

        public ActionResult Requests()
        {
            Session["Filter"] = null;
            return PartialView("_Requests");
        }

        public ActionResult Reports()
        {
            var model = new ReportsModel();
            return PartialView("_Reports", model);
        }

        #endregion

        #region TAB - REQUESTS

        /// <summary>
        /// Gridview for Requests.
        /// </summary>
        public ActionResult GridForRequests(string filter)
        {
            return PartialView("Grid/_GridForRequests", Application.Filter != null ? RequestsRepository.GetAllRequests() : RequestsRepository.Requests);
        }

        /// <summary>
        /// Filter gridview on basis of filters.
        /// </summary>
        /// <param name="filter">Whether to show all request or relevant one only.</param>
        /// <returns>Return updated gridview.</returns>
        [HttpPost]
        public ActionResult GridForRequests_CustomHandler(string filter)
        {
            if (filter.Equals("All"))
            {
                Session["Filter"] = "All";
                return PartialView("Grid/_GridForRequests", RequestsRepository.GetAllRequests());
            }
            Session["Filter"] = null;
            return PartialView("Grid/_GridForRequests", RequestsRepository.Requests);

        }

        // Request details
        [HttpPost]
        public ActionResult RequestDetails(int? requestId, string status, string filter)
        {
            string msg;

            if (Application.LoggedSupervisorId != null)
            {
                if (requestId != null)
                {
                    // Set status to be used in Detailed products and items grid
                    ViewData["Status"] = status.Equals("Approved") || status.Equals("Rejected") || status.Equals("Manager approval pending") ? null : status;

                    var reqId = (int)requestId;
                    Session["ReqId"] = reqId;

                    // Set filter session to get proper request id details
                    Session["Filter"] = filter.Equals("All") ? "All" : null;

                    return PartialView("_RequestDetails", GetRequestModel(reqId));
                }

                msg = Common.ErrorMsgReqNotFound;
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

        [HttpPost]
        public ActionResult SaveStatus(int statusId, int? requestId, int requestorId)
        {
            string msg;

            if (Application.LoggedSupervisorId != null)
            {
                if (requestId != null)
                {
                    if (!statusId.Equals(1) || !statusId.Equals(6))
                    {
                        var supId = (int)Application.LoggedSupervisorId;
                        var reqId = (int)requestId;
                        ViewData["Status"] = statusId;

                        try
                        {
                            RequestsRepository.SaveRequest(requestorId, supId, statusId, reqId);

                            try
                            {
                                #region Send email to Manager

                                var sIdForEmail = Application.GetStatusId("SIdEmail");

                                if (!statusId.Equals(sIdForEmail))
                                    return Json(new
                                                {
                                                    success = true,
                                                    message =
                                                        "Request with Request Id: " + reqId + " saved successfully!"
                                                });

                                // RoleId 1 stands for Manager
                                var user = UserRepository.Users.FirstOrDefault(u => u.RoleId.Equals(1));

                                if (user != null)
                                    Application.SendEmail(user.FirstName, user.Email, reqId);

                                #endregion
                            }
                            catch (Exception exc)
                            {
                                Logger.LogError(exc, "Error while sending email from Supervisor zone");
                            }

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
                            Logger.LogError(exc, "Error while saving status for request from Supervisor zone");
                        }
                    }
                    else
                    {
                        msg = "Supervisor can set two statuses 'Manager approval pending' or 'Closed' only.";
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

        /// <summary>
        /// Get request model.
        /// </summary>
        private static RequestDetailModel GetRequestModel(int reqId)
        {
            var model = new RequestDetailModel();
            model.SetReqDetails(reqId);

            return model;
        }

        #endregion

        #region Tab Request's --> Req Shelters

        /// <summary>
        /// Gridview for requested shelters.
        /// </summary>
        public ActionResult GridForReqShelters(string status)
        {
            Session["Status"] = ViewData["Status"] = status;
            var reqId = Session["ReqId"] != null ? int.Parse(Convert.ToString(Session["ReqId"])) : 0;

            var model = GetRequestModel(reqId);
            return PartialView("Grid/_ReqShelterGrid", model.Shelters);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchUpdateForReqShelters(MVCxGridViewBatchUpdateValues<ShelterRequest, int> updatedProducts, string status)
        {
            RequestDetailModel model;
            Session["Status"] = ViewData["Status"] = status; // To show batchEdit commands
            var reqId = Session["ReqId"] != null ? int.Parse(Convert.ToString(Session["ReqId"])) : 0;

            if (ModelState.IsValid)
            {
                try
                {
                    RequestsRepository.RecommendShelters(updatedProducts);
                    model = GetRequestModel(reqId);
                    return PartialView("Grid/_ReqShelterGrid", model.Shelters);
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                    Logger.LogError(e, "Error while recommending shelters for request from Supervisor zone");
                    BaseRepository.OimsDataContext.ClearChanges(); // Roll back all changes
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please, correct all errors.";
            }

            // If reached here, show errors
            model = GetRequestModel(reqId);
            return PartialView("Grid/_ReqShelterGrid", model.Shelters);
        }

        #endregion

        #region Tab Request's --> Req Items

        /// <summary>
        /// Gridview for requested items.
        /// </summary>
        public ActionResult GridForReqItems(string status)
        {
            Session["Status"] = ViewData["Status"] = status;
            var reqId = Session["ReqId"] != null ? int.Parse(Convert.ToString(Session["ReqId"])) : 0;

            var model = GetRequestModel(reqId);
            return PartialView("Grid/_ReqItemGrid", model.Items);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BatchUpdateForReqItems(MVCxGridViewBatchUpdateValues<ItemRequest, int> updatedItems, string status)
        {
            RequestDetailModel model;
            Session["Status"] = ViewData["Status"] = status; // To show batchEdit commands
            var reqId = Session["ReqId"] != null ? int.Parse(Convert.ToString(Session["ReqId"])) : 0;

            if (ModelState.IsValid)
            {
                try
                {
                    RequestsRepository.RecommendItems(updatedItems);
                    model = GetRequestModel(reqId);
                    return PartialView("Grid/_ReqItemGrid", model.Items);
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.ToString();
                    BaseRepository.OimsDataContext.ClearChanges(); // Roll back all changes
                    Logger.LogError(e, "Error while recommending items for request from Supervisor zone");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please, correct all errors.";
            }

            model = GetRequestModel(reqId);
            return PartialView("Grid/_ReqItemGrid", model.Items);
        }

        #endregion

        #region TAB - USERS

        /// <summary>
        ///     Gridview for Users.
        /// </summary>
        public ActionResult GridForUsers()
        {
            return PartialView("Grid/_GridForUsers", UserRepository.Users);
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForUsers([ModelBinder(typeof(DevExpressEditorsBinder))] UsersModel model)
        {
            try
            {
                // Remove modelstate errors for specific keys (as they will be null)
                if (ModelState.ContainsKey("ConfirmPassword"))
                    ModelState["ConfirmPassword"].Errors.Clear();

                if (ModelState.ContainsKey("U_Id"))
                    ModelState["U_Id"].Errors.Clear();

                if (ModelState.IsValid)
                {
                    if (UserRepository.IsUniqueEmail(model, Action.Update))
                    {
                        UserRepository.UpdateUser(model);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Supervisor.UErrorMsgEmailExist;
                    }
                }
                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while updating users grid from Supervisor zone");
            }

            return PartialView("Grid/_GridForUsers", UserRepository.Users);
        }

        /// <summary>
        ///     Delete row from grid.
        /// </summary>
        public ActionResult DeleteGridForUsers([ModelBinder(typeof(DevExpressEditorsBinder))] int userId)
        {
            try
            {
                if (userId > 0)
                {
                    UserRepository.DeleteUser(userId);
                }
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMsg"] = exc.Message;
                Logger.LogError(exc, "Error while deleting user from Supervisor zone");
            }

            return PartialView("Grid/_GridForUsers", UserRepository.Users);
        }

        /// <summary>
        ///     Create new user.
        /// </summary>
        /// <returns>Return partial view stating results of action.</returns>
        [HttpPost]
        public ActionResult UserPost(UsersModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (UserRepository.IsUniqueEmail(model, Action.New))
                    {
                        UserRepository.SaveNewUser(model);

                        TempData["ConfirmMessage"] = string.Format(Common.ConfirmDiv,
                       string.Format(Supervisor.UMsgUserCreated, model.FirstName));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Supervisor.UErrorMsgEmailExist);
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgBlankFields);
                }
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc, "Error while saving new user from Supervisor zone");
            }

            return PartialView("_Result");
        }

        /// <summary>
        /// Get list of emails in JSON format.
        /// </summary>
        [OutputCache(CacheProfile = "SearchCache")]
        public ActionResult GetEmails(string searchTerm)
        {
            var jsonSerialiser = new JavaScriptSerializer();

            if (searchTerm == null) return new ContentResult { Content = null, ContentType = "application/json" };

            var emails = UserRepository.Users.Where(u => u.Email.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            var json = jsonSerialiser.Serialize(emails);

            return new ContentResult { Content = json, ContentType = "application/json" };
        }

        #endregion

        #region TAB - VENDORS

        /// <summary>
        ///     Gridview for Vendors.
        /// </summary>
        public ActionResult GridForVendors()
        {
            return PartialView("Grid/_GridForVendors", VendorRepository.Vendors);
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForVendors([ModelBinder(typeof(DevExpressEditorsBinder))] VendorsModel model)
        {
            try
            {
                if (VendorRepository.IsUniqueVendor(model, Action.Update))
                {
                    VendorRepository.UpdateVendor(model);
                }
                else
                {
                    TempData["ErrorMessage"] = Supervisor.VErrorMsgVendorExist;
                }
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while updating vendors from Supervisor zone");
            }

            return PartialView("Grid/_GridForVendors", VendorRepository.Vendors);
        }

        /// <summary>
        ///     Delete row from grid.
        /// </summary>
        public ActionResult DeleteGridForVendors([ModelBinder(typeof(DevExpressEditorsBinder))] string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    VendorRepository.DeleteVendor(name);
                }
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while deleting vendors from Supervisor zone");
            }

            return PartialView("Grid/_GridForVendors", VendorRepository.Vendors);
        }

        /// <summary>
        ///     Create new vendor.
        /// </summary>
        /// <returns>Return partial view stating results of action.</returns>
        [HttpPost]
        public ActionResult VendorPost(VendorsModel model)
        {
            try
            {

                if (VendorRepository.IsUniqueVendor(model, Action.New))
                {
                    VendorRepository.SaveNewVendor(model);

                    TempData["ConfirmMessage"] = string.Format(Common.ConfirmDiv,
                   string.Format(Supervisor.VMsgVendorCreated, model.Name));
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Supervisor.VErrorMsgVendorExist);
                }

            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc, "Error while saving new vendor from Supervisor zone");
            }

            return PartialView("_Result");
        }

        /// <summary>
        /// Get list of vendors in JSON format.
        /// </summary>
        [OutputCache(CacheProfile = "SearchCache")]
        public ActionResult GetVendors(string searchTerm)
        {
            var jsonSerialiser = new JavaScriptSerializer();

            if (searchTerm == null) return new ContentResult { Content = null, ContentType = "application/json" };

            var vendors = VendorRepository.Vendors.Where(v => v.Name.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            var json = jsonSerialiser.Serialize(vendors);

            return new ContentResult { Content = json, ContentType = "application/json" };
        }

        #endregion

        #region TAB - ITEMS

        /// <summary>
        ///     Gridview for Items.
        /// </summary>
        public ActionResult GridForItems()
        {
            return PartialView("Grid/_GridForItems", ItemRepository.Items);
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForItems([ModelBinder(typeof(DevExpressEditorsBinder))] ItemsModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ItemRepository.IsUniqueItem(model))
                    {
                        ItemRepository.UpdateItem(model);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Supervisor.IErrorMsgItemExists;
                    }
                }
                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while updating items from Supervisor zone");
            }

            return PartialView("Grid/_GridForItems", ItemRepository.Items);
        }

        /// <summary>
        ///     Delete row from grid.
        /// </summary>
        public ActionResult DeleteGridForItems([ModelBinder(typeof(DevExpressEditorsBinder))] int itemId)
        {
            try
            {
                if (itemId > 0)
                {
                    var isItemInUse = ShelterRepository.ShelterItems.Any(sd => sd.I_Id.Equals(itemId));

                    if (!isItemInUse)
                        ItemRepository.DeleteItem(itemId);
                    else
                        TempData["DeleteErrorMsg"] = string.Format(Supervisor.IErrorMsgItemBeingUse, itemId);
                }
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMsg"] = exc.Message;
                Logger.LogError(exc, "Error while deleting item from Supervisor zone");
            }

            return PartialView("Grid/_GridForItems", ItemRepository.Items);
        }

        /// <summary>
        ///     Create new item(s).
        /// </summary>
        /// <returns>Return partial view stating results of action.</returns>
        [HttpPost]
        public ActionResult ItemPost()
        {
            try
            {
                // Create a list to hold list of items
                var itemList = new List<Item>();

                // Loop through the request.forms
                for (var i = 0; i <= Request.Form.Count; i++)
                {
                    var name = Request.Form["ItemName[" + i + "]"];
                    var qty = Request.Form["Quantity[" + i + "]"];
                    var size = Request.Form["Size[" + i + "]"];
                    var marking = Request.Form["Marking[" + i + "]"];

                    if (name == null || size == null || marking == null) continue;

                    if (string.IsNullOrEmpty(qty))
                        qty = "0";

                    var itemExist = BaseRepository.OimsDataContext.Items.Any(n => n.I_Name == name);
                    if (!itemExist)
                    {
                        var item = new Item { I_Name = name, I_Quantity = float.Parse(qty, CultureInfo.InvariantCulture.NumberFormat), Size = size, Marking = marking };
                        itemList.Add(item);
                    }
                    else
                    {
                        throw new CustomException(Supervisor.IErrorMsgItemExist);
                    }
                }

                // Save items to database
                if (itemList.Count > 0)
                {
                    // Ref - http://stackoverflow.com/questions/3811464/how-to-get-duplicate-items-from-a-list-using-linq?lq=1
                    // Best - http://stackoverflow.com/questions/1606679/remove-duplicates-in-the-list-using-linq?rq=1
                    var duplicateExist = itemList.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).Any();

                    if (duplicateExist)
                    {
                        TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Supervisor.IErrorMsgSameItem);
                    }
                    else
                    {
                        ItemRepository.SaveItems(itemList);
                        TempData["ConfirmMessage"] = string.Format(Common.ConfirmDiv, Supervisor.IMsgItemCreated);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgBlankFields);
                }
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc, "Error while saving items from Supervisor zone");
            }

            return PartialView("_Result");
        }

        /// <summary>
        /// Get list of items in JSON format.
        /// </summary>
        [OutputCache(CacheProfile = "SearchCache")]
        public ActionResult GetItems(string searchTerm)
        {
            var jsonSerialiser = new JavaScriptSerializer();

            if (searchTerm == null) return new ContentResult { Content = null, ContentType = "application/json" };

            var items = ItemRepository.Items.Where(i => i.ItemName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            var json = jsonSerialiser.Serialize(items);

            return new ContentResult { Content = json, ContentType = "application/json" };
        }

        #endregion

        #region TAB - ITEMS LOG

        /// <summary>
        ///     Gridview for Items.
        /// </summary>
        public ActionResult GridForItemsLog()
        {
            return PartialView("Grid/_GridForItemsLog", ItemsLogRepository.ItemsLog);
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForItemsLog([ModelBinder(typeof(DevExpressEditorsBinder))] ItemsLogModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ItemsLogRepository.UpdateItemLog(model);
                }
                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while updating items log from Supervisor zone");
            }

            return PartialView("Grid/_GridForItemsLog", ItemsLogRepository.ItemsLog);
        }

        /// <summary>
        ///     Delete row from grid.
        /// </summary>
        public ActionResult DeleteGridForItemsLog([ModelBinder(typeof(DevExpressEditorsBinder))] int id)
        {
            try
            {
                if (id > 0)
                {
                    ItemsLogRepository.DeleteItemLog(id);
                }
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while deleting items log from Supervisor zone");
            }

            return PartialView("Grid/_GridForItemsLog", ItemsLogRepository.ItemsLog);
        }

        /// <summary>
        ///     Create new item(s) log.
        /// </summary>
        /// <returns>Return partial view stating results of action.</returns>
        [HttpPost]
        public ActionResult ItemsLogPost(string vendor)
        {
            try
            {
                //Create a list to hold list of items log
                var itemLogList = new List<Itemslog>();

                //Loop through the request.forms
                for (var i = 0; i <= Request.Form.Count; i++)
                {
                    var item = Request.Form["Item[" + i + "]"];
                    var qty = Request.Form["Quantity[" + i + "]"];

                    if (item == null || qty == null) continue;

                    var itemLog = new Itemslog
                                  {
                                      ItemId = Convert.ToInt32(item),
                                      ItemQuantity = float.Parse(qty, CultureInfo.InvariantCulture.NumberFormat),
                                      VendorName = vendor,
                                      EntryDate = DateTime.Now
                                  };

                    itemLogList.Add(itemLog);
                }

                // Save items log to database
                if (itemLogList.Count > 0)
                {
                    var duplicateExist = itemLogList.GroupBy(n => n).Any(c => c.Count() > 1);

                    if (duplicateExist)
                    {
                        TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Supervisor.IErrorMsgSameItem);
                    }
                    else
                    {
                        ItemsLogRepository.SaveItemsLog(itemLogList);
                        TempData["ConfirmMessage"] = string.Format(Common.ConfirmDiv, Supervisor.IL_MsgLogCreated);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgBlankFields);
                }
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc, "Error while saving items log from Supervisor zone");
            }

            return PartialView("_Result");
        }

        #endregion

        #region TAB - SHELTERS

        /// <summary>
        /// Get possible stock existence as per database's records.
        /// </summary>
        /// <param name="shelterId"></param>
        /// <returns></returns>
        public ActionResult PossibleShelterStock(int shelterId)
        {
            var stockList = new List<float>();
            var items = ShelterRepository.GetShelterItemsByKey(shelterId);

            foreach (var item in items)
            {
                var itemQuantity = item.ItemQuantity;
                var totalItemsQuantity = ItemRepository.GetItemTotalQuantity(item.ItemId);

                if (itemQuantity.Equals(0) || totalItemsQuantity < itemQuantity)
                {
                    stockList.Add(0);
                    break;
                }

                var possComb = totalItemsQuantity / itemQuantity;
                stockList.Add(possComb);
            }

            if (stockList.Count <= 0) return Content(Convert.ToString(0));
            var stock = Math.Floor((decimal)stockList.Min());
            return Content(stockList.Count > 0 ? Convert.ToString(stock) : Convert.ToString(0));
        }

        /// <summary>
        ///     Gridview for Shelters.
        /// </summary>
        public ActionResult GridForShelters()
        {
            return PartialView("Grid/_GridForShelters", ShelterRepository.Shelters);
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForShelters([ModelBinder(typeof(DevExpressEditorsBinder))] SheltersModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ShelterRepository.IsUniqueShelter(model))
                    {
                        ShelterRepository.UpdateShelter(model);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Supervisor.SErrorMsgProductExist;
                    }
                }

                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.ToString();
                Logger.LogError(exc, "Error while updating shelter from Supervisor zone");
            }

            return PartialView("Grid/_GridForShelters", ShelterRepository.Shelters);
        }

        /// <summary>
        ///     Delete row from grid.
        /// </summary>
        public ActionResult DeleteGridForShelters([ModelBinder(typeof(DevExpressEditorsBinder))] int shelterId)
        {
            try
            {
                if (shelterId > 0)
                {
                    ShelterRepository.DeleteShelter(shelterId);
                }
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMsg"] = exc.Message;
                Logger.LogError(exc, "Error while deleting shelter from Supervisor zone");
            }

            return PartialView("Grid/_GridForShelters", ShelterRepository.Shelters);
        }

        /// <summary>
        ///     Create new shelter.
        /// </summary>
        /// <returns>Return partial view stating results of action.</returns>
        [HttpPost]
        public ActionResult ShelterPost()
        {
            var prodId = 0;

            try
            {
                // Get product name
                var product = Request.Form["Product[0]"];

                //Create a list to hold list of items
                var prodList = new List<Shelterdescription>();

                var productExist = BaseRepository.OimsDataContext.Shelters.Any(n => n.P_Name == product);
                if (!productExist)
                {
                    // Create product, save and get product id
                    var prod = new Shelter { P_Name = product };
                    prodId = ShelterRepository.SaveShelter(prod);

                    if (prodId > 0)
                    {
                        //Loop through the request.forms
                        for (var i = 0; i <= Request.Form.Count; i++)
                        {
                            var item = Request.Form["Item[" + i + "]"];
                            var qty = Request.Form["Quantity[" + i + "]"];

                            if (item == null || qty == null) continue;

                            // Create product items list prepared
                            var prodDesc = new Shelterdescription
                            {
                                P_Id = prodId,
                                I_Id = Convert.ToInt32(item),
                                I_Qty = Convert.ToInt32(qty)
                            };

                            prodList.Add(prodDesc);
                        }
                    }
                }
                else
                {
                    throw new CustomException(Supervisor.SErrorMsgProductExist);
                }

                // Save items of product to database
                if (prodList.Count > 0)
                {
                    var duplicateExist = prodList.GroupBy(n => n.I_Id).Any(c => c.Count() > 1);

                    if (duplicateExist)
                    {
                        DeleteShelter(prodId);
                        TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Supervisor.SErrorMsgSameItem);
                    }
                    else
                    {
                        ShelterRepository.SaveShelterItems(prodList);
                        TempData["ConfirmMessage"] = string.Format(Common.ConfirmDiv, string.Format(Supervisor.SMsgProductCreated, product));
                    }
                }
                else
                {
                    DeleteShelter(prodId);
                    TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, Common.ErrorMsgBlankFields);
                }
            }
            catch (Exception exc)
            {
                DeleteShelter(prodId);
                TempData["ErrorMessage"] = string.Format(Common.ErrorDiv, exc.Message);
                Logger.LogError(exc, "Error while saving shelter from Supervisor zone");
            }

            return PartialView("_Result");
        }

        private static void DeleteShelter(int shelterId)
        {
            try
            {
                if (shelterId > 0)
                    ShelterRepository.DeleteShelter(shelterId);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while deleting shelter from Supervisor zone");
            }
        }

        /// <summary>
        ///  Show items used in current shelter.
        /// </summary>
        /// <param name="shelterId"></param>
        /// <returns></returns>
        public ActionResult ShelterItems(int shelterId)
        {
            ViewData["ShelterId"] = shelterId;

            return PartialView("Grid/_GridForShelterItems", new ShelterItemsModel { ShelterKey = shelterId, ShelterItems = ShelterRepository.GetShelterItemsDetail(shelterId) });
        }

        /// <summary>
        /// Get list of products in JSON format.
        /// </summary>
        [OutputCache(CacheProfile = "SearchCache")]
        public ActionResult GetShelters(string searchTerm)
        {
            var jsonSerialiser = new JavaScriptSerializer();

            if (searchTerm == null) return new ContentResult { Content = null, ContentType = "application/json" };

            var products = ShelterRepository.Shelters.Where(p => p.ShelterName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            var json = jsonSerialiser.Serialize(products);

            return new ContentResult { Content = json, ContentType = "application/json" };
        }

        #endregion

        #region Tab Shelter's --> Items

        /// <summary>
        /// Grid for shelter's items.
        /// </summary>
        /// <param name="shelterId">ProductId of which items to be shown.</param>
        /// <returns></returns>
        public ActionResult GridForShelterItems(int shelterId)
        {
            ViewData["ShelterId"] = shelterId;
            return PartialView("Grid/_GridForShelterItems", GetShelterItems(shelterId));
        }

        /// <summary>
        /// New row of grid.
        /// </summary>
        public ActionResult NewGridForShelterItems([ModelBinder(typeof(DevExpressEditorsBinder))] SheltersDescInfo model, [ModelBinder(typeof(DevExpressEditorsBinder))] int shelterId)
        {
            ViewData["ShelterId"] = shelterId;

            try
            {
                ModelState["InStock"].Errors.Clear(); // Don't want to check InStock
                if (ModelState.IsValid)
                {
                    if (ShelterRepository.IsUniqueItem(model, Action.New))
                        ShelterRepository.NewShelterItem(model);
                    else
                        TempData["ErrorMessage"] = Supervisor.SErrorMsgForItemExist;
                }
                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while saving new shelter item in Shelters grid from Supervisor zone");
            }

            return PartialView("Grid/_GridForShelterItems", GetShelterItems(shelterId));
        }

        /// <summary>
        ///     Update row of grid.
        /// </summary>
        public ActionResult UpdateGridForShelterItems([ModelBinder(typeof(DevExpressEditorsBinder))] SheltersDescInfo model, [ModelBinder(typeof(DevExpressEditorsBinder))] int shelterId)
        {
            ViewData["ShelterId"] = shelterId;
            try
            {
                if (ModelState.IsValid)
                {
                    if (ShelterRepository.IsUniqueItem(model, Action.Update))
                        ShelterRepository.UpdateShelterItem(model);
                    else
                        TempData["ErrorMessage"] = Supervisor.SErrorMsgForItemExist;
                }
                else
                    TempData["ErrorMessage"] = Common.ErrorMsgForGrid;
            }
            catch (Exception exc)
            {
                TempData["ErrorMessage"] = exc.Message;
                Logger.LogError(exc, "Error while updating shelter's items from Supervisor zone");
            }

            return PartialView("Grid/_GridForShelterItems", GetShelterItems(shelterId));
        }

        /// <summary>
        ///     Delete row of grid.
        /// </summary>
        public ActionResult DeleteGridForShelterItems([ModelBinder(typeof(DevExpressEditorsBinder))] SheltersDescInfo model, [ModelBinder(typeof(DevExpressEditorsBinder))] int shelterId)
        {
            ViewData["ShelterId"] = shelterId;
            try
            {
                ShelterRepository.DeleteShelterItem(model);
            }
            catch (Exception exc)
            {
                TempData["DeleteErrorMsg"] = exc.Message;
                Logger.LogError(exc, "Error while deleting shelter's items from Supervisor zone");
            }

            return PartialView("Grid/_GridForShelterItems", GetShelterItems(shelterId));
        }

        /// <summary>
        /// Get model for ShelterItems.
        /// </summary>
        /// <param name="shelterId">shelterId for which items to be shown.</param>
        /// <returns></returns>
        private static ShelterItemsModel GetShelterItems(int shelterId)
        {
            return new ShelterItemsModel { ShelterKey = shelterId, ShelterItems = ShelterRepository.GetShelterItemsDetail(shelterId) };
        }

        #endregion

        #region TAB - REPORTS

        /// <summary>
        /// For handling report posts.
        /// </summary>
        public ActionResult ReportPost(ReportsModel model)
        {
            switch (model.Category)
            {
                case "Shelters":
                    model.ReportViewerName = "reportViewerShelterDetail";
                    model.ReportUrl = "Report/_ShelterDetail";

                    Session["ShelterId"] = model.ShelterId;
                    ViewData["Report"] = new ShelterDetail();
                    break;

                case "Items":
                    model.ReportViewerName = "reportViewerItemStock";
                    model.ReportUrl = "Report/_ItemStock";

                    Session["ReportFilter"] = model.SearchParam;
                    ViewData["Report"] = new ItemsStock();
                    break;
            }

            return PartialView("Report/_ReportContent", model);
        }

        #region Item zone

        public ActionResult ItemStockReport()
        {
            ViewData["Report"] = new ItemsStock();
            return PartialView("Report/_ItemStock");
        }

        public ActionResult ItemStockExport()
        {
            return ReportViewerExtension.ExportTo(new ItemsStock());
        }

        #endregion

        #region Shelter zone

        public ActionResult ShelterDetailReport()
        {
            ViewData["Report"] = new ShelterDetail();
            return PartialView("Report/_ShelterDetail");
        }

        public ActionResult ShelterDetailExport()
        {
            return ReportViewerExtension.ExportTo(new ShelterDetail());
        }

        #endregion

        #endregion
    }
}