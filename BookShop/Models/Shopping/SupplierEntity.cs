using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
  public  class SupplierEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [Display(Name = "Contact")]
        public string contact { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Physical Address")]
        public string Address { get; set; }



    }
}
