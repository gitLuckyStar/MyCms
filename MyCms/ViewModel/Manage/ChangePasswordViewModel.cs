using System.ComponentModel.DataAnnotations;

namespace MyCms
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = ("لطفا {0} را وارد کنید"))]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage =
            "رمزعبور جدید و تکرار رمز عبور جدید با یکدیگر همخوانی ندارند")]
        public string ConfirmPassword { get; set; }
    }
}
