using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using OIMS.Models.Requestor;
using OIMS.Repository;
using OIMS.Repository.Supervisor;
using RequestsRepository = OIMS.Repository.Manager.RequestsRepository;

namespace OIMS.Models.Manager
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

        public List<ShelterRequest> Products { get; set; }
        public List<ItemRequest> Items { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }

        public float ProductStock { get; set; }
        public float ItemStock { get; set; }

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
            DbStatusId = request.StatusId;
            RequestDate = request.CreatedOn;
            RequestStatus = request.Status;
            RequestorName = GetUserName(request.RequestorId);
            UpdatorName = GetUserName(request.UpdatorId);
            UpdateDate = request.UpdatedOn;

            Products = GetShelters(reqId);
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
        /// Get products in order as per request id.
        /// </summary>
        private static List<ShelterRequest> GetShelters(int reqId)
        {
            var prodList = new List<ShelterRequest>();
            var products = BaseRepository.OimsDataContext.Sheltersrequests.Where(p => p.O_Id.Equals(reqId));

            foreach (var prod in products)
            {
                var prodData = ShelterRepository.Shelters.FirstOrDefault(p => p.ShelterId.Equals(prod.P_Id));

                var oi = new ShelterRequest
                {
                    Id = prod.OP_Id,
                    ProdRecom = prod.OP_ProdRecom,
                    Count = prod.OP_Quantity,
                    TotalStock = (float)Math.Floor((double.Parse(PossibleRequestStock(prod.P_Id))))
                };

                if (prodData != null)
                    oi.Name = prodData.ShelterName;

                prodList.Add(oi);
            }
            return prodList;
        }

        /// <summary>
        /// Get items in order as per request id.
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
                             Count = item.OI_Quantity,
                             TotalStock = float.Parse(GetItemStock(item.I_Id))
                         };

                if (itemData != null)
                    oi.Name = itemData.ItemName;

                itemList.Add(oi);
            }
            return itemList;
        }

        /// <summary>
        /// Get possible product stock by existing items.
        /// </summary>
        private static string PossibleRequestStock(int productId)
        {
            var items = ShelterRepository.GetShelterItemsByKey(productId);

            var stockList = (from item in items let itemQuantity = item.ItemQuantity let totalItemsQuantity = ItemRepository.GetItemTotalQuantity(item.ItemId) where totalItemsQuantity > itemQuantity select totalItemsQuantity / itemQuantity).ToList();

            return stockList.Count > 0 ? string.Format("{0:N2}", stockList.Min()) : Convert.ToString(0);

        }

        /// <summary>
        /// Get possible product stock by existing items.
        /// </summary>
        private static string GetItemStock(int itemId)
        {
            var items = ItemRepository.GetItemTotalQuantity(itemId);

            return items > 0 ? string.Format("{0}", items) : Convert.ToString(0);

        }

        #endregion
    }
}