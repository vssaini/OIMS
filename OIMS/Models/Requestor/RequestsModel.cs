using System;

namespace OIMS.Models.Requestor
{
    public class RequestsModel
    {
        #region Properties

        public int RequestId { get; set; }
        public string Job { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Company { get; set; }
        public DateTime CreatedOn { get; set; }
        public int RequestorId { get; set; }
        public int? UpdatorId { get; set; }
        public DateTime? UpdatedOn { get; set; }

        #endregion
    }
}