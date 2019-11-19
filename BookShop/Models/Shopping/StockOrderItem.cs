using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public class StockOrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }
        [ForeignKey("StockOrder")]
        public string OrderNumber { get; set; }
        public virtual StockOrder StockOrder { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }
        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }
        public bool Replied { get; set; }
        public Nullable<DateTime> DateReplied { get; set; }

        public bool Accepted { get; set; }
        public Nullable<DateTime> DateAccepted { get; set; }

        public bool Shipped { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> DateShipped { get; set; }

        public double sub_Total()
        {
            
            return SubTotal= Quantity * UnitPrice;
        }
    }
}
