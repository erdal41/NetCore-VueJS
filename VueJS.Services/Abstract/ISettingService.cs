using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface ISettingService
    {
        #region General
        Task<IDataResult<GeneralSettingDto>> GetHeadGeneralSettingAsync();
        Task<IDataResult<GeneralSettingDto>> GetGeneralSettingAsync();
        Task<IDataResult<GeneralSettingUpdateDto>> GetGeneralSettingUpdateDtoAsync();
        Task<IDataResult<GeneralSettingDto>> GeneralSettingUpdateAsync(GeneralSettingUpdateDto generalSettingUpdateDto, int userId);
        #endregion
    }
}