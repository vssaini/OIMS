using System;
using System.Linq;
using System.Text;
using System.Web;
using OIMS.Repository.Supervisor;
using OimsDataModel;

namespace OIMS.Repository
{
    /// <summary>
    /// Class for managing database related job.
    /// </summary>
    public class BaseRepository
    {
        const string OimsDbContextKey = "OimsDbContext";

        #region Properties

        /// <summary>
        /// Oims database context.
        /// </summary>
        public static OimsDbContext OimsDataContext
        {
            get
            {
               if (HttpContext.Current.Items[OimsDbContextKey] == null)
                    HttpContext.Current.Items[OimsDbContextKey] = new OimsDbContext();
                return (OimsDbContext)HttpContext.Current.Items[OimsDbContextKey];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get information about user on basis of roles.
        /// </summary>
        /// <param name="roleId">RoleId of user's roles.</param>
        /// <param name="email">Email address of user (unique).</param>
        /// <param name="username">Return concatenated first name and last name as username.</param>
        /// <param name="userId">Return user id of user.</param>
        public static void GetUserInfo(int roleId, string email, out string username, out string userId)
        {
            string userName = null;
            string userid = null;

            var builder = new StringBuilder();

            var user = (from u in UserRepository.Users
                        where u.RoleId.Equals(roleId) && u.Email.Equals(email)
                        select u).FirstOrDefault();

            if (user != null)
            {
                builder.Append(user.FirstName).Append(" ").Append(user.LastName);
                userName = builder.ToString();
                userid = Convert.ToString(user.UserId);
            }

            userId = userid;
            username = userName;
        }

        /// <summary>
        /// Check if user exist in database matching with credentials.
        /// </summary>
        /// <param name="email">Email address of the user.</param>
        /// <param name="password">Password of the user.</param>
        /// <returns>Return true or false based on user existence.</returns>
        public static bool CheckUserExist(string email, string password)
        {
            var user = (from u in UserRepository.Users
                        where u.Email.Equals(email) && u.Password.Equals(password)
                        select u).FirstOrDefault();

            return user != null;
        }

        #endregion
    }
}