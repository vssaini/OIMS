using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using OIMS.Repository.Supervisor;

namespace OIMS.Models.Supervisor
{
    public class VendorsModel
    {
        #region Properties
       
        /// <summary>
        /// Vendor name for selected name from Vendors.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "VErrorMsgName")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Address { get; set; }

        [StringLength(10, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Phone { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Email { get; set; }

        /// <summary>
        /// Vendor name for selected vendor from Vendors.
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Get list of  vendors.
        /// </summary>
        public IEnumerable<SelectListItem> Vendors
        {
            get
            {
                return VendorRepository.Vendors.Select(item => new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Name) }).ToList();
            }
        }

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
        
        #endregion
    }
}