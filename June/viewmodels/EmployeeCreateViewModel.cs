using June.models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace June.viewmodels
{
    public class EmployeeCreateViewModel
    {
        
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invalid email format")]

        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public IFormFile Photo { get; set; }




    }
}
