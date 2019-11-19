using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{
    public partial class Notifications
    {
        [Key]
        public int NotificatioID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsViewed { get; set; }
        public string Url { get; set; }
        public string ReplyToEmail { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
