using System.Collections.Generic;
using System.Linq;
using OIMS.Models.Supervisor;
using OimsDataModel;

namespace OIMS.Repository.Supervisor
{
    public class UserRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of users in database binded with UsersModel.
        /// </summary>
        public static IList<UsersModel> Users
        {
            get
            {
                var users = (from user in OimsDataContext.Users
                             select new UsersModel
                             {
                                 UserId = user.U_Id,
                                 FirstName = user.U_FirstName,
                                 LastName = user.U_LastName,
                                 Email = user.U_Email,
                                 Password = user.U_Password,
                                 RoleId = user.R_Id
                             }).ToList();
                return users;
            }
        }


        /// <summary>
        /// Get list of roles in database binded with RolesModel
        /// </summary>
        public static IList<RolesModel> Roles
        {
            get
            {
                var roles = (from role in OimsDataContext.Roles
                             select new RolesModel
                             {
                                 RoleId = role.R_Id,
                                 Role = role.R_Name
                             }).ToList();

                return roles;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get specific editable user.
        /// </summary>
        public static User GetEditableUser(int id)
        {
            return (from u in OimsDataContext.Users
                    where u.U_Id == id
                    select u).FirstOrDefault();
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        public static void SaveNewUser(UsersModel user)
        {
            var newUser = new User
                          {
                              R_Id = user.RoleId, // Will create user with role specified
                              U_FirstName = user.FirstName,
                              U_LastName = user.LastName,
                              U_Email = user.Email,
                              U_Password = user.Password
                          };

            OimsDataContext.Add(newUser);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Delete user with specific user id.
        /// </summary>
        public static void DeleteUser(int id)
        {
            var user = GetEditableUser(id);
            OimsDataContext.Delete(user);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Update user.
        /// </summary>
        public static void UpdateUser(UsersModel user)
        {
            var editUser = GetEditableUser(user.UserId);
            if (editUser == null) return;

            editUser.U_FirstName = user.FirstName;
            editUser.U_LastName = user.LastName;
            editUser.U_Email = user.Email;
            editUser.U_Password = user.Password;
            editUser.R_Id = user.RoleId;

            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Check if email address is unique.
        /// </summary>
        /// <param name="model">UsersModel object.</param>
        /// <param name="action">Action enum.</param>
        /// <returns></returns>
        public static bool IsUniqueEmail(UsersModel model, Action action)
        {
            var unique = false;

            switch (action)
            {
                case Action.New:
                    var user = Users.FirstOrDefault(u => u.Email.Equals(model.Email));
                    unique = user == null;
                    break;

                case Action.Update:
                    var emailRecord = Users.FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower()));
                    unique = emailRecord == null || emailRecord.UserId.Equals(model.UserId);
                    break;
            }

            return unique;
        }

        #endregion
    }
}