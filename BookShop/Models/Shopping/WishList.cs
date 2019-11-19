using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
	public class WishList
	{
		[Key]
		public string WishList_id { get; set; }
		public DateTime date_created { get; set; }

		public WishList_Item Wish_Items { get; set; }
	}
}
