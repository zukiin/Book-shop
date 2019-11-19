using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models.Shopping
{
    public class StockOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OrderNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Shipped { get; set; }
        public string Status { get; set; }
        public bool Packed { get; set; }

        public virtual ICollection<StockOrderItem> StockOrderItems { get; set; }
        public virtual ICollection<SupplierQuoteItem> SupplierQuoteItems { get; set; }

        public StockOrder()
        {
            this.DateCreated = DateTime.Now;
        }
    }
}
