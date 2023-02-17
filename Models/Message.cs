using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Message
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public String Title { get; set; }

        [Required]
        [StringLength(254, MinimumLength = 1)]
        public String Content { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        public Person Sender { get; set; }

        public Person Addressee { get; set; }

    }

}