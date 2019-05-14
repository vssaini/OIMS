using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using OIMS.Global;
using OIMS.Models.Requestor;
using OrderItem = OIMS.Models.Supervisor.ItemRequest;
using OrderProduct = OIMS.Models.Supervisor.ShelterRequest;

namespace OIMS.Repository.Supervisor
{
    public class RequestsRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of requests as per status.
        /// </summary>
        public static IList<RequestsModel> Requests
        {
            get
            {
                var requests = (from request in OimsDataContext.Requests
                                where request.O_Status.Equals(1) || request.O_Status.Equals(5)
                                select new RequestsModel
                                       {
                                           RequestId = request.O_Id,
                                           RequestorId = request.O_CreatedBy,
                                           CreatedOn = request.O_CreatedDate,
                                           StatusId = request.O_Status,
                                           Status = GetRequestStatus(request.O_Status),
                                           Job = request.O_Job,
                                           Company = request.O_Company,
                                           UpdatorId = request.O_UpdatedBy,
                                           UpdatedOn = request.O_UpdatedDate
                                       }).ToList();
                return requests;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get request status as per status id.
        /// </summary>
        public static string GetRequestStatus(int statusId)
        {
            var status = OimsDataContext.Requeststatus.FirstOrDefault(s => s.S_Id.Equals(statusId));
            return status != null ? status.S_Name : null;
        }

        /// <summary>
        /// Delete request and respective related request messages, request products and request items entries.
        /// </summary>
        /// <param name="reqId">Requst Id</param>
        public static void DeleteRequest(int reqId)
        {
            var request = OimsDataContext.Requests.FirstOrDefault(o => o.O_Id.Equals(reqId));

            OimsDataContext.Delete(request);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Get list of statuses for Supervisor.
        /// </summary>
        public static IEnumerable<SelectListItem> GetStatusList()
        {
            return (from s in OimsDataContext.Requeststatus
                    where s.ForRole.Equals(2) || s.ForRole.Equals(0)
                    select new SelectListItem { Text = s.S_Name, Value = Convert.ToString(s.S_Id) }).ToList();

        }

        /// <summary>
        /// Get all requests.
        /// </summary>
        /// <returns></returns>
        public static IList<RequestsModel> GetAllRequests()
        {
            var requests = (from request in OimsDataContext.Requests
                            select new RequestsModel
                            {
                                RequestId = request.O_Id,
                                RequestorId = request.O_CreatedBy,
                                CreatedOn = request.O_CreatedDate,
                                StatusId = request.O_Status,
                                Status = GetRequestStatus(request.O_Status),
                                Job = request.O_Job,
                                Company = request.O_Company,
                                UpdatorId = request.O_UpdatedBy,
                                UpdatedOn = request.O_UpdatedDate
                            }).ToList();

            return requests;
        }

        #endregion

        #region Request Detail Methods

        #region Recommend

        /// <summary>
        /// Recommend products as per stock and requested.
        /// </summary>
        /// <param name="updatedProducts">The list of products that been modified and need to update.</param>
        public static void RecommendShelters(MVCxGridViewBatchUpdateValues<OrderProduct, int> updatedProducts)
        {
            var orderShelters = OimsDataContext.Sheltersrequests;

            // Note: Key to understand terms
            // oProd.OP_Quantity - The quantity of shelters that were requested in total
            // oProd.OP_Pending - The quantity of shelters left pending to be delivered
            // oProd.OP_ProdAlloc - The quantity of shelters that have been allocated (after manager approval)
            // prod.ProdRecom - The quantity of shelter that had been marked as recommend by supervisor

            // Save items in separate other table for keeping track
            foreach (var prod in updatedProducts.Update)
            {
                if (!updatedProducts.IsValid(prod)) continue;

                var oProd = orderShelters.FirstOrDefault(i => i.OP_Id.Equals(prod.Id));
                if (oProd == null) continue;

                var prodAlloc = oProd.OP_ProdAlloc ?? 0;
                var prodAllRec = prodAlloc + prod.ProdRecom;

                #region Throw custom exception for some conditions

                // If Recommended more than Requested
                if (prod.ProdRecom > oProd.OP_Quantity)
                    throw new CustomException(Resources.Supervisor.RGridErrorMsgForExtraRecommend);

                // If sum of Allocated + Recommend exceed than Requested
                if (prodAllRec > oProd.OP_Quantity)
                    throw new CustomException(Resources.Supervisor.RGridErrorMsgForPartial);

                #endregion

                oProd.OP_ProdRecom = prod.ProdRecom;
                oProd.OP_Pending = (int?)(oProd.OP_Quantity - prodAllRec);

                OimsDataContext.FlushChanges();
            }

            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Recommend items as per stock and requested.
        /// </summary>
        /// <param name="updatedItems">The list of items that been modified and need to update.</param>
        public static void RecommendItems(MVCxGridViewBatchUpdateValues<OrderItem, int> updatedItems)
        {
            var orderItems = OimsDataContext.Itemsrequests;

            // Save items in separate other table for keeping track
            foreach (var item in updatedItems.Update)
            {
                if (!updatedItems.IsValid(item)) continue;

                var oItem = orderItems.FirstOrDefault(i => i.OI_Id.Equals(item.Id));
                if (oItem == null) continue;

                var itemAlloc = oItem.OI_ItemAlloc ?? 0;
                var itemAllRec = itemAlloc + item.ItemRecom;

                #region Throw custom exception for some conditions

                //if (Application.ReqStatus.Equals("Partial approved") && item.ItemRecom < oItem.OI_ItemRecom)
                //    throw new CustomException(Resources.Supervisor.RGridErrorMsgForPartial);

                if (item.ItemRecom > oItem.OI_Quantity)
                    throw new CustomException(Resources.Supervisor.RGridErrorMsgForExtraRecommend);

                // If sum of Allocated + Recommend exceed than Requested
                if (itemAllRec > oItem.OI_Quantity)
                    throw new CustomException(Resources.Supervisor.RGridErrorMsgForPartial);

                #endregion

                oItem.OI_ItemRecom = item.ItemRecom;
                oItem.OI_Pending = oItem.OI_Quantity - itemAllRec;
                OimsDataContext.FlushChanges();
            }

            OimsDataContext.SaveChanges();
        }

        #endregion

        /// <summary>
        /// Save request by moving each request details into 'allocated'. Also subtract same item quantities from stock too.
        /// </summary>
        /// <param name="requestorId">Id of the requestor who created the request.</param>
        /// <param name="updatorId">Id of the updator who is updating the request.</param>
        /// <param name="statusId">Status id for the request.</param>
        /// <param name="requestId">Request id assigned to current request.</param>
        public static void SaveRequest(int requestorId, int updatorId, int statusId, int requestId)
        {
            #region 1. If item (oi) in request, save items in table 'allocated' using 'orderitem'

            var orderItems = OimsDataContext.Itemsrequests.Where(o => o.O_Id.Equals(requestId) && o.OI_CreatedBy.Equals(requestorId));

            foreach (var oi in orderItems)
            {
                if (oi == null) continue;

                var iAlloc = OimsDataContext.Allocateds.FirstOrDefault(a => a.UserId.Equals(requestorId) && a.OrderId.Equals(requestId) && a.ItemId.Equals(oi.I_Id));

                // If item already in allocated for same request
                if (iAlloc != null)
                {
                    var itemRecom = oi.OI_ItemRecom ?? 0;

                    // Set mark quantity in allocated
                    var markQty = iAlloc.MarkQty;
                    iAlloc.MarkQty = markQty + itemRecom;
                    UpdateItemStock(iAlloc, itemRecom);
                }
                else
                {
                    var alloc = new OimsDataModel.Allocated
                    {
                        OrderId = requestId,
                        UserId = requestorId,
                        ItemId = oi.I_Id,
                        MarkQty = oi.OI_ItemRecom != null ? (float)oi.OI_ItemRecom : 0
                    };

                    OimsDataContext.Add(alloc);
                    UpdateItemStock(alloc);
                }
            }

            #endregion

            #region 2. If shelter (op) in request, save products in table 'allocated' using 'orderproduct'

            var orderProducts = OimsDataContext.Sheltersrequests.Where(o => o.O_Id.Equals(requestId) && o.OP_CreatedBy.Equals(requestorId));

            foreach (var op in orderProducts)
            {
                if (op == null) continue;

                // Get respective item's
                var pId = op.P_Id;
                var prodItems = ShelterRepository.ShelterItems.Where(p => p.P_Id.Equals(pId));

                foreach (var pi in prodItems)
                {
                    var pAlloc = OimsDataContext.Allocateds.FirstOrDefault(a => a.UserId.Equals(requestorId) && a.OrderId.Equals(requestId) && a.ItemId.Equals(pi.I_Id));

                    if (pAlloc == null)
                    {
                        if (op.OP_ProdRecom == null) continue;

                        var newAlloc = new OimsDataModel.Allocated
                                       {
                                           OrderId = requestId,
                                           UserId = requestorId,
                                           ItemId = pi.I_Id,
                                           MarkQty = pi.I_Qty * (int)op.OP_ProdRecom
                                       };

                        OimsDataContext.Add(newAlloc);
                        UpdateItemStock(newAlloc);
                    }
                    else
                    {
                        // If re-recommending, adjust the stock taking previous one 
                        // and later one in context
                        var prodRecom = op.OP_ProdRecom ?? 0;

                        // Set mark quantity in allocated
                        var markQty = pAlloc.MarkQty;
                        prodRecom = pi.I_Qty * prodRecom;
                        pAlloc.MarkQty = markQty + prodRecom;

                        UpdateItemStock(pAlloc, prodRecom);
                    }
                }
            }

            #endregion

            #region 3. Save status for current request

            var order = OimsDataContext.Requests.FirstOrDefault(o => o.O_Id.Equals(requestId));
            if (order == null) return;

            order.O_Status = statusId;
            order.O_UpdatedBy = updatorId;
            order.O_UpdatedDate = DateTime.Now;
            OimsDataContext.FlushChanges();

            #endregion

            // 4. Finally save all changes
            OimsDataContext.SaveChanges();
        }

        #region Helpers

        /// <summary>
        /// Update the stock of item as per the Allocated.
        /// </summary>
        /// <param name="alloc"></param>
        private static void UpdateItemStock(OimsDataModel.Allocated alloc)
        {
            // Get item matching with item id in 'allocated' from 'items'
            var item = OimsDataContext.Items.FirstOrDefault(i => i.I_Id.Equals(alloc.ItemId));

            if (item == null || !(alloc.MarkQty <= item.I_Quantity)) return;

            item.I_Quantity = item.I_Quantity - alloc.MarkQty;
            OimsDataContext.FlushChanges();
        }

        /// <summary>
        /// Update the stock of item as per the Allocated for the items that are re-recommended.
        /// </summary>
        /// <param name="alloc"></param>
        /// <param name="quantToReduce">Quantity to reduce.</param>
        private static void UpdateItemStock(OimsDataModel.Allocated alloc, float quantToReduce)
        {
            // Get item matching with item id in 'allocated' from 'items'
            var item = OimsDataContext.Items.FirstOrDefault(i => i.I_Id.Equals(alloc.ItemId));

            if (item == null) return;
            item.I_Quantity = item.I_Quantity - quantToReduce;
            OimsDataContext.FlushChanges();
        }

        #endregion

        #endregion
    }
}