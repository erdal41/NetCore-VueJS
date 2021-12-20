using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<GeneralSettingDto, GeneralSetting>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<GeneralSetting, GeneralSettingDto>();

            CreateMap<GeneralSettingUpdateDto, GeneralSetting>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<GeneralSetting, GeneralSettingUpdateDto>();

            CreateMap<SeoGeneralSettingDto, SeoGeneralSetting>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<SeoGeneralSetting, SeoGeneralSettingDto>();

            CreateMap<SeoGeneralSettingUpdateDto, SeoGeneralSetting>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<SeoGeneralSetting, SeoGeneralSettingUpdateDto>();
        }
    }
}