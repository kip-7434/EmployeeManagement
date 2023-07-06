﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace June.viewmodels
{
    public class ForgotPassword
    {
[Required]
[EmailAddress]
public string Email { get; set; }
    }
}
