using System;
using System.ComponentModel.DataAnnotations;

namespace OIMS.Models.Supervisor
{
    public class ItemsLogModel
    {
        #region Properties

        [Key]
        public int Id { get; set; }

        public int ItemId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemQty")]
        public float ItemQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemName")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string VendorName { get; set; }

        public DateTime EntryDate { get; set; }

        #endregion
    }
}