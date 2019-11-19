using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShop.Models
{
    public class Rental
    {
        [Key]
        public string Rent_id { get; set; }
        public DateTime date_created { get; set; }

        public virtual ICollection<Rented_Item> Rented_Item { get; set; }
    }
}
