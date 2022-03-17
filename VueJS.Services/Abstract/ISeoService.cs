using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface ISeoService
    {
        #region General

        Task<IDataResult<SeoGeneralSettingDto>> GetHeadSeoSettingAsync();
        Task<IDataResult<SeoGeneralSettingDto>> GetSeoGeneralSettingDtoAsync();
        Task<IDataResult<SeoGeneralSettingUpdateDto>> GetSeoGeneralSettingUpdateDtoAsync();
        Task<IDataResult<SeoGeneralSettingDto>> SeoGeneralSettingUpdateAsync(SeoGeneralSettingUpdateDto seoSettingUpdateDto, int userId);

        #endregion

        #region Object

        Task<IDataResult<SeoObjectSettingDto>> GetSeoObjectSettingDtoAsync(int objectId, ObjectType objectType);
        Task<IDataResult<SeoObjectSettingUpdateDto>> GetSeoObjectSettingUpdateDtoAsync(int objectId, ObjectType objectType);
        Task<IDataResult<SeoObjectSettingDto>> SeoObjectSettingUpdateAsync(int objectId, ObjectType objectType, SeoObjectSettingUpdateDto seoObjectSettingUpdateDto, int userId);
        Task<IDataResult<SeoObjectSettingDto>> SeoObjectSettingAddAsync(ObjectType objectType, int objectId, SeoObjectSettingAddDto seoObjectSettingAddDto, int userId);

        #endregion
    }
}