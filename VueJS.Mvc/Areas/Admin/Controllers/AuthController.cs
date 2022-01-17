using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Models;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections.Generic;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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
        private IConfiguration _configuration;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ISettingService settingService, IMailService mailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _settingService = settingService;
            _mailService = mailService;
            _configuration = configuration;
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
            if (user != null && await _userManager.CheckPasswordAsync(user, loginViewModel.UserLoginDto.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.UserLoginDto.Password,
       loginViewModel.UserLoginDto.RememberMe, false);
                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                       issuer: _configuration["JWT:ValidIssuer"],
                       audience: _configuration["JWT:ValidAudience"],
                       expires: DateTime.Now.AddHours(3),
                       claims: authClaims,
                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                       );

                    var loginViewModelJson = new LoginViewModel
                    {
                        UserLoginDto = loginViewModel.UserLoginDto,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
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