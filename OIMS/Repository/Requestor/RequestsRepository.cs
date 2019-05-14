using System;
using System.Collections.Generic;
using System.Linq;
using OIMS.Global;
using OIMS.Models.Requestor;
using OimsDataModel;

namespace OIMS.Repository.Requestor
{
    public class RequestsRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of requests as per user.
        /// </summary>
        public static IList<RequestsModel> Requests
        {
            get
            {
                if (Application.LoggedRequestorId == null)
                    return null;

                var userId = (int)Application.LoggedRequestorId;

                var requests = (from request in OimsDataContext.Requests
                                where request.O_CreatedBy.Equals(userId)
                                select new RequestsModel
                                       {
                                           RequestId = request.O_Id,
                                           Job = request.O_Job,
                                           Company = request.O_Company,
                                           CreatedOn = request.O_CreatedDate,
                                           Status = GetOrderStatus(request.O_Status),
                                           UpdatorId = request.O_UpdatedBy,
                                           UpdatedOn = request.O_UpdatedDate
                                       }).ToList();

                return requests;
            }
        }

        /// <summary>
        /// Get list of companies.
        /// </summary>
        public static IList<Company> Companies
        {
            get
            {
                return OimsDataContext.Companies.ToList();
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
        /// Delete request and respective related order messages, order products and order items entries.
        /// </summary>
        /// <param name="reqId">Requst Id</param>
        public static void DeleteRequest(int reqId)
        {
            var request = OimsDataContext.Requests.FirstOrDefault(o => o.O_Id.Equals(reqId));

            OimsDataContext.Delete(request);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Save status for current request id.
        /// </summary>
        /// <param name="reqId">Request for which status would be saved.</param>
        /// <param name="statusId">Status selected by user.</param>
        /// <param name="userId">User who modified the request.</param>
        public static void SaveStatus(int reqId, int statusId, int userId)
        {
            var request = OimsDataContext.Requests.FirstOrDefault(o => o.O_Id.Equals(reqId));

            if (request == null) return;

            request.O_Status = statusId;
            request.O_UpdatedBy = userId;
            request.O_UpdatedDate = DateTime.Now;
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Get list of RequestsModel containing jobs for respective user.
        /// </summary>
        /// <returns></returns>
        public static IList<RequestsModel> GetJobRequests()
        {
            if (Application.LoggedRequestorId == null)
                return null;

            var userId = (int)Application.LoggedRequestorId;

            var requests = (from order in OimsDataContext.Requests
                            where order.O_CreatedBy.Equals(userId)
                            select new RequestsModel
                            {
                                Job = order.O_Job
                            }).Distinct().ToList();

            return requests;
        }

        #endregion
    }
}