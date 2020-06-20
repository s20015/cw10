using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw10.DTOs
{
    public class StudentResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string IndexNumber { get; set; }
    }
}
