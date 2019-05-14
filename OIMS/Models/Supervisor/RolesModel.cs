using System.ComponentModel.DataAnnotations;

namespace OIMS.Models.Supervisor
{
    public class RolesModel
    {
        /// <summary>
        /// The role id.
        /// </summary>
        [Key]
        [Display(Name="Role")]
        public int RoleId{ get; set; }

        /// <summary>
        /// The name of the role for concerned id.
        /// </summary>
        public string Role { get; set; }
    }
}
