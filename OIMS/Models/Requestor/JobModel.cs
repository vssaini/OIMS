using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OIMS.Repository.Requestor;

namespace OIMS.Models.Requestor
{
    public class JobModel
    {
        /// <summary>
        /// The job selected by requestor.
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// The company selected by requestor.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get list of jobs for logged in requestor.
        /// </summary>
        public IEnumerable<SelectListItem> Jobs
        {
            get
            {
                var firstItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = Resources.Requestor.Req_SelectJobOption }, 1);

                var items = (from o in RequestsRepository.GetJobRequests()
                             where o.Job != null
                             select new SelectListItem { Text = o.Job, Value = o.Job }).ToList();

                return firstItem.Concat(items);
            }
        }

        /// <summary>
        /// Get list of companies.
        /// </summary>
        public IEnumerable<SelectListItem> Companies
        {
            get
            {
                var firstItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = Resources.Requestor.Req_SelectCompanyOption }, 1);

                var items = (from o in RequestsRepository.Companies
                             select new SelectListItem { Text = o.CompanyName, Value = o.CompanyName }).ToList();

                return firstItem.Concat(items);
            }
        }
    }
}