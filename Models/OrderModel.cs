using Casestudy.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Casestudy.Models
{
    public class OrderModel
    {
        private AppDbContext _db;
        public OrderModel(AppDbContext ctx)
        {
            _db = ctx;
        }
        public int AddOrder(Dictionary<string, object> items, string user, ref string backOrderMessage)
        {

            int orderId = -1;
            using (_db)
            {
                //Using transaction for multitple entities
                using (var _trans = _db.Database.BeginTransaction())
                {
                    try
                    {
                        Order order = new Order();
                        order.UserId = user;
                        order.OrderDate = DateTime.Now;
                        order.OrderAmount = 0;

                        //Calculate totals and add the order row to the table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                order.OrderAmount += Decimal.Parse(String.Format("{0:.00}", item.MSRP * item.Qty));
                            }
                        }

                        _db.Orders.Add(order);
                        _db.SaveChanges();

                        //Add each item to the OrderItems table
                        foreach (var key in items.Keys)
                        {
                            ProductViewModel item = JsonConvert.DeserializeObject<ProductViewModel>(Convert.ToString(items[key]));
                            if (item.Qty > 0)
                            {
                                OrderLineItem oLineItem = new OrderLineItem();
                                oLineItem.Product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                                oLineItem.OrderId = order.Id;
                                oLineItem.Product.QtyOnHand = item.QtyOnHand;
                                oLineItem.Product.QtyOnBackOrder = item.QtyOnBackOrder;
                                oLineItem.Product.BrandId = item.BrandId;
                                oLineItem.Product.CostPrice = item.CostPrice;                            
                                oLineItem.Product.MSRP = item.MSRP;
                                oLineItem.Product.Description = item.Description;                            
                                oLineItem.Product.GraphicName = item.GraphicName;
                                oLineItem.Product.ProductName = item.ProductName;

                                //If qty sold is greater than qty on hand, need to note the backorder
                                if (item.Qty > oLineItem.Product.QtyOnHand)
                                {
                                    oLineItem.QtySold = oLineItem.Product.QtyOnHand;
                                    oLineItem.Product.QtyOnHand = 0;
                                    oLineItem.Product.QtyOnBackOrder += item.Qty - item.QtyOnHand;
                                    oLineItem.QtyOrdered = item.Qty;
                                    oLineItem.QtyBackOrdered += item.Qty - item.QtyOnHand;
                                    oLineItem.SellingPrice = item.MSRP * item.Qty;
                                    backOrderMessage += "<br />" + oLineItem.QtyBackOrdered + " " + item.ProductName + "(s) placed on back order.";
                                }
                                else
                                {
                                    oLineItem.Product.QtyOnHand -= item.Qty;
                                    oLineItem.QtySold = item.Qty;
                                    oLineItem.QtyOrdered = item.Qty;
                                    oLineItem.QtyBackOrdered = 0;
                                    oLineItem.SellingPrice = item.MSRP * item.Qty;
                                }

                                _db.OrderLineItems.Add(oLineItem);
                                _db.SaveChanges();
                            }
                        }

                        _trans.Commit();
                        orderId = order.Id;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _trans.Rollback();
                    }
                }
            }
            return orderId;
        }
        public List<Order> GetOrders(string id)
        {
            return _db.Orders.Where(item => item.UserId.Equals(id)).ToList<Order>();
        }
        public List<OrderViewModel> GetOrderDetails(int oid, string uid)
        {
            List<OrderViewModel> allDetails = new List<OrderViewModel>();
            // LINQ way of doing INNER JOINS
            var results = from o in _db.Set<Order>()
                          join oi in _db.Set<OrderLineItem>() on o.Id equals oi.OrderId
                          join p in _db.Set<Product>() on oi.ProductId equals p.Id
                          where (o.UserId == uid && o.Id == oid)
                          select new OrderViewModel
                          {
                              Id = oi.OrderId,
                              UserId = uid,
                              OrderDate = o.OrderDate,
                              OrderAmount = o.OrderAmount,
                              QtyOrdered = oi.QtyOrdered,
                              QtySold = oi.QtySold,
                              QtyBackOrdered = oi.QtyBackOrdered,
                              SellingPrice = oi.SellingPrice,
                              ProductName = p.ProductName,
                              MSRP = p.MSRP
                          };
            allDetails = results.ToList<OrderViewModel>();
            return allDetails;
        }
    }
}