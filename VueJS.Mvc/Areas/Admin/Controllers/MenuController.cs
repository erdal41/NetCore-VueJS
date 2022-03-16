using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using VueJS.Services.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models.Data;
using VueJS.Mvc.Areas.Admin.Models.View;
using VueJS.Shared.Utilities.Results.ComplexTypes;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _menuService = menuService;
        }

        #region MENU

        [HttpGet("/admin/menu-allmenus")]
        public async Task<JsonResult> AllMenus()
        {
            return Json(new MenuViewModel { MenuListDto = await _menuService.GetAllAsync() });
        }

        [HttpPost("/admin/menu-new")]
        public async Task<JsonResult> New(MenuDataModel menuDataModel)
        {
            return Json(new MenuViewModel { MenuDto = await _menuService.AddAsync(menuDataModel.MenuAddDto) });
        }

        [HttpGet("/admin/menu-edit")]
        public async Task<JsonResult> Edit(int menuId)
        {
            return Json(new MenuViewModel { MenuUpdateDto = await _menuService.GetMenuUpdateDtoAsync(menuId) });
        }

        [HttpPost("/admin/menu-edit")]
        public async Task<JsonResult> Edit(MenuDataModel menuDataModel)
        {
            return Json(new MenuViewModel { MenuDto = await _menuService.UpdateAsync(menuDataModel.MenuUpdateDto) });
        }

        [HttpPost("/admin/menu-delete")]
        public async Task<JsonResult> Delete(int menuId)
        {
            return Json(await _menuService.DeleteAsync(menuId));
        }

        #endregion

        #region MENU DETAIL

        [HttpGet("/admin/menu-allmenudetails")]
        public async Task<JsonResult> AllMenuDetails(int menuId)
        {
            var result = await _menuService.MenuDetailGetAllAsync(menuId);
            return Json(new MenuViewModel { MenuDetailListDto = result });
        }

        [HttpGet("/admin/menu-parentlist")]
        public async Task<JsonResult> GetParentList(int? menuId)
        {
            return Json(new MenuViewModel { MenuDetailListDto = await _menuService.MenuDetailGetAllParentAsync(menuId) });
        }


        [HttpPost("/admin/menu-newmenudetail")]
        public async Task<JsonResult> NewMenuDetail(MenuDataModel menuDataModel)
        {
            return Json(new MenuViewModel { MenuDetailDto = await _menuService.MenuDetailAddAsync(menuDataModel.MenuDetailAddDto) });
        }

        [HttpGet("/admin/menu-editmenudetail")]
        public async Task<JsonResult> EditMenuDetail(int menuId)
        {
            return Json(new MenuViewModel { MenuDetailUpdateDto = await _menuService.GetMenuDetailUpdateDtoAsync(menuId) });
        }

        [HttpPost("/admin/menu-editmenudetail")]
        public async Task<JsonResult> EditMenuDetail(MenuDataModel menuDataModel)
        {
            return Json(new MenuViewModel { MenuDetailDto = await _menuService.MenuDetailUpdateAsync(menuDataModel.MenuDetailUpdateDto) });
        }


        [HttpPost("/admin/menu-deletemenudetail")]
        public async Task<IActionResult> DeleteMenuDetail(int menuId)
        {
            return Json(await _menuService.MenuDetailDeleteAsync(menuId));
        }

        #endregion
    }
}