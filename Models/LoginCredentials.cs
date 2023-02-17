using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TabApp.Models
{
    public class LoginCredentials
    {
        [ForeignKey("Person")]
        public int ID { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public String UserName { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public String Password { get; set; }

        public virtual Person Person { get; set; }
    }
}
