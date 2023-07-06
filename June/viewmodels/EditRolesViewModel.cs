using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace June.viewmodels
{
    public class EditRolesViewModel
    {
        public EditRolesViewModel()
        {
            Users = new List<string>();
        }
        public String Id { get; set; }
        [Required(ErrorMessage= "Role Name is Required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
