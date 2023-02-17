using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Item
    {
        public int ID { get; set; }

        [Display(Name = "Serial number")]
        [StringLength(100, MinimumLength = 3)]
        [Required]
        public String SerialNumber { get; set; }


        [StringLength(100, MinimumLength = 3)]
        [Required]
        public String Description { get; set; }

        public Person Person { get; set; }
        
        public ICollection<Repair> Repair { get; set; }
    }

}