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
using VueJS.Services.Concrete;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(SignInManager<User> signInManager, RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
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

        [HttpPost("/admin/user-new")]
        public async Task<JsonResult> New(UserViewModel userViewModel)
        {
            var user = Mapper.Map<User>(userViewModel.UserAddDto);
            var result = await UserManager.CreateAsync(user, userViewModel.UserAddDto.Password);
            if (result.Succeeded)
            {
                var userViewModelJson = new UserViewModel
                {
                    UserDto = new UserDto
                    {
                        User = user,
                        ResultStatus = ResultStatus.Success,
                        Message = user.UserName + " adlı kullanıcı eklendi."
                    }
                };
                return Json(userViewModelJson);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    if (error.Code == "DuplicateUserName")
                    {
                        errorMessages += "* Bu kullanıcı adı zaten kayıtlı.\n";
                    }
                    if (error.Code == "DuplicateEmail")
                    {
                        errorMessages += "* Bu e-posta adresi zaten kayıtlı.\n";
                    }
                }

                var userViewModelJsonError = new UserViewModel
                {
                    UserDto = new UserDto
                    {
                        User = null,
                        ResultStatus = ResultStatus.Error,
                        Message = errorMessages
                    }
                };
                return Json(userViewModelJsonError);
            }
        }

        [HttpGet("/admin/user-edit")]
        public async Task<JsonResult> Edit(int user)
        {
            var userResult = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == user);
            if (userResult != null)
            {
                var userUpdateDto = Mapper.Map<UserUpdateDto>(userResult);

                var roles = await _roleManager.Roles.ToListAsync();
                var userRoles = await UserManager.GetRolesAsync(userResult);

                var userViewModel = new UserViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserRolesViewModel = new UserRolesViewModel
                    {
                        Roles = roles,
                        UserRoles = userRoles
                    }
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

        [HttpPost("/admin/user-edit")]
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

        [HttpPost("/admin/user/roleassign")]
        public async Task<IActionResult> RoleAssign(UserRolesViewModel userRolesViewModel)
        {

            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userRolesViewModel.UserId);
            if (user != null)
            {
                var roles = await _roleManager.Roles.ToListAsync();
                foreach (var role in roles)
                {
                    await UserManager.RemoveFromRoleAsync(user, role.Name);
                }

                foreach (var role in userRolesViewModel.UserRoles)
                {
                    await UserManager.AddToRoleAsync(user, role);

                }
                await UserManager.UpdateSecurityStampAsync(user);

                var userRolesViewModelJson = new UserRolesViewModel
                {
                    UserDto = new UserDto
                    {
                        User = user,
                        Message = $"{user.UserName} kullanıcısına ait rol atama işlemi başarıyla tamamlandı.",
                        ResultStatus = ResultStatus.Success
                    },
                };
                return Json(userRolesViewModelJson);
            }
            else
            {
                var userRolesViewModelJsonError = new UserRolesViewModel
                {
                    UserDto = new UserDto
                    {
                        User = null,
                        Message = $"{user.UserName} kullanıcısına ait rol atama işlemi başarısız oldu.",
                        ResultStatus = ResultStatus.Error
                    },
                };
                return Json(userRolesViewModelJsonError);
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

                    var userViewModelJson = new UserViewModel
                    {
                        UserDto = new UserDto
                        {
                            User = user,
                            Message = $"{user.UserName} kullanıcısının şifre güncelleme işlemi başarıyla tamamlandı.",
                            ResultStatus = ResultStatus.Success
                        },
                    };
                    return Json(userViewModelJson);
                }
                else
                {
                    var userViewModelJsonError = new UserViewModel
                    {
                        UserDto = new UserDto
                        {
                            User = null,
                            Message = "Şifre güncelleme işlemi başarısız oldu. Lütfen bilgilerinizi kontrol ediniz.",
                            ResultStatus = ResultStatus.Error
                        },
                    };
                    return Json(userViewModelJsonError);
                }
            }
            else
            {
                var userViewModelJsonError = new UserViewModel
                {
                    UserDto = new UserDto
                    {
                        User = null,
                        Message = "Eski şifreniz yanlış. Lütfen kontrol ediniz.",
                        ResultStatus = ResultStatus.Error
                    },
                };
                return Json(userViewModelJsonError);
            }
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