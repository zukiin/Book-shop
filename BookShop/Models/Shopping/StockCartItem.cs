using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public partial class StockCartItem
    {
        [Key]
        public string CartItemID { get; set; }
        [ForeignKey("StockCart")]
        public string CartID { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }
        [DataType(DataType.Currency)]
        public double SubTotal { get; set; }
        public virtual StockCart StockCart { get; set; }
        public virtual Book Book { get; set; }
    }
}
