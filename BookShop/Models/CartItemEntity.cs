using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models
{
   public class CartItemEntity
    {

        [Key]
        public string cart_item_id { get; set; }
        [ForeignKey("Cart")]
        public string cart_id { get; set; }
        public int BookId { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }

        public virtual Book Book { get; set; }
        public virtual CartEntity Cart { get; set; }
    }
}
