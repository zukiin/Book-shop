using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
	public class WishList_Item
	{
		[Key]
		public string WishList_Item_Id { get; set; }

		public int BookId { get; set; }

		[ForeignKey("wishLists")]
		public string WishList_id { get; set; }

		public double price { get; set; }

		public bool isAvailable { get; set; }

		public DateTime Date_Created { get; set; }

		public ICollection<WishList> wishLists { get; set; }
		public ICollection<Book> books { get; set; }
	}
}
