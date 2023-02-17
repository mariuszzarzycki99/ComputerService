using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabApp.Models
{
    public class RepairStatus
    {
        
        public int ID { get; set; }

        public string Status { get; set; }

        public ICollection<Repair> Repair { get; set; }
    }
}
