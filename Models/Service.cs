using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Service
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime WarrantyDate { get; set; }

        [DataType(DataType.Currency)]
        public int? PartsCost { get; set; } 

        public Repair Repair { get; set; }

        public Person Person { get; set; }

        public PriceList PriceList {get; set;}

    }

}