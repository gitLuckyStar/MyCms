using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyCms
{
    public class ForgotPasswordViewModel
    {

        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("ایمیل"))]
        [EmailAddress]        
        public string Email { get; set; }
    }
}
