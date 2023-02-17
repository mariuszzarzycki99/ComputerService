using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TabApp.Models
{
    public class PriceList
    {
        public int ID { get; set; }
        
        public int Price { get; set; }
        public String Description { get; set; }
        public ICollection<Service> Service { get; set; }
    }

}