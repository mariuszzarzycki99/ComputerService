using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TabApp.Models
{
    public class Worker
    {
        [Key]
        public int PersonID { get; set; }

        [DataType(DataType.Currency)]
        public int Earnings { get; set; }

        [RegularExpression(@"^[0-9]*$")]
        [StringLength(11, MinimumLength = 11)]
        [Required]
        public String PESEL { get; set; }

        [Display(Name = "Account Number")]
        [RegularExpression(@"^[0-9]*$")]
        [StringLength(26, MinimumLength = 26)]
        [Required]
        public String AccountNumber { get; set; }

        [Display(Name = "Job Position")]
        [StringLength(60)]
        [Required]
        public String JobPosition { get; set; }

        public Person Person { get; set; }
    }
}