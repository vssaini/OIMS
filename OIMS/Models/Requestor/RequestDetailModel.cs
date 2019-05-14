using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OIMS.Repository;
using OIMS.Repository.Supervisor;
using RequestsRepository = OIMS.Repository.Requestor.RequestsRepository;

namespace OIMS.Models.Requestor
{
    public class RequestDetailModel
    {
        #region Properties

        public int RequestId { get; set; }
        public string RequestStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public string UpdatorName { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<ShelterRequest> Products { get; set; }
        public List<ItemRequest> Items { get; set; }
        public List<string> Messages { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set request details for Manager or Supervisor.
        /// </summary>
        /// <param name="reqId">Request Id of request</param>
        public void SetReqDetails(int reqId)
        {
            var request = RequestsRepository.Requests.FirstOrDefault(u => u.RequestId.Equals(reqId));

            if (request == null) return;

            RequestId = request.RequestId;
            RequestDate = request.CreatedOn;
            RequestStatus = request.Status;
            UpdatorName = GetUserName(request.UpdatorId);
            UpdateDate = request.UpdatedOn;

            Products = GetShelters(reqId);
            Items = GetItems(reqId);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get username as per the id.
        /// </summary>
        private static string GetUserName(int? userId)
        {
            if (userId == null)
                return null;

            var user = UserRepository.Users.FirstOrDefault(u => u.UserId.Equals(userId));

            if (user == null) return null;

            var builder = new StringBuilder(user.FirstName);
            builder.Append(" ").Append(user.LastName);

            return builder.ToString();
        }

        /// <summary>
        /// Get shelters in request as per request id.
        /// </summary>
        private static List<ShelterRequest> GetShelters(int reqId)
        {
            var prodList = new List<ShelterRequest>();
            var products = BaseRepository.OimsDataContext.Sheltersrequests.Where(p => p.O_Id.Equals(reqId));

            foreach (var prod in products)
            {
                var prodData = ShelterRepository.Shelters.FirstOrDefault(p => p.ShelterId.Equals(prod.P_Id));

                var oi = new ShelterRequest { Count = prod.OP_Quantity, ProdRecom = prod.OP_ProdRecom, ProdAlloc = prod.OP_ProdAlloc };

                if (prodData != null)
                    oi.Name = prodData.ShelterName;

                prodList.Add(oi);
            }
            return prodList;
        }

        /// <summary>
        /// Get items in request as per request id.
        /// </summary>
        private static List<ItemRequest> GetItems(int reqId)
        {
            var itemList = new List<ItemRequest>();
            var items = BaseRepository.OimsDataContext.Itemsrequests.Where(i => i.O_Id.Equals(reqId));

            foreach (var item in items)
            {
                var itemData = ItemRepository.Items.FirstOrDefault(i => i.ItemId.Equals(item.I_Id));

                var oi = new ItemRequest { Count = item.OI_Quantity, ItemRecom = item.OI_ItemRecom, ItemAlloc = item.OI_ItemAlloc };

                if (itemData != null)
                    oi.Name = itemData.ItemName;

                itemList.Add(oi);
            }
            return itemList;
        }

        #endregion

    }

    #region Classes

    /// <summary>
    /// Class for keeping record of requested item.
    /// </summary>
    public class ItemRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Count { get; set; }
        public float? ItemRecom { get; set; }
        public float? ItemAlloc { get; set; }
        public float TotalStock { get; set; }
    }

    /// <summary>
    /// Class for keeping record of requested shelter.
    /// </summary>
    public class ShelterRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Count { get; set; }
        public int? ProdRecom { get; set; }
        public int? ProdAlloc { get; set; }
        public float TotalStock { get; set; }
    }

    #endregion
}