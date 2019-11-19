using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookShop.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Edition { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string PlaceOfPublication { get; set; }
        public string YearOfPublication { get; set; }
        public byte[] Image { get; set; }
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Display(Name = "Mark-Up Percentage")]
        public double Mark_Up { get; set; }

        [Display(Name = "Re_Order Level")]
        public double ReOrder_Level { get; set; }

        [Display(Name = "Quantity in Stock")]
        public int Quantity_In_Stock { get; set; }

        [Display(Name = "Quantity Received")]
        public int QTY_Received { get; set; }

        [Display(Name = "Re-Order Percentage")]
        public double ReOrder_Percent { get; set; }
    }
}