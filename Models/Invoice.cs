using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Invoice
    {
        public int ID { get; set; }

        public String NIP { get; set; }

        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        public Person Person { get; set; }

        [ForeignKey("RepairID")]
        public Repair Repair { get; set; }

    }
}