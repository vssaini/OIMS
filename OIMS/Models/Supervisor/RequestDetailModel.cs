using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using OIMS.Global;
using OIMS.Repository;
using OIMS.Repository.Supervisor;

namespace OIMS.Models.Supervisor
{
    public class RequestDetailModel
    {
        #region Properties

        public int RequestId { get; set; }
        public int StatusId { get; set; }
        public int DbStatusId { get; set; }
        public string RequestStatus { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestorName { get; set; }
        public string UpdatorName { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<ShelterRequest> Shelters { get; set; }
        public List<ItemRequest> Items { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public float ShelterStock { get; set; }
        public float ItemStock { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set request details for Manager or Supervisor.
        /// </summary>
        /// <param name="reqId">Request Id of request</param>
        public void SetReqDetails(int reqId)
        {
            var request = Application.Filter!=null ? 
                RequestsRepository.GetAllRequests().FirstOrDefault(u => u.RequestId.Equals(reqId)) : 
                RequestsRepository.Requests.FirstOrDefault(u => u.RequestId.Equals(reqId));

            if (request == null) return;

            RequestId = request.RequestId;
            DbStatusId = request.StatusId;
            RequestDate = request.CreatedOn;
            RequestStatus = request.Status;
            RequestorName = GetUserName(request.RequestorId);
            UpdatorName = GetUserName(request.UpdatorId);
            UpdateDate = request.UpdatedOn;

            Shelters = GetShelters(reqId);
            Items = GetItems(reqId);
            Statuses = RequestsRepository.GetStatusList();
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
            var shelterList = new List<ShelterRequest>();
            var shelters = BaseRepository.OimsDataContext.Sheltersrequests.Where(p => p.O_Id.Equals(reqId));

            foreach (var shelter in shelters)
            {
                var shelterData = ShelterRepository.Shelters.FirstOrDefault(p => p.ShelterId.Equals(shelter.P_Id));

                var oi = new ShelterRequest
                {
                    Id = shelter.OP_Id,
                    ProdRecom = shelter.OP_ProdRecom,
                    ProdAlloc = shelter.OP_ProdAlloc,
                    ProdPending = shelter.OP_Pending,
                    CountReq = shelter.OP_Quantity,
                    TotalStock = (float)Math.Floor((PossibleShelterStock(shelter.P_Id)))
                };

                if (shelterData != null)
                    oi.Name = shelterData.ShelterName;

                shelterList.Add(oi);
            }
            return shelterList;
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

                var oi = new ItemRequest
                         {
                             Id = item.OI_Id,
                             ItemRecom = item.OI_ItemRecom,
                             ItemAlloc = item.OI_ItemAlloc,
                             ItemPending = item.OI_Pending,
                             CountReq = item.OI_Quantity,
                             TotalStock = float.Parse(GetItemStock(item.I_Id))
                         };

                if (itemData != null)
                    oi.Name = itemData.ItemName;

                itemList.Add(oi);
            }
            return itemList;
        }

        /// <summary>
        /// Get possible shelter stock by existing items.
        /// </summary>
        public static decimal PossibleShelterStock(int shelterId)
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

            if (stockList.Count <= 0) return 0;
            var stock = Math.Floor((decimal)stockList.Min());
            return stockList.Count > 0 ? stock : 0;
        }

        /// <summary>
        /// Get possible product stock by existing items.
        /// </summary>
        private static string GetItemStock(int itemId)
        {
            var items = ItemRepository.GetItemTotalQuantity(itemId);
            return Convert.ToString(items);
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
        public float CountReq { get; set; }
        public float? ItemRecom { get; set; }
        public float? ItemAlloc { get; set; }
        public float? ItemPending { get; set; }
        public float TotalStock { get; set; }
    }

    /// <summary>
    /// Class for keeping record of requested shelter.
    /// </summary>
    public class ShelterRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float CountReq { get; set; }
        public int? ProdRecom { get; set; }
        public int? ProdAlloc { get; set; }
        public int? ProdPending { get; set; }
        public float TotalStock { get; set; }
    }

    #endregion
}