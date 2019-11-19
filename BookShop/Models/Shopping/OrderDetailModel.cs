using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
	public class OrderDetailModel
	{
		public CustomerEntity customer { get; set; }
		public OrderEntity order { get; set; }
		public string shipping_method { get; set; }
		public Order_Delivery delivery { get; set; }
		public Shipping_Address address { get; set; }
		[Display(Name = "Payment Method")]
		public string payment_Method { get; set; }
		public PaymentEntity payment { get; set; }
		public List<OrderItemEntity> order_items { get; set; }
		[Display(Name = "Order Total")]
		[DataType(DataType.Currency)]
		public decimal order_total { get; set; }
	}
}
