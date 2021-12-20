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

        Task<IDataResult<SeoObjectSettingDto>> GetSeoObjectSettingDtoAsync(int objectId, SubObjectType subSeoObjectType);
        Task<IDataResult<SeoObjectSettingUpdateDto>> GetSeoObjectSettingUpdateDtoAsync(int objectId, SubObjectType subSeoObjectType);
        Task<IResult> SeoObjectSettingUpdateAsync(int objectId, SubObjectType subSeoObjectType, SeoObjectSettingUpdateDto seoObjectSettingUpdateDto, int userId);
        Task<IDataResult<SeoObjectSettingDto>> SeoObjectSettingAddAsync(ObjectType seoObjectType, SubObjectType subSeoObjectType, int objectId, SeoObjectSettingAddDto seoObjectSettingAddDto, int userId);

        #endregion
    }
}