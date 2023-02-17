using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Repair
    {
        public int ID { get; set; }

        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? IssueDate { get; set; }

        [DataType(DataType.Currency)]
        public int? Cost { get; set; }

        public bool Warranty {get; set;}      

        public Item Item { get; set; }

        public Invoice Invoice { get; set; }

        public virtual RepairStatus RepairStatus { get; set; }
       
        [ForeignKey("PickupCodeID")]
        public virtual PickupCode PickupCode { get; set; }

        public ICollection<Service> Service { get; set; }

    }

}