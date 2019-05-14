using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OIMS.Global;
using OIMS.Repository.Supervisor;

namespace OIMS.Models
{
    public class ReportsModel
    {
        /// <summary>
        /// Get or set report viewer name.
        /// </summary>
        public string ReportViewerName { get; set; }

        /// <summary>
        /// Get or set url for respective report.
        /// </summary>
        public string ReportUrl { get; set; }

        /// <summary>
        /// Search parameter for items.
        /// </summary>
        public string SearchParam { get; set; }

        /// <summary>
        /// Category of report selected by user for viewing report.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Get list of  user status.
        /// </summary>
        public IEnumerable<SelectListItem> Categories
        {
            get
            {
                var optionsList = new List<SelectListItem>
                                {
                                    new SelectListItem {Text = Resources.Supervisor.TabShelters, Value = Resources.Supervisor.TabShelters, Selected = true},
                                    new SelectListItem {Text = Resources.Supervisor.TabItems, Value = Resources.Supervisor.TabItems}
                                };

                return optionsList;
            }
        }

        /// <summary>
        /// Represent single shelter's id selected by end user.
        /// </summary>
        public string ShelterId { get; set; }

        /// <summary>
        /// Get list of  shelters.
        /// </summary>
        public IEnumerable<SelectListItem> Shelters
        {
            get
            {
                var firstItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = Resources.Supervisor.RSelectShelterOption }, 1);

                var shelters = ShelterRepository.Shelters.Select(s=> new SelectListItem { Text = s.ShelterName, Value = Convert.ToString(s.ShelterId) }).ToList();

                return Application.LoggedRequestorId != null ? firstItem.Concat(shelters) : shelters;
            }
        }
    }
}