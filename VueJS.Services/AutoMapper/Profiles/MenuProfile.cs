using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<MenuAddDto, Menu>();
            CreateMap<MenuUpdateDto, Menu>();
            CreateMap<Menu, MenuUpdateDto>();

            CreateMap<MenuDetailAddDto, MenuDetail>();
            CreateMap<MenuDetailUpdateDto, MenuDetail>();
            CreateMap<MenuDetail, MenuDetailUpdateDto>();
        }
    }
}