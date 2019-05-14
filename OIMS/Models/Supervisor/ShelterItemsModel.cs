using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using OIMS.Repository.Supervisor;

namespace OIMS.Models.Supervisor
{
    /// <summary>
    /// Action for grid modification.
    /// </summary>
    public enum Action { Update, New }

    public class ShelterItemsModel
    {
        [Key]
        public int ShelterKey { get; set; }

        public List<SheltersDescInfo> ShelterItems { get; set; }

        /// <summary>
        /// Item name for selected item from Items.
        /// </summary>
        [Display(ResourceType = typeof(Resources.Supervisor), Name = "SItemLabel")]
        public string Item { get; set; }

        /// <summary>
        /// Get list of  items.
        /// </summary>
        public IEnumerable<SelectListItem> Items
        {
            get
            {
                return ItemRepository.Items.Select(item => new SelectListItem { Text = item.ItemName, Value = Convert.ToString(item.ItemId) }).ToList();
            }
        }
    }

    public class SheltersDescInfo
    {
        /// <summary>
        /// Shelter item key as primary key for tracking saved items.
        /// </summary>
        [Key]
        public int ShelterDescKey { get; set; }

        /// <summary>
        /// Shelter ID for which item details will be retrieved.
        /// </summary>
        public int ShelterId { get; set; }

        /// <summary>
        /// Item ID that will be used for getting item name.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemName")]
        public int ItemId { get; set; }

        /// <summary>
        /// Get or set item quantity to be used for creating shelter.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemQty")]
        public int ItemQuantity { get; set; }

        #region For using to show item's details

        public string ItemName { get; set; }

        public string Size { get; set; }

        public string Marking { get; set; }

        public string EnterQuantity { get; set; }

        public float InStock { get; set; }

        public string Vendor { get; set; }

        public DateTime? UpdatedOn { get; set; }

        #endregion
    }
}
