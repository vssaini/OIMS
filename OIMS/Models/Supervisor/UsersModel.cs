using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using OIMS.Repository.Supervisor;

namespace OIMS.Models.Supervisor
{
    public class UsersModel
    {
        #region Properties

        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgFirstName")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgLastName")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgEmail")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgPassword")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgConfirmPassword")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "ErrorMsgCharLimit")]
        public string ConfirmPassword { get; set; }

        #region Roles

        [Required(ErrorMessageResourceType = typeof(Resources.Supervisor), ErrorMessageResourceName = "UErrorMsgRole")]
        public int RoleId { get; set; }

        /// <summary>
        /// Get list of  user status.
        /// </summary>
        public IEnumerable<SelectListItem> Roles
        {
            get
            {
                var roleList = UserRepository.Roles.Select(role => new SelectListItem { Text = role.Role, Value = Convert.ToString(role.RoleId) }).ToList();

                return roleList;
            }
        }

        #endregion


        #endregion
    }
}