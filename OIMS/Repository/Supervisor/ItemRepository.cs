using System.Collections.Generic;
using System.Linq;
using OIMS.Global;
using OIMS.Models.Supervisor;
using OimsDataModel;

namespace OIMS.Repository.Supervisor
{
    public class ItemRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of items in database binded with ItemsModel.
        /// </summary>
        public static IList<ItemsModel> Items
        {
            get
            {
                var items = (from item in OimsDataContext.Items
                             select new ItemsModel
                             {
                                 ItemId = item.I_Id,
                                 ItemName = item.I_Name,
                                 ItemQuantity = item.I_Quantity,
                                 Size = item.Size,
                                 Marking = item.Marking,
                                 Vendor = item.Vendor,
                                 UpdatedOn = item.UpdatedOn
                             }).ToList();
                return items;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get specific editable item.
        /// </summary>
        public static Item GetEditableItem(int id)
        {
            return (from i in OimsDataContext.Items
                    where i.I_Id == id
                    select i).FirstOrDefault();
        }

        /// <summary>
        /// Create new items.
        /// </summary>
        public static void SaveItems(List<Item> itemList)
        {
            itemList.ForEach(item => OimsDataContext.Add(item));
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Delete Item with specific Item id.
        /// </summary>
        public static void DeleteItem(int id)
        {
            var item = GetEditableItem(id);
            OimsDataContext.Delete(item);
            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Update Item.
        /// </summary>
        public static void UpdateItem(ItemsModel item)
        {
            var editItem = GetEditableItem(item.ItemId);
            if (editItem == null) return;

            editItem.I_Name = item.ItemName;
            editItem.I_Quantity = item.ItemQuantity;
            editItem.Size = item.Size;
            editItem.Marking = item.Marking;
            editItem.Vendor = item.Vendor;
            editItem.UpdatedOn = item.UpdatedOn;

            OimsDataContext.SaveChanges();
        }

        /// <summary>
        /// Get total count of items from database.
        /// </summary>
        public static float GetItemTotalQuantity(int id)
        {
            var item = Items.FirstOrDefault(i => i.ItemId.Equals(id));
            return item != null ? item.ItemQuantity : 0;
        }

        /// <summary>
        /// Check if item going to update is unique or not.
        /// </summary>
        public static bool IsUniqueItem(ItemsModel item)
        {
            var itemRecord = Items.FirstOrDefault(i => i.ItemName.ToLower().Equals(item.ItemName.ToLower()));

            var unique = itemRecord == null || itemRecord.ItemId.Equals(item.ItemId);
            return unique;
        }

        #endregion

        #region Method - Report's items

        /// <summary>
        /// Get items filtered on basis of param.
        /// </summary>
        /// <returns></returns>
        public static IList<ItemsModel> GetItems()
        {
            var slNo = 1;
            IList<ItemsModel> items = new List<ItemsModel>();

            var param = Application.ReportFilter;

            if (string.IsNullOrEmpty(param))
            {
                foreach (var item in OimsDataContext.Items)
                {
                    var im = new ItemsModel
                             {
                                 ItemId = slNo,
                                 ItemName = item.I_Name,
                                 ItemQuantity = item.I_Quantity
                             };

                    items.Add(im);
                    slNo++;
                }

                items = items.OrderBy(i => i.ItemId).ToList();
                return items;
            }

            foreach (var item in OimsDataContext.Items)
            {
                if (!item.I_Name.Contains(param)) continue;
                var im = new ItemsModel
                         {
                             ItemId = slNo,
                             ItemName = item.I_Name,
                             ItemQuantity = item.I_Quantity
                         };

                items.Add(im);
                slNo++;
            }

            items = items.OrderBy(i => i.ItemId).ToList();
            return items;
        }

        #endregion
    }
}