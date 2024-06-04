using System.ComponentModel.DataAnnotations;

namespace MyCms
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("رمز عبور"))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [Display(Name = ("تکرار رمز عبور"))]
        [Compare(nameof(NewPassword))]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }

        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
