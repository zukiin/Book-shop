using BookShop.BusinessLogic;
using BookShop.Models;
using BookShop.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BookShop.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        private ApplicationDbContext db;
        private Cart_Business cart_Business;
        private Address_Business address_business;
        private Order_Business order_Business;
        private Customer_Business customer_Business;
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        public ShoppingController()
        {
            this.db = new ApplicationDbContext();
            this.cart_Business = new Cart_Business();
            this.address_business = new Address_Business();
        }

        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }
        public ActionResult Index2()
        {
            return View(db.Books.ToList());
        }
        public ActionResult add_to_cart(int id, CartItemEntity model)
        {
            var item = db.Books.Find(id);

            if (item != null)
            {
                cart_Business.add_item_to_cart(id);

              
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
      


        public ActionResult remove_from_cart(string id)
        {
            var item = cart_Business.get_Cart_Items().FirstOrDefault(x => x.cart_item_id == id);
            if (item != null)
            {
                cart_Business.remove_item_from_cart(id: id);
               
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
  

        public ActionResult ShoppingCart()
        {
            ViewBag.Total = cart_Business.get_cart_total(cart_Business.GetCartID());
            ViewBag.TotalQTY = cart_Business.get_Cart_Items().FindAll(x => x.cart_id == cart_Business.GetCartID()).Sum(q => q.quantity);
            return View(cart_Business.get_Cart_Items().FindAll(x => x.cart_id == cart_Business.GetCartID()));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<CartItemEntity> items)
        {


            foreach (var i in items)
            {
                cart_Business.updateCart(i.cart_item_id, i.quantity);
            }
            ViewBag.Total = cart_Business.get_cart_total(shoppingCartID);
            ViewBag.TotalQTY = cart_Business.get_Cart_Items().FindAll(x => x.cart_id == shoppingCartID).Sum(q => q.quantity);
            return View(cart_Business.get_Cart_Items().FindAll(x => x.cart_id == cart_Business.GetCartID()));
        }
  


        public ActionResult countCartItems()
        {
            int qty = cart_Business.get_Cart_Items().Sum(x => x.quantity);
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (cart_Business.get_Cart_Items().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleast one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("HowToGetMyOrder");
        }
        [Authorize]
        public ActionResult HowToGetMyOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HowToGetMyOrder(string street_number, string street_name, string State, string ZipCode)
        {
            Session["street_number"] = street_number;
            Session["street_name"] = street_name;
            Session["State"] = State;
            Session["ZipCode"] = ZipCode;

            return RedirectToAction("PlaceOrder", new { id = "deliver" });

        }
        public ActionResult PlaceOrder(string id)
        {
            var customer = customer_Business.findCustomer_by_email(HttpContext.User.Identity.Name);
            /* Place the order */
            order_Business.addOrder(customer);
            /* Get the last placed order by the customer */
            var order = order_Business.all_Orders()
                .FindAll(x => x.Email == customer.Email)
                .OrderByDescending(x => x.date_created)
                .FirstOrDefault();


            if (id == "deliver")
            {
                address_business.addOrderAddress(new Shipping_Address()
                {
                    Order_ID = order.Order_ID,
                    street_number = Convert.ToInt16(Session["street_number"].ToString()),
                    Street_name = Session["street_name"].ToString(),
                    State = Session["State"].ToString(),
                    ZipCode = Session["ZipCode"].ToString(),

                    Building_Name = "",
                    Floor = "",
                    Contact_Number = "",
                    Comments = "",
                    Address_Type = ""
                });
            }

            var items = cart_Business.get_Cart_Items();

            /* Migrate cart items to map as order items */
            order_Business.addOrderItems(order, cart_Business.get_Cart_Items());
            /* Empty the cart items */
            cart_Business.empty_Cart();
            /* Update Order Tracking Report */
            //order_Business.addOrderTrackingReport(new OrderTrackingEntity()
            //{
            //    order_ID = order.Order_ID,
            //    date = DateTime.Now,
            //    status = "Awaiting Payment",
            //    Recipient = ""
            //});

            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.Order_ID });

        }
        [Authorize]
        public ActionResult Payment(string id)
        {
            return View(order_Business.GetOrderDetail(id));
        }
        [Authorize]
        public ActionResult Secure_Payment(string id)
        {
            var order = order_Business.findOrder_by_id(id);
            return Redirect(PaymentLink(order_Business.get_order_total(order.Order_ID).ToString(), "Order Payment | Order No: " + order.Order_ID, order.Order_ID));
        }
        public ActionResult Return_Url(string id)
        {
            var order = order_Business.findOrder_by_id(id);

            ViewBag.Order = order;
            ViewBag.Account = customer_Business.findCustomer_by_email(order.Email);
            //ViewBag.Address = address_Business.allOrderAddreses().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = order_Business.allOrderItems(order.Order_ID);
            ViewBag.Total = order_Business.get_order_total(order.Order_ID);
            return View();
        }


        public ActionResult Payment_Successfull(string id)
        {
            try
            {
                var order = order_Business.findOrder_by_id(id);
                order_Business.update_Stock(id);
                order_Business.add_payment(order.Order_ID);
               // order_Business.expectedDeliveryDateReport(order);


              
            }
            catch (Exception ex) { }
            return View();
        }

        public string PaymentLink(string totalCost, string paymentSubjetc, string order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl, cancelUrl, PF_NotifyURL;


            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10002201";
                merchantKey = "25lbpwmazv8rn";
            }

            var stringBuilder = new StringBuilder();
            PF_NotifyURL = Url.Action("Payment_Successfull", "Shopping",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);
            returnUrl = Url.Action("Order_Details", "Orders",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);
            cancelUrl = Url.Action("Payment", "Shopping",
                new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
                "http", Request.Url.Host);

            /* mechant details */
            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode(returnUrl));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode(cancelUrl));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(PF_NotifyURL));
            /* buyer details */
            var customer = order_Business.all_Orders().FirstOrDefault(x => x.Order_ID == order_id).Customer;
            if (customer != null)
            {
                stringBuilder.Append("&name_first=" + HttpUtility.HtmlEncode(customer.FirstName));
                stringBuilder.Append("&name_last=" + HttpUtility.HtmlEncode(customer.LastName));
                stringBuilder.Append("&email_address=" + HttpUtility.HtmlEncode(customer.Email));
                stringBuilder.Append("&cell_number=" + HttpUtility.HtmlEncode(customer.phone));
            }
            /* Transaction details */
            var order = order_Business.findOrder_by_id(order_id);
            if (order != null)
            {
                stringBuilder.Append("&m_payment_id=" + HttpUtility.HtmlEncode(order.Order_ID));
                stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode((decimal)order_Business.get_order_total(order.Order_ID)));
                stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
                stringBuilder.Append("&item_description=" + HttpUtility.HtmlEncode(paymentSubjetc));

                stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
                stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));
            }

            return (site + stringBuilder);
        }

    }

}