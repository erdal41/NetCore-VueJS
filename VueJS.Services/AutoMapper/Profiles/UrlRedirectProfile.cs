using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class UrlRedirectProfile : Profile
    {
        public UrlRedirectProfile()
        {
            CreateMap<UrlRedirectAddDto, UrlRedirect>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<UrlRedirectUpdateDto, UrlRedirect>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<UrlRedirect, UrlRedirectUpdateDto>();
        }
    }
}