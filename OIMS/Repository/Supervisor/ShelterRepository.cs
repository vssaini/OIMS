using System;
using System.Collections.Generic;
using System.Linq;
using OIMS.Global;
using OIMS.Models.Supervisor;
using OimsDataModel;
using Action = OIMS.Models.Supervisor.Action;

namespace OIMS.Repository.Supervisor
{
    public class ShelterRepository : BaseRepository
    {
        #region Properties - Shelters

        /// <summary>
        /// Get list of shelters in database binded with SheltersModel.
        /// </summary>
        public static IList<SheltersModel> Shelters
        {
            get
            {
                var shelters = (from shelter in OimsDataContext.Shelters
                                orderby shelter.P_Name
                                select new SheltersModel
                                {
                                    ShelterId = shelter.P_Id,
                                    ShelterName = shelter.P_Name,
                                    ShelterStock = shelter.P_Dummy

                                }).ToList();
                return shelters;
            }
        }

        #endregion

        #region Properties - Shelter's Items

        /// <summary>
        /// Get items related to Shelters.
        /// </summary>
        public static IList<Shelterdescription> ShelterItems
        {
            get
            {
                var shelterItems = (from si in OimsDataContext.Shelterdescriptions
                                    orderby si.I_Id
                                    select si).ToList();
                return shelterItems;
            }
        }

        #endregion

        #region Methods - Shelters

        /// <summary>
        /// Get specific editable shelter.
        /// </summary>
        public static Shelter GetEditableShelter(int id)
        {
            return (from i in OimsDataContext.Shelters
                    where i.P_Id == id
                    select i).FirstOrDefault();
        }

        /// <summary>
        /// Create new shelter and return shelterid of saved shelter.
        /// </summary>
        public static int SaveShelter(Shelter shelter)
        {
            OimsDataContext.Add(shelter);
            OimsDataContext.SaveChanges();

            // Get product id of saved product
            var prodId = from p in Shelters
                         where p.ShelterName == shelter.P_Name
                         select p.ShelterId;

            return prodId.FirstOrDefault();
        }


        /// <summary>
        /// Delete product with specific product id.
        /// </summary>
        public static void DeleteShelter(int id)
        {
            var product = GetEditableShelter(id);

            DeleteItems(id); // Delete related items to product
            OimsDataContext.Delete(product); // Delete product
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Update product.
        /// </summary>
        public static void UpdateShelter(SheltersModel shelter)
        {
            var editProduct = GetEditableShelter(shelter.ShelterId);
            if (editProduct == null) return;

            editProduct.P_Name = shelter.ShelterName;
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Check if shelter going to update is unique or not.
        /// </summary>
        public static bool IsUniqueShelter(SheltersModel shelter)
        {
            var prodRecord = Shelters.FirstOrDefault(p => p.ShelterName.ToLower().Equals(shelter.ShelterName.ToLower()));

            var unique = prodRecord == null || prodRecord.ShelterId.Equals(shelter.ShelterId);
            return unique;
        }

        #endregion

        #region Methods - Shelter's Items

        /// <summary>
        /// Get items used for specific shelte.
        /// </summary>
        /// <param name="id">ShelterID for which to retrieive items.</param>
        public static List<SheltersDescInfo> GetShelterItemsByKey(int id)
        {
            var items = (from sd in OimsDataContext.Shelterdescriptions
                         where sd.P_Id.Equals(id)
                         select new SheltersDescInfo
                         {
                             ShelterDescKey = sd.PD_Id,
                             ShelterId = sd.P_Id,
                             ItemId = sd.I_Id,
                             ItemQuantity = sd.I_Qty
                         }).ToList();

            return items;
        }

        /// <summary>
        /// Get items used for specific shelter.
        /// </summary>
        /// <param name="shelterId">ShelterID for which to retrieive items.</param>
        /// <param name="qtyMultiple">Number of shelter user is requesting.</param>
        public static List<SheltersDescInfo> GetShelterItemsByKey(int shelterId, int qtyMultiple)
        {
            var slNo = 1;
            var items = new List<SheltersDescInfo>();

            foreach (var sd in OimsDataContext.Shelterdescriptions.Where(item => item.P_Id.Equals(shelterId)))
            {
                // Get item's details
                string iName, iSize, iMarking;
                SetItemValues(sd.I_Id, out iName, out iSize, out iMarking);

                var sdi = new SheltersDescInfo
                          {
                              ShelterDescKey = sd.PD_Id,
                              ShelterId = sd.P_Id,
                              ItemId = slNo,
                              ItemName = iName,
                              Size = iSize,
                              Marking = iMarking,
                              ItemQuantity = sd.I_Qty,
                              EnterQuantity = Convert.ToString(sd.I_Qty * qtyMultiple)
                          };

                items.Add(sdi);
                slNo++;
            }

            return items;
        }

        /// <summary>
        /// Get items used for specific shelter along with other details.
        /// </summary>
        /// <param name="shelterId">ShelterID for which to retrieive items.</param>
        public static List<SheltersDescInfo> GetShelterItemsDetail(int shelterId)
        {
            var items = new List<SheltersDescInfo>();

            foreach (var sd in OimsDataContext.Shelterdescriptions.Where(item => item.P_Id.Equals(shelterId)))
            {
                var shd = sd; // For fixing compiler version issue (suggested by ReSharper)

                // Get item's details
                string iName, iSize, iMarking;
                SetItemValues(shd.I_Id, out iName, out iSize, out iMarking);

                var sdi = new SheltersDescInfo
                {
                    ShelterDescKey = shd.PD_Id,
                    ShelterId = shd.P_Id,
                    ItemId = shd.I_Id,
                    ItemName = iName,
                    Size = iSize,
                    Marking = iMarking,
                    ItemQuantity = shd.I_Qty,
                    InStock = ItemRepository.GetItemTotalQuantity(shd.I_Id),
                    Vendor = ItemRepository.Items.Where(i => i.ItemId.Equals(shd.I_Id)).Select(i => i.Vendor).FirstOrDefault()
                };

                items.Add(sdi);
            }

            return items;
        }

        /// <summary>
        /// Get specific editable product item.
        /// </summary>
        /// <param name="pId">Primary key of Product.</param>
        /// <param name="pDescId">Primary key of product item record.</param>
        /// <param name="itemId">Primary key of item record.</param>
        /// <returns></returns>
        private static Shelterdescription GetEditableShelterItem(int pId, int pDescId, int itemId)
        {
            if (itemId > 0)
            {
                return (from sd in OimsDataContext.Shelterdescriptions
                        where sd.P_Id == pId && sd.PD_Id == pDescId && sd.I_Id == itemId
                        select sd).FirstOrDefault();
            }
            return (from sd in OimsDataContext.Shelterdescriptions
                    where sd.P_Id == pId && sd.PD_Id == pDescId
                    select sd).FirstOrDefault();
        }

        /// <summary>
        /// Get specific editable product.
        /// </summary>
        private static IEnumerable<Shelterdescription> GetEditableShelterItems(int id)
        {
            return (from i in OimsDataContext.Shelterdescriptions
                    where i.P_Id == id
                    select i);
        }

        /// <summary>
        /// Insert new shelter item for specific shelter.
        /// </summary>
        public static void NewShelterItem(SheltersDescInfo sheltertem)
        {
            var newShelterItem = new Shelterdescription
            {
                P_Id = sheltertem.ShelterId,
                I_Id = sheltertem.ItemId,
                I_Qty = sheltertem.ItemQuantity
            };

            OimsDataContext.Add(newShelterItem);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Save new items of shelter.
        /// </summary>
        public static void SaveShelterItems(List<Shelterdescription> sdList)
        {
            sdList.ForEach(pdItem => OimsDataContext.Add(pdItem));
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Update shelter's item.
        /// </summary>
        public static void UpdateShelterItem(SheltersDescInfo shelterItem)
        {
            var editProductItem = GetEditableShelterItem(shelterItem.ShelterId, shelterItem.ShelterDescKey, 0);
            if (editProductItem == null) return;

            editProductItem.I_Id = shelterItem.ItemId;
            editProductItem.I_Qty = shelterItem.ItemQuantity;
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Delete product with specific product id.
        /// </summary>
        public static void DeleteShelterItem(SheltersDescInfo shelterItem)
        {
            var editProductItem = GetEditableShelterItem(shelterItem.ShelterId, shelterItem.ShelterDescKey, shelterItem.ItemId);

            if (editProductItem == null) return;

            OimsDataContext.Delete(editProductItem);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Delete items related to single product.
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteItems(int id)
        {
            var items = GetEditableShelterItems(id);

            foreach (var item in items)
            {
                OimsDataContext.Delete(item);
            }
        }

        /// <summary>
        /// Check if the item modified for current product is unique for same. That is, it not exist already.
        /// </summary>
        /// <param name="sDesc">ProductDescInfo model.</param>
        /// <param name="action">Name of action for which need to check.</param>
        /// <returns>Return true if item is unique else false.</returns>
        public static bool IsUniqueItem(SheltersDescInfo sDesc, Action action)
        {
            var unique = false;

            switch (action)
            {
                case Action.New:
                    // Check if ItemId selected already exists for this ShelterId
                    var siN = (from sd in ShelterItems
                               where sd.I_Id == sDesc.ItemId && sd.P_Id == sDesc.ShelterId
                               select sd).FirstOrDefault();

                    unique = siN == null;
                    break;

                case Action.Update:
                    var siU = (from sd in ShelterItems
                               where sd.I_Id == sDesc.ItemId && sd.P_Id == sDesc.ShelterId
                               select sd).FirstOrDefault();

                    if (siU != null)
                    {
                        if (siU.PD_Id == sDesc.ShelterDescKey)
                        {
                            unique = true;
                        }
                    }
                    else
                    {
                        unique = true;
                    }
                    break;
            }

            return unique;

        }

        /// <summary>
        /// Set shelter item values for displaying details in Requestor zone.
        /// </summary>
        private static void SetItemValues(int itemId, out string itemName, out string size, out string marking)
        {
            string iName = null, iSize = null, iMarking = null;

            foreach (var item in ItemRepository.Items.Where(item => item.ItemId.Equals(itemId)))
            {
                iName = item.ItemName;
                iSize = item.Size;
                iMarking = item.Marking;
            }

            itemName = iName;
            size = iSize;
            marking = iMarking;
        }

        #endregion

        #region Methods - Report's shelters

        /// <summary>
        ///  Get list of shelters in database binded with SheltersModel.
        /// </summary>
        public static SheltersModel GetShelter()
        {
            int shelterId;
            Int32.TryParse(Application.ShelterId, out shelterId);
            return Shelters.FirstOrDefault(p => p.ShelterId.Equals(shelterId));
        }

        /// <summary>
        /// Calculate total stock of products.
        /// </summary>
        /// <param name="shelterId">The product id</param>
        /// <returns></returns>
        public static decimal PossibleShelterStock(int shelterId)
        {
            var stockList = new List<float>();
            var items = GetShelterItemsByKey(shelterId);

            foreach (var item in items)
            {
                var itemQuantity = item.ItemQuantity;
                var totalItemsQuantity = ItemRepository.GetItemTotalQuantity(item.ItemId);

                if (itemQuantity.Equals(0) || totalItemsQuantity < itemQuantity)
                {
                    stockList.Add(0);
                    break;
                }

                var possComb = totalItemsQuantity / itemQuantity;
                stockList.Add(possComb);
            }

            if (stockList.Count <= 0) return 0;
            var stock = Math.Floor((decimal)stockList.Min());
            return stockList.Count > 0 ? stock : 0;
        }

        /// <summary>
        /// Get items used for specific shelter along with other details.
        /// </summary>
        /// <param name="shelterId">ShelterID for which to retrieive items.</param>
        public static List<SheltersDescInfo> GetShelterItemsForReport(int shelterId)
        {
            var slNo = 1;
            var items = new List<SheltersDescInfo>();

            foreach (var sd in OimsDataContext.Shelterdescriptions.Where(item => item.P_Id.Equals(shelterId)))
            {
                // Get item's details
                string iName, iSize, iMarking;
                SetItemValues(sd.I_Id, out iName, out iSize, out iMarking);

                var sdesc = sd;
                var sdi = new SheltersDescInfo
                {
                    ShelterDescKey = slNo,
                    ShelterId = sd.P_Id,
                    ItemName = iName,
                    Size = iSize,
                    Marking = iMarking,
                    ItemQuantity = sd.I_Qty,
                    InStock = ItemRepository.GetItemTotalQuantity(sd.I_Id),
                    Vendor = ItemRepository.Items.Where(i => i.ItemId.Equals(sdesc.I_Id)).Select(i => i.Vendor).FirstOrDefault(),
                    UpdatedOn = ItemRepository.Items.Where(i => i.ItemId.Equals(sd.I_Id)).Select(i => i.UpdatedOn).FirstOrDefault()
                };

                items.Add(sdi);
                slNo++;
            }

            // Order list by to show items in ascending order
            return items.OrderBy(sd => sd.ShelterDescKey).ToList();
        }

        #endregion
    }
}