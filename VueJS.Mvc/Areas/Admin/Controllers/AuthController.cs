using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Models.View;
using AutoMapper;
using System;
using VueJS.Mvc.Areas.Admin.Helper.Abstract;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ISettingService _settingService;
        private readonly IMailService _mailService;
        private readonly IJwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;
        public AuthController(
            UserManager<User> userManager,
            IMapper mapper,
            SignInManager<User> signInManager,
            ISettingService settingService,
            IMailService mailService,
            IJwtHelper jwtHelper,
            IConfiguration configuration) : base(userManager, mapper)
        {
            _signInManager = signInManager;
            _settingService = settingService;
            _mailService = mailService;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        [HttpGet("/admin/auth-islogin")]
        public JsonResult IsLogin()
        {
            try
            {
                if (LoggedInUser != null)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpGet("/admin/auth-login")]
        public async Task<IActionResult> Login()
        {
            var generalSetting = await _settingService.GetGeneralSettingAsync();
            var loginViewModel = new LoginViewModel
            {
                GeneralSettingDto = generalSetting.Data
            };
            return Json(loginViewModel);
        }

        [HttpPost("/admin/auth-login")]
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
                    var roles = await UserManager.GetRolesAsync(user);
                    userLoginViewModel.Roles = roles;

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in roles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = _jwtHelper.CreateToken(authClaims);
                    var refreshToken = _jwtHelper.GenerateRefreshToken();

                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    await UserManager.UpdateAsync(user);

                    var loginViewModelJson = new LoginViewModel
                    {
                        UserLoginViewModel = userLoginViewModel,
                        TokenModel = new TokenModel
                        {
                            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                            RefreshToken = refreshToken,
                            Expiration = token.ValidTo
                        }
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

        [HttpPost("/admin/auth-refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            var principal = _jwtHelper.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;

            var user = await UserManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = _jwtHelper.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await UserManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await UserManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await UserManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = UserManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await UserManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [HttpGet("/admin/auth-unauthorizedaccess")]
        public ViewResult UnauthorizedAccess()
        {
            return View();
        }

        [HttpGet("/admin/auth-logout")]
        public async Task<JsonResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Json(true);
        }

        [HttpPost("/admin/auth-forgotpassword")]
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