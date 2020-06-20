using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cw10.DTOs
{
    public class EnrollmentRequest
    {
        [RegularExpression("^s[0-9]{5}$")]
        public string IndexNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Studies { get; set; }
    }
}
