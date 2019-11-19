using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
   public class PaymentEntity
    {
        [Key]
        [Display(Name = "ID")]
        public int payment_ID { get; set; }
        [Display(Name = "Customer")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public double AmountPaid { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string PaymentFor { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [ForeignKey("Order")]
        public string Order_ID { get; set; }

        public virtual CustomerEntity Customer { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
