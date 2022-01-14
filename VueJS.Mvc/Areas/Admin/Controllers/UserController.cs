using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _signInManager = signInManager;
        }

        [HttpGet("/admin/user/getallusers")]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();
            var userListDto = JsonConvert.SerializeObject(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(userListDto);
        }

        [HttpGet("/admin/user/new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("/admin/user/new")]
        public async Task<JsonResult> New(UserViewModel userViewModel)
        {
            var user = Mapper.Map<User>(userViewModel.UserAddDto);
            var result = await UserManager.CreateAsync(user, userViewModel.UserAddDto.Password);
            if (result.Succeeded)
            {
                var userAjaxViewModel = JsonConvert.SerializeObject(new UserViewModel
                {
                    User = user
                }, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(userAjaxViewModel);
            }
            else
            {
                var userAjaxViewModel = JsonConvert.SerializeObject(new UserViewModel
                {
                    User = null
                });
                return Json(userAjaxViewModel);
            }
        }

        [HttpGet("/admin/user/edit")]
        public IActionResult Edit(int user)
        {
            return View();
        }

        [HttpGet("/admin/user/getajaxedit")]
        public async Task<JsonResult> GetAjaxEdit(int user)
        {
            var userResult = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == user);
            if (userResult != null)
            {
                var userUpdateDto = Mapper.Map<UserUpdateDto>((object)userResult);
                var userViewModel = JsonConvert.SerializeObject(new UserViewModel
                {
                    UserUpdateDto = userUpdateDto
                }, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(userViewModel);
            }
            else
            {
                var userViewModel = JsonConvert.SerializeObject(new UserViewModel
                {
                    UserUpdateDto = null
                });
                return Json(userViewModel);
            }
        }

        [HttpPost("/admin/user/edit")]
        public async Task<JsonResult> Edit(UserViewModel userViewModel)
        {
            var oldUser = await UserManager.FindByIdAsync(userViewModel.UserUpdateDto.Id.ToString());
            var updatedUserDto = Mapper.Map(userViewModel.UserUpdateDto, oldUser);
            var result = await UserManager.UpdateAsync(updatedUserDto);
            if (result.Succeeded)
            {
                var userAjaxViewModel = JsonConvert.SerializeObject(updatedUserDto, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(userAjaxViewModel);
            }
            else
            {
                var userAjaxViewModel = JsonConvert.SerializeObject(updatedUserDto, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(userAjaxViewModel);
            }
        }

        [HttpPost("/admin/user/delete")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonConvert.SerializeObject(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla silinmiştir.",
                    User = user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error.Description}\n";
                }

                var deletedUserErrorModel = JsonConvert.SerializeObject(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message =
                        $"{user.UserName} adlı kullanıcı adına sahip kullanıcı silinirken bazı hatalar oluştu.\n{errorMessages}",
                    User = user
                });
                return Json(deletedUserErrorModel);
            }
        }

        [HttpPost("/admin/user/multidelete")]
        public async Task<JsonResult> MultiDelete(int[] userIds)
        {
            var jsonResult = string.Empty;

            foreach (var userId in userIds)
            {
                var user = await UserManager.FindByIdAsync(userId.ToString());
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    var deletedUser = JsonConvert.SerializeObject(new UserDto
                    {
                        ResultStatus = ResultStatus.Success,
                        Message = $"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla silinmiştir.",
                        User = user
                    });
                    jsonResult = deletedUser;
                }
                else
                {
                    string errorMessages = String.Empty;
                    foreach (var error in result.Errors)
                    {
                        errorMessages = $"*{error.Description}\n";
                    }

                    var deletedUserErrorModel = JsonConvert.SerializeObject(new UserDto
                    {
                        ResultStatus = ResultStatus.Error,
                        Message =
                            $"{user.UserName} adlı kullanıcı adına sahip kullanıcı silinirken bazı hatalar oluştu.\n{errorMessages}",
                        User = user
                    });
                    return Json(deletedUserErrorModel);
                }
            }
            return Json(jsonResult);
        }

        [HttpGet("/admin/user/passwordchange")]
        public ViewResult PasswordChange()
        {
            return View();
        }

        [HttpPost("/admin/user/passwordchange")]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
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
                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen, girmiş olduğunuz şu anki şifrenizi kontrol ediniz.");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
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
            var tokenConvert = t.Replace("%2F", "/").Replace("%2B","+").Replace("%3D","=");
            IdentityResult result = await UserManager.ResetPasswordAsync(user, tokenConvert, userPasswordResetDto.NewPassword);
            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user);
                var jsonResult = JsonConvert.SerializeObject(result);
                return Json(jsonResult);
            }
            else
            {
                var jsonResult = JsonConvert.SerializeObject(result);
                return Json(jsonResult);
            }
        }
    }
}