using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testing.Models
{
    public class Employee
    {
        public int id { get; set; }
        [Required]
        public string EmpName { get; set; }
         [Required]
        public string Email { get; set; }
         [Required]
         [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }

}