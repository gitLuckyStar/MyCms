using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCms
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [MaxLength(256)]
        [Display(Name = ("نام کاربری"))]
        [Remote("IsUserNameInUse", "Account", HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("ایمیل"))]
        [MaxLength(256)]
        [EmailAddress(ErrorMessage =("{0} مورد نظر معتبر نیست"))]
        [Remote("IsEmailInUse", "Account", HttpMethod ="Post",AdditionalFields ="__RequestVerificationToken")]
        public string Email { get; set; }
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("رمز عبور"))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("تکرار رمز عبور"))]        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage =
            "رمزعبور جدید و تکرار رمز عبور جدید با یکدیگر همخوانی ندارند")]
        public string ConfirmPassword { get; set; }
    }
}
