using System;
using System.Collections.Generic;
using OIMS.Global;
using OIMS.Models.Supervisor;
using OIMS.Repository.Requestor;

namespace OIMS.Models.Requestor
{
    public class CartModel
    {
        #region Properties

        public int CartId { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? ItemId { get; set; }
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        
        /// <summary>
        /// Get list of cart stuffs selected as per user
        /// </summary>
        public List<CartStuff> CartStuffs
        {
            get
            {
               return CartRepository.GetCartStuffs(Application.LoggedRequestorId);
            }
        }

        /// <summary>
        /// Return total count of products or items selected as per user.
        /// </summary>
        public int TotalProducts
        {
            get
            {
                return CartRepository.GetTotalStuffCount(Application.LoggedRequestorId);
            }
        }

        #region Models initialized

        /// <summary>
        /// Shelter items model.
        /// </summary>
        public ShelterItemsModel SiModel
        {
            get
            {
                var model = new ShelterItemsModel();
                return model;
            }
        }

        /// <summary>
        /// Shelters model.
        /// </summary>
        public SheltersModel SModel
        {
            get
            {
                var model = new SheltersModel();
                return model;
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represent cart stuff.
    /// </summary>
    public class CartStuff
    {
        public int CartId { get; set; }
        public int? ProductId { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public float Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}