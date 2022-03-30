using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models.View;
using System;
using System.Threading.Tasks;
using VueJS.Services.Abstract;
using System.Collections.Generic;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPostService _postService;
        private readonly IUploadService _uploadService;

        public UserController(SignInManager<User> signInManager, RoleManager<Role> roleManager, IUploadService uploadService, IPostService postService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _postService = postService;
            _uploadService = uploadService;
        }

        [HttpGet("/admin/user-allusers")]
        public async Task<JsonResult> GetAllUsers()
        {
            var allUsers = await UserManager.Users.Include(u => u.ProfileImage).Include(u => u.Posts).ToListAsync();
            if (allUsers.Count > 0)
            {
                List<User> users = new List<User>();
                foreach (var user in allUsers)
                {
                    if (user.ProfileImageId != null)
                    {
                        var upload = await _uploadService.GetAsync(user.ProfileImageId.Value);
                        user.ProfileImage = upload.Data.Upload;
                    }
                    users.Add(user);
                }
                return Json(new UserListDto { Users = users });
            }
            else
            {
                return Json(new UserListDto { Users = null });
            }
        }

        [HttpPost("/admin/user-new")]
        public async Task<JsonResult> New(UserViewModel userViewModel)
        {
            var user = Mapper.Map<User>(userViewModel.UserAddDto);
            var result = await UserManager.CreateAsync(user, userViewModel.UserAddDto.Password);
            if (result.Succeeded)
            {
                return Json(new UserViewModel { UserDto = new UserDto { User = user } });
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
                return Json(new UserViewModel
                {
                    UserDto = new UserDto
                    {
                        User = null,
                        ErrorMessages = errorMessages
                    }
                });
            }
        }

        [HttpGet("/admin/user-edit")]
        public async Task<JsonResult> Edit(int userId)
        {
            var userResult = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userResult == null) return Json(new UserViewModel { UserUpdateDto = null });

            var userUpdateDto = Mapper.Map<UserUpdateDto>(userResult);
            if (userResult.ProfileImageId != null)
            {
                var profileImage = await _uploadService.GetAsync(userResult.ProfileImageId.Value);
                userUpdateDto.ProfileImage = profileImage.Data.Upload;
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await UserManager.GetRolesAsync(userResult);

            return Json(new UserViewModel
            {
                UserUpdateDto = userUpdateDto,
                UserRolesViewModel = new UserRolesViewModel
                {
                    Roles = roles,
                    UserRoles = userRoles
                }
            });
        }

        [HttpPost("/admin/user-edit")]
        public async Task<JsonResult> Edit(UserViewModel userViewModel)
        {
            var oldUser = await UserManager.FindByIdAsync(userViewModel.UserUpdateDto.Id.ToString());
            var updatedUser = Mapper.Map(userViewModel.UserUpdateDto, oldUser);
            var result = await UserManager.UpdateAsync(updatedUser);
            if (!result.Succeeded) return Json(new UserViewModel { User = null });
            return Json(new UserViewModel { User = updatedUser });
        }

        [HttpGet("/admin/user-allroles")]
        public async Task<JsonResult> AllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            if (roles.Count < 1) return Json(new UserRolesViewModel { Roles = null });
            return Json(new UserRolesViewModel { Roles = roles });
        }

        [HttpPost("/admin/user-roleassign")]
        public async Task<IActionResult> RoleAssign(UserRolesViewModel userRolesViewModel)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userRolesViewModel.UserId);
            if (user == null) return Json(new UserRolesViewModel { UserDto = new UserDto { User = null } });

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
            return Json(new UserRolesViewModel { UserDto = new UserDto { User = user } });
        }

        [HttpPost("/admin/user-delete")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new UserDto { User = user });
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages += $"* {error.Description}\n";
                }

                return Json(new UserDto
                {
                    User = null,
                    ErrorMessages = errorMessages
                });
            }
        }

        [HttpPost("/admin/user-passwordchange")]
        public async Task<JsonResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var isVerified = await UserManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
            if (!isVerified) return Json(new UserViewModel { UserDto = new UserDto { User = null } });

            var result = await UserManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword,
                userPasswordChangeDto.NewPassword);
            if (!result.Succeeded) return Json(new UserViewModel { UserDto = new UserDto { User = null } });

            await UserManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
            return Json(new UserViewModel { UserDto = new UserDto { User = user } });
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
                return Json(new UserDto { User = user });
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages += $"* {error.Description}\n";
                }

                return Json(new UserDto
                {
                    User = null,
                    ErrorMessages = errorMessages
                });
            }
        }
    }
}