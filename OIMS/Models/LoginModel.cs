using OIMS.Repository;

namespace OIMS.Models
{
    public enum Roles { Manager, Supervisor, Requestor }

    public class LoginModel
    {
        #region Properties

        // For manager
        public string ManagerUsername { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPass { get; set; }

        // For supervisor
        public string SupervisorUsername { get; set; }
        public string SupervisorEmail { get; set; }
        public string SupervisorPass { get; set; }

        // For requestor
        public string RequestorEmail { get; set; }
        public string RequestorPass { get; set; }

        #endregion

        /// <summary>
        /// Authenticate user using existing credentials and passwords.
        /// </summary>
        /// <param name="roles">Role of user.</param>
        /// <returns>True or False based on user authenticated.</returns>
        public bool AuthenticateUser(Roles roles)
        {
            switch (roles)
            {
                case Roles.Manager:
                    return BaseRepository.CheckUserExist(ManagerEmail, ManagerPass);
                case Roles.Supervisor:
                    return BaseRepository.CheckUserExist(SupervisorEmail, SupervisorPass);
                case Roles.Requestor:
                    return BaseRepository.CheckUserExist(RequestorEmail, RequestorPass);
            }

            return false;
        }
    }
}