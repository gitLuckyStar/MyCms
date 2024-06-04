using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using ServiceLayer;


namespace MyCms.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageSender _messageSender;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IMessageSender messageSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageSender = messageSender;
        }
        #region Register        
        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion

        #region Register  [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model,IFormCollection form)
        {
            #region Google reCaptcha
            if (reCaptchaValidation.GreCaptcha(form) == false)
            {
                ViewBag.GreCaptcha = "خطا، لطفا دوباره امتحان کنید";
                return View();
            }
            #endregion

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage = Url.Action("ConfirmEmail", "Account",
                    new
                    {
                        username = user.UserName,
                        token = emailConfirmationToken
                    }, Request.Scheme
                    );
                    await _messageSender.SendEmailAsync(model.Email, "تایید ایمیل کاربری : MyCms", emailMessage);
                    ViewData["Message"] = "اگر ایمیل شما معتبر باشد، لینکی جهت تایید ایمیل برای شما ارسال میشود";

                    return View(model);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {            
            if (TempData.ContainsKey("Message"))
            {
                //Peek Method will read the data and preserve the key for next request
                ViewData["Message"] = TempData.Peek("Message");
                ViewData["FailedMessage"] = null;
            }
            if (TempData.ContainsKey("FailedMessage"))
            {
                //Peek Method will read the data and preserve the key for next request
                ViewData["Message"] = null;
                ViewData["FailedMessage"] = TempData.Peek("FailedMessage");
            }
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["returnUrl"] = returnUrl;
            return View();
        }
        #endregion

        #region Login [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, IFormCollection form, string returnUrl = null)
        {
            #region Google reCaptcha
            if (reCaptchaValidation.GreCaptcha(form) == false)
            {
                ViewBag.GreCaptcha = "خطا، لطفا دوباره امتحان کنید";
                return View();
            }
            #endregion

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && !user.EmailConfirmed &&
                            (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "ایمیل تایید نشده است جهت ورود لطفا ایمیل خود را تایید کنید");
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage = Url.Action("ConfirmEmail", "Account",
                    new
                    {
                        username = user.UserName,
                        token = emailConfirmationToken
                    }, Request.Scheme
                    );
                    await _messageSender.SendEmailAsync(user.Email, "تایید ایمیل کاربری : MyCms", emailMessage);
                    ViewData["Message"] = "اگر ایمیل شما معتبر باشد ،لینکی جهت تایید ایمیل برای شما ارسال میشود";
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ViewData["ErrorMessage"] = "اکانت شما به دلیل 5 بار ورود ناموفق به مدت 5 دقیقه قفل شده است";
                    return View(model);
                }
                ModelState.AddModelError("", "نام کاربری یا رمزعبور اشتباه است");
            }            
            return View(model);
        }
        #endregion

        #region Logout
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region IsEmailInUse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }

            return Json("ایمیل وارد شده از قبل موجود است");
        }
        #endregion

        #region IsUserNameInUse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserNameInUse(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Json(true);
            }

            return Json("نام کاربری وارد شده از قبل موجود است");
        }
        #endregion

        #region ConfirmEmail
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewData["Message"] = "ایمیل شما با موفقیت تایید شد";
                return View();
            }
            ViewData["FailedMessage"] = "خطایی رخ داده است ، ایمیل شما تایید نشد";
            return View();
        }
        #endregion

        #region ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }
        #endregion

        #region ForgotPassword [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewData["Message"] = "اگر ایمیل شما معتبر باشد، لینک فراموشی رمز عبور برای شما ارسال می شود";
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View("Login");
                }
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordUrl = Url.Action("ResetPassword", "Account", new
                {
                    email = user.Email,
                    token = resetPasswordToken
                }, Request.Scheme
                );

                await _messageSender.SendEmailAsync(user.Email, "بازنشانی رمز عبور : MyCms", resetPasswordUrl);
            }
            return View(model);
        }
        #endregion

        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        #endregion

        #region ResetPassword [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["FailedMessage"] = "خطایی رخ داده ، رمز عبور تغییر نکرد";
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Token,model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["Message"] = "رمز عبور شما با موفقیت تغییر یافت";
                    return RedirectToAction("Login");
                }
                foreach(var error in  result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        #endregion
    }
}
