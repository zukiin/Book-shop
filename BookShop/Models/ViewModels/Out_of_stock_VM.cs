using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.ViewModels
{
    public class Out_of_stock_VM
    {
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Item Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int stockQTY { get; set; }
        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }

    }
}
