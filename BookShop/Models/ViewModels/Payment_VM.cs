using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.ViewModels
{
   public class Payment_VM
    {
        [Key]
        public int Payment_VMId { get; set; }
        public int VisitId { get; set; }
        public string PrimaryDiagnosis { get; set; }
        public string symptoms { get; set; }
        public string RefNo { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public double AmountPaid { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string PaymentFor { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

    }
}
