using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TabApp.Models
{
    public class Person
    {
        public int ID { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public String Name { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public String Surname { get; set; }

        [StringLength(60)]
        [Required]
        public String Address { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public String Email { get; set; }

        [Required]
        public String Role { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"^[0-9]*$")]
        [StringLength(9, MinimumLength = 9)]
        [Required]
        public String PhoneNumber { get; set; }

        public virtual Worker Worker { get; set; }

        public virtual LoginCredentials LoginCredentials { get; set; }

        public ICollection<Item> Item { get; set; }

        public ICollection<Message> SendMessage { get; set; }

        public ICollection<Message> ReciveMessage { get; set; }

        public ICollection<Invoice> Invoice { get; set; }

        public ICollection<Service> Service { get; set; }
    }

}