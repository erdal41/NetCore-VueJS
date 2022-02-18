using AutoMapper;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System.Threading.Tasks;

namespace VueJS.Services.Concrete
{
    public class SettingManager : ManagerBase, ISettingService
    {
        public SettingManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        #region General
        public async Task<IDataResult<GeneralSettingDto>> GetHeadGeneralSettingAsync()
        {
            var generalSetting = await UnitOfWork.GeneralSettings.FirstOrDefaultAsync(gs => gs.Logo, gs => gs.MobileLogo, gs => gs.Icon);
            if (generalSetting != null)
            {
                Mapper.Map<GeneralSettingDto>(generalSetting);
                return new DataResult<GeneralSettingDto>(ResultStatus.Success, new GeneralSettingDto
                {
                    GeneralSetting = generalSetting
                });
            }
            return new DataResult<GeneralSettingDto>(ResultStatus.Error, "Genel ayarlar yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
        }

        public async Task<IDataResult<GeneralSettingDto>> GetGeneralSettingAsync()
        {
            var generalSetting = await UnitOfWork.GeneralSettings.FirstOrDefaultAsync(gs => gs.Logo, gs => gs.MobileLogo, gs => gs.Icon);
            if (generalSetting != null)
            {
                Mapper.Map<GeneralSettingDto>(generalSetting);
                return new DataResult<GeneralSettingDto>(ResultStatus.Success, new GeneralSettingDto
                {
                    GeneralSetting = generalSetting
                });
            }
            return new DataResult<GeneralSettingDto>(ResultStatus.Error, "Genel ayarlar yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
        }

        public async Task<IDataResult<GeneralSettingUpdateDto>> GetGeneralSettingUpdateDtoAsync()
        {
            var generalSetting = await UnitOfWork.GeneralSettings.FirstOrDefaultAsync(gs => gs.Logo, gs => gs.MobileLogo, gs => gs.Icon);
            if (generalSetting != null)
            {
                var generalSettingUpdateDto = Mapper.Map<GeneralSettingUpdateDto>(generalSetting);
                return new DataResult<GeneralSettingUpdateDto>(ResultStatus.Success, generalSettingUpdateDto);
            }
            else
            {
                return new DataResult<GeneralSettingUpdateDto>(ResultStatus.Error, "Genel ayarlar yüklenirken bir hata oluştu. Hata devam ederse lütfen yönetici ile iletişime geçiniz.", null);
            }
        }

        public async Task<IDataResult<GeneralSettingDto>> GeneralSettingUpdateAsync(GeneralSettingUpdateDto generalSettingUpdateDto, int userId)
        {
            var oldGeneralSetting = await UnitOfWork.GeneralSettings.GetAsync(gs => gs.Id == generalSettingUpdateDto.Id);
            var generalSetting = Mapper.Map<GeneralSettingUpdateDto, GeneralSetting>(generalSettingUpdateDto, oldGeneralSetting);
            generalSetting.UserId = userId;
            var updatedGeneralSetting = await UnitOfWork.GeneralSettings.UpdateAsync(generalSetting);
            await UnitOfWork.SaveAsync();
            return new DataResult<GeneralSettingDto>(ResultStatus.Success, "Genel ayarlar başarılı bir şekilde güncellendi!", new GeneralSettingDto
            {
                GeneralSetting = updatedGeneralSetting,
                ResultStatus = ResultStatus.Success,
                Message = "Genel ayarlar başarılı bir şekilde güncellendi!"
            });
        }

        #endregion
    }
}