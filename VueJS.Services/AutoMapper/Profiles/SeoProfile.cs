using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class SeoProfile : Profile
    {
        public SeoProfile()
        {
            CreateMap<SeoGeneralSettingUpdateDto, SeoGeneralSetting>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<SeoGeneralSetting, SeoGeneralSettingUpdateDto>();

            CreateMap<SeoObjectSettingUpdateDto, SeoObjectSetting>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<SeoObjectSetting, SeoObjectSettingUpdateDto>();

            CreateMap<SeoObjectSettingAddDto, SeoObjectSetting>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<SeoObjectSetting, SeoObjectSettingAddDto>();
        }
    }
}