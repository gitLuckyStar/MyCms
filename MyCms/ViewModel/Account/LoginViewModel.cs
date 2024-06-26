﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCms
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("نام کاربری"))]
        public string UserName { get; set; }
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("رمز عبور"))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = ("مرا به خاطر بسپار"))]
        public bool RememberMe { get; set; }

    }
}
