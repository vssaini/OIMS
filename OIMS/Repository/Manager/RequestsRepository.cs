using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using OIMS.Models.Requestor;

namespace OIMS.Repository.Manager
{
    public class RequestsRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of requests for Manager.
        /// </summary>
        public static IList<RequestsModel> Requests
        {
            get
            {
                var requests = (from request in OimsDataContext.Requests
                                where request.O_Status.Equals(Int32.Parse(ConfigurationManager.AppSettings["MgrApproval"]))
                                select new RequestsModel
                                {
                                    RequestId = request.O_Id,
                                    RequestorId = request.O_CreatedBy,
                                    CreatedOn = request.O_CreatedDate,
                                    StatusId = request.O_Status,
                                    Status = GetOrderStatus(request.O_Status),
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
        /// Get order status as per status id.
        /// </summary>
        public static string GetOrderStatus(int statusId)
        {
            var status = OimsDataContext.Requeststatus.FirstOrDefault(s => s.S_Id.Equals(statusId));
            return status != null ? status.S_Name : null;
        }

        /// <summary>
        /// Get list of statuses for Manager.
        /// </summary>
        public static IEnumerable<SelectListItem> GetStatusList()
        {
            var statuses = (from s in OimsDataContext.Requeststatus
                            where s.ForRole.Equals(1) || s.ForRole.Equals(0)
                            select new SelectListItem { Text = s.S_Name, Value = Convert.ToString(s.S_Id) }).ToList();

            return FirstItem.Concat(statuses);
        }

        /// <summary>
        /// Add default first item to SelectListItem
        /// </summary>
        private static IEnumerable<SelectListItem> FirstItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = Resources.Supervisor.RSelectStatusOption
                }, 1);
            }
        }

        /// <summary>
        /// Save request status as approved or rejected.
        /// </summary>
        /// <param name="requestorId">Id of the requestor who created the request.</param>
        /// <param name="updatorId">Id of the updator who is updating the request.</param>
        /// <param name="statusId">Status id for the request.</param>
        /// <param name="requestId">Request id assigned to current request.</param>
        public static void SaveRequest(int requestorId, int updatorId, int statusId, int requestId)
        {
            var shelterStatusId = 0;
            var itemStatusId = 0;
            var approveStatusId = 0;

            #region 1. Set stock as per status

            // StatusId - 3 : Approved 
            // StatusId - 4 : Rejected
            // StatusId - 5 : Partial approved

            switch (statusId)
            {
                case 3:
                    var orderItems = OimsDataContext.Itemsrequests.Where(o => o.O_Id.Equals(requestId) && o.OI_CreatedBy.Equals(requestorId));
                    foreach (var oi in orderItems)
                    {
                        // Set allocate details in 'orderitem'
                        var itemAlloc = oi.OI_ItemAlloc ?? 0;
                        var itemRecom = oi.OI_ItemRecom ?? 0;
                        oi.OI_ItemAlloc = itemRecom + itemAlloc;

                        // Save status
                        shelterStatusId = (itemRecom + itemAlloc).Equals(oi.OI_Quantity) ? 3 : 5;
                        approveStatusId = shelterStatusId;

                        // Reset recommend and save changes
                        oi.OI_ItemRecom = null;
                        OimsDataContext.FlushChanges();
                    }

                    var orderProducts = OimsDataContext.Sheltersrequests.Where(o => o.O_Id.Equals(requestId) && o.OP_CreatedBy.Equals(requestorId));
                    foreach (var op in orderProducts)
                    {
                        // Set allocate details in 'orderproduct'
                        var prodAlloc = op.OP_ProdAlloc != null ? (float)op.OP_ProdAlloc : 0;
                        var prodRecom = op.OP_ProdRecom ?? 0;
                        op.OP_ProdAlloc = (int?)(prodRecom + prodAlloc);

                        // Save status
                        itemStatusId = (prodRecom + prodAlloc).Equals(op.OP_Quantity) ? 3 : 5;
                        approveStatusId = itemStatusId;

                        // Reset recommend and save changes
                        op.OP_ProdRecom = null;
                        OimsDataContext.FlushChanges();
                    }
                    break;

                case 4:
                    var allocs = OimsDataContext.Allocateds.Where(a => a.UserId.Equals(requestorId) && a.OrderId.Equals(requestId));

                    foreach (var alloc in allocs)
                    {
                        var item = OimsDataContext.Items.FirstOrDefault(i => i.I_Id.Equals(alloc.ItemId));
                        if (item == null) continue;

                        item.I_Quantity = item.I_Quantity + alloc.MarkQty;
                        OimsDataContext.FlushChanges();
                        approveStatusId = 4;
                    }
                    break;
            }

            #endregion

            #region 2. Save status for current request

            var request = OimsDataContext.Requests.FirstOrDefault(o => o.O_Id.Equals(requestId));
            if (request == null) return;

            //TODO: Test if the status fixed or not.
            if (shelterStatusId > 0 && itemStatusId > 0)
            {
                if (shelterStatusId.Equals(3) && itemStatusId.Equals(3))
                    request.O_Status = 3;
                else
                    request.O_Status = 5;
            }
            else
            {
                if (approveStatusId > 0)
                    request.O_Status = approveStatusId;
            }

            request.O_UpdatedBy = updatorId;
            request.O_UpdatedDate = DateTime.Now;
            OimsDataContext.FlushChanges();

            #endregion

            // 3. Finally save all changes
            OimsDataContext.SaveChanges();
        }

        #endregion
    }
}