using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public class Company
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyID { get; set; }
        [Required]
        [Display(Name = "Name")]
        [Index("Company_Index", IsUnique = true)]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Index("Company_Email_Index", IsUnique = true)]
        [MinLength(3)]
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Tel")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered Tel format is not valid. it should be something like: 0314345659")]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Partner Since")]
        public System.DateTime Member_Since { get; set; }
    }
}
