﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Models.Shopping
{

    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Address_ID { get; set; }

        [Display(Name = "Street Number")]
        public int street_number { get; set; }

        [Display(Name = "Street Name")]
        public string Street_name { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip-Code")]
        public string ZipCode { get; set; }


    }
    public class Shipping_Address : Address
    {
        public Shipping_Address()
        { }
        public string Building_Name { get; set; }
        public string Floor { get; set; }
        public string Contact_Number { get; set; }
        public string Address_Type { get; set; }// business, home etc.
        public string Comments { get; set; }

        [ForeignKey("Order")]
        public string Order_ID { get; set; }
        public virtual OrderEntity Order { get; set; }
    }

}
