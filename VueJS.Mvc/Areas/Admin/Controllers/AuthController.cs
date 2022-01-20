using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Models;
using Microsoft.Extensions.Configuration;
using VueJS.Entities.Dtos;
using AutoMapper;
using System.Collections.Generic;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ISettingService _settingService;
        private readonly IMailService _mailService;

        public AuthController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, ISettingService settingService, IMailService mailService) : base(userManager, mapper)
        {
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
            var user = await UserManager.FindByEmailAsync(loginViewModel.UserLoginDto.Email);
            if (user != null && await UserManager.CheckPasswordAsync(user, loginViewModel.UserLoginDto.Password))
            {
                var userLoginViewModel = Mapper.Map<UserLoginViewModel>(user);

                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.UserLoginDto.Password,
       loginViewModel.UserLoginDto.RememberMe, false);
                if (result.Succeeded)
                {
                    var roles = UserManager.GetRolesAsync(user);
                    userLoginViewModel.Roles = roles.Result;
                    var loginViewModelJson = new LoginViewModel
                    {
                        UserLoginViewModel = userLoginViewModel
                    };
                    return Json(loginViewModelJson);
                }
            }
            var loginViewModelJsonError = new LoginViewModel
            {
                UserLoginViewModel = null,
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
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                string token = await UserManager.GeneratePasswordResetTokenAsync(user);
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