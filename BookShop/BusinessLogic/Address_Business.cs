using BookShop.Models;
using BookShop.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShop.BusinessLogic
{
    public class Address_Business
    {
        private ApplicationDbContext db;

        public Address_Business()
        {
            this.db = new ApplicationDbContext();
        }
        public List<Shipping_Address> allOrderAddreses()
        {
            return db.Shipping_Addresses.ToList();
        }
        public void addOrderAddress(Shipping_Address order_Address)
        {
            try
            {
                db.Shipping_Addresses.Add(order_Address);
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public Shipping_Address GetShipping_Address(int address_id)
        {
            return db.Shipping_Addresses.FirstOrDefault(x => x.Address_ID == address_id);
        }
        public Shipping_Address GetShipping_Address(string order_id)
        {
            return db.Shipping_Addresses.FirstOrDefault(x => x.Order_ID == order_id);
        }
    }
}
