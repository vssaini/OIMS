using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OIMS.Global;
using OIMS.Models.Requestor;
using OIMS.Repository.Supervisor;
using OimsDataModel;

namespace OIMS.Repository.Requestor
{
    public class CartRepository : BaseRepository
    {
        #region Properties

        /// <summary>
        /// Get list of cart items in cart binded with CartModel.
        /// </summary>
        public static IList<CartModel> Cartstuffs
        {
            get
            {
                var items = (from item in OimsDataContext.Cartstuffs
                             select new CartModel
                             {
                                 CartId = item.Id,
                                 UserId = item.U_Id,
                                 ItemId = item.I_Id,
                                 ProductId = item.P_Id,
                                 Quantity = item.Quantity,
                                 CreatedOn = item.CreatedOnUtc

                             }).ToList();
                return items;
            }
        }

        #endregion

        #region Methods

        #region Cart item count, items list, add & delete

        /// <summary>
        /// Get total count of stuffs as per user id.
        /// </summary>
        public static int GetTotalStuffCount(int? userId)
        {
            var items = Cartstuffs.Where(i => i.UserId.Equals(userId));
            return items.Count();
        }

        /// <summary>
        /// Get list of cart items as per user id.
        /// </summary>
        public static List<CartStuff> GetCartStuffs(int? userId)
        {
            var itemList = new List<CartStuff>(); // Prepare cart as per user id

            var items = ItemRepository.Items; // Get list of items to get item name
            var products = ShelterRepository.Shelters; // Get list of products to get product name

            // Get single cart item as per user id
            var cStuffs = Cartstuffs.Where(c => c.UserId.Equals(userId));

            foreach (var ci in cStuffs)
            {
                var cartItem = new CartStuff();

                // Set item id and item name
                if (ci.ItemId != null)
                {
                    var item = items.FirstOrDefault(i => i.ItemId.Equals(ci.ItemId));

                    if (item != null)
                    {
                        cartItem.ItemId = item.ItemId;
                        cartItem.ItemName = item.ItemName;
                    }
                }

                // Set product id and product name
                if (ci.ProductId != null)
                {
                    var prod = products.FirstOrDefault(i => i.ShelterId.Equals(ci.ProductId));

                    if (prod != null)
                    {
                        cartItem.ProductId = prod.ShelterId;
                        cartItem.ItemName = prod.ShelterName;
                    }
                }

                // Set rest of properties
                cartItem.CartId = ci.CartId;
                cartItem.Quantity = ci.Quantity;
                cartItem.CreatedOn = ci.CreatedOn;

                // Add cart item to cart item list
                itemList.Add(cartItem);
            }

            return itemList;
        }

        /// <summary>
        /// Add item to cart per user.
        /// </summary>
        public static void AddItemToCart(int userId, int itemId, float quantity)
        {
            // Check if item already exists
            var item = OimsDataContext.Cartstuffs.FirstOrDefault(ci => ci.I_Id.Equals(itemId) && ci.U_Id.Equals(userId));

            if (item != null) //  If exist, update it
            {
                item.Quantity = item.Quantity + quantity;
                OimsDataContext.SaveChanges();
            }
            else
            {
                var ci = new Cartstuff
                {
                    U_Id = userId,
                    I_Id = itemId,
                    Quantity = quantity,
                    CreatedOnUtc = DateTime.Now
                };

                OimsDataContext.Add(ci);
                OimsDataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Add product to cart per user.
        /// </summary>
        public static void AddProductToCart(int userId, int prodId, float quantity)
        {
            var product = OimsDataContext.Cartstuffs.FirstOrDefault(ci => ci.P_Id.Equals(prodId) && ci.U_Id.Equals(userId));

            if (product != null)
            {
                product.Quantity = product.Quantity + quantity;
                OimsDataContext.SaveChanges();
            }
            else
            {
                var ci = new Cartstuff
                {
                    U_Id = userId,
                    P_Id = prodId,
                    Quantity = quantity,
                    CreatedOnUtc = DateTime.Now
                };

                OimsDataContext.Add(ci);
                OimsDataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Delete cart item per user.
        /// </summary>
        /// <param name="userId">UserId of current logged in user.</param>
        /// <param name="cartId">CartId of item in cart.</param>
        public static void DeleteCartStuff(int userId, int cartId)
        {
            var ci = OimsDataContext.Cartstuffs.FirstOrDefault(i => i.U_Id.Equals(userId) && i.Id.Equals(cartId));
            OimsDataContext.Delete(ci);
            OimsDataContext.SaveChanges();
        }

        #endregion

        /// <summary>
        /// Save cart items as single request (or order)
        /// </summary>
        /// <param name="userId">UserId of user.</param>
        public static void SaveRequest(int userId)
        {
            // 1. Create an order id and get id. Also save request status
            var request = new Request
            {
                O_CreatedBy = userId,
                O_CreatedDate = DateTime.Now,
                O_Job = Application.Job,
                O_Company = Application.Company,
                O_Status = Int32.Parse(ConfigurationManager.AppSettings["InProcess"]) // For in process (by default)
            };

            OimsDataContext.Add(request);
            OimsDataContext.FlushChanges(); // Soft commit to database (marked state as dirty)
            var orderId = request.O_Id;

            // 2. Get list of cart items categorized into items and products for the current user
            var items = Cartstuffs.Where(ci => ci.UserId.Equals(userId) && ci.ItemId != null).ToList();
            var products = Cartstuffs.Where(ci => ci.UserId.Equals(userId) && ci.ProductId != null).ToList();

            // 3. Save items in 'itemsRequest' table
            if (items.Any())
            {
                var oiList = items.Select(item => new Itemsrequest
                                                  {
                                                      O_Id = orderId,
                                                      I_Id = item.ItemId != null ? (int)item.ItemId : 0,
                                                      OI_Quantity = item.Quantity,
                                                      OI_Pending = item.Quantity,
                                                      OI_CreatedBy = userId,
                                                      OI_CreatedDate = DateTime.Now
                                                  }).ToList();

                OimsDataContext.Add(oiList);
                OimsDataContext.FlushChanges(true);
            }

            // 4. Save shelters in 'sheltersRequest' table
            if (products.Any())
            {
                var opList = products.Select(prod => new Sheltersrequest
                                                     {
                                                         O_Id = orderId,
                                                         P_Id = prod.ProductId != null ? (int)prod.ProductId : 0,
                                                         OP_Quantity = prod.Quantity,
                                                         OP_Pending = (int?) prod.Quantity,
                                                         OP_CreatedBy = userId,
                                                         OP_CreatedDate = DateTime.Now
                                                     }).ToList();

                OimsDataContext.Add(opList);
                OimsDataContext.FlushChanges(true);
            }

            // 5. Delete all cart items for current user (as they are saved as request)
            var cartstuffs = OimsDataContext.Cartstuffs.Where(ci => ci.U_Id.Equals(userId));

            foreach (var ci in cartstuffs)
            {
                OimsDataContext.Delete(ci);
            }

            // 6. Save all changes to database
            OimsDataContext.SaveChanges();
        }

        #endregion
    }
}