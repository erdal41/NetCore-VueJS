using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Threading.Tasks;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, SignInManager<User> signInManager, RoleManager<Role> roleManager) : base(userManager, mapper, imageHelper)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet("/admin/user/allusers")]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();
            if (users.Count > 0)
            {
                var userListDto = new UserListDto
                {
                    Users = users,
                    ResultStatus = ResultStatus.Success
                };
                return Json(userListDto);
            }
            else
            {
                var userListDtoError = new UserListDto
                {
                    Users = null,
                    ResultStatus = ResultStatus.Error
                };
                return Json(userListDtoError);
            }
        }

        [HttpPost("/admin/user/new")]
        public async Task<JsonResult> New(UserViewModel userViewModel)
        {
            var user = Mapper.Map<User>(userViewModel.UserAddDto);
            var result = await UserManager.CreateAsync(user, userViewModel.UserAddDto.Password);
            if (result.Succeeded)
            {
                var userViewModelJson = new UserViewModel
                {
                    User = user
                };
                return Json(userViewModelJson);
            }
            else
            {
                var userViewModelJsonError = new UserViewModel
                {
                    User = null
                };
                return Json(userViewModelJsonError);
            }
        }

        [HttpGet("/admin/user/edit")]
        public async Task<JsonResult> Edit(int user)
        {
            var userResult = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == user);
            if (userResult != null)
            {
                var userUpdateDto = Mapper.Map<UserUpdateDto>(userResult);

                var roles = await _roleManager.Roles.ToListAsync();
                var userRoles = await UserManager.GetRolesAsync(userResult);

                UserRoleAssignDto userRoleAssignDto = new UserRoleAssignDto
                {
                    UserId = userResult.Id,
                    UserName = userResult.UserName
                };

                foreach (var role in roles)
                {
                    RoleAssignDto rolesAssignDto = new RoleAssignDto
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        HasRole = userRoles.Contains(role.Name)
                    };
                    userRoleAssignDto.RoleAssignDtos.Add(rolesAssignDto);
                }

                var userViewModel = new UserViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserRoleAssignDto = userRoleAssignDto
                };
                return Json(userViewModel);
            }
            else
            {
                var userViewModelError = new UserViewModel
                {
                    UserUpdateDto = null
                };
                return Json(userViewModelError);
            }
        }

        [HttpPost("/admin/user/edit")]
        public async Task<JsonResult> Edit(UserViewModel userViewModel)
        {
            var oldUser = await UserManager.FindByIdAsync(userViewModel.UserUpdateDto.Id.ToString());
            var updatedUser = Mapper.Map(userViewModel.UserUpdateDto, oldUser);
            var result = await UserManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
            {
                var userViewModelJson = new UserViewModel
                {
                    User = updatedUser
                };
                return Json(userViewModelJson);
            }
            else
            {
                var userViewModelJsonError = new UserViewModel
                {
                    User = null
                };
                return Json(userViewModelJsonError);
            }
        }

        [HttpPost("/admin/user/delete")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var userDto = new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı silindi.",
                    User = user
                };
                return Json(userDto);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error.Description}\n";
                }

                var userDtoError = new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message =
                        $"{user.UserName} adlı kullanıcı silinirken bazı hatalar oluştu.\n{errorMessages}",
                    User = user
                };
                return Json(userDtoError);
            }
        }

        [HttpPost("/admin/user/passwordchange")]
        public async Task<JsonResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {

            var user = await UserManager.GetUserAsync(HttpContext.User);
            var isVerified = await UserManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
            if (isVerified)
            {
                var result = await UserManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword,
                    userPasswordChangeDto.NewPassword);
                if (result.Succeeded)
                {
                    await UserManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                    return Json(userPasswordChangeDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return Json(userPasswordChangeDto);
                }
            }
            else
            {
                ModelState.AddModelError("", "Lütfen, girmiş olduğunuz şu anki şifrenizi kontrol ediniz.");
                return Json(userPasswordChangeDto);
            }
        }

        [HttpGet("/password-reset/")]
        public IActionResult PasswordReset(int u, string t)
        {
            return View();
        }

        [HttpPost("/password-reset")]
        public async Task<JsonResult> PasswordReset(UserPasswordResetDto userPasswordResetDto, string u, string t)
        {
            var user = await UserManager.FindByIdAsync(u);
            var tokenConvert = t.Replace("%2F", "/").Replace("%2B", "+").Replace("%3D", "=");
            IdentityResult result = await UserManager.ResetPasswordAsync(user, tokenConvert, userPasswordResetDto.NewPassword);
            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user);
                return Json(result);
            }
            else
            {
                return Json(result);
            }
        }
    }
}