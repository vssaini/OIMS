using System;
using System.ComponentModel.DataAnnotations;

namespace OIMS.Models.Supervisor
{
    public class ItemsModel
    {
        #region Properties

        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemName")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string ItemName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgItemQty")]
        public float ItemQuantity { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgSize")]
        public string Size { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "IErrorMsgMarking")]
        public string Marking { get; set; }

        public string Vendor { get; set; }
        public DateTime? UpdatedOn { get; set; }

        #endregion
    }
}