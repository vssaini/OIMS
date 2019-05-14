using System.Collections.Generic;
using System.Linq;
using OIMS.Models.Supervisor;
using OimsDataModel;

namespace OIMS.Repository.Supervisor
{
    public class VendorRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of vendors in database binded with UsersModel.
        /// </summary>
        public static IList<VendorsModel> Vendors
        {
            get
            {
                var vendors = (from vendor in OimsDataContext.Vendors
                             select new VendorsModel
                             {
                                 Name = vendor.Name,
                                 Address = vendor.Address,
                                 Email = vendor.Email,
                                 Phone = vendor.Phone
                             }).ToList();
                return vendors;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get specific editable vendor.
        /// </summary>
        public static Vendor GetEditableVendor(string vendorName)
        {
            return (from v in OimsDataContext.Vendors
                    where v.Name == vendorName
                    select v).FirstOrDefault();
        }

        /// <summary>
        /// Create new vendor.
        /// </summary>
        public static void SaveNewVendor(VendorsModel vendor)
        {
            var newVendor= new Vendor
                          {
                              Name = vendor.Name,
                              Address = vendor.Address,
                              Phone = vendor.Phone,
                              Email = vendor.Email
                          };

            OimsDataContext.Add(newVendor);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Delete vendor with specific name.
        /// </summary>
        public static void DeleteVendor(string vendorName)
        {
            var vendor = GetEditableVendor(vendorName);
            OimsDataContext.Delete(vendor);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Update vendor.
        /// </summary>
        public static void UpdateVendor(VendorsModel vendor)
        {
            var editVendor = GetEditableVendor(vendor.Name);
            if (editVendor == null) return;

            editVendor.Address = vendor.Address;
            editVendor.Email = vendor.Email;
            editVendor.Phone = vendor.Phone;

            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Check if vendor name is unique.
        /// </summary>
        /// <param name="model">VendorsModel object.</param>
        /// <param name="action">Action enum.</param>
        /// <returns></returns>
        public static bool IsUniqueVendor(VendorsModel model, Action action)
        {
            var unique = false;

            switch (action)
            {
                case Action.New:
                    var vendor = Vendors.FirstOrDefault(v => v.Name.Equals(model.Name));
                    unique = vendor == null;
                    break;

                case Action.Update:
                    var nameRecord = Vendors.FirstOrDefault(v => v.Name.ToLower().Equals(model.Email.ToLower()));
                    unique = nameRecord == null || nameRecord.Name.Equals(model.Name);
                    break;
            }

            return unique;
        }

        #endregion
    }
}