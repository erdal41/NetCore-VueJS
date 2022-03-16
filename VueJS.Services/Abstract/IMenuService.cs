using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface IMenuService
    {
        #region MENU

        Task<IDataResult<MenuDto>> GetAsync(int menuId);
        Task<IDataResult<MenuListDto>> GetAllAsync();
        Task<IDataResult<MenuUpdateDto>> GetMenuUpdateDtoAsync(int menuId);
        Task<IDataResult<MenuDto>> AddAsync(MenuAddDto menuAddDto);
        Task<IDataResult<MenuDto>> UpdateAsync(MenuUpdateDto menuUpdateDto);
        Task<IResult> DeleteAsync(int menuId);

        #endregion


        #region MENU DETAILS

        Task<IDataResult<MenuDetailDto>> MenuDetailGetAsync(int menuDetailId);
        Task<IDataResult<MenuDetailListDto>> MenuDetailGetAllAsync(int menuId);
        Task<IDataResult<MenuDetailListDto>> MenuDetailGetAllParentAsync(int? menuDetailId);
        Task<IDataResult<MenuDetailUpdateDto>> GetMenuDetailUpdateDtoAsync(int menuDetailId);
        Task<IDataResult<MenuDetailDto>> MenuDetailAddAsync(MenuDetailAddDto menuDetailAddDto);
        Task<IDataResult<MenuDetailDto>> MenuDetailUpdateAsync(MenuDetailUpdateDto menuDetailUpdateDto);
        Task<IResult> MenuDetailDeleteAsync(int menuDetailId);

        #endregion
    }
}