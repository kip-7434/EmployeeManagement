using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace June.viewmodels
{
    public class EmployeeEditViewModel :EmployeeCreateViewModel
    {
public int Id { get; set; }
        public string ExistingPhotopath { get; set; }
    }
}
