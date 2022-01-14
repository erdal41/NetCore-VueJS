using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Models;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISettingService _settingService;
        private readonly IMailService _mailService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ISettingService settingService, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settingService = settingService;
            _mailService = mailService;
        }

        [HttpGet("/admin/auth/login")]
        public async Task<IActionResult> Login()
        {
            var generalSetting = await _settingService.GetGeneralSettingAsync();
            var loginViewModel = new LoginViewModel
            {
                GeneralSettingDto = generalSetting.Data
            };
            return Json(loginViewModel);
        }

        [HttpPost("/admin/auth/login")]
        public async Task<JsonResult> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.UserLoginDto.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.UserLoginDto.Password,
                    loginViewModel.UserLoginDto.RememberMe, false);
                if (result.Succeeded)
                {
                    var loginViewModelJson = new LoginViewModel
                    {
                        UserLoginDto = loginViewModel.UserLoginDto,
                    };
                    return Json(loginViewModelJson);
                }
            }
            var loginViewModelJsonError = new LoginViewModel
            {
                UserLoginDto = null,
            };
            return Json(loginViewModelJsonError);
        }

        [HttpGet("/admin/auth/unauthorizedaccess")]
        public ViewResult UnauthorizedAccess()
        {
            return View();
        }

        [HttpGet("/admin/auth/logout")]
        public async Task<JsonResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Json(true);
        }

        [HttpPost("/admin/auth/forgotpassword")]
        public async Task<JsonResult> ForgotPassword(string email)
        {
            var domainName = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var bodyHtml = $"<a target=\"_blank\" href=\"{domainName}{Url.Action("PasswordReset", "User", new { u = user.Id, t = token })}\">Yeni şifrenizi oluşturmak için tıklayınız.</a>";
                var result = _mailService.SendResetPassword(email, bodyHtml);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    return Json(result);
                }
                else
                {
                    return Json(result);
                }
            }
            else
            {
                return Json(user);
            }
        }
    }
}