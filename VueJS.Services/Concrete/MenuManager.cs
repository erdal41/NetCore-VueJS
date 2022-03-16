using AutoMapper;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Services.Utilities;
using VueJS.Shared.Utilities.Results.Concrete;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;
using VueJS.Services.Extensions;
using System.Collections.Generic;
using System.Linq;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Services.Concrete
{
    public class MenuManager : ManagerBase, IMenuService
    {
        public MenuManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }


        #region MENU

        public async Task<IDataResult<MenuDto>> GetAsync(int menuId)
        {
            var menu = await UnitOfWork.Menus.GetAsync(t => t.Id == menuId);
            if (menu == null) return new DataResult<MenuDto>(ResultStatus.Error, Messages.Menu.NotFound(false), null);
            return new DataResult<MenuDto>(ResultStatus.Success, new MenuDto { Menu = menu });
        }

        public async Task<IDataResult<MenuListDto>> GetAllAsync()
        {
            var menus = await UnitOfWork.Menus.GetAllAsync(null);
            if (menus.Count < 1) return new DataResult<MenuListDto>(ResultStatus.Error, Messages.Menu.NotFound(true), null);
            return new DataResult<MenuListDto>(ResultStatus.Success, new MenuListDto { Menus = menus });
        }

        public async Task<IDataResult<MenuUpdateDto>> GetMenuUpdateDtoAsync(int menuId)
        {
            var result = await UnitOfWork.Menus.AnyAsync(c => c.Id == menuId);
            if (!result) return new DataResult<MenuUpdateDto>(ResultStatus.Error, Messages.Menu.NotFound(false), null);

            var menu = await UnitOfWork.Menus.GetAsync(c => c.Id == menuId);
            var menuUpdateDto = Mapper.Map<MenuUpdateDto>(menu);
            return new DataResult<MenuUpdateDto>(ResultStatus.Success, menuUpdateDto);
        }

        public async Task<IDataResult<MenuDto>> AddAsync(MenuAddDto menuAddDto)
        {
            var nameCheck = await UnitOfWork.Menus.GetAllAsync(t => t.Name == menuAddDto.Name);
            if (nameCheck.Count != 0) return new DataResult<MenuDto>(ResultStatus.Error, Messages.Menu.TitleCheck(), null);
            var menu = Mapper.Map<Menu>(menuAddDto);
            var addedMenu = await UnitOfWork.Menus.AddAsync(menu);
            await UnitOfWork.SaveAsync();
            return new DataResult<MenuDto>(ResultStatus.Success, Messages.Menu.Add(addedMenu.Name), new MenuDto { Menu = addedMenu });
        }

        public async Task<IDataResult<MenuDto>> UpdateAsync(MenuUpdateDto menuUpdateDto)
        {
            var nameCheck = await UnitOfWork.Menus.GetAllAsync(t => (t.Name == menuUpdateDto.Name && t.Id == menuUpdateDto.Id) || (t.Name != menuUpdateDto.Name && t.Id == menuUpdateDto.Id));
            if (nameCheck.Count != 1) return new DataResult<MenuDto>(ResultStatus.Error, Messages.Menu.TitleCheck(), null);

            var oldMenu = await UnitOfWork.Menus.GetAsync(p => p.Id == menuUpdateDto.Id);
            var menu = Mapper.Map<MenuUpdateDto, Menu>(menuUpdateDto, oldMenu);
            var updatedMenu = await UnitOfWork.Menus.UpdateAsync(menu);
            await UnitOfWork.SaveAsync();
            return new DataResult<MenuDto>(ResultStatus.Success, Messages.Menu.Update(menu.Name), new MenuDto { Menu = menu });
        }

        public async Task<IResult> DeleteAsync(int menuId)
        {
            var menu = await UnitOfWork.Menus.GetAsync(t => t.Id == menuId);
            if (menu == null) return new Result(ResultStatus.Error, Messages.Menu.NotFound(false));

            var menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(md => md.MenuId == menuId);

            foreach (var menuDetail in menuDetails)
            {
                await UnitOfWork.MenuDetails.DeleteAsync(menuDetail);
            }
            await UnitOfWork.Menus.DeleteAsync(menu);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Menu.Delete(menu.Name));
        }

        #endregion


        #region MENU DETAILS

        public async Task<IDataResult<MenuDetailDto>> MenuDetailGetAsync(int menuDetailId)
        {
            var menuDetail = await UnitOfWork.MenuDetails.GetAsync(t => t.Id == menuDetailId);
            if (menuDetail == null) return new DataResult<MenuDetailDto>(ResultStatus.Error, Messages.MenuDetail.NotFound(false), null);
            return new DataResult<MenuDetailDto>(ResultStatus.Success, new MenuDetailDto { MenuDetail = menuDetail });
        }

        public async Task<IDataResult<MenuDetailListDto>> MenuDetailGetAllAsync(int menuId)
        {
            var menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(md => md.MenuId == menuId && md.ParentId == null, t => t.Parent, t => t.Children, t => t.Menu);
            if (menuDetails.Count < 1) return new DataResult<MenuDetailListDto>(ResultStatus.Error, Messages.MenuDetail.NotFound(true), null);

            foreach (var menuDetail in menuDetails)
            {
                if (menuDetail.SubObjectType == SubObjectType.page || menuDetail.SubObjectType == SubObjectType.article || menuDetail.SubObjectType == SubObjectType.basepage)
                {
                    menuDetail.Object = await UnitOfWork.Posts.GetAsync(p => p.PostType == menuDetail.SubObjectType && p.Id == menuDetail.ObjectId);
                }
                else if (menuDetail.SubObjectType == SubObjectType.category)
                {
                    menuDetail.Object = await UnitOfWork.Terms.GetAsync(p => p.TermType == menuDetail.SubObjectType && p.Id == menuDetail.ObjectId);
                }

                foreach (var child in menuDetail.Children)
                {
                    if (child.SubObjectType == SubObjectType.page || menuDetail.SubObjectType == SubObjectType.article || menuDetail.SubObjectType == SubObjectType.basepage)
                    {
                        child.Object = await UnitOfWork.Posts.GetAsync(p => p.PostType == child.SubObjectType && p.Id == child.ObjectId);
                    }
                    else if (menuDetail.SubObjectType == SubObjectType.category)
                    {
                        child.Object = await UnitOfWork.Terms.GetAsync(p => p.TermType == child.SubObjectType && p.Id == child.ObjectId);
                    }
                }

                MenuDetail parent = menuDetail.Parent;
                menuDetail.Parents = new List<MenuDetail>();
                while (parent != null)
                {
                    menuDetail.Parents.Add(parent);
                    parent = await UnitOfWork.MenuDetails.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                }
            }
            return new DataResult<MenuDetailListDto>(ResultStatus.Success, new MenuDetailListDto { MenuDetails = menuDetails });
        }

        public async Task<IDataResult<MenuDetailListDto>> MenuDetailGetAllParentAsync(int? menuDetailId)
        {
            IList<MenuDetail> menuDetails = null;
            if (menuDetailId == null)
            {
                menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(t => t.Id == menuDetailId.Value);
            }
            else
            {
                var currentMenuDetail = await UnitOfWork.MenuDetails.GetAsync(t => t.Id == menuDetailId.Value, c => c.Children, c => c.Parent, c => c.Parents);

                if (currentMenuDetail != null)
                {
                    MenuDetail parent = currentMenuDetail.Parent;
                    currentMenuDetail.Parents = new List<MenuDetail>();
                    while (parent != null)
                    {
                        currentMenuDetail.Parents.Add(parent);
                        parent = await UnitOfWork.MenuDetails.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                    }

                    List<int> parentIds = new List<int>();
                    foreach (var par in currentMenuDetail.Parents)
                    {
                        parentIds.Add(par.Id);
                    }

                    List<int> childIds = new List<int>();
                    foreach (var child in currentMenuDetail.Children)
                    {
                        childIds.Add(child.Id);
                    }
                    menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(c => c.Id != menuDetailId.Value && !childIds.Contains(c.Id));
                }
                else
                {
                    menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(c => c.Id != menuDetailId.Value);
                }
            }
            return new DataResult<MenuDetailListDto>(ResultStatus.Success, new MenuDetailListDto { MenuDetails = menuDetails });
        }

        public async Task<IDataResult<MenuDetailUpdateDto>> GetMenuDetailUpdateDtoAsync(int menuDetailId)
        {
            var result = await UnitOfWork.MenuDetails.AnyAsync(c => c.Id == menuDetailId);
            if (!result) return new DataResult<MenuDetailUpdateDto>(ResultStatus.Error, Messages.MenuDetail.NotFound(false), null);

            var menuDetail = await UnitOfWork.MenuDetails.GetAsync(c => c.Id == menuDetailId, c => c.Parent, c => c.Children);
            MenuDetail parent = menuDetail.Parent;
            menuDetail.Parents = new List<MenuDetail>();
            while (parent != null)
            {
                menuDetail.Parents.Add(parent);
                parent = await UnitOfWork.MenuDetails.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
            }
            var menuDetailUpdateDto = Mapper.Map<MenuDetailUpdateDto>(menuDetail);
            return new DataResult<MenuDetailUpdateDto>(ResultStatus.Success, menuDetailUpdateDto);
        }

        public async Task<IDataResult<MenuDetailDto>> MenuDetailAddAsync(MenuDetailAddDto menuDetailAddDto)
        {
            var menuDetail = Mapper.Map<MenuDetail>(menuDetailAddDto);
            var addedMenuDetail = await UnitOfWork.MenuDetails.AddAsync(menuDetail);
            await UnitOfWork.SaveAsync();
            return new DataResult<MenuDetailDto>(ResultStatus.Success, new MenuDetailDto { MenuDetail = addedMenuDetail });
        }

        public async Task<IDataResult<MenuDetailDto>> MenuDetailUpdateAsync(MenuDetailUpdateDto menuDetailUpdateDto)
        {
            var oldMenuDetail = await UnitOfWork.MenuDetails.GetAsync(p => p.Id == menuDetailUpdateDto.Id);
            if (oldMenuDetail == null) return new DataResult<MenuDetailDto>(ResultStatus.Error, Messages.MenuDetail.NotFound(false), null);

            var menuDetail = Mapper.Map<MenuDetailUpdateDto, MenuDetail>(menuDetailUpdateDto, oldMenuDetail);
            var updatedMenuDetail = await UnitOfWork.MenuDetails.UpdateAsync(menuDetail);
            await UnitOfWork.SaveAsync();
            return new DataResult<MenuDetailDto>(ResultStatus.Success, new MenuDetailDto { MenuDetail = menuDetail });
        }

        public async Task<IResult> MenuDetailDeleteAsync(int menuId)
        {
            var menuDetail = await UnitOfWork.MenuDetails.GetAsync(t => t.Id == menuId);
            if (menuDetail == null) return new Result(ResultStatus.Error, Messages.MenuDetail.NotFound(false));

            await UnitOfWork.MenuDetails.DeleteAsync(menuDetail);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success);
        }

        #endregion
    }
}