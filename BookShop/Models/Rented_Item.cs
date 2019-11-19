using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookShop.Models
{
    public class Rented_Item
    {
        [Key]
        public string Rented_item_id { get; set; }
        [ForeignKey("Rental")]
        public string Rental_id { get; set; }
        public int BookId { get; set; }
        public string ISBN { get; set; }


        public double price { get; set; }

        public virtual Book Book { get; set; }
        public virtual Rental Rentals { get; set; }
    }
}