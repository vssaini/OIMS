using System;
using System.Collections.Generic;
using System.Linq;
using OIMS.Models.Supervisor;
using OimsDataModel;

namespace OIMS.Repository.Supervisor
{
    public class ItemsLogRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of items log in database binded with ItemsLogModel.
        /// </summary>
        public static IList<ItemsLogModel> ItemsLog
        {
            get
            {
                var itemsLog = (from il in OimsDataContext.Itemslogs
                                select new ItemsLogModel
                                {
                                    Id = il.Id,
                                    ItemId = il.ItemId,
                                    ItemQuantity = il.ItemQuantity,
                                    VendorName = il.VendorName,
                                    EntryDate = il.EntryDate
                                }).ToList();
                return itemsLog;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get specific editable item log.
        /// </summary>
        public static Itemslog GetEditableItemLog(int id)
        {
            return (from i in OimsDataContext.Itemslogs
                    where i.Id == id
                    select i).FirstOrDefault();
        }

        /// <summary>
        /// Create new items log.
        /// </summary>
        public static void SaveItemsLog(List<Itemslog> itemLogList)
        {
            itemLogList.ForEach(il => OimsDataContext.Add(il));
            OimsDataContext.SaveChanges();

            #region Add item's stock

            foreach (var il in itemLogList)
            {
                var itemsModel = GetItemsModel(il.ItemId);
                if (itemsModel == null) continue;

                // Add new quantity to existing quantity in stock
                itemsModel.ItemQuantity = itemsModel.ItemQuantity + il.ItemQuantity;
                itemsModel.Vendor = il.VendorName;
                itemsModel.UpdatedOn = DateTime.Now;
                ItemRepository.UpdateItem(itemsModel);
            }

            #endregion
        }

        /// <summary>
        /// Update Item log.
        /// </summary>
        public static void UpdateItemLog(ItemsLogModel itemLog)
        {
            var editItemLog = GetEditableItemLog(itemLog.Id);
            if (editItemLog == null) return;

            var oldItemQtyFromLog = editItemLog.ItemQuantity;

            editItemLog.ItemId = itemLog.ItemId;
            editItemLog.ItemQuantity = itemLog.ItemQuantity;
            editItemLog.VendorName = itemLog.VendorName;
            editItemLog.EntryDate = itemLog.EntryDate;

            OimsDataContext.SaveChanges();

            #region Update item's stock

            var itemsModel = GetItemsModel(itemLog.ItemId);
            if (itemsModel == null) return;

            // Calculate item quantity to set
            var itemCurrentStock = itemsModel.ItemQuantity;
            var newItemQtyFromLog = itemLog.ItemQuantity;
            var itemQtyToSet = (itemCurrentStock - oldItemQtyFromLog) + newItemQtyFromLog;

            itemsModel.ItemQuantity = itemQtyToSet;
            itemsModel.Vendor = editItemLog.VendorName;
            itemsModel.UpdatedOn = DateTime.Now;
            ItemRepository.UpdateItem(itemsModel);

            #endregion
        }

        /// <summary>
        /// Delete Item log with specific id.
        /// </summary>
        public static void DeleteItemLog(int id)
        {
            var itemLog = GetEditableItemLog(id);

            // Get details for updating items stock
            var itemId = itemLog.ItemId;
            var newItemQtyFromLog = itemLog.ItemQuantity;
            var vendorName = itemLog.VendorName;

            OimsDataContext.Delete(itemLog);
            OimsDataContext.SaveChanges();

            #region Delete item's stock

            var itemsModel = GetItemsModel(itemId);
            if (itemsModel == null) return;

            // Calculate item quantity to set
            var itemCurrentStock = itemsModel.ItemQuantity;
            var itemQtyToSet = itemCurrentStock - newItemQtyFromLog;

            itemsModel.ItemQuantity = itemQtyToSet;
            itemsModel.Vendor = vendorName;
            itemsModel.UpdatedOn = DateTime.Now;
            ItemRepository.UpdateItem(itemsModel);

            #endregion
        }

        /// <summary>
        /// Get items model based on item id passed.
        /// </summary>
        private static ItemsModel GetItemsModel(int itemId)
        {
            var im = (from item in OimsDataContext.Items
                      where item.I_Id.Equals(itemId)
                      select new ItemsModel
                      {
                          ItemId = item.I_Id,
                          ItemName = item.I_Name,
                          ItemQuantity = item.I_Quantity,
                          Size = item.Size,
                          Marking = item.Marking,
                          Vendor = item.Vendor,
                          UpdatedOn = item.UpdatedOn
                      });

            return im.FirstOrDefault();
        }

        #endregion
    }
}