using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TabApp.Models
{
    public class PickupCode
    {
        public int ID { get; set; }

        public string Value { get; set; }

        public virtual Repair Repair { get; set; }
    }
}
