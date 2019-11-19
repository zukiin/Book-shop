using BookShop.Models;
using BookShop.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShop.BusinessLogic
{
    public class Order_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //private Address_Business address_Business = new Address_Business();
        //private Payment_Business Payment_Business = new Payment_Business();

        public List<OrderEntity> all_Orders()
        {
            return db.Orders.ToList();
        }
        public List<OrderEntity> cust_find_by_status(string status)
        {
            return db.Orders.Where(p => p.status.ToLower() == status.ToLower()).ToList();
        }

        public OrderEntity findOrder_by_id(string id)
        {
            return db.Orders.Find(id);
        }
        public List<OrderItemEntity> _Order_items(string id)
        {
            return findOrder_by_id(id).Order_Items.ToList();
        }
        public OrderDetailModel GetOrderDetail(string id)
        {
            try
            {
                string shipping_method = "Collect at warehouse", payment_method = "Awaiting Payment";
                

                return new OrderDetailModel()
                {
                    customer = findOrder_by_id(id).Customer,
                    order = findOrder_by_id(id),
                    shipping_method = shipping_method,
                    delivery = null,
                   //address = address_Business.GetShipping_Address(id),
                    payment_Method = payment_method,
                   // payment = Payment_Business.GetOrderPayment(id),
                    order_items = _Order_items(id),
                    order_total = (decimal)get_order_total(id)
                };
            }
            catch (Exception ex)
            {
                return new OrderDetailModel();
            }
        }
        public List<OrderTrackingEntity> get_tracking_report(string id)
        {
            return db.Order_Trackings.Where(x => x.order_ID == id).ToList();
        }

        public void mark_as_packed(string id)
        {
            var order = findOrder_by_id(id);
            order.packed = true;
            if (db.Shipping_Addresses.Where(p => p.Order_ID == id) != null)
            {
                order.status = "With courier";
                //order tracking
                db.Order_Trackings.Add(new OrderTrackingEntity()
                {
                    order_ID = order.Order_ID,
                    date = DateTime.Now,
                    status = "Order Packed, Now with our courier",
                    Recipient = ""
                });
            }

            db.SaveChanges();
        }
        public void schedule_OrderDelivery(string order_Id, DateTime date)
        {
            var order = findOrder_by_id(order_Id);
            order.status = "Scheduled for delivery";
            //order tracking
            db.Order_Trackings.Add(new OrderTrackingEntity()
            {
                order_ID = order.Order_ID,
                date = DateTime.Now,
                status = "Scheduled for delivery on " + date.ToLongDateString(),
                Recipient = ""
            });
            db.SaveChanges();
        }

        public List<OrderItemEntity> allOrderItems(string order_id)
        {
            return db.Order_Items.Where(x => x.Order_id == order_id).ToList();
        }


        public void addOrder(CustomerEntity customer)
        {
            try
            {
                OrderEntity model = new OrderEntity();
                model.Order_ID = GenerateOrderNumber(10);
                model.customerId = customer.CustomerId;
                model.Email = customer.Email;
                model.date_created = DateTime.Now;
                model.shipped = false;
                model.status = "Awaiting Payment";
                //db.Orders.Add(new OrderEntity()
                //{
                //	Order_ID = GenerateOrderNumber(10),
                //	Email = customer.Email,
                //	date_created = DateTime.Now,
                //	shipped = false,
                //	status = "Awaiting Payment"
                //});

                db.Orders.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public void addOrderItems(OrderEntity order, List<CartItemEntity> items)
        {
            foreach (var item in items)
            {
                OrderItemEntity model = new OrderItemEntity();

                model.Order_id = order.Order_ID;
                model.BookId = item.BookId;
                model.quantity = item.quantity;
                model.price = item.price;

                db.Order_Items.Add(model);
                db.SaveChanges();
            }
        }

        public void addOrderTrackingReport(OrderTrackingEntity tracking)
        {
            try
            {
                db.Order_Trackings.Add(tracking);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public OrderEntity GetOrder(string OrderNumber)
        {
            return db.Orders.Find(OrderNumber);
        }


        public void AddOrderTrackingReport(OrderTrackingEntity tracking)
        {
            try
            {
                db.Order_Trackings.Add(tracking);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public void expectedDeliveryDateReport(OrderEntity order)
        {
            try
            {
                var expected_Date = DateTime.Now.AddDays(3);
                do
                {
                    expected_Date = expected_Date.AddDays(1);
                } while (expected_Date.DayOfWeek.ToString().ToLower() == "sunday" ||
                    expected_Date.DayOfWeek.ToString().ToLower() == "saturday");

                if (IsDeliveryRequested(order.Order_ID))
                {
                    addOrderTrackingReport(new OrderTrackingEntity()
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Expected delivery on " + expected_Date.ToLongDateString() + " before 5pm",
                        Recipient = ""
                    });
                }
                else
                {
                    expected_Date = DateTime.Now.AddHours(1);

                    addOrderTrackingReport(new OrderTrackingEntity()
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Can be collected during business hours as from " + expected_Date.ToLongDateString() + " " + expected_Date.ToLongTimeString(),
                        Recipient = ""
                    });
                }


            }
            catch (Exception ex) { }
        }


        public bool IsDeliveryRequested(string order_id)
        {
            return db.Shipping_Addresses.FirstOrDefault(p => p.Order_ID == order_id) != null;
        }

        public void add_payment(string order_id)
        {
            var order = db.Orders.Find(order_id);
            var email = order.Customer.Email;
            try
            {

                {
                    db.Payments.Add(new PaymentEntity()
                    {
                        Date = DateTime.Now,
                        Email = email,
                        AmountPaid = get_order_total(order.Order_ID),
                        PaymentFor = "Order " + order_id + " Payment",
                        PaymentMethod = "EFT via PayFast Online",
                        Order_ID = order_id
                    });
                    db.SaveChanges();
                    //updateOrderReport(order_id);
                }
            }
            catch (Exception) { }
        }



        public void updateOrderReport(string order_id)
        {
            var order = db.Orders.Find(order_id);
            try
            {
                if (IsPaymentComplete(order_id))
                {
                    order.status = "At warehouse";
                    db.SaveChanges();
                    //order tracking
                    db.Order_Trackings.Add(new OrderTrackingEntity()
                    {
                        order_ID = order.Order_ID,
                        date = DateTime.Now,
                        status = "Payment Recieved | Order still at warehouse",
                        Recipient = ""
                    });
                    db.SaveChanges();
                }
            }
            catch (Exception) { }
        }
        public bool IsPaymentComplete(string order_id)
        {
            return db.Payments.ToList()
                      .FindAll(x => x.Order_ID == order_id)
                      .Sum(x => x.AmountPaid) >= get_order_total(order_id);
        }

        public void update_Stock(string id)
        {
            var order = db.Orders.Find(id);
            List<OrderItemEntity> items = db.Order_Items.ToList().FindAll(x => x.Order_id == id);
            foreach (var item in items)
            {
                var product = db.Books.Find(item.BookId);
                if (product != null)
                {
                    if ((product.Quantity_In_Stock - item.quantity) >= 0)
                    {
                        product.Quantity_In_Stock -= item.quantity;
                    }
                    else
                    {
                        item.quantity = product.Quantity_In_Stock;
                        product.Quantity_In_Stock = 0;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
        public double get_order_total(string id)
        {
            double amount = 0;
            foreach (var item in db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }

        public string GenerateOrderNumber(int length)
        {
            var random = new Random();
            string number = string.Empty;
            for (int i = 0; i < length; i++)
                number = String.Concat(number, random.Next(10).ToString());
            while (GetOrder(number) != null)
                number = GenerateOrderNumber(length);
            return number;
        }

    }

}
