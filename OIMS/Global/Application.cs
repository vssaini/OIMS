using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using Resources;

namespace OIMS.Global
{
    /// <summary>
    /// Type of items of cart
    /// </summary>
    public enum CartType { None, Product, Item }

    public enum Status { Approved, Closed, Dispatched, InProcess, ApprovalPending, Rejected }

    public class Application
    {
        #region Properties

        /// <summary>
        /// Check if website is still in trial mode (30 days)
        /// </summary>
        public static bool IsTrialMode
        {
            get
            {
                var trialStartDate = new DateTime(2014, 12, 09);
                var currentDate = DateTime.Now;
                var daysPassed = (currentDate - trialStartDate).Days;
                return daysPassed <= 61;
            }
        }

        /// <summary>
        /// The key used to store the license into cache
        /// </summary>
        public const string LicenseCacheKey = "License";

        /// <summary>
        /// Check if website is using valid license file.
        /// </summary>
        public static bool IsLicensed
        {
            get
            {
                return ValidateLicenseFile();
            }
        }

        /// <summary>
        /// Get rool url of website.
        /// </summary>
        public static string RootUrl
        {
            get
            {
                // Set website virtual path for images
                var virtualPath = HttpRuntime.AppDomainAppVirtualPath;

                if (virtualPath != null)
                {
                    virtualPath = virtualPath.EndsWith("/") ? virtualPath : virtualPath + "/";
                }

                return virtualPath;
            }
        }

        /// <summary>
        /// Get url for logo.
        /// </summary>
        public static string LogoUrl
        {
            get
            {
                return string.Format("{0}Content/Images/Logo.png", RootUrl);
            }
        }

        #region Session handlers

        /// <summary>
        /// Get session value of logged in manager.
        /// </summary>
        public static string LoggedManager
        {
            get
            {
                var user = HttpContext.Current.Session["Manager"];
                if (user != null)
                    return user as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of logged in supervisor.
        /// </summary>
        public static string LoggedSupervisor
        {
            get
            {
                var user = HttpContext.Current.Session["Supervisor"];
                if (user != null)
                    return user as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of logged in requestor.
        /// </summary>
        public static string LoggedRequestor
        {
            get
            {
                var user = HttpContext.Current.Session["Requestor"];
                if (user != null)
                    return user as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of logged in manager id.
        /// </summary>
        public static int? LoggedManagerId
        {
            get
            {
                var userId = HttpContext.Current.Session["ManagerId"] as string;

                if (userId == null)
                    return null;

                return Int32.Parse(userId);
            }
        }

        /// <summary>
        /// Get session value of logged in supervisor id.
        /// </summary>
        public static int? LoggedSupervisorId
        {
            get
            {
                var userId = HttpContext.Current.Session["SupervisorId"] as string;

                if (userId == null)
                    return null;

                return Int32.Parse(userId);
            }
        }

        /// <summary>
        /// Get session value of logged in requestor id.
        /// </summary>
        public static int? LoggedRequestorId
        {
            get
            {
                var userId = HttpContext.Current.Session["RequestorId"] as string;

                if (userId == null)
                    return null;

                return Int32.Parse(userId);
            }
        }

        /// <summary>
        /// Get session value of name of the job for requests.
        /// </summary>
        public static string Job
        {
            get
            {
                var job = HttpContext.Current.Session["Job"];
                if (job != null)
                    return job as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of name of the company for requests.
        /// </summary>
        public static string Company
        {
            get
            {
                var company = HttpContext.Current.Session["Company"];
                if (company != null)
                    return company as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of filter for requests.
        /// </summary>
        public static string Filter
        {
            get
            {
                var filter = HttpContext.Current.Session["Filter"];
                if (filter != null)
                    return filter as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of status of selected request.
        /// </summary>
        public static string ReqStatus
        {
            get
            {
                var status = HttpContext.Current.Session["Status"];
                if (status != null)
                    return status as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of filter for report.
        /// </summary>
        public static string ReportFilter
        {
            get
            {
                var filter = HttpContext.Current.Session["ReportFilter"];
                if (filter != null)
                    return filter as string;
                return null;
            }
        }

        /// <summary>
        /// Get session value of shelter id for report.
        /// </summary>
        public static string ShelterId
        {
            get
            {
                var shelterId = HttpContext.Current.Session["ShelterId"];
                if (shelterId != null)
                    return shelterId as string;
                return null;
            }
        }


        #endregion

        #region Password Security

        /// <summary>
        /// Encrypt password.
        /// </summary>
        public static string EncryptPassword(string password)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            var clearBytes = Encoding.Unicode.GetBytes(password);

            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return password;

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
        }

        /// <summary>
        /// Decrypt password.
        /// </summary>
        public static string DecryptPassword(string cipherText)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            var cipherBytes = Convert.FromBase64String(cipherText);

            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return cipherText;

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #endregion

        /// <summary>
        /// Get status id from configuration as per key.
        /// </summary>
        /// <param name="key">AppSettings key.</param>
        /// <returns>Return status id.</returns>
        public static int GetStatusId(string key)
        {
            return Int32.Parse(ConfigurationManager.AppSettings[key]);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validate license file for evaluation purpose.
        /// </summary>
        /// <returns>Return true if valid license file found.</returns>
        public static bool ValidateLicenseFile()
        {
            string licKey = null;

            // Check if layout is in the cache
            if (HttpContext.Current.Cache[LicenseCacheKey] != null)
                return (bool)HttpContext.Current.Cache[LicenseCacheKey];

            // Read license file
            var virDir = string.Concat((VirtualPathUtility.ToAbsolute("~")), Common.LicensePath);
            var licenseFile = HttpContext.Current.Server.MapPath(virDir);

            if (File.Exists(licenseFile))
                licKey = File.ReadAllText(licenseFile);

            // If no content in file
            if (string.IsNullOrEmpty(licKey)) return false;

            // It must also contain -'s
            if (licKey.Length != 45 || !licKey.Contains("-"))
                return false;

            // It has to have 6 parts or its invalid
            var splitKey = licKey.Split('-');
            if (splitKey.Length != 6) return false;

            // Join elements 1 through 5, then convert to a byte array
            var baseKey = string.Join("-", splitKey, 0, 5);
            var asciiBytes = Encoding.ASCII.GetBytes(baseKey);

            // Get the CRC
            var baseCrc = Crc32.Compute(asciiBytes);

            // Now let's compare the CRC to make sure its a valid key
            var valid = string.Equals(splitKey[5], baseCrc.ToString("X8"), StringComparison.OrdinalIgnoreCase);

            // Insert into cache. Add dependency on the file
            HttpContext.Current.Cache.Insert(LicenseCacheKey, valid, new CacheDependency(licenseFile));

            // Return layout item from cache
            return (bool)HttpContext.Current.Cache[LicenseCacheKey];
        }

        /// <summary>
        /// Reset all sessions to null values.
        /// </summary>
        public static void ResetSession()
        {
            var session = HttpContext.Current.Session;

            // Reset all session(s) to null
            session["Manager"] = session["Supervisor"] = session["Requestor"] = null;
            session["ManagerId"] = session["SupervisorId"] = session["RequestorId"] = null;
            session["Job"] = session["Company"] = session["Filter"] = session["Status"] = null;
            session["ShelterId"] = null;
        }

        /// <summary>
        /// Send email to email id passed.
        /// </summary>
        /// <param name="fullName">Full name of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="reqId">Request id for current approving request.</param>
        public static void SendEmail(string fullName, string email, int reqId)
        {
            // Read emailing information from web.config
            var fromEmail = ConfigurationManager.AppSettings["FromEmail"];
            var fromName = ConfigurationManager.AppSettings["FromName"];
            var smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            var smtpPort = Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            var smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
            var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            // Email body
            var emailBody = "<html><body>" +
                            "<p style='color: #222222; font:normal 9pt Verdana; margin: 10px 0;'>Hi {DISPLAY_NAME},</p>" +
                            "<p style='color: #222222; font:normal 9pt Verdana; margin: 10px 0;'>A request was submitted by Supervisor <b>" + LoggedSupervisor + "</b> for approval. Please approve it from OIMS.</p>" +
                            "<p style='color: #222222; font:normal 9pt Verdana; margin: 10px 0;'>Thank you!</p>" +
                            "</body></html>";

            // Create a new mail message
            var message = new MailMessage { From = new MailAddress(fromEmail, fromName) };

            // Set To and Subject            
            message.To.Add(new MailAddress(email, fullName));
            message.Subject = "Approve request Id - " + reqId;

            // Get the template HTML content
            var template = string.Format("<body>{0}</body>", emailBody);

            // Replace display name in the template
            template = template.Replace("{DISPLAY_NAME}", fullName);

            // Create html view
            message.IsBodyHtml = true;
            var htmlView = AlternateView.CreateAlternateViewFromString(template, Encoding.UTF8, "text/html");
            message.AlternateViews.Add(htmlView);

            // Prepare SMTP server
            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = enableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword)
            };

            // Send mail message
            smtp.Send(message);
        }

        #endregion
    }
}