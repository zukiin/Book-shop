﻿using BookShop.Models;
using BookShop.Models.Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookShop.BusinessLogic
{
    public class Cart_Business
    {
        private ApplicationDbContext db;
        public static string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        public Cart_Business()
        {
            this.db = new ApplicationDbContext();
        }

        public void add_item_to_cart(int id)
        {
            shoppingCartID = GetCartID();

            var item = db.Books.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.Cart_Items.FirstOrDefault(x => x.cart_id == shoppingCartID && x.BookId == item.BookId);
                if (cartItem == null)
                {
                    var cart = db.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        db.Carts.Add(entity: new CartEntity()
                        {
                            cart_id = shoppingCartID,
                            date_created = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.Cart_Items.Add(entity: new CartItemEntity()
                    {
                        cart_item_id = Guid.NewGuid().ToString(),
                        cart_id = shoppingCartID,
                        BookId = item.BookId,
                        quantity = 1,
                        price = item.Price
                    }
                        );
                }
                else
                {
                    cartItem.quantity++;
                }
                db.SaveChanges();
            }
        }
        public void remove_item_from_cart(string id)
        {
            shoppingCartID = GetCartID();

            var item = db.Cart_Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.Cart_Items.FirstOrDefault(predicate: x => x.cart_id == shoppingCartID && x.BookId == item.BookId);
                if (cartItem != null)
                {
                    db.Cart_Items.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }
        public List<CartItemEntity> get_Cart_Items()
        {
            shoppingCartID = GetCartID();
            return db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID);
        }
        public void updateCart(string id, int qty)
        {
            var item = db.Cart_Items.Find(id);
            if (qty < 0)
                item.quantity = qty / -1;
            else if (qty == 0)
                remove_item_from_cart(item.cart_item_id);
            else if (item.Book.Quantity_In_Stock < qty)
                item.quantity = item.Book.Quantity_In_Stock;
            else
                item.quantity = qty;
            db.SaveChanges();
        }
        public double get_cart_total(string id)
        {
            double amount = 0;
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void empty_Cart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID))
            {
                db.Cart_Items.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(shoppingCartID));
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
    }
}
