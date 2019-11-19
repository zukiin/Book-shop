using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public class SupplierItem
    {
        [Key, Column(Order = 0)]
        [Required]
        [ForeignKey("Supplier")]
        [Display(Name = "Supplier")]
        public int CompanyID { get; set; }
        public virtual ItemSupplier Supplier { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        [ForeignKey("Book")]
        [Display(Name = "Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
