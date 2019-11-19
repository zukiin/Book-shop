using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public  class Invoice
    {
        [Key]
        public long InvoiceNumber { get; set; }
    }
    public class Stock_Invoice : Invoice
    {

    }
    public class Order_Invoice : Invoice
    {

    }
}
