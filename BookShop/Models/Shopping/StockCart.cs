using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public partial class StockCart
    {
        [Key]
        public string CartID { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<StockCartItem> StockCartItems { get; set; }
    }
}
