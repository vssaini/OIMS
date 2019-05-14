using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using OIMS.Global;
using OIMS.Repository.Supervisor;

namespace OIMS.Models.Supervisor
{
    public class SheltersModel
    {
        [Key]
        public int ShelterId { get; set; }

        public string ShelterName { get; set; }

        public int? ShelterStock{ get; set; }

        /// <summary>
        /// Get list of  shelters.
        /// </summary>
        public IEnumerable<SelectListItem> Shelters
        {
            get
            {
                var firstItem = Enumerable.Repeat(new SelectListItem { Value = "-1", Text = Resources.Supervisor.RSelectShelterOption }, 1);

                var shelters =  ShelterRepository.Shelters.Select(product => new SelectListItem { Text = product.ShelterName, Value = Convert.ToString(product.ShelterId) }).ToList();

                return Application.LoggedRequestorId != null ? firstItem.Concat(shelters) : shelters;
            }
        }
    }
}
