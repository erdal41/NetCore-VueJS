using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Services.Abstract;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Models;

namespace VueJS.Mvc.Areas.Admin.ViewComponents
{
    [ViewComponent]
    [Route("[viewcomponent]")]
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly ISettingService _settingService;

        public AdminMenuViewComponent(UserManager<User> userManager, ISettingService settingService)
        {
            _userManager = userManager;
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return Content("Kullanıcı bulunamadı.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null)
                return Content("Roller bulunamadı.");
            var generalSetting = await _settingService.GetGeneralSettingAsync();
            return View(new AdminMenuViewModel
            {
                User = user,
                Roles = roles,
                GeneralSettingDto = generalSetting.Data
            });
        }
    }
}