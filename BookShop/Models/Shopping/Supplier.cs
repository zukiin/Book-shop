using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public partial class ItemSupplier : Company
    {
        public virtual ICollection<SupplierItem> SupplierItems { get; set; }
        public virtual ICollection<SupplierQuoteItem> SupplierQuoteItems { get; set; }

        //public virtual Order order { get; set; }
    }
}
